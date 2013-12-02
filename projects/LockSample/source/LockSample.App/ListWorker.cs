//-----------------------------------------------------------------------
// <copyright file="ListWorker.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace LockSample
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    internal sealed class ListWorker
    {
        private readonly IList<int> list;

        public ListWorker(IList<int> list)
        {
            this.list = list;
        }

        public async Task RemoveAsync()
        {
            int n = this.list.Count;
            if (n > 0)
            {
                this.list.RemoveAt(n - 1);
                await Task.Yield();
                this.list.RemoveAt(n - 2);
            }
        }

        public async Task RemoveAllAsync()
        {
            while (this.list.Count > 0)
            {
                await this.RemoveAsync();
            }
        }

        public async Task AppendAsync()
        {
            int n = this.list.Count;
            this.list.Add(n + 1);
            await Task.Yield();
            this.list.Add(n + 2);
        }

        public async Task EnumerateAsync()
        {
            int lastItem = 0;
            foreach (int item in this.list)
            {
                if (lastItem != (item - 1))
                {
                    throw new InvalidOperationException(string.Format(
                        CultureInfo.InvariantCulture,
                        "State corruption detected; expected {0} but saw {1} in next list entry.",
                        lastItem + 1,
                        item));
                }

                lastItem = item;
                await Task.Yield();
            }
        }
    }
}