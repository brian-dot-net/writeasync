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

        // Advanced operations
        SquareRoot = 10,

        // Generic request
        Request = 20,
        RequestCompleted = 21,
        RequestError = 22,

        // Connection
        ConnectionOpening = 30,
        ConnectionOpened = 31,
        ConnectionAborting = 32,
        ConnectionError = 33,
    }
}
