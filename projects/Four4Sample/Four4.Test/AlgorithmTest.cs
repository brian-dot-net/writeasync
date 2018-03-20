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

        [Fact]
        public void RunWithOtherDigitAndDifferentMinMax()
        {
            string[] output = Run("3", "7", "11");

            output.Should().HaveCount(8);
            output[0].Should().Match("Solving 3 3s (min=7, max=11)...");
            output[1].Should().Match("Found 5 results in * ms.");
            output[2].Should().Be("7 = (((((3)!)!-((3)!)!))!+(3)!)");
            output[3].Should().Be("8 = (((3)!/3)^3)");
            output[4].Should().Be("9 = ((sqrt(((3)!+3)))!+3)");
            output[5].Should().Be("10 = (sqrt(((3)!+3))/.3)");
            output[6].Should().Be("11 = (33/3)");
        }

        [Fact]
        public void RunWithMoreThan100Results()
        {
            string[] output = Run("4", "2", "102");

            output.Should().HaveCount(104);
            output[0].Should().Match("Solving 4 4s (min=2, max=102)...");
            output[1].Should().Match("Found 101 results in * ms.");
            output[2].Should().Be("2 = ((44+4)/(4)!)");
            output[101].Should().Be("101 = ((44/.4_)+sqrt(4))");
            output[102].Should().Be("102 = ((44+(4)!)/sqrt(.4_))");
        }

        [Fact]
        public void AllowHigherNumberOfDigits()
        {
            string[] output = Run("3", "333", "333");

            output[0].Should().Match("Solving 3 3s (min=333, max=333)...");
            output[1].Should().Match("Found 1 results in * ms.");
            output[2].Should().Be("333 = 333");
        }

        private static string[] Run(params string[] args)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                new Algorithm(args).Run(writer);
            }

            return sb.ToString().Split(Environment.NewLine);
        }
    }
}
