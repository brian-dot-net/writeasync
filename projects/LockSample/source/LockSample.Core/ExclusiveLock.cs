//-----------------------------------------------------------------------
// <copyright file="ExclusiveLock.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace LockSample
{
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
            if (this.owner.State == null)
            {
                this.owner = new Token(new object());
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
            if (this.nextOwner != null)
            {
                this.owner = new Token(new object());
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
    }
}
