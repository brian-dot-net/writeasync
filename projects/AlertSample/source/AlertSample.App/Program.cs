//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Alert alert = new Alert("AlertSample", 5.0d, 10.0d);
            try
            {
                alert.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e);
                throw;
            }
            finally
            {
                alert.Stop();
            }
        }
    }
}
