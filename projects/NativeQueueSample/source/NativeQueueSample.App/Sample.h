//-----------------------------------------------------------------------
// <copyright file="Sample.h" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#pragma once

#include <sstream>
#include "ppltasks_extra.h"

namespace NativeQueueSample
{
    template<typename TProducerQueue>
    concurrency::task<void> EnqueueLoopAsync(TProducerQueue & queue, concurrency::cancellation_token token)
    {
        return concurrency::extras::create_delayed_task(std::chrono::milliseconds(1), [&queue, token]
        {
            shared_ptr<int> i = std::make_shared<int>();
            return concurrency::extras::create_iterative_task([&queue, token, i]()
            {
                if (token.is_canceled())
                {
                    return concurrency::task_from_result(false);
                }

                int next = ++*i;
                queue.Enqueue(next);
                return concurrency::extras::create_delayed_task(std::chrono::milliseconds(1), []() { return true; });
            });
        });
    }

    template<typename TConsumerQueue>
    concurrency::task<void> DequeueLoopAsync(TConsumerQueue & queue, concurrency::cancellation_token token)
    {
        return concurrency::extras::create_delayed_task(std::chrono::milliseconds(1), [&queue, token]
        {
            std::shared_ptr<int> previous = std::make_shared<int>();
            return create_iterative_task([&queue, token, previous]()
            {
                if (token.is_canceled())
                {
                    return concurrency::task_from_result(false);
                }

                concurrency::task<int> dequeueTask = queue.DequeueAsync();
                return dequeueTask.then([previous](concurrency::task<int> t)
                {
                    int current = t.get();
                    if ((current - *previous) != 1)
                    {
                        throw GetOutOfOrderError(current, *previous);
                    }

                    *previous = current;
                    return true;
                });
            });
        });
    }

    std::runtime_error GetOutOfOrderError(int current, int previous)
    {
        std::stringstream ss;
        ss << "Invalid data! Current is " << current << " but previous was " << previous << ".";
        return std::runtime_error(ss.str());
    }

    concurrency::task<void> PrintException(concurrency::task<void> parentTask)
    {
        return parentTask.then([](concurrency::task<void> t)
        {
            try
            {
                t.get();
            }
            catch (std::exception & e)
            {
                std::cout << "ERROR: " << e.what();
                throw;
            }
        });
    }
}