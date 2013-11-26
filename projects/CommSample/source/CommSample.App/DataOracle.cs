//-----------------------------------------------------------------------
// <copyright file="DataOracle.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    internal sealed class DataOracle
    {
        private readonly Dictionary<byte, int> patterns;

        public DataOracle()
        {
            this.patterns = new Dictionary<byte, int>();
        }

        public void AddPattern(byte fill, int length)
        {
            this.patterns.Add(fill, length);
        }

        public void VerifyLastSeen(byte lastSeen, int lastCount)
        {
            int expectedCountMultiple;
            if (!this.patterns.TryGetValue(lastSeen, out expectedCountMultiple))
            {
                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    "State corruption detected; byte 0x{0:X} was unexpected.",
                    lastSeen);
                throw new InvalidOperationException(message);
            }

            if (lastCount % expectedCountMultiple != 0)
            {
                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    "State corruption detected; count of {0} for byte 0x{1:X} is not a multiple of {2}.",
                    lastCount,
                    lastSeen,
                    expectedCountMultiple);
                throw new InvalidOperationException(message);
            }
        }
    }
}
