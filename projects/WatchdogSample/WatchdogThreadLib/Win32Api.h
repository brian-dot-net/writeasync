#pragma once

#include "noncopyable.h"
#include <Windows.h>

namespace WatchdogThreadLib
{
    class Win32Api : private noncopyable
    {
    public:
        typedef HANDLE Handle;
        typedef WAITORTIMERCALLBACK WaitOrTimerCallback;

        Win32Api() { }

        ~Win32Api() { }

        Handle CreateEventW(bool manualReset, bool initialState, wchar_t const * rawName)
        {
            return ::CreateEventW(nullptr, manualReset, initialState, rawName);
        }

        void CloseHandle(Handle handle)
        {
            ::CloseHandle(handle);
        }

        bool SetEvent(Handle handle)
        {
            return ::SetEvent(handle) != 0;
        }

        unsigned long GetLastError(Handle handle)
        {
            UNREFERENCED_PARAMETER(handle);
            return ::GetLastError();
        }

        bool RegisterWaitForSingleObject(Handle * newWaitObject, Handle handle, WaitOrTimerCallback callback, void * context, unsigned long milliseconds, unsigned long flags)
        {
            return ::RegisterWaitForSingleObject(newWaitObject, handle, callback, context, milliseconds, flags) != 0;
        }

        void UnregisterWait(Handle waitHandle)
        {
            ::UnregisterWait(waitHandle);
        }
    };
}