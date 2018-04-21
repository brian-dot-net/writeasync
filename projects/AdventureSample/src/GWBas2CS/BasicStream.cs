// <copyright file="BasicStream.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using GWParse.Lines;

    public static class BasicStream
    {
        public static async Task<BasicLine[]> ReadAsync(Stream input)
        {
            List<BasicLine> lines = new List<BasicLine>();
            using (StreamReader reader = new StreamReader(input))
            {
                string line = await reader.ReadLineAsync();
                if (line != null)
                {
                    lines.Add(BasicLine.FromString(line));
                }
            }

            return lines.ToArray();
        }
    }
}
