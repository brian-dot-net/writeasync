//-----------------------------------------------------------------------
// <copyright file="EventWindowTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class EventWindowTest
    {
        public EventWindowTest()
        {
        }

        [Fact]
        public void Add_adds_operation_to_pending()
        {
            EventWindow window = new EventWindow();
            int eventId = 1;
            Guid instanceId = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);

            window.Add(eventId, instanceId);

            Assert.Equal(1, window.GetPendingCount(eventId));
        }
    }
}
