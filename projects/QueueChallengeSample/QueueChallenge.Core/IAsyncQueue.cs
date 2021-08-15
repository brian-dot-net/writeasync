// <copyright file="IAsyncQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge
{
    using System.Threading.Tasks;

    public interface IAsyncQueue<T>
    {
        Task<T> DequeueAsync();
    }
}
