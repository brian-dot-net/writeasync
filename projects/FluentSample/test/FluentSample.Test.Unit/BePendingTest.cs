//-----------------------------------------------------------------------
// <copyright file="BePendingTest.cs" company="Brian Rogers">
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
    public class BePendingTest
    {
        [TestMethod]
        public void PendingTaskShouldPass()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BePending();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void CompletedTaskShouldFail()
        {
            Task task = TaskBuilder.Completed();

            Action act = () => task.Should().BePending();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending but was RanToCompletion.");
        }

        [TestMethod]
        public void FaultedTaskShouldFail()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BePending();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending but was Faulted.");
        }

        [TestMethod]
        public void FaultedTaskShouldFailWithReason()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BePending(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending because I said so but was Faulted.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task task = null;

            Action act = () => task.Should().BePending();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReason()
        {
            Task task = null;

            Action act = () => task.Should().BePending(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending because I said so but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReasonFormatted()
        {
            Task task = null;

            Action act = () => task.Should().BePending("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending because I said so but was <null>.");
        }
    }
}
