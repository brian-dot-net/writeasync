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
        public ExclusiveLock()
        {
        }

        public Task<Token> AcquireAsync()
        {
            return Task.FromResult(new Token());
        }

        public void Release(Token token)
        {
        }

        public struct Token
        {
        }
    }
}
