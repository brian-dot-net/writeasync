#pragma once

extern "C"
{
    unsigned long InitializeWatchdog(wchar_t const * name, void ** handle);
    unsigned long SignalWatchdog(wchar_t const * name);
    unsigned long DestroyWatchdog(void * handle);
}