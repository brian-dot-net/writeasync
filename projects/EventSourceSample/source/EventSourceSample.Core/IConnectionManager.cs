//-----------------------------------------------------------------------
// <copyright file="IConnectionManager.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System.Threading.Tasks;

    public interface IConnectionManager<TProxy>
    {
        TProxy Proxy { get; }

        Task ConnectAsync();

        void Invalidate();
    }
}
