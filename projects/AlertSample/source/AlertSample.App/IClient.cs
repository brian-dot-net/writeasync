//-----------------------------------------------------------------------
// <copyright file="IClient.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System.ServiceModel;
    using System.Threading.Tasks;

    [ServiceContract]
    internal interface IClient
    {
        [OperationContract(IsOneWay = false)]
        void Send();
    }

    [ServiceContract(Name = "IClient")]
    internal interface IClientAsync
    {
        [OperationContract(IsOneWay = false, AsyncPattern = true)]
        Task SendAsync();
    }
}
