//-----------------------------------------------------------------------
// <copyright file="Test1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CleanupSample.Test.Unit
{
    using System.Threading.Tasks;
    using Xunit;

    public class Test1
    {
        public Test1()
        {
        }

        [Fact]
        public void Should_return_name_after_completing_sync()
        {
            Class1 c = new Class1("MyName");
            
            Task<string> task = c.DoAsync();

            Assert.True(task.IsCompleted);
            Assert.Equal("MyName", task.Result);
        }
    }
}
