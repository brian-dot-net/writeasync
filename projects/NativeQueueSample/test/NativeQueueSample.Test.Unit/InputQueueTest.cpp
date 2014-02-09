//-----------------------------------------------------------------------
// <copyright file="InputQueueTest.cpp" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#include <ppl.h>
#include "InputQueue.h"
#include "CppUnitTest.h"

using namespace concurrency;
using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace std;

namespace NativeQueueSample
{
    template <typename T>
    task<T> & AssertTaskPending(task<T> & task)
    {
        if (task.is_done())
        {
            // Before asserting, force rethrow of exception if task finished with error.
            // Otherwise, we run the risk of a "double fault" due to an unobserved task
            // error, which can end up hanging the VS test executor.
            Logger::WriteMessage(L"Task completed unexpectedly.");
            task.get();
        }

        Assert::IsFalse(task.is_done());
        return task;
    }
    
    TEST_CLASS(InputQueueTest)
    {
    public:
        TEST_METHOD(Dequeue_completes_after_enqueue)
        {
            InputQueue<wstring> queue;

            task<wstring> task = AssertTaskPending(queue.DequeueAsync());

            queue.Enqueue(wstring(L"a"));

            Assert::IsTrue(task.is_done());
            Assert::AreEqual(wstring(L"a"), task.get());
        }

        TEST_METHOD(Enqueue_then_dequeue_completes_sync)
        {
            InputQueue<wstring> queue;

            queue.Enqueue(wstring(L"a"));

            task<wstring> task = queue.DequeueAsync();

            Assert::IsTrue(task.is_done());
            Assert::AreEqual(wstring(L"a"), task.get());
        }

        TEST_METHOD(Multiple_enqueues_then_dequeues_complete_sync_in_order)
        {
            InputQueue<wstring> queue;

            queue.Enqueue(wstring(L"a"));
            queue.Enqueue(wstring(L"b"));

            task<wstring> task = queue.DequeueAsync();

            Assert::IsTrue(task.is_done());
            Assert::AreEqual(wstring(L"a"), task.get());

            task = queue.DequeueAsync();

            Assert::IsTrue(task.is_done());
            Assert::AreEqual(wstring(L"b"), task.get());
        }

        TEST_METHOD(Enqueue_then_dequeue_repeated_twice_completes_sync)
        {
            InputQueue<wstring> queue;

            queue.Enqueue(wstring(L"a"));

            task<wstring> task = queue.DequeueAsync();

            Assert::IsTrue(task.is_done());
            Assert::AreEqual(wstring(L"a"), task.get());

            queue.Enqueue(wstring(L"b"));

            task = queue.DequeueAsync();

            Assert::IsTrue(task.is_done());
            Assert::AreEqual(wstring(L"b"), task.get());
        }

        TEST_METHOD(Dequeue_then_enqueue_repeated_twice_completes_pending)
        {
            InputQueue<wstring> queue;

            task<wstring> task = AssertTaskPending(queue.DequeueAsync());

            queue.Enqueue(wstring(L"a"));
            
            Assert::IsTrue(task.is_done());
            Assert::AreEqual(wstring(L"a"), task.get());

            task = AssertTaskPending(queue.DequeueAsync());

            queue.Enqueue(wstring(L"b"));

            Assert::IsTrue(task.is_done());
            Assert::AreEqual(wstring(L"b"), task.get());
        }
    };
}