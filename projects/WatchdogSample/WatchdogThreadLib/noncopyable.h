#pragma once

namespace WatchdogThreadLib
{
    class noncopyable
    {
    protected:
        noncopyable() { }

    private:
        noncopyable(noncopyable const &) = delete;
        noncopyable & operator=(noncopyable const &) = delete;
    };
}