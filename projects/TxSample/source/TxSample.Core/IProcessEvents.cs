//-----------------------------------------------------------------------
// <copyright file="IProcessEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TxSample
{
    using System;

    public interface IProcessEvents
    {
        event EventHandler<ProcessEventArgs> ProcessStarted;

        event EventHandler<ProcessEventArgs> ProcessStopped;
    }
}
