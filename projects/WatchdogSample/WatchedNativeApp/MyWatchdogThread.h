#pragma once

#include <stdexcept>
#include "WatchdogThread.h"

class MyWatchdogThread
{
public:
    MyWatchdogThread(wchar_t const * signalName)
        : handle_(nullptr)
    {
        unsigned long errorCode = InitializeWatchdog(signalName, &handle_);
        if (errorCode != 0)
        {
            throw std::runtime_error("Failed to initialize watchdog.");
        }
    }

    static void Signal(wchar_t const * signalName)
    {
        SignalWatchdog(signalName);
    }

    ~MyWatchdogThread()
    {
        DestroyWatchdog(handle_);
    }

private:
    void * handle_;
};