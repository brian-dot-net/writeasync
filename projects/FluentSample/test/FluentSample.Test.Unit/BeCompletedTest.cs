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
        public void CompletedTaskShouldPass()
        {
            Task task = CompletedTask();

            Action act = () => task.Should().BeCompleted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task task = PendingTask();

            Action act = () => task.Should().BeCompleted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed but was WaitingForActivation.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task task = null;

            Action act = () => task.Should().BeCompleted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed but was <null>.");
        }

        [TestMethod]
        public void FaultedTaskShouldPass()
        {
            Task task = FaultedTask();

            Action act = () => task.Should().BeCompleted();

            act.ShouldNotThrow();
        }

        private static Task CompletedTask()
        {
            return Task.FromResult(false);
        }

        private static Task PendingTask()
        {
            return new TaskCompletionSource<bool>().Task;
        }

        private static Task FaultedTask()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.SetException(new InvalidCastException("Expected failure."));
            return tcs.Task;
        }
    }
}
