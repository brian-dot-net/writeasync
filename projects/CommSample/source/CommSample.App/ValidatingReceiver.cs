//-----------------------------------------------------------------------
// <copyright file="ValidatingReceiver.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Threading.Tasks;

    internal sealed class ValidatingReceiver
    {
        private readonly Receiver receiver;
        private readonly DataOracle oracle;

        private byte lastSeen;
        private int lastCount;

        public ValidatingReceiver(MemoryChannel channel, Logger logger, int bufferSize, DataOracle oracle)
        {
            this.receiver = new Receiver(channel, logger, bufferSize);
            this.oracle = oracle;
            this.receiver.DataReceived += this.OnDataReceived;
        }

        public Task<long> RunAsync()
        {
            return this.receiver.RunAsync();
        }

        private void OnDataReceived(object sender, DataEventArgs e)
        {
            for (int i = 0; i < e.BytesRead; ++i)
            {
                if (this.lastSeen != e.Buffer[i])
                {
                    if (this.lastSeen != 0)
                    {
                        this.oracle.VerifyLastSeen(this.lastSeen, this.lastCount);
                    }

                    this.lastSeen = e.Buffer[i];
                    this.lastCount = 0;
                }

                ++this.lastCount;
            }
        }
    }
}
