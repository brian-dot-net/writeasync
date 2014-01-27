//-----------------------------------------------------------------------
// <copyright file="QueueEventId.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample
{
    public enum QueueEventId : ushort
    {
        None = 0,

        // Queue
        Enqueue = 1,
        EnqueueCompleted = 2,
        Dequeue = 3,
        DequeueCompleted = 4,
        QueueDispose = 5,
    }
}
