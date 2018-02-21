namespace gwbas2cs
{
    public sealed class Sample
    {
        private readonly string name;

        public Sample(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
