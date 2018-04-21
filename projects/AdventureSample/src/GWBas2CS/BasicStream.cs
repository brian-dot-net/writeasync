// <copyright file="BasicStream.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS
{
    using System.IO;
    using System.Threading.Tasks;
    using GWParse.Lines;

    public static class BasicStream
    {
        public static Task<BasicLine[]> ReadAsync(Stream input)
        {
            using (input)
            {
                return Task.FromResult(new BasicLine[0]);
            }
        }
    }
}
