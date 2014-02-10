//-----------------------------------------------------------------------
// <copyright file="InputQueue.h" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma once

#include <string>
#include <ppltasks.h>
#include <queue>

namespace NativeQueueSample
{
    template <typename T>
    class InputQueue
    {
    public:
        InputQueue()
            : items_(),
            pending_(),
            syncRoot_()
        {
        }
        
        ~InputQueue()
        {
        }

        void Enqueue(T item)
        {
            std::unique_ptr<concurrency::task_completion_event<T>> current;
            {
                concurrency::critical_section::scoped_lock lock(syncRoot_);
                if (!pending_)
                {
                    items_.push(item);
                }
                else
                {
                    current = std::move(pending_);
                }
            }

            if (current)
            {
                current->set(item);
            }
        }

        concurrency::task<T> DequeueAsync()
        {
            concurrency::critical_section::scoped_lock lock(syncRoot_);
            if (pending_)
            {
                throw concurrency::invalid_operation("A dequeue operation is already in progress.");
            }

            std::unique_ptr<concurrency::task_completion_event<T>> current = make_unique<concurrency::task_completion_event<T>>();
            concurrency::task<T> task(*current);
            if (!items_.empty())
            {
                T item = items_.front();
                items_.pop();
                current->set(item);
            }
            else
            {
                pending_ = move(current);
            }

            return task;
        }

    private:
        std::queue<T> items_;
        std::unique_ptr<concurrency::task_completion_event<T>> pending_;
        concurrency::critical_section syncRoot_;

        InputQueue(InputQueue const & other);
        InputQueue & operator=(InputQueue const & other);
    };
}