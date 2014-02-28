//-----------------------------------------------------------------------
// <copyright file="RetryContext.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample
{
    using System;
    using System.Collections.Generic;

    public class RetryContext
    {
        private readonly Dictionary<string, object> data;

        public RetryContext()
        {
            this.data = new Dictionary<string, object>();
        }

        public int Iteration { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public bool Succeeded { get; set; }

        public AggregateException Exception { get; set; }

        public void Add(string name, object value)
        {
            this.data[name] = value;
        }

        public TValue Get<TValue>(string name)
        {
            TValue value = default(TValue);
            object objValue;
            if (this.data.TryGetValue(name, out objValue))
            {
                value = (TValue)objValue;
            }

            return value;
        }
    }
}
