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

        public void BeCompleted(string because = "", params object[] reasonArgs)
        {
            this.AssertCondition("completed", because, reasonArgs);
        }

        public void BeCompletedSuccessfully(string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(this.subject != null)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected task to be completed successfully{reason} but was {0}.", this.subject);

            Execute.Assertion
                .ForCondition(this.subject.Status == TaskStatus.RanToCompletion)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected task to be completed successfully{reason} but was {0}.", this.subject.Status);
        }

        public void BeFaulted(string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(this.subject != null)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected task to be faulted{reason} but was {0}.", this.subject);

            Execute.Assertion
                .ForCondition(this.subject.IsFaulted)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected task to be faulted{reason} but was {0}.", this.subject.Status);
        }

        private void AssertCondition(string expectedState, string because, object[] reasonArgs)
        {
            string failureMessage = "Expected task to be " + expectedState + "{reason} but was {0}.";
            Execute.Assertion
                .ForCondition(this.subject != null)
                .BecauseOf(because, reasonArgs)
                .FailWith(failureMessage, this.subject);
            Execute.Assertion
                .ForCondition(this.subject.IsCompleted)
                .BecauseOf(because, reasonArgs)
                .FailWith(failureMessage, this.subject.Status);
        }
    }
}
