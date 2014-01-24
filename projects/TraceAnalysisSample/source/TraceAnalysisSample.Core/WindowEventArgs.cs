//-----------------------------------------------------------------------
// <copyright file="WindowEventArgs.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;

    public class WindowEventArgs : EventArgs
    {
        public WindowEventArgs(EventWindow window)
        {
            this.Window = window;
        }

        public EventWindow Window { get; private set; }
    }
}
