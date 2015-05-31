#include <Windows.h>

#include "WatchdogThread.h"
#include "Watchdog.h"
#include "Win32ManualResetEvent.h"

using namespace std;
using namespace WatchdogThreadLib;

void WatchdogFailFast()
{
    RaiseFailFastException(nullptr, nullptr, 0);
}

IEvent * CreateFailFastEvent(wchar_t const * name)
{
    return new Win32ManualResetEvent(name);
}

unsigned long InitializeWatchdog(wchar_t const * name, void ** handle)
{
    return Watchdog::Initialize(CreateFailFastEvent, name, WatchdogFailFast, handle);
}

unsigned long SignalWatchdog(wchar_t const * name)
{
    return Watchdog::Signal(CreateFailFastEvent, name);
}

unsigned long DestroyWatchdog(void * handle)
{
    return Watchdog::Destroy(handle);
}