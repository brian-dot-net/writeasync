//-----------------------------------------------------------------------
// <copyright file="Test1.cpp" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#include <ppl.h>
#include "Class1.h"
#include "CppUnitTest.h"

using namespace concurrency;
using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace std;

namespace NativeQueueSample
{		
    TEST_CLASS(Test1)
    {
    public:
        TEST_METHOD(Should_return_name_after_completing_sync)
        {
            Class1 c(L"MyName");

            task<wstring> task = c.DoAsync();

            Assert::IsTrue(task.is_done());
            Assert::AreEqual(wstring(L"MyName"), task.get());
        }
    };
}