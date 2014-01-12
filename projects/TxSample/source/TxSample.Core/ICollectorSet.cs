//-----------------------------------------------------------------------
// <copyright file="ICollectorSet.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TxSample
{
    public interface ICollectorSet : ISessionController
    {
        void Delete();
    }
}
