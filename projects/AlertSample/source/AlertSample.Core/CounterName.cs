//-----------------------------------------------------------------------
// <copyright file="CounterName.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System.Collections.Generic;
    using System.Text;

    public class CounterName
    {
        public CounterName()
        {
        }

        public string Machine { get; set; }

        public string Category { get; set; }

        public string Counter { get; set; }

        public string Instance { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Machine))
            {
                sb.Append(this.Machine);
            }

            sb.Append('\\');
            sb.Append(this.Category);
            if (!string.IsNullOrEmpty(this.Instance))
            {
                sb.Append('(');
                sb.Append(this.Instance);
                sb.Append(')');
            }

            sb.Append('\\');
            sb.Append(this.Counter);

            return sb.ToString();
        }
    }
}
