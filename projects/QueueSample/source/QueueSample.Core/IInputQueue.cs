//-----------------------------------------------------------------------
// <copyright file="IInputQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample
{
    using System;
    using System.Threading.Tasks;

    public interface IInputQueue<T> : IDisposable
    {
        Task<T> DequeueAsync();

        void Enqueue(T item);
    }
}
