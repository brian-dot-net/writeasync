//-----------------------------------------------------------------------
// <copyright file="IClient.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System.ServiceModel;

    [ServiceContract]
    internal interface IClient
    {
        [OperationContract(IsOneWay = false)]
        void Send();
    }
}
