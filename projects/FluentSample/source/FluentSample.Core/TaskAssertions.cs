//-----------------------------------------------------------------------
// <copyright file="TaskAssertions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentSample
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using FluentAssertions.Specialized;

    public class TaskAssertions<TTask> where TTask : Task
    {
        private readonly TTask subject;

        public TaskAssertions(TTask subject)
        {
            this.subject = subject;
        }

        public TaskAssertions<TTask> And
        {
            get { return this; }
        }

        public TTask Which
        {
            get { return this.subject; }
        }

        public TaskAssertions<TTask> BeCompleted(string because = "", params object[] reasonArgs)
        {
            return this.AssertCondition(t => t.IsCompleted, "completed", because, reasonArgs);
        }

        public TaskAssertions<TTask> BeCompletedSuccessfully(string because = "", params object[] reasonArgs)
        {
            return this.AssertCondition(t => t.IsCompleted && !t.IsFaulted && !t.IsCanceled, "completed successfully", because, reasonArgs);
        }

        public TaskAssertions<TTask> BeFaulted(string because = "", params object[] reasonArgs)
        {
            return this.AssertCondition(t => t.IsFaulted, "faulted", because, reasonArgs);
        }

        public TaskAssertions<TTask> BePending(string because = "", params object[] reasonArgs)
        {
            return this.AssertCondition(t => !t.IsCompleted, "pending", because, reasonArgs);
        }

        public TaskAssertions<TTask> BeCanceled(string because = "", params object[] reasonArgs)
        {
            return this.AssertCondition(t => t.IsCanceled, "canceled", because, reasonArgs);
        }

        public ExceptionAssertions<TException> WithException<TException>(string because = "", params object[] reasonArgs) where TException : Exception
        {
            Action act = this.Throw;
            return act.ShouldThrow<TException>(because, reasonArgs);
        }

        private TaskAssertions<TTask> AssertCondition(Predicate<TTask> predicate, string expectedState, string because, object[] reasonArgs)
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

        private void Throw()
        {
            Exception exception = this.subject.Exception;
            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
