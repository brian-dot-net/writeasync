//-----------------------------------------------------------------------
// <copyright file="IProducerQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample
{
    public interface IProducerQueue<T>
    {
        void Enqueue(T item);
    }
}
