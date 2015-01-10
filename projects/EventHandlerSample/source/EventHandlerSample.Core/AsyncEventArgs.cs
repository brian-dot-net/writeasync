//-----------------------------------------------------------------------
// <copyright file="AsyncEventArgs.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AsyncEventArgs : EventArgs
    {
        public AsyncEventArgs()
        {
            this.Tasks = new List<Func<Task>>();
        }

        public IList<Func<Task>> Tasks { get; private set; }
    }
}
