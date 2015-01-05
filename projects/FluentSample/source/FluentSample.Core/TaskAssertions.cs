//-----------------------------------------------------------------------
// <copyright file="TaskAssertions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentSample
{
    using System.Threading.Tasks;
    using FluentAssertions.Execution;

    public class TaskAssertions
    {
        private readonly Task subject;

        public TaskAssertions(Task subject)
        {
            this.subject = subject;
        }

        public void BeCompleted()
        {
            Execute.Assertion
                .ForCondition(this.subject != null)
                .FailWith("Expected task to be completed but was {0}.", this.subject);

            Execute.Assertion
                .ForCondition(this.subject.IsCompleted)
                .FailWith("Expected task to be completed but was {0}.", this.subject.Status);
        }

        public void BeCompletedSuccessfully()
        {
            Execute.Assertion
                .ForCondition(this.subject != null)
                .FailWith("Expected task to be completed successfully but was {0}.", this.subject);

            Execute.Assertion
                .ForCondition(this.subject.Status == TaskStatus.RanToCompletion)
                .FailWith("Expected task to be completed successfully but was {0}.", this.subject.Status);
        }
    }
}
