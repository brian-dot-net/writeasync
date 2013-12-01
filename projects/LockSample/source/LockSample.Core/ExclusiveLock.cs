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
        private readonly LockState state;

        private Token owner;
        private TaskCompletionSource<Token> nextOwner;

        public ExclusiveLock()
        {
            this.state = new LockState();
        }

        public Task<Token> AcquireAsync()
        {
            if (this.owner.State == null)
            {
                this.owner = new Token(this.state);
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
            if (token.State != this.state)
            {
                throw new InvalidOperationException("The token is invalid.");
            }

            if (this.nextOwner != null)
            {
                this.owner = new Token(this.state);
                this.nextOwner.SetResult(this.owner);
            }
        }

        public struct Token
        {
            private readonly object state;

            public Token(object state)
            {
                this.state = state;
            }

            public object State
            {
                get { return this.state; }
            }
        }

        private sealed class LockState
        {
            public LockState()
            {
            }
        }
    }
}
