#pragma once

#include <memory>
#include "IEvent.h"

namespace WatchdogThreadLib
{
    class Watchdog : private noncopyable
    {
    public:
        Watchdog(std::unique_ptr<IEvent> failFastEvent, std::function<void()> failFast)
            : failFastEvent_(std::move(failFastEvent)),
            failFast_(failFast),
            magic_(Magic)
        { }

        IEvent & get_Event() { return *failFastEvent_; }

        static unsigned long Initialize(std::function<IEvent *(wchar_t const *)> createEvent, wchar_t const * name, std::function<void()> failFast, void ** watchdog)
        {
            if (!watchdog)
            {
                return ErrorInvalidHandle;
            }

            *watchdog = nullptr;
            std::unique_ptr<IEvent> failFastEvent(createEvent(name));
            if (!failFastEvent)
            {
                return ErrorInvalidHandle;
            }

            unsigned long errorCode = failFastEvent->get_LastError();
            if (errorCode != 0)
            {
                return errorCode;
            }

            std::unique_ptr<Watchdog> dog(new Watchdog(move(failFastEvent), failFast));
            errorCode = dog->Start();
            if (errorCode != 0)
            {
                return errorCode;
            }

            *watchdog = dog.release();

            return errorCode;
        }

        static unsigned long Signal(std::function<IEvent *(wchar_t const *)> createEvent, wchar_t const * name)
        {
            std::unique_ptr<IEvent> eventToSignal(createEvent(name));
            if (!eventToSignal)
            {
                return ErrorInvalidHandle;
            }

            unsigned long errorCode = eventToSignal->get_LastError();
            if (errorCode != 0)
            {
                return errorCode;
            }

            eventToSignal->Signal();
            return eventToSignal->get_LastError();
        }

        static unsigned long Destroy(void * watchdog)
        {
            if (!watchdog)
            {
                return ErrorInvalidHandle;
            }

            Watchdog * rawDog = static_cast<Watchdog *>(watchdog);
            if (rawDog->magic_ != Magic)
            {
                return ErrorInvalidHandle;
            }

            delete rawDog;
            return 0;
        }

        unsigned int Start()
        {
            failFastEvent_->Schedule(failFast_);
            return failFastEvent_->get_LastError();
        }

        ~Watchdog()
        {
        }

    private:
        static unsigned long const ErrorInvalidHandle = 6;
        static int const Magic = 0x1234AD09;

        std::unique_ptr<IEvent> failFastEvent_;
        std::function<void()> failFast_;
        int magic_;
    };
}