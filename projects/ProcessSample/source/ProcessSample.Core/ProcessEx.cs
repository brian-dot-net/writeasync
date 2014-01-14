//-----------------------------------------------------------------------
// <copyright file="ProcessEx.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;

    public class ProcessEx
    {
        public ProcessEx(IProcess inner)
        {
            inner.Exited += this.OnProcessExited;
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
        }
    }
}
