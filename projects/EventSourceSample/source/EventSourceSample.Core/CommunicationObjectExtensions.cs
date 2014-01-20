//-----------------------------------------------------------------------
// <copyright file="CommunicationObjectExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System.ServiceModel;
    using System.Threading.Tasks;

    public static class CommunicationObjectExtensions
    {
        public static Task OpenAsync(this ICommunicationObject commObj)
        {
            return Task.Factory.FromAsync(
                (c, s) => ((ICommunicationObject)s).BeginOpen(c, s),
                r => ((ICommunicationObject)r.AsyncState).EndOpen(r),
                commObj);
        }

        public static Task CloseAsync(this ICommunicationObject commObj)
        {
            return Task.Factory.FromAsync(
                (c, s) => ((ICommunicationObject)s).BeginClose(c, s),
                r => ((ICommunicationObject)r.AsyncState).EndClose(r),
                commObj);
        }
    }
}
