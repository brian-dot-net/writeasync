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

        [Fact]
        public void Throw_on_end_of_one_sync_step_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowOnEndOfOneStepOperation op = new ThrowOnEndOfOneStepOperation(expected);
            AggregateException ae = Assert.Throws<AggregateException>(() => op.Start());

            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.Same(expected, ae.InnerExceptions[0]);
        }

        [Fact]
        public void Throw_during_one_void_sync_step_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowDuringOneStepVoidOperation op = new ThrowDuringOneStepVoidOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throw_async_during_one_void_step_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAsyncDuringOneStepVoidOperation op = new ThrowAsyncDuringOneStepVoidOperation(expected);
            AggregateException ae = Assert.Throws<AggregateException>(() => op.Start());

            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.Same(expected, ae.InnerExceptions[0]);
        }

        [Fact]
        public void Throw_async_during_one_step_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAsyncDuringOneStepOperation op = new ThrowAsyncDuringOneStepOperation(expected);
            AggregateException ae = Assert.Throws<AggregateException>(() => op.Start());

            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.Same(expected, ae.InnerExceptions[0]);
        }

        [Fact]
        public void Completes_async_after_one_async_step()
        {
            OneAsyncStepOperation op = new OneAsyncStepOperation();
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete(1234);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1234, task.Result);
        }

        [Fact]
        public void Completes_with_async_exception_after_one_async_step_with_async_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            TwoAsyncStepOperation op = new TwoAsyncStepOperation();
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete(expected);

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
            Assert.Equal(1, task.Exception.InnerExceptions.Count);
            Assert.Same(expected, task.Exception.InnerExceptions[0]);
        }

        [Fact]
        public void Completes_with_async_exception_after_two_async_steps_with_async_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            TwoAsyncStepOperation op = new TwoAsyncStepOperation();
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete(null);

            Assert.False(task.IsCompleted);

            op.Complete(expected);

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
            Assert.Equal(1, task.Exception.InnerExceptions.Count);
            Assert.Same(expected, task.Exception.InnerExceptions[0]);
        }

        [Fact]
        public void Completes_with_async_exception_after_one_async_step_and_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAfterAsyncStepOperation op = new ThrowAfterAsyncStepOperation(expected);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete();

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
            Assert.Equal(1, task.Exception.InnerExceptions.Count);
            Assert.Same(expected, task.Exception.InnerExceptions[0]);
        }

        [Fact]
        public void Throws_sync_and_runs_finally_after_one_step_throws_sync()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowDuringAsyncStepWithFinallyOperation op = new ThrowDuringAsyncStepWithFinallyOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
            Assert.True(op.RanFinally);
        }

        [Fact]
        public void Completes_async_and_runs_finally_after_one_step_completes_async()
        {
            OneAsyncStepWithFinallyOperation op = new OneAsyncStepWithFinallyOperation(1234);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete(null);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1234, task.Result);
        }

        [Fact]
        public void Completes_with_async_exception_and_runs_finally_after_one_step_completes_with_async_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            OneAsyncStepWithFinallyOperation op = new OneAsyncStepWithFinallyOperation(1234);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete(expected);

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
            Assert.Equal(1, task.Exception.InnerExceptions.Count);
            Assert.Same(expected, task.Exception.InnerExceptions[0]);
            Assert.Equal(1234, op.ResultAccessor);
        }

        [Fact]
        public void Completes_with_async_exception_and_runs_finally_after_one_step_completes_async_then_throws_sync_exception_on_end()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowOnEndOfOneAsyncStepWithFinallyOperation op = new ThrowOnEndOfOneAsyncStepWithFinallyOperation(expected);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete();

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
            Assert.Equal(1, task.Exception.InnerExceptions.Count);
            Assert.Same(expected, task.Exception.InnerExceptions[0]);
            Assert.True(op.RanFinally);
        }

        [Fact]
        public void Completes_with_async_exception_and_runs_finally_after_one_step_completes_async_then_throws_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAfterAsyncStepWithFinallyOperation op = new ThrowAfterAsyncStepWithFinallyOperation(expected);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete();

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.NotNull(task.Exception);
            Assert.Equal(1, task.Exception.InnerExceptions.Count);
            Assert.Same(expected, task.Exception.InnerExceptions[0]);
            Assert.True(op.RanFinally);
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

        private sealed class ThrowOnEndOfOneStepOperation : AsyncOperation<int>
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
                    thisPtr => Step.TaskFromResult(1),
                    (thisPtr, r) => thisPtr.ThrowSync(r));
            }

            private void ThrowSync(int result)
            {
                if (result == 1)
                {
                    throw this.exception;
                }
            }
        }

        private sealed class ThrowDuringOneStepVoidOperation : AsyncOperation<int>
        {
            private readonly Exception exception;

            public ThrowDuringOneStepVoidOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(this, thisPtr => thisPtr.ThrowSync());
            }

            private Task<int> ThrowSync()
            {
                throw this.exception;
            }
        }

        private sealed class ThrowAsyncDuringOneStepVoidOperation : AsyncOperation<int>
        {
            private readonly Exception exception;

            public ThrowAsyncDuringOneStepVoidOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(this.exception, e => Step.TaskFromException(e));
            }
        }

        private sealed class ThrowAsyncDuringOneStepOperation : AsyncOperation<int>
        {
            private readonly Exception exception;

            public ThrowAsyncDuringOneStepOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this.exception,
                    e => Step.TaskFromException<int>(e),
                    (e, r) => Invalid());
            }

            private static void Invalid()
            {
                throw new InvalidOperationException("This shouldn't happen.");
            }

            private void ThrowSync(int result)
            {
                if (result == 1)
                {
                    throw this.exception;
                }
            }
        }

        private sealed class OneAsyncStepOperation : AsyncOperation<int>
        {
            private readonly TaskCompletionSource<int> tcs;

            public OneAsyncStepOperation()
            {
                this.tcs = new TaskCompletionSource<int>();
            }

            public void Complete(int result)
            {
                this.tcs.SetResult(result);
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => thisPtr.tcs.Task,
                    (thisPtr, r) => thisPtr.Result = r);
            }
        }

        private sealed class TwoAsyncStepOperation : AsyncOperation<int>
        {
            private TaskCompletionSource<bool> tcs;

            public TwoAsyncStepOperation()
            {
            }

            public void Complete(Exception exception)
            {
                if (exception != null)
                {
                    this.tcs.SetException(exception);
                }
                else
                {
                    this.tcs.SetResult(false);
                }
            }

            protected override IEnumerator<Step> Steps()
            {
                this.tcs = new TaskCompletionSource<bool>();
                yield return Step.Await(this, thisPtr => thisPtr.tcs.Task);
                
                this.tcs = new TaskCompletionSource<bool>();
                yield return Step.Await(this, thisPtr => thisPtr.tcs.Task);
            }
        }

        private sealed class ThrowAfterAsyncStepOperation : AsyncOperation<int>
        {
            private readonly Exception exception;
            private readonly TaskCompletionSource<bool> tcs;

            public ThrowAfterAsyncStepOperation(Exception exception)
            {
                this.exception = exception;
                this.tcs = new TaskCompletionSource<bool>();
            }

            public void Complete()
            {
                this.tcs.SetResult(false);
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(this, thisPtr => thisPtr.tcs.Task);
                throw this.exception;
            }
        }

        private sealed class ThrowDuringAsyncStepWithFinallyOperation : AsyncOperation<int>
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
                    yield return Step.Await(this, thisPtr => thisPtr.Throw());
                }
                finally
                {
                    this.RanFinally = true;
                }
            }

            private Task Throw()
            {
                throw this.exception;
            }
        }

        private sealed class OneAsyncStepWithFinallyOperation : AsyncOperation<int>
        {
            private readonly int result;
            private readonly TaskCompletionSource<bool> tcs;

            public OneAsyncStepWithFinallyOperation(int result)
            {
                this.result = result;
                this.tcs = new TaskCompletionSource<bool>();
            }

            public int ResultAccessor
            {
                get { return this.Result; }
            }

            public void Complete(Exception exception)
            {
                if (exception == null)
                {
                    this.tcs.SetResult(false);
                }
                else
                {
                    this.tcs.SetException(exception);
                }
            }

            protected override IEnumerator<Step> Steps()
            {
                try
                {
                    yield return Step.Await(this.tcs.Task, t => t);
                }
                finally
                {
                    this.Result = this.result;
                }
            }
        }

        private sealed class ThrowOnEndOfOneAsyncStepWithFinallyOperation : AsyncOperation<int>
        {
            private readonly Exception exception;
            private readonly TaskCompletionSource<int> tcs;

            public ThrowOnEndOfOneAsyncStepWithFinallyOperation(Exception exception)
            {
                this.exception = exception;
                this.tcs = new TaskCompletionSource<int>();
            }

            public bool RanFinally { get; private set; }

            public void Complete()
            {
                this.tcs.SetResult(1);
            }

            protected override IEnumerator<Step> Steps()
            {
                try
                {
                    yield return Step.Await(this, thisPtr => thisPtr.tcs.Task, (thisPtr, r) => thisPtr.Throw(r));
                }
                finally
                {
                    this.RanFinally = true;
                }
            }

            private void Throw(int result)
            {
                if (result == 1)
                {
                    throw this.exception;
                }
            }
        }

        private sealed class ThrowAfterAsyncStepWithFinallyOperation : AsyncOperation<int>
        {
            private readonly Exception exception;
            private readonly TaskCompletionSource<bool> tcs;

            public ThrowAfterAsyncStepWithFinallyOperation(Exception exception)
            {
                this.exception = exception;
                this.tcs = new TaskCompletionSource<bool>();
            }

            public bool RanFinally { get; private set; }

            public void Complete()
            {
                this.tcs.SetResult(false);
            }

            protected override IEnumerator<Step> Steps()
            {
                try
                {
                    yield return Step.Await(this, thisPtr => thisPtr.tcs.Task);
                    throw this.exception;
                }
                finally
                {
                    this.RanFinally = true;
                }
            }
        }
    }
}
