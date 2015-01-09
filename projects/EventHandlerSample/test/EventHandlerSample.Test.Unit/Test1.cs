//-----------------------------------------------------------------------
// <copyright file="Test1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample.Test.Unit
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Test1
    {
        public Test1()
        {
        }

        [TestMethod]
        public void ShouldReturnNameAfterCompletingSync()
        {
            Class1 c = new Class1("MyName");
            
            Task<string> task = c.DoAsync();

            task.IsCompleted.Should().BeTrue();
            task.Result.Should().Be("MyName");
        }
    }
}
