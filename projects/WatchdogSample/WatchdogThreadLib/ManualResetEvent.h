#pragma once

#include <exception>
#include <memory>
#include <string>
#include "IEvent.h"

namespace WatchdogThreadLib
{
    template<typename TApi>
    class ManualResetEvent : public IEvent
    {
    public:
        ManualResetEvent(std::wstring const & name)
            : api_(),
            name_(name),
            event_(api_.CreateEventW(true, false, name_.c_str())),
            waiter_(),
            callback_(),
            lastError_(event_ ? 0 : api_.GetLastError(event_))
        {
        }

        unsigned long get_LastError() const { return lastError_; }

        std::wstring const & get_Name() const { return name_; }

        TApi & get_Api() { return api_; }

        void Schedule(std::function<void()> callback)
        {
            ValidateEvent();

            if (waiter_)
            {
                throw std::runtime_error("A waiter is already scheduled.");
            }

            callback_ = callback;
            bool result = api_.RegisterWaitForSingleObject(&waiter_, event_, ManualResetEvent::CallbackWrapper, this, -1, 0x18);
            CheckError(result);
        }

        void Signal()
        {
            ValidateEvent();

            bool result = api_.SetEvent(event_);
            CheckError(result);
        }

        ~ManualResetEvent()
        {
            if (waiter_)
            {
                api_.UnregisterWait(waiter_);
            }

            if (event_)
            {
                api_.CloseHandle(event_);
            }
        }

    private:
        TApi api_;
        std::wstring name_;
        typename TApi::Handle event_;
        typename TApi::Handle waiter_;
        std::function<void()> callback_;
        unsigned long lastError_;

        static void CallbackWrapper(void * context, unsigned char timerOrWaitFired)
        {
            static_cast<ManualResetEvent *>(context)->InvokeCallback();
        }

        void InvokeCallback()
        {
            callback_();
        }

        void CheckError(bool result)
        {
            if (!result)
            {
                lastError_ = api_.GetLastError(event_);
            }
        }

        void ValidateEvent()
        {
            if (!event_)
            {
                throw std::runtime_error("Event handle is invalid.");
            }
        }
    };
}