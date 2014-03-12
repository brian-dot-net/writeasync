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
    }
}
