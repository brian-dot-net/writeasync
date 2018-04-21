// <copyright file="Test.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;

    internal static class Test
    {
        public static string Translate(string name, string inputCode)
        {
            string outputCode;
            using (MemoryStream output = new MemoryStream())
            {
                Stream input = new MemoryStream(Encoding.UTF8.GetBytes(inputCode));

                Task task = BasicProgram.TranslateAsync(name, input, output);

                Exception exc = task.Exception;
                exc.Should().BeNull("{0}", exc);
                task.IsCompletedSuccessfully.Should().BeTrue();
                outputCode = Encoding.UTF8.GetString(output.ToArray());
            }

            return outputCode;
        }
    }
}
