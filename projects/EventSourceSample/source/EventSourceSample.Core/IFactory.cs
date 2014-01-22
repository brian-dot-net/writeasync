//-----------------------------------------------------------------------
// <copyright file="IFactory.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    public interface IFactory<TOutput>
    {
        TOutput Create();
    }
}
