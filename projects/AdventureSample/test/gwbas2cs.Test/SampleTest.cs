namespace gwbas2cs.Test
{
    using FluentAssertions;
    using Xunit;

    public class SampleTest
    {
        [Fact]
        public void ReturnsNameForToString()
        {
            new Sample("my name").ToString().Should().Be("my name");
        }
    }
}
