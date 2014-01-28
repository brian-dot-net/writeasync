//-----------------------------------------------------------------------
// <copyright file="IConsumerQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample
{
    using System;
    using System.Threading.Tasks;

    public interface IConsumerQueue<T> : IDisposable
    {
        Task<T> DequeueAsync();
    }
}
