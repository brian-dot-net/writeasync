// <copyright file="AlgorithmTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4.Test
{
    using System;
    using System.IO;
    using System.Text;
    using FluentAssertions;
    using Xunit;

    public sealed class AlgorithmTest
    {
        [Fact]
        public void RunWithDefaultValues()
        {
            string[] output = Run();

            output.Should().HaveCount(103);
            output[0].Should().Match("Solving 4 4s (min=1, max=100)...");
            output[1].Should().Match("Found 100 results in * ms.");
            output[2].Should().Be("1 = ((44-44))!");
            output[101].Should().Be("100 = ((44-4)/.4)");
            output[102].Should().BeEmpty();
        }

        private static string[] Run()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                new Algorithm().Run(writer);
            }

            return sb.ToString().Split(Environment.NewLine);
        }
    }
}
