//-----------------------------------------------------------------------
// <copyright file="BeCompletedTest.cs" company="Brian Rogers">
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
    public class BeCompletedTest
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

        [TestMethod]
        public void NullTaskShouldFailBeCompleted()
        {
            Task task = null;

            Action act = () => task.Should().BeCompleted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed but was <null>.");
        }

        [TestMethod]
        public void FaultedTaskShouldPassBeCompleted()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.SetException(new InvalidCastException("Expected failure."));
            Task task = tcs.Task;

            Action act = () => task.Should().BeCompleted();

            act.ShouldNotThrow();
        }
    }
}
