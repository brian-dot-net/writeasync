//-----------------------------------------------------------------------
// <copyright file="ExclusiveLock.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace LockSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ExclusiveLock
    {
        private readonly Queue<OwnerToken> nextOwners;
        private OwnerToken owner;

        public ExclusiveLock()
        {
            this.nextOwners = new Queue<OwnerToken>();
        }

        public Task<Token> AcquireAsync()
        {
            OwnerToken nextOwner = new OwnerToken();
            Task<Token> task = nextOwner.Task;
            lock (this.nextOwners)
            {
                if (this.owner == null)
                {
                    this.owner = nextOwner;
                    nextOwner.Complete();
                }
                else
                {
                    this.nextOwners.Enqueue(nextOwner);
                }
            }

            return task;
        }

        public void Release(Token token)
        {
            OwnerToken nextOwner = null;
            lock (this.nextOwners)
            {
                if (this.owner != token)
                {
                    throw new InvalidOperationException("The token is not valid.");
                }

                this.owner = null;
                if (this.nextOwners.Count > 0)
                {
                    nextOwner = this.nextOwners.Dequeue();
                    this.owner = nextOwner;
                }
            }

            if (nextOwner != null)
            {
                nextOwner.Complete();
            }
        }

        public abstract class Token
        {
            protected Token()
            {
            }
        }

        private sealed class OwnerToken : Token
        {
            private readonly TaskCompletionSource<Token> tcs;

            public OwnerToken()
            {
                this.tcs = new TaskCompletionSource<Token>();
            }

            public Task<Token> Task
            {
                get { return this.tcs.Task; }
            }

            public void Complete()
            {
                this.tcs.SetResult(this);
            }
        }
    }
}
