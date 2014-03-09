//-----------------------------------------------------------------------
// <copyright file="AsyncOperationTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnumSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class AsyncOperationTest
    {
        public AsyncOperationTest()
        {
        }

        [Fact]
        public void Set_result_in_ctor_and_break_completes_sync()
        {
            SetResultInCtorOperation op = new SetResultInCtorOperation(1234);
            Task<int> task = op.Start();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1234, task.Result);
        }

        [Fact]
        public void Set_result_in_enumerator_and_break_completes_sync()
        {
            SetResultInEnumeratorOperation op = new SetResultInEnumeratorOperation(1234);
            Task<int> task = op.Start();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1234, task.Result);
        }

        [Fact]
        public void Set_result_in_finally_and_break_completes_sync()
        {
            SetResultInFinallyOperation op = new SetResultInFinallyOperation(1234);
            Task<int> task = op.Start();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1234, task.Result);
        }

        [Fact]
        public void Set_result_after_one_sync_step_completes_sync()
        {
            SetResultAfterOneStepOperation op = new SetResultAfterOneStepOperation(1234);
            Task<int> task = op.Start();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1234, task.Result);
        }

        [Fact]
        public void Throw_before_yield_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowBeforeYieldOperation op = new ThrowBeforeYieldOperation(expected);            
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throw_after_one_sync_step_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAfterOneStepOperation op = new ThrowAfterOneStepOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throw_in_finally_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowInFinallyOperation op = new ThrowInFinallyOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Set_result_during_one_sync_step_completes_sync()
        {
            SetResultDuringOneStepOperation op = new SetResultDuringOneStepOperation(1234);
            Task<int> task = op.Start();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1234, task.Result);
        }

        [Fact]
        public void Throw_during_one_sync_step_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowDuringOneStepOperation op = new ThrowDuringOneStepOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
        }

        private sealed class SetResultInCtorOperation : AsyncOperation<int>
        {
            public SetResultInCtorOperation(int result)
            {
                this.Result = result;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield break;
            }
        }

        private sealed class SetResultInEnumeratorOperation : AsyncOperation<int>
        {
            private readonly int result;

            public SetResultInEnumeratorOperation(int result)
            {
                this.result = result;
            }

            protected override IEnumerator<Step> Steps()
            {
                this.Result = this.result;
                yield break;
            }
        }

        private sealed class SetResultInFinallyOperation : AsyncOperation<int>
        {
            private readonly int result;

            public SetResultInFinallyOperation(int result)
            {
                this.result = result;
            }

            protected override IEnumerator<Step> Steps()
            {
                try
                {
                    yield break;
                }
                finally
                {
                    this.Result = this.result;
                }
            }
        }

        private sealed class SetResultAfterOneStepOperation : AsyncOperation<int>
        {
            private readonly int result;

            public SetResultAfterOneStepOperation(int result)
            {
                this.result = result;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(false, r => Step.TaskFromResult(r));
                this.Result = this.result;
            }
        }

        private sealed class ThrowBeforeYieldOperation : AsyncOperation<int>
        {
            private readonly Exception exception;

            public ThrowBeforeYieldOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                throw this.exception;
            }
        }

        private sealed class ThrowAfterOneStepOperation : AsyncOperation<int>
        {
            private readonly Exception exception;

            public ThrowAfterOneStepOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(false, r => Step.TaskFromResult(r));
                throw this.exception;
            }
        }

        private sealed class ThrowInFinallyOperation : AsyncOperation<int>
        {
            private readonly Exception exception;

            public ThrowInFinallyOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                try
                {
                    yield break;
                }
                finally
                {
                    throw this.exception;
                }
            }
        }

        private sealed class SetResultDuringOneStepOperation : AsyncOperation<int>
        {
            private readonly int result;

            public SetResultDuringOneStepOperation(int result)
            {
                this.result = result;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => Step.TaskFromResult(thisPtr.result),
                    (thisPtr, r) => thisPtr.Result = r);
            }
        }

        private sealed class ThrowDuringOneStepOperation : AsyncOperation<int>
        {
            private readonly Exception exception;

            public ThrowDuringOneStepOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => thisPtr.ThrowSync(),
                    (thisPtr, r) => thisPtr.Invalid());
            }

            private Task<int> ThrowSync()
            {
                throw this.exception;
            }

            private void Invalid()
            {
                throw new InvalidOperationException("This shouldn't happen.");
            }
        }
    }
}
