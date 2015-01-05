//-----------------------------------------------------------------------
// <copyright file="TaskAssertionsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentSample;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TaskAssertionsTest
    {
        [TestMethod]
        public void CompletedTaskShouldPassBeCompleted()
        {
            Task task = Task.FromResult(false);

            Action act = () => task.Should().BeCompleted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void PendingTaskShouldFailBeCompleted()
        {
            Task task = new TaskCompletionSource<bool>().Task;

            Action act = () => task.Should().BeCompleted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed but was WaitingForActivation.");
        }
    }
}
