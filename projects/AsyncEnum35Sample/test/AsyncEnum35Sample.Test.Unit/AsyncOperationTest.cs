//-----------------------------------------------------------------------
// <copyright file="AsyncOperationTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnum35Sample.Test.Unit
{
    using System;
    using System.Collections.Generic;
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
            IAsyncResult result = op.Start(null, null);

            Assert.True(result.IsCompleted);
            Assert.True(result.CompletedSynchronously);
            Assert.Equal(1234, SetResultInCtorOperation.End(result));
        }

        [Fact]
        public void Set_result_in_enumerator_and_break_completes_sync()
        {
            SetResultInEnumeratorOperation op = new SetResultInEnumeratorOperation(1234);
            IAsyncResult result = op.Start(null, null);

            Assert.True(result.IsCompleted);
            Assert.True(result.CompletedSynchronously);
            Assert.Equal(1234, SetResultInEnumeratorOperation.End(result));
        }

        [Fact]
        public void Set_result_in_finally_and_break_completes_sync()
        {
            SetResultInFinallyOperation op = new SetResultInFinallyOperation(1234);
            IAsyncResult result = op.Start(null, null);

            Assert.True(result.IsCompleted);
            Assert.True(result.CompletedSynchronously);
            Assert.Equal(1234, SetResultInFinallyOperation.End(result));
        }

        [Fact]
        public void Set_result_after_one_sync_step_completes_sync()
        {
            SetResultAfterOneStepOperation op = new SetResultAfterOneStepOperation(1234);
            IAsyncResult result = op.Start(null, null);

            Assert.True(result.IsCompleted);
            Assert.True(result.CompletedSynchronously);
            Assert.Equal(1234, SetResultAfterOneStepOperation.End(result));
        }

        [Fact]
        public void Throw_before_yield_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowBeforeYieldOperation op = new ThrowBeforeYieldOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start(null, null));

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throw_after_one_sync_step_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAfterOneStepOperation op = new ThrowAfterOneStepOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start(null, null));

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throw_in_finally_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowInFinallyOperation op = new ThrowInFinallyOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start(null, null));

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Set_result_during_one_sync_step_completes_sync()
        {
            SetResultDuringOneStepOperation op = new SetResultDuringOneStepOperation(1234);
            IAsyncResult result = op.Start(null, null);

            Assert.True(result.IsCompleted);
            Assert.True(result.CompletedSynchronously);
            Assert.Equal(1234, SetResultDuringOneStepOperation.End(result));
        }

        [Fact]
        public void Throw_during_one_sync_step_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowDuringOneStepOperation op = new ThrowDuringOneStepOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start(null, null));

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throw_on_end_of_one_sync_step_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowOnEndOfOneStepOperation op = new ThrowOnEndOfOneStepOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start(null, null));

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Completes_async_after_one_async_step()
        {
            OneAsyncStepOperation op = new OneAsyncStepOperation();
            IAsyncResult result = op.Start(null, null);

            Assert.False(result.IsCompleted);

            op.Complete(1234);

            Assert.True(result.IsCompleted);
            Assert.False(result.CompletedSynchronously);
            Assert.Equal(1234, OneAsyncStepOperation.End(result));
        }

        [Fact]
        public void Completes_with_async_exception_after_one_async_step_with_async_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            OneAsyncStepOperation op = new OneAsyncStepOperation();
            IAsyncResult result = op.Start(null, null);

            Assert.False(result.IsCompleted);

            op.Complete(expected);
            
            Assert.True(result.IsCompleted);
            Assert.False(result.CompletedSynchronously);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => OneAsyncStepOperation.End(result));
            Assert.Same(expected, actual);
        }

        [Fact]
        public void Completes_with_async_exception_after_two_async_steps_with_async_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            TwoAsyncStepOperation op = new TwoAsyncStepOperation();
            IAsyncResult result = op.Start(null, null);

            Assert.False(result.IsCompleted);

            op.Complete(1234);

            Assert.False(result.IsCompleted);

            op.Complete(expected);

            Assert.True(result.IsCompleted);
            Assert.False(result.CompletedSynchronously);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => TwoAsyncStepOperation.End(result));
            Assert.Same(expected, actual);
        }

        [Fact]
        public void Completes_with_async_exception_after_one_async_step_and_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAfterAsyncStepOperation op = new ThrowAfterAsyncStepOperation(expected);
            IAsyncResult result = op.Start(null, null);

            Assert.False(result.IsCompleted);

            op.Complete();

            Assert.True(result.IsCompleted);
            Assert.False(result.CompletedSynchronously);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => ThrowAfterAsyncStepOperation.End(result));
            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throws_sync_and_runs_finally_after_one_step_throws_sync()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowDuringAsyncStepWithFinallyOperation op = new ThrowDuringAsyncStepWithFinallyOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start(null, null));

            Assert.Same(expected, actual);
            Assert.True(op.RanFinally);
        }

        private abstract class TestAsyncOperation : AsyncOperation<int>
        {
            protected TestAsyncOperation()
            {
            }
        }

        private sealed class SetResultInCtorOperation : TestAsyncOperation
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

        private sealed class SetResultInEnumeratorOperation : TestAsyncOperation
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

        private sealed class SetResultInFinallyOperation : TestAsyncOperation
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

        private sealed class SetResultAfterOneStepOperation : TestAsyncOperation
        {
            private readonly int result;

            public SetResultAfterOneStepOperation(int result)
            {
                this.result = result;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    false,
                    (t, c, s) => new CompletedAsyncResult<bool>(t, c, s),
                    (t, r) => CompletedAsyncResult<bool>.End(r));
                this.Result = this.result;
            }
        }

        private sealed class ThrowBeforeYieldOperation : TestAsyncOperation
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

        private sealed class ThrowAfterOneStepOperation : TestAsyncOperation
        {
            private readonly Exception exception;

            public ThrowAfterOneStepOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    false,
                    (t, c, s) => new CompletedAsyncResult<bool>(t, c, s),
                    (t, r) => CompletedAsyncResult<bool>.End(r));
                throw this.exception;
            }
        }

        private sealed class ThrowInFinallyOperation : TestAsyncOperation
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

        private sealed class SetResultDuringOneStepOperation : TestAsyncOperation
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
                    (thisPtr, c, s) => new CompletedAsyncResult<int>(thisPtr.result, c, s),
                    (thisPtr, r) => thisPtr.Result = CompletedAsyncResult<int>.End(r));
            }
        }

        private sealed class ThrowDuringOneStepOperation : TestAsyncOperation
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
                    (thisPtr, c, s) => thisPtr.ThrowSync(),
                    (thisPtr, r) => thisPtr.Invalid());
            }

            private IAsyncResult ThrowSync()
            {
                throw this.exception;
            }

            private void Invalid()
            {
                throw new InvalidOperationException("This shouldn't happen.");
            }
        }

        private sealed class ThrowOnEndOfOneStepOperation : TestAsyncOperation
        {
            private readonly Exception exception;

            public ThrowOnEndOfOneStepOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => new CompletedAsyncResult<int>(1, c, s),
                    (thisPtr, r) => thisPtr.ThrowSync(CompletedAsyncResult<int>.End(r)));
            }

            private void ThrowSync(int result)
            {
                if (result == 1)
                {
                    throw this.exception;
                }
            }
        }

        private sealed class OneAsyncStepOperation : TestAsyncOperation
        {
            private AsyncResult<int> result;

            public OneAsyncStepOperation()
            {
            }

            public void Complete(int result)
            {
                this.result.SetAsCompleted(result, false);
            }

            public void Complete(Exception exception)
            {
                this.result.SetAsCompleted(exception, false);
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => thisPtr.result = new AsyncResult<int>(c, s),
                    (thisPtr, r) => thisPtr.Result = ((AsyncResult<int>)r).EndInvoke());
            }
        }

        private sealed class TwoAsyncStepOperation : TestAsyncOperation
        {
            private AsyncResult<int> result;

            public TwoAsyncStepOperation()
            {
            }

            public void Complete(int result)
            {
                this.result.SetAsCompleted(result, false);
            }

            public void Complete(Exception exception)
            {
                this.result.SetAsCompleted(exception, false);
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => thisPtr.result = new AsyncResult<int>(c, s),
                    (thisPtr, r) => thisPtr.Result = ((AsyncResult<int>)r).EndInvoke());
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => thisPtr.result = new AsyncResult<int>(c, s),
                    (thisPtr, r) => thisPtr.Result = ((AsyncResult<int>)r).EndInvoke());
            }
        }

        private sealed class ThrowAfterAsyncStepOperation : TestAsyncOperation
        {
            private readonly Exception exception;
            private AsyncResult<bool> result;

            public ThrowAfterAsyncStepOperation(Exception exception)
            {
                this.exception = exception;
            }

            public void Complete()
            {
                this.result.SetAsCompleted(false, false);
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => thisPtr.result = new AsyncResult<bool>(c, s),
                    (thisPtr, r) => ((AsyncResult<bool>)r).EndInvoke());
                throw this.exception;
            }
        }

        private sealed class ThrowDuringAsyncStepWithFinallyOperation : TestAsyncOperation
        {
            private readonly Exception exception;

            public ThrowDuringAsyncStepWithFinallyOperation(Exception exception)
            {
                this.exception = exception;
            }

            public bool RanFinally { get; private set; }

            protected override IEnumerator<Step> Steps()
            {
                try
                {
                    yield return Step.Await(
                        this,
                        (thisPtr, c, s) => thisPtr.Throw(),
                        (thisPtr, r) => Invalid());
                }
                finally
                {
                    this.RanFinally = true;
                }
            }

            private static void Invalid()
            {
                throw new InvalidOperationException("This shouldn't happen.");
            }

            private IAsyncResult Throw()
            {
                throw this.exception;
            }
        }
    }
}
