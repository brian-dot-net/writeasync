//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnum35Sample
{
    using System;
    using System.Text;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            string path = "ExampleFile.txt";
            Random random = new Random();
            byte[] writtenBytes = new byte[1000000];
            for (int i = 0; i < writtenBytes.Length; ++i)
            {
                writtenBytes[i] = (byte)('A' + (i % 7));
            }

            AsyncFileWriter writer = new AsyncFileWriter(99999);
            IAsyncResult writeResult = writer.BeginWriteAllBytes(path, writtenBytes, null, null);
            writer.EndWriteAllBytes(writeResult);
            Console.WriteLine("Wrote {0} bytes.", writtenBytes.Length);

            AsyncFileReader reader = new AsyncFileReader(100001);
            IAsyncResult readResult = reader.BeginReadAllBytes(path, null, null);
            byte[] readBytes = reader.EndReadAllBytes(readResult);
            Console.WriteLine("Read {0} bytes.", readBytes.Length);
            Console.WriteLine("First 30 bytes: " + Encoding.ASCII.GetString(readBytes, 0, 30));
            Console.WriteLine("Last 30 bytes: " + Encoding.ASCII.GetString(readBytes, readBytes.Length - 30, 30));

            if (writtenBytes.Length != readBytes.Length)
            {
                throw new InvalidOperationException("Lengths do not match.");
            }

            for (int i = 0; i < readBytes.Length; ++i)
            {
                if (writtenBytes[i] != readBytes[i])
                {
                    throw new InvalidOperationException("Mismatching byte values at index " + i);
                }
            }

            Console.WriteLine("All byte values match!");
        }
    }
}
