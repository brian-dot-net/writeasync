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
        Dequeue = 2,
        QueueDispose = 3,
    }
}
