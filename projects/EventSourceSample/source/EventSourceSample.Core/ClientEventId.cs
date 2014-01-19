//-----------------------------------------------------------------------
// <copyright file="ClientEventId.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    public enum ClientEventId : ushort
    {
        None = 0,

        // Basic operations
        Add = 1,
        Subtract = 2,
    }
}
