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
        private readonly Queue<TaskCompletionSource<Token>> nextOwners;
        private Token owner;

        public ExclusiveLock()
        {
            this.nextOwners = new Queue<TaskCompletionSource<Token>>();
        }

        public Task<Token> AcquireAsync()
        {
            Task<Token> task;
            if (this.owner == null)
            {
                this.owner = new OwnerToken();
                task = Task.FromResult(this.owner);
            }
            else
            {
                TaskCompletionSource<Token> nextOwner = new TaskCompletionSource<Token>();
                task = nextOwner.Task;
                this.nextOwners.Enqueue(nextOwner);
            }

            return task;
        }

        public void Release(Token token)
        {
            if (this.owner != token)
            {
                throw new InvalidOperationException("The token is not valid.");
            }

            this.owner = null;
            if (this.nextOwners.Count > 0)
            {
                TaskCompletionSource<Token> nextOwner = this.nextOwners.Dequeue();
                this.owner = new OwnerToken();
                nextOwner.SetResult(this.owner);
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
            public OwnerToken()
            {
            }
        }
    }
}
