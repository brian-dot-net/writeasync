//-----------------------------------------------------------------------
// <copyright file="BeCanceledTest.cs" company="Brian Rogers">
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
    public class BeCanceledTest
    {
        [TestMethod]
        public void CanceledTaskShouldPass()
        {
            Task task = TaskBuilder.Canceled();

            Action act = () => task.Should().BeCanceled();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void CompletedTaskShouldFail()
        {
            Task task = TaskBuilder.Completed();

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was RanToCompletion.");
        }

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was WaitingForActivation.");
        }
    }
}
