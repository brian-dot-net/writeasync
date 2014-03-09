//-----------------------------------------------------------------------
// <copyright file="AsyncOperationTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnumSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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

        [Fact]
        public void Race_with_completion_does_not_cause_infinite_stack_recursion()
        {
            RaceWithCompletionOperation op = new RaceWithCompletionOperation(1000);
            Task<int> task = op.Start();

            Assert.NotEqual(TaskStatus.Faulted, task.Status);
        }

        [Fact]
        public void Completes_async_after_legacy_result_completes_async()
        {
            OneLegacyAsyncStepOperation op = new OneLegacyAsyncStepOperation(1234);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete(null);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1234, task.Result);
        }

        [Fact]
        public void Throws_sync_after_legacy_result_throws_on_begin()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowOnBeginOperation op = new ThrowOnBeginOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throws_sync_after_legacy_result_completes_sync_and_throws_on_end()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowOnEndSyncOperation op = new ThrowOnEndSyncOperation(expected);
            AggregateException ae = Assert.Throws<AggregateException>(() => op.Start());

            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.Same(expected, ae.InnerExceptions[0]);
        }

        [Fact]
        public void Throws_async_after_legacy_result_completes_and_throws_on_end()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            OneLegacyAsyncStepOperation op = new OneLegacyAsyncStepOperation(-1);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete(expected);

            Assert.True(task.IsFaulted);
            Assert.NotNull(task.Exception);
            Assert.Equal(1, task.Exception.InnerExceptions.Count);
            Assert.Same(expected, task.Exception.InnerExceptions[0]);
        }

        [Fact]
        public void Completes_successfully_after_catching_and_handling_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowSyncAndHandleOperation op = new ThrowSyncAndHandleOperation(1234, expected);
            Task<int> task = op.Start();

            Assert.True(task.IsCompleted);
            Assert.Equal(1234, task.Result);
            Assert.Same(expected, op.CaughtException);
        }

        [Fact]
        public void Completes_successfully_after_catching_and_handling_async_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAsyncAndHandleOperation op = new ThrowAsyncAndHandleOperation(1234, expected);
            Task<int> task = op.Start();

            Assert.True(task.IsCompleted);
            Assert.Equal(1234, task.Result);
            Assert.Same(expected, op.CaughtException);
        }

        [Fact]
        public void Completes_successfully_but_skips_after_step_after_catching_and_handling_async_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAsyncWithResultAndHandleOperation op = new ThrowAsyncWithResultAndHandleOperation(1234, expected);
            Task<int> task = op.Start();

            Assert.True(task.IsCompleted);
            Assert.Equal(1234, task.Result);
            Assert.Same(expected, op.CaughtException);
        }

        [Fact]
        public void Completes_successfully_after_catching_and_handling_legacy_throw_on_begin()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowOnBeginAndHandleOperation op = new ThrowOnBeginAndHandleOperation(1234, expected);
            Task<int> task = op.Start();

            Assert.True(task.IsCompleted);
            Assert.Equal(1234, task.Result);
            Assert.Same(expected, op.CaughtException);
        }

        [Fact]
        public void Completes_successfully_after_catching_and_handling_legacy_sync_completion_throw_on_end()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowOnEndSyncAndHandleOperation op = new ThrowOnEndSyncAndHandleOperation(1234, expected);
            Task<int> task = op.Start();

            Assert.True(task.IsCompleted);
            Assert.Equal(1234, task.Result);
            Assert.Same(expected, op.CaughtException);
        }

        [Fact]
        public void Completes_successfully_after_catching_and_handling_legacy_async_completion_throw_on_end()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowOnEndAsyncAndHandleOperation op = new ThrowOnEndAsyncAndHandleOperation(1234, expected);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete();

            Assert.True(task.IsCompleted);
            Assert.Equal(1234, task.Result);
            Assert.Same(expected, op.CaughtException);
        }

        [Fact]
        public void Completes_successfully_but_skips_after_step_after_catching_and_handling_deferred_async_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowAsyncDeferredAndHandleOperation op = new ThrowAsyncDeferredAndHandleOperation(1234, expected);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete();

            Assert.True(task.IsCompleted);
            Assert.Equal(1234, task.Result);
            Assert.Same(expected, op.CaughtException);
        }

        [Fact]
        public void Throws_sync_no_matching_exception_handler_on_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            NoMatchingHandlerSyncOperation op = new NoMatchingHandlerSyncOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Completes_successfully_on_matching_exception_handler_on_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            SecondMatchingHandlerSyncOperation op = new SecondMatchingHandlerSyncOperation(1234, expected);
            Task<int> task = op.Start();

            Assert.True(task.IsCompleted);
            Assert.Equal(1234, task.Result);
            Assert.Same(expected, op.CaughtException);
        }

        [Fact]
        public void Throws_sync_all_handlers_return_false_on_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ReturnFalseHandlersSyncOperation op = new ReturnFalseHandlersSyncOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throws_sync_on_throw_from_handler_on_sync_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowFromHandlerSyncOperation op = new ThrowFromHandlerSyncOperation(expected);
            InvalidTimeZoneException actual = Assert.Throws<InvalidTimeZoneException>(() => op.Start());

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Throws_async_on_throw_from_handler_on_async_exception()
        {
            InvalidTimeZoneException expected = new InvalidTimeZoneException("Expected.");
            ThrowFromHandlerAsyncOperation op = new ThrowFromHandlerAsyncOperation(expected);
            Task<int> task = op.Start();

            Assert.False(task.IsCompleted);

            op.Complete();

            Assert.True(task.IsFaulted);
            Assert.NotNull(task.Exception);
            Assert.Equal(1, task.Exception.InnerExceptions.Count);
            Assert.Same(expected, task.Exception.InnerExceptions[0]);
        }

        private static class Legacy
        {
            public static AsyncResult BeginOp(AsyncCallback callback, object state)
            {
                return new AsyncResult(callback, state);
            }

            public static void EndOp(IAsyncResult result)
            {
                ((AsyncResult)result).EndInvoke();
            }
        }

        private abstract class TestAsyncOperation : AsyncOperation<int>
        {
            protected TestAsyncOperation()
            {
                this.RunMoveNextSynchronously = true;
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
                yield return Step.Await(false, r => Step.TaskFromResult(r));
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
                yield return Step.Await(false, r => Step.TaskFromResult(r));
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
                    thisPtr => Step.TaskFromResult(thisPtr.result),
                    (thisPtr, r) => thisPtr.Result = r);
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

        private sealed class ThrowDuringOneStepVoidOperation : TestAsyncOperation
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

        private sealed class ThrowAsyncDuringOneStepVoidOperation : TestAsyncOperation
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

        private sealed class ThrowAsyncDuringOneStepOperation : TestAsyncOperation
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

        private sealed class OneAsyncStepOperation : TestAsyncOperation
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

        private sealed class TwoAsyncStepOperation : TestAsyncOperation
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

        private sealed class ThrowAfterAsyncStepOperation : TestAsyncOperation
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

        private sealed class OneAsyncStepWithFinallyOperation : TestAsyncOperation
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

        private sealed class ThrowOnEndOfOneAsyncStepWithFinallyOperation : TestAsyncOperation
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

        private sealed class ThrowAfterAsyncStepWithFinallyOperation : TestAsyncOperation
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

        private sealed class RaceWithCompletionOperation : AsyncOperation<int>
        {
            private readonly int iterationCount;

            private TaskCompletionSource<bool> tcs;

            public RaceWithCompletionOperation(int iterationCount)
            {
                this.iterationCount = iterationCount;
                this.SchedulingMoveNext += (o, e) => this.Complete();
            }

            protected override IEnumerator<Step> Steps()
            {
                for (int i = 0; i < this.iterationCount; ++i)
                {
                    this.tcs = new TaskCompletionSource<bool>();
                    yield return Step.Await(this.tcs.Task, t => t);
                }
            }

            private void Complete()
            {
                StackTrace stackTrace = new StackTrace(false);
                if (stackTrace.FrameCount > 100)
                {
                    throw new InvalidOperationException("Stack too deep!");
                }

                this.tcs.SetResult(false);
            }
        }

        private sealed class OneLegacyAsyncStepOperation : TestAsyncOperation
        {
            private readonly int result;

            private AsyncResult asyncResult;

            public OneLegacyAsyncStepOperation(int result)
            {
                this.result = result;
            }

            public void Complete(Exception exception)
            {
                this.asyncResult.SetAsCompleted(exception, false);
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => thisPtr.asyncResult = Legacy.BeginOp(c, s),
                    (thisPtr, r) => Legacy.EndOp(r));
                this.Result = this.result;
            }
        }

        private sealed class ThrowOnBeginOperation : TestAsyncOperation
        {
            private readonly Exception exception;

            public ThrowOnBeginOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => thisPtr.Throw(),
                    (thisPtr, r) => Legacy.EndOp(r));
            }

            private IAsyncResult Throw()
            {
                throw this.exception;
            }
        }

        private sealed class ThrowOnEndSyncOperation : TestAsyncOperation
        {
            private readonly Exception exception;

            public ThrowOnEndSyncOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => thisPtr.Begin(c, s),
                    (thisPtr, r) => Legacy.EndOp(r));
            }

            private IAsyncResult Begin(AsyncCallback callback, object state)
            {
                AsyncResult result = new AsyncResult(callback, state);
                result.SetAsCompleted(this.exception, true);
                return result;
            }
        }

        private sealed class ThrowSyncAndHandleOperation : TestAsyncOperation
        {
            private readonly int result;
            private readonly Exception exception;

            public ThrowSyncAndHandleOperation(int result, Exception exception)
            {
                this.result = result;
                this.exception = exception;
            }

            public Exception CaughtException { get; private set; }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this, 
                    thisPtr => thisPtr.Throw(),
                    Catch<Exception>.AndHandle(this, (thisPtr, e) => thisPtr.Handle(e)));
                this.Result = this.result;
            }

            private Task Throw()
            {
                throw this.exception;
            }

            private bool Handle(Exception caughtException)
            {
                this.CaughtException = caughtException;
                return true;
            }
        }

        private sealed class ThrowAsyncAndHandleOperation : TestAsyncOperation
        {
            private readonly int result;
            private readonly Exception exception;

            public ThrowAsyncAndHandleOperation(int result, Exception exception)
            {
                this.result = result;
                this.exception = exception;
            }

            public Exception CaughtException { get; private set; }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => Step.TaskFromException(thisPtr.exception),
                    Catch<Exception>.AndHandle(this, (thisPtr, e) => thisPtr.Handle(e)));
                this.Result = this.result;
            }

            private Task Throw()
            {
                throw this.exception;
            }

            private bool Handle(Exception caughtException)
            {
                this.CaughtException = caughtException;
                return true;
            }
        }

        private sealed class ThrowAsyncWithResultAndHandleOperation : TestAsyncOperation
        {
            private readonly int result;
            private readonly Exception exception;

            public ThrowAsyncWithResultAndHandleOperation(int result, Exception exception)
            {
                this.result = result;
                this.exception = exception;
            }

            public Exception CaughtException { get; private set; }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => Step.TaskFromException<int>(thisPtr.exception),
                    (thisPtr, r) => Invalid(),
                    Catch<Exception>.AndHandle(this, (thisPtr, e) => thisPtr.Handle(e)));
                this.Result = this.result;
            }

            private static void Invalid()
            {
                throw new InvalidOperationException("This shouldn't happen.");
            }

            private Task Throw()
            {
                throw this.exception;
            }

            private bool Handle(Exception caughtException)
            {
                this.CaughtException = caughtException;
                return true;
            }
        }

        private sealed class ThrowOnBeginAndHandleOperation : TestAsyncOperation
        {
            private readonly int result;
            private readonly Exception exception;

            public ThrowOnBeginAndHandleOperation(int result, Exception exception)
            {
                this.result = result;
                this.exception = exception;
            }

            public Exception CaughtException { get; private set; }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => thisPtr.Throw(),
                    (thisPtr, r) => Invalid(),
                    Catch<Exception>.AndHandle(this, (thisPtr, e) => thisPtr.Handle(e)));
                this.Result = this.result;
            }

            private static void Invalid()
            {
                throw new InvalidOperationException("This shouldn't happen.");
            }

            private IAsyncResult Throw()
            {
                throw this.exception;
            }

            private bool Handle(Exception caughtException)
            {
                this.CaughtException = caughtException;
                return true;
            }
        }

        private sealed class ThrowOnEndSyncAndHandleOperation : TestAsyncOperation
        {
            private readonly int result;
            private readonly Exception exception;

            public ThrowOnEndSyncAndHandleOperation(int result, Exception exception)
            {
                this.result = result;
                this.exception = exception;
            }

            public Exception CaughtException { get; private set; }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => thisPtr.Begin(c, s),
                    (thisPtr, r) => Legacy.EndOp(r),
                    Catch<Exception>.AndHandle(this, (thisPtr, e) => thisPtr.Handle(e)));
                this.Result = this.result;
            }

            private IAsyncResult Begin(AsyncCallback callback, object state)
            {
                AsyncResult result = new AsyncResult(callback, state);
                result.SetAsCompleted(this.exception, true);
                return result;
            }

            private bool Handle(Exception caughtException)
            {
                this.CaughtException = caughtException;
                return true;
            }
        }

        private sealed class ThrowOnEndAsyncAndHandleOperation : TestAsyncOperation
        {
            private readonly int result;
            private readonly Exception exception;

            private AsyncResult asyncResult;

            public ThrowOnEndAsyncAndHandleOperation(int result, Exception exception)
            {
                this.result = result;
                this.exception = exception;
            }

            public Exception CaughtException { get; private set; }

            public void Complete()
            {
                this.asyncResult.SetAsCompleted(this.exception, false);
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    (thisPtr, c, s) => this.asyncResult = Legacy.BeginOp(c, s),
                    (thisPtr, r) => Legacy.EndOp(r),
                    Catch<Exception>.AndHandle(this, (thisPtr, e) => thisPtr.Handle(e)));
                this.Result = this.result;
            }

            private bool Handle(Exception caughtException)
            {
                this.CaughtException = caughtException;
                return true;
            }
        }

        private sealed class ThrowAsyncDeferredAndHandleOperation : TestAsyncOperation
        {
            private readonly int result;
            private readonly Exception exception;
            private readonly TaskCompletionSource<bool> tcs;

            public ThrowAsyncDeferredAndHandleOperation(int result, Exception exception)
            {
                this.result = result;
                this.exception = exception;
                this.tcs = new TaskCompletionSource<bool>();
            }

            public Exception CaughtException { get; private set; }

            public void Complete()
            {
                this.tcs.SetException(this.exception);
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => thisPtr.tcs.Task,
                    (thisPtr, r) => Invalid(),
                    Catch<Exception>.AndHandle(this, (thisPtr, e) => thisPtr.Handle(e)));
                this.Result = this.result;
            }

            private static void Invalid()
            {
                throw new InvalidOperationException("This shouldn't happen.");
            }

            private Task Throw()
            {
                throw this.exception;
            }

            private bool Handle(Exception caughtException)
            {
                this.CaughtException = caughtException;
                return true;
            }
        }

        private sealed class NoMatchingHandlerSyncOperation : TestAsyncOperation
        {
            private readonly Exception exception;

            public NoMatchingHandlerSyncOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => thisPtr.Throw(),
                    Catch<ArgumentException>.AndHandle(this, (thisPtr, e) => true),
                    Catch<ArgumentNullException>.AndHandle(this, (thisPtr, e) => true));
            }

            private Task Throw()
            {
                throw this.exception;
            }
        }

        private sealed class SecondMatchingHandlerSyncOperation : TestAsyncOperation
        {
            private readonly int result;
            private readonly Exception exception;

            public SecondMatchingHandlerSyncOperation(int result, Exception exception)
            {
                this.result = result;
                this.exception = exception;
            }

            public Exception CaughtException { get; private set; }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => thisPtr.Throw(),
                    Catch<ArgumentException>.AndHandle(this, (thisPtr, e) => true),
                    Catch<InvalidTimeZoneException>.AndHandle(this, (thisPtr, e) => thisPtr.Handle(e)));
                this.Result = this.result;
            }

            private Task Throw()
            {
                throw this.exception;
            }

            private bool Handle(InvalidTimeZoneException e)
            {
                this.CaughtException = e;
                return true;
            }
        }

        private sealed class ReturnFalseHandlersSyncOperation : TestAsyncOperation
        {
            private readonly Exception exception;

            public ReturnFalseHandlersSyncOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => thisPtr.Throw(),
                    Catch<Exception>.AndHandle(this, (thisPtr, e) => false),
                    Catch<InvalidTimeZoneException>.AndHandle(this, (thisPtr, e) => false));
            }

            private Task Throw()
            {
                throw this.exception;
            }
        }

        private sealed class ThrowFromHandlerSyncOperation : TestAsyncOperation
        {
            private readonly Exception exception;

            public ThrowFromHandlerSyncOperation(Exception exception)
            {
                this.exception = exception;
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => Throw(),
                    Catch<ArgumentException>.AndHandle(this, (thisPtr, e) => thisPtr.ThrowFromHandler()));
            }

            private static Task Throw()
            {
                throw new ArgumentException("Shouldn't see this.");
            }

            private bool ThrowFromHandler()
            {
                throw this.exception;
            }
        }

        private sealed class ThrowFromHandlerAsyncOperation : TestAsyncOperation
        {
            private readonly Exception exception;
            private readonly TaskCompletionSource<bool> tcs;

            public ThrowFromHandlerAsyncOperation(Exception exception)
            {
                this.exception = exception;
                this.tcs = new TaskCompletionSource<bool>();
            }

            public void Complete()
            {
                this.tcs.SetException(new ArgumentException("Shouldn't see this."));
            }

            protected override IEnumerator<Step> Steps()
            {
                yield return Step.Await(
                    this,
                    thisPtr => thisPtr.tcs.Task,
                    Catch<ArgumentException>.AndHandle(this, (thisPtr, e) => thisPtr.ThrowFromHandler()));
            }

            private bool ThrowFromHandler()
            {
                throw this.exception;
            }
        }
    }
}
