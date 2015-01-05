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
            Task task = TaskBuilder.Completed();

            Action act = () => task.Should().BeCompleted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeCompleted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReason()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeCompleted(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReasonFormatted()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeCompleted("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task task = null;

            Action act = () => task.Should().BeCompleted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReason()
        {
            Task task = null;

            Action act = () => task.Should().BeCompleted(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed because I said so but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReasonFormatted()
        {
            Task task = null;

            Action act = () => task.Should().BeCompleted("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed because I said so but was <null>.");
        }

        [TestMethod]
        public void FaultedTaskShouldPass()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BeCompleted();

            act.ShouldNotThrow();
        }
    }
}
