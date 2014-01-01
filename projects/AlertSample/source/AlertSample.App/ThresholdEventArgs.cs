//-----------------------------------------------------------------------
// <copyright file="ThresholdEventArgs.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;

    internal sealed class ThresholdEventArgs : EventArgs
    {
        public ThresholdEventArgs(bool isLowerBound)
        {
            this.IsLowerBound = isLowerBound;
        }

        public bool IsLowerBound { get; private set; }
    }
}
