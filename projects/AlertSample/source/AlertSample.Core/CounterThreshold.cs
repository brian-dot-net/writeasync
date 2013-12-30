//-----------------------------------------------------------------------
// <copyright file="CounterThreshold.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    public class CounterThreshold
    {
        public CounterThreshold()
        {
        }

        public CounterName Name { get; set; }

        public ThresholdCondition Condition { get; set; }

        public double Value { get; set; }

        public override string ToString()
        {
            string condition = null;
            switch (this.Condition)
            {
                case ThresholdCondition.Above:
                    condition = ">";
                    break;
                case ThresholdCondition.Below:
                    condition = "<";
                    break;
            }

            return this.Name + condition + this.Value;
        }
    }
}
