//-----------------------------------------------------------------------
// <copyright file="Class1.h" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma once

#include <string>
#include <ppltasks.h>

namespace NativeQueueSample
{
    class Class1
    {
    public:
        Class1(std::wstring const & name);
        
        ~Class1();

        concurrency::task<std::wstring> DoAsync() const;

    private:
        Class1(Class1 const & other);
        Class1 & operator=(Class1 const & other);

        std::wstring name_;
    };
}