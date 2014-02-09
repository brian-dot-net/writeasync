//-----------------------------------------------------------------------
// <copyright file="InputQueue.h" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma once

#include <string>
#include <ppltasks.h>

namespace NativeQueueSample
{
    template <typename T>
    class InputQueue
    {
    public:
        InputQueue()
            : tce_()
        {
        }
        
        ~InputQueue()
        {
        }

        void Enqueue(T item)
        {
            if (!tce_)
            {
                tce_ = make_unique<concurrency::task_completion_event<T>>();
            }

            tce_->set(item);
        }

        concurrency::task<T> DequeueAsync()
        {
            if (!tce_)
            {
                tce_ = make_unique<concurrency::task_completion_event<T>>();
            }

            return concurrency::task<T>(*tce_);
        }

    private:
        std::unique_ptr<concurrency::task_completion_event<T>> tce_;

        InputQueue(InputQueue const & other);
        InputQueue & operator=(InputQueue const & other);
    };
}