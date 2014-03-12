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
                yield return new Step();
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
                yield return new Step();
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
    }
}
