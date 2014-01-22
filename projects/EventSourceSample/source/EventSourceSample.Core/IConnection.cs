//-----------------------------------------------------------------------
// <copyright file="IConnection.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System.Threading.Tasks;

    public interface IConnection<TProxy>
    {
        TProxy Instance { get; }

        Task OpenAsync();
    }
}
