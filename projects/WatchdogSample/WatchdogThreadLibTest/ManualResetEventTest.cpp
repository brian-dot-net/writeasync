#include "CppUnitTest.h"

#include <map>
#include "ManualResetEvent.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace WatchdogThreadLib;
using namespace std;

namespace WatchdogThreadLibTest
{
    TEST_CLASS(WatchdogTest)
    {
    public:
        TEST_METHOD(ShouldSetLastErrorIfFailCreate)
        {
            ManualResetEvent<StubApi> mre(L"failCreate");

            unsigned long expectedError = 0xFC;
            Assert::AreEqual(expectedError, mre.get_LastError());
        }

        TEST_METHOD(ShouldDisallowSignalIfFailCreate)
        {
            ManualResetEvent<StubApi> mre(L"failCreate");
            bool threwException = false;

            try
            {
                mre.Schedule([](){});
            }
            catch (runtime_error &)
            {
                threwException = true;
            }

            Assert::IsTrue(threwException, L"Runtime error was not thrown.");
        }

        TEST_METHOD(ShouldDisallowScheduleIfFailCreate)
        {
            ManualResetEvent<StubApi> mre(L"failCreate");
            bool threwException = false;

            try
            {
                mre.Schedule([](){});
            }
            catch (runtime_error &)
            {
                threwException = true;
            }

            Assert::IsTrue(threwException, L"Runtime error was not thrown.");
        }

        TEST_METHOD(ShouldSetEventOnSignal)
        {
            ManualResetEvent<StubApi> mre(L"good");

            mre.Signal();

            StubEvent * signaledEvent = mre.get_Api().get_Event(L"good");
            Assert::IsTrue(signaledEvent->get_Signaled());
        }

        TEST_METHOD(ShouldSetLastErrorOnSetEventFailure)
        {
            ManualResetEvent<StubApi> mre(L"failSetEvent");
            StubEvent * signaledEvent = mre.get_Api().get_Event(L"failSetEvent");
            unsigned long expectedError = 123;
            mre.get_Api().set_LastError(expectedError);
            signaledEvent->set_SignalReturn(false);

            mre.Signal();

            Assert::AreEqual(expectedError, mre.get_LastError());
        }

        TEST_METHOD(ShouldScheduleWaitAndExecuteCallbackAfterSignal)
        {
            ManualResetEvent<StubApi> mre(L"waiting");
            bool executed = false;

            mre.Schedule([&executed](){ executed = true; });
            mre.Signal();

            Assert::IsTrue(executed);
        }

        TEST_METHOD(ShouldSetLastErrorOnScheduleFailure)
        {
            ManualResetEvent<StubApi> mre(L"failSchedule");
            StubEvent * scheduledEvent = mre.get_Api().get_Event(L"failSchedule");
            unsigned long expectedError = 321;
            mre.get_Api().set_LastError(expectedError);
            scheduledEvent->set_ScheduleReturn(false);

            mre.Schedule([](){});

            Assert::AreEqual(expectedError, mre.get_LastError());
        }

        TEST_METHOD(ShouldDisallowScheduleIfAlreadyScheduled)
        {
            ManualResetEvent<StubApi> mre(L"scheduleTwice");
            mre.Schedule([](){});
            bool threwException = false;

            try
            {
                mre.Schedule([](){});
            }
            catch (runtime_error &)
            {
                threwException = true;
            }

            Assert::IsTrue(threwException, L"Runtime error was not thrown.");
        }

    private:
        class StubEvent;
        class StubWaiter;

        class StubApi : private noncopyable
        {
        public:
            typedef void(*WaitOrTimerCallback)(void *, unsigned char);
            typedef void * Handle;
            typedef map<wstring, StubEvent *> EventMap;
            typedef map<wstring, StubWaiter *> WaiterMap;

            StubApi()
                : events_(),
                waiters_(),
                lastError_(0)
            { }

            StubEvent * get_Event(wstring const & name)
            {
                auto it = events_.find(name);
                if (it == events_.end())
                {
                    return nullptr;
                }

                return it->second;
            }

            unsigned long get_LastError() const { return lastError_; }

            void set_LastError(unsigned long value) { lastError_ = value; }

            Handle CreateEventW(bool manualReset, bool initialState, wchar_t const * rawName)
            {
                Assert::IsTrue(manualReset);
                Assert::IsFalse(initialState);

                wstring name(rawName);
                if (name == L"failCreate")
                {
                    return nullptr;
                }

                StubEvent * newEvent = new StubEvent(rawName);
                events_[name] = newEvent;
                return newEvent;
            }

            void CloseHandle(Handle handle)
            {
                if (!handle)
                {
                    throw runtime_error("Attempting to destroy a null event!");
                }

                StubEvent * oldEvent = static_cast<StubEvent *>(handle);
                events_.erase(events_.find(oldEvent->get_Name()));
                delete oldEvent;
            }

            bool SetEvent(Handle handle)
            {
                return static_cast<StubEvent *>(handle)->Signal();
            }

            unsigned long GetLastError(Handle handle)
            {
                if (!handle)
                {
                    return 0xFC;
                }

                return lastError_;
            }

            bool RegisterWaitForSingleObject(Handle * newWaitObject, Handle handle, WaitOrTimerCallback callback, void * context, unsigned long milliseconds, unsigned long flags)
            {
                unsigned long expectedMilliseconds = -1;
                unsigned long expectedFlags = 0x18;
                Assert::AreEqual(expectedFlags, flags);
                Assert::AreEqual(expectedMilliseconds, milliseconds);

                StubWaiter * newWaiter = new StubWaiter(L"waiter");
                waiters_[newWaiter->get_Name()] = newWaiter;
                *newWaitObject = newWaiter;

                return static_cast<StubEvent *>(handle)->Schedule(callback, context);
            }

            void UnregisterWait(Handle waitHandle)
            {
                if (!waitHandle)
                {
                    throw runtime_error("Attempting to destroy a null waiter!");
                }

                StubWaiter * oldWaiter = static_cast<StubWaiter *>(waitHandle);
                waiters_.erase(waiters_.find(oldWaiter->get_Name()));
                delete oldWaiter;
            }

            ~StubApi()
            {
                if (!events_.empty())
                {
                    throw runtime_error("Memory leak -- events were not closed.");
                }

                if (!waiters_.empty())
                {
                    throw runtime_error("Memory leak -- waiters were not closed.");
                }
            }

        private:
            EventMap events_;
            WaiterMap waiters_;
            unsigned long lastError_;
        };

        class StubWaiter : private noncopyable
        {
        public:
            StubWaiter(wstring const & name)
                : name_(name)
            {
            }

            wstring const & get_Name() const { return name_; }

        private:
            wstring name_;
        };

        class StubEvent : private noncopyable
        {
        public:
            StubEvent(wstring const & name)
                : name_(name),
                callback_(),
                context_(),
                signaled_(false),
                signalReturn_(true),
                scheduleReturn_(true)
            {
            }

            wstring const & get_Name() const { return name_; }

            bool get_Signaled() const { return signaled_; }

            void set_SignalReturn(bool value) { signalReturn_ = value; }

            void set_ScheduleReturn(bool value) { scheduleReturn_ = value; }

            bool Schedule(StubApi::WaitOrTimerCallback callback, void * context)
            {
                callback_ = callback;
                context_ = context;
                return scheduleReturn_;
            }

            bool Signal()
            {
                signaled_ = true;
                if (callback_)
                {
                    callback_(context_, 1);
                }

                return signalReturn_;
            }

        private:
            wstring name_;
            StubApi::WaitOrTimerCallback callback_;
            void * context_;
            bool signaled_;
            bool signalReturn_;
            bool scheduleReturn_;
        };
    };
}