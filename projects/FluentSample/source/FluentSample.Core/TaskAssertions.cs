//-----------------------------------------------------------------------
// <copyright file="TaskAssertions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentSample
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions.Execution;

    public class TaskAssertions
    {
        private readonly Task subject;

        public TaskAssertions(Task subject)
        {
            this.subject = subject;
        }

        public TaskAssertions And
        {
            get { return this; }
        }

        public TaskAssertions BeCompleted(string because = "", params object[] reasonArgs)
        {
            return this.AssertCondition(t => t.IsCompleted, "completed", because, reasonArgs);
        }

        public TaskAssertions BeCompletedSuccessfully(string because = "", params object[] reasonArgs)
        {
            return this.AssertCondition(t => t.IsCompleted && !t.IsFaulted, "completed successfully", because, reasonArgs);
        }

        public TaskAssertions BeFaulted(string because = "", params object[] reasonArgs)
        {
            return this.AssertCondition(t => t.IsFaulted, "faulted", because, reasonArgs);
        }

        public void BePending(string because = "")
        {
            this.AssertCondition(t => !t.IsCompleted, "pending", because, new object[0]);
        }

        private TaskAssertions AssertCondition(Predicate<Task> predicate, string expectedState, string because, object[] reasonArgs)
        {
            string failureMessage = "Expected task to be " + expectedState + "{reason} but was {0}.";
            Execute.Assertion
                .ForCondition(this.subject != null)
                .BecauseOf(because, reasonArgs)
                .FailWith(failureMessage, this.subject);
            Execute.Assertion
                .ForCondition(predicate(this.subject))
                .BecauseOf(because, reasonArgs)
                .FailWith(failureMessage, this.subject.Status);
            return this;
        }
    }
}
