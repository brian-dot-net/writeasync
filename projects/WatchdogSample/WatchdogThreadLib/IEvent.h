#pragma once

#include <functional>
#include <string>
#include "noncopyable.h"

namespace WatchdogThreadLib
{
    class IEvent : private noncopyable
    {
    public:
        virtual std::wstring const & get_Name() const = 0;

        virtual unsigned long get_LastError() const = 0;

        virtual void Schedule(std::function<void()> callback) = 0;

        virtual void Signal() = 0;

        virtual ~IEvent() { }

    protected:
        IEvent() { }
    };
}