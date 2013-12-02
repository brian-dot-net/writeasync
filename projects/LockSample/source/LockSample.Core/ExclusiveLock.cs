//-----------------------------------------------------------------------
// <copyright file="ExclusiveLock.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace LockSample
{
    using System;
    using System.Threading.Tasks;

    public class ExclusiveLock
    {
        private Token owner;
        private TaskCompletionSource<Token> nextOwner;

        public ExclusiveLock()
        {
        }

        public Task<Token> AcquireAsync()
        {
            if (this.owner == null)
            {
                this.owner = new OwnerToken();
            }
            else
            {
                this.nextOwner = new TaskCompletionSource<Token>();
            }

            Task<Token> task;
            if (this.nextOwner == null)
            {
                task = Task.FromResult(this.owner);
            }
            else
            {
                task = this.nextOwner.Task;
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
            if (this.nextOwner != null)
            {
                this.owner = new OwnerToken();
                this.nextOwner.SetResult(this.owner);
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
