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

        [Fact]
        public void Add_same_type_twices_adds_2_operations_to_pending()
        {
            EventWindow window = new EventWindow();
            int eventId = 1;
            Guid instanceId1 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            Guid instanceId2 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);

            window.Add(eventId, instanceId1);
            window.Add(eventId, instanceId2);

            Assert.Equal(2, window.GetPendingCount(eventId));
        }

        [Fact]
        public void Add_different_types_adds_2_separate_operations_to_pending()
        {
            EventWindow window = new EventWindow();
            int eventId1 = 1;
            Guid instanceId1 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            int eventId2 = 2;
            Guid instanceId2 = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2);

            window.Add(eventId1, instanceId1);
            window.Add(eventId2, instanceId2);

            Assert.Equal(1, window.GetPendingCount(eventId1));
            Assert.Equal(1, window.GetPendingCount(eventId2));
        }
    }
}
