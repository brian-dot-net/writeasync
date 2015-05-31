#pragma once

#include "Win32Api.h"
#include "ManualResetEvent.h"

namespace WatchdogThreadLib
{
    typedef ManualResetEvent<Win32Api> Win32ManualResetEvent;
}