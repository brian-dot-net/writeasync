#include "CppUnitTest.h"

#include "IEvent.h"
#include "Watchdog.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace WatchdogThreadLib;
using namespace std;

namespace WatchdogThreadLibTest
{
    TEST_CLASS(WatchdogTest)
    {
    public:
        TEST_METHOD(ShouldScheduleAndExecuteCallbackAfterSignal)
        {
            int failFastCount = 0;
            StubEvent * failFastEvent = new StubEvent(L"myEvent", [](){});
            Watchdog dog(unique_ptr<IEvent>(failFastEvent), [&failFastCount](){ ++failFastCount; });

            dog.Start();

            Assert::IsTrue(failFastEvent->get_Scheduled());
            Assert::AreEqual(0, failFastCount);

            failFastEvent->Signal();

            Assert::IsFalse(failFastEvent->get_Scheduled());
            Assert::AreEqual(1, failFastCount);
        }

        TEST_METHOD(ShouldDestroyEventWhenWatchdogIsDestroyed)
        {
            bool eventDestroyed = false;
            StubEvent * failFastEvent = new StubEvent(L"myEvent", [&eventDestroyed]() { eventDestroyed = true; });
            {
                Watchdog dog(unique_ptr<IEvent>(failFastEvent), [](){});
            }

            Assert::IsTrue(eventDestroyed);
        }

        TEST_METHOD(InitializeShouldReturnStartedWatchdogHandleOnSuccess)
        {
            int failFastCount = 0;
            void * rawDog = nullptr;

            unsigned long errorCode = Watchdog::Initialize(StubEvent::Create, L"myEvent", [&failFastCount](){ ++failFastCount; }, &rawDog);

            Assert::IsNotNull(rawDog);
            unique_ptr<Watchdog> dog(static_cast<Watchdog *>(rawDog));
            unsigned long expectedErrorCode = 0;
            Assert::AreEqual(expectedErrorCode, errorCode);
            IEvent & failFastEvent = dog->get_Event();
            Assert::AreEqual(L"myEvent", failFastEvent.get_Name().c_str());

            failFastEvent.Signal();

            Assert::AreEqual(1, failFastCount);
        }

        TEST_METHOD(InitializeOnEventCreationFailureShouldDestroyEventAndReturnError)
        {
            void * rawDog = nullptr;
            bool destroyed = false;
            auto onDestroy = [&destroyed](){ destroyed = true; };
            unsigned long expectedErrorCode = 1234;
            auto createEvent = [expectedErrorCode, &onDestroy](wchar_t const * name)
            {
                FaultyStubEvent * faultyEvent = new FaultyStubEvent(expectedErrorCode);
                faultyEvent->set_OnDestroy(onDestroy);
                return faultyEvent;
            };

            unsigned long errorCode = Watchdog::Initialize(createEvent, L"myEvent", [](){}, &rawDog);

            Assert::IsNull(rawDog);
            Assert::IsTrue(destroyed);
            Assert::AreEqual(expectedErrorCode, errorCode);
        }

        TEST_METHOD(InitializeOnEventScheduleFailureShouldDestroyEventAndWatchdogAndReturnError)
        {
            void * rawDog = nullptr;
            bool destroyed = false;
            auto onDestroy = [&destroyed](){ destroyed = true; };
            FaultyStubEvent * faultyEvent = nullptr;
            auto createEvent = [&onDestroy, &faultyEvent](wchar_t const * name)
            {
                faultyEvent = new FaultyStubEvent();
                faultyEvent->set_OnDestroy(onDestroy);
                return faultyEvent;
            };

            unsigned long expectedErrorCode = 1122;
            auto onSchedule = [expectedErrorCode, &faultyEvent](){ faultyEvent->set_LastError(expectedErrorCode); };

            unsigned long errorCode = Watchdog::Initialize(createEvent, L"myEvent", onSchedule, &rawDog);

            Assert::IsNull(rawDog);
            Assert::IsTrue(destroyed);
            Assert::AreEqual(expectedErrorCode, errorCode);
        }

        TEST_METHOD(DestroySuccessfullyDestroysValidWatchdog)
        {
            bool destroyed = false;
            StubEvent * failFastEvent = new StubEvent(L"myEvent", [&destroyed](){ destroyed = true; });
            Watchdog * dog = new Watchdog(unique_ptr<IEvent>(failFastEvent), [](){});

            unsigned long errorCode = Watchdog::Destroy(dog);

            unsigned long expectedErrorCode = 0;
            Assert::AreEqual(expectedErrorCode, errorCode);
            Assert::IsTrue(destroyed);
        }

        TEST_METHOD(DestroyNullPointerReturnsError)
        {
            unsigned long errorCode = Watchdog::Destroy(nullptr);

            unsigned long expectedErrorCode = 6;
            Assert::AreEqual(expectedErrorCode, errorCode);
        }

        TEST_METHOD(InitializeWithNullHandleReturnsError)
        {
            void ** handle = nullptr;

            unsigned long errorCode = Watchdog::Initialize([](wchar_t const *) -> IEvent *{ throw runtime_error("Shouldn't be called"); }, L"myEvent", [](){}, handle);

            unsigned long expectedErrorCode = 6;
            Assert::AreEqual(expectedErrorCode, errorCode);
        }

        TEST_METHOD(InitializeWithNullEventCreationReturnsError)
        {
            void * handle;
            unsigned long errorCode = Watchdog::Initialize([](wchar_t const *){ return nullptr; }, L"myEvent", [](){}, &handle);

            Assert::IsNull(handle);
            unsigned long expectedErrorCode = 6;
            Assert::AreEqual(expectedErrorCode, errorCode);
        }

        TEST_METHOD(DestroyWrongTypeReturnsError)
        {
            unique_ptr<int> wrongType(new int(1));
            unsigned long errorCode = Watchdog::Destroy(wrongType.get());

            unsigned long expectedErrorCode = 6;
            Assert::AreEqual(expectedErrorCode, errorCode);
        }

        TEST_METHOD(SignalSuccessfullySignalsAndDestroysNamedEvent)
        {
            bool destroyed = false;
            auto onDestroy = [&destroyed](){ destroyed = true; };
            bool signaled = false;
            auto callback = [&signaled](){ signaled = true; };
            auto createEvent = [&onDestroy, &callback](wchar_t const * name)
            {
                StubEvent * newEvent = new StubEvent(name, onDestroy);
                newEvent->Schedule(callback);
                return newEvent;
            };

            unsigned long errorCode = Watchdog::Signal(createEvent, L"myEvent");

            unsigned long expectedErrorCode = 0;
            Assert::AreEqual(expectedErrorCode, errorCode);
            Assert::IsTrue(signaled);
            Assert::IsTrue(destroyed);
        }

        TEST_METHOD(SignalWhenEventSignalFailsDestroysEventAndReturnsError)
        {
            bool destroyed = false;
            auto onDestroy = [&destroyed](){ destroyed = true; };
            unsigned long expectedErrorCode = 1357;
            auto createEvent = [&onDestroy, expectedErrorCode](wchar_t const *)
            {
                FaultyStubEvent * newEvent = new FaultyStubEvent();
                newEvent->set_OnDestroy(onDestroy);
                newEvent->set_LastError(expectedErrorCode);
                return newEvent;
            };

            unsigned long errorCode = Watchdog::Signal(createEvent, L"myEvent");

            Assert::AreEqual(expectedErrorCode, errorCode);
            Assert::IsTrue(destroyed);
        }

        TEST_METHOD(SignalWhenEventCreationFailsDestroysEventAndReturnsError)
        {
            bool destroyed = false;
            auto onDestroy = [&destroyed](){ destroyed = true; };
            unsigned long expectedErrorCode = 246;
            auto createEvent = [&onDestroy, expectedErrorCode](wchar_t const *)
            {
                FaultyStubEvent * newEvent = new FaultyStubEvent(expectedErrorCode);
                newEvent->set_OnDestroy(onDestroy);
                return newEvent;
            };

            unsigned long errorCode = Watchdog::Signal(createEvent, L"myEvent");

            Assert::AreEqual(expectedErrorCode, errorCode);
            Assert::IsTrue(destroyed);
        }

        TEST_METHOD(SignalWhenEventIsNullReturnsError)
        {
            unsigned long expectedErrorCode = 6;

            unsigned long errorCode = Watchdog::Signal([](wchar_t const *){ return nullptr; }, L"myEvent");

            Assert::AreEqual(expectedErrorCode, errorCode);
        }

    private:

        class FaultyStubEvent : public IEvent
        {
        public:
            FaultyStubEvent(unsigned long creationErrorCode = 0)
                : onDestroy_(),
                lastError_(creationErrorCode),
                creationErrorCode_(creationErrorCode)
            {
            }

            wstring const & get_Name() const { throw runtime_error("Should not be called."); }

            unsigned long get_LastError() const { return lastError_; }

            void set_LastError(unsigned long value) { lastError_ = value; }

            void set_OnDestroy(function<void()> value) { onDestroy_ = value; }

            void Schedule(function<void()> callback)
            {
                FailIfBadEvent();                
                callback();
            }

            void Signal()
            {
                FailIfBadEvent();
            }

            ~FaultyStubEvent()
            {
                onDestroy_();
            }

        private:
            wstring noName_;
            function<void()> onDestroy_;
            unsigned long lastError_;
            unsigned long creationErrorCode_;

            void FailIfBadEvent()
            {
                if (creationErrorCode_ != 0)
                {
                    throw runtime_error("Method should not have been called for bad event.");
                }
            }
        };

        class StubEvent : public IEvent
        {
        public:
            StubEvent(wstring const & name, function<void()> onDestroy = [](){})
                : name_(name),
                callback_(),
                onDestroy_(onDestroy),
                lastError_(0),
                scheduled_(false)
            { }

            wstring const & get_Name() const { return name_; }

            bool get_Scheduled() const { return scheduled_; }

            unsigned long get_LastError() const { return lastError_; }

            static IEvent * Create(wchar_t const * name) { return new StubEvent(name); }

            void Schedule(function<void()> callback)
            {
                scheduled_ = true;
                callback_ = callback;
            }

            void Signal()
            {
                scheduled_ = false;
                callback_();
            }

            ~StubEvent()
            {
                onDestroy_();
            }

        private:
            wstring name_;
            function<void()> callback_;
            function<void()> onDestroy_;
            unsigned long lastError_;
            bool scheduled_;
        };
    };
}