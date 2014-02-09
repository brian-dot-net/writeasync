//-----------------------------------------------------------------------
// <copyright file="Class1.cpp" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#include "Class1.h"

using namespace concurrency;
using namespace std;

namespace NativeQueueSample
{
    Class1::Class1(wstring const & name)
        : name_(name)
    {
    }

    Class1::~Class1()
    {
    }

    task<wstring> Class1::DoAsync() const
    {
        return task_from_result(name_);
    }
}