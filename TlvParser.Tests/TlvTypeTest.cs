using System;
using Xunit;

namespace TlvParser.Tests
{
    public class TlvTypeTest
    {
        private readonly Parser _parser;

        public TlvTypeTest()
        {
            _parser = new Parser();
        }

        [Theory]
        [InlineData(0b0001_0000, TlvType.OBJECT_INSTANCE)]
        [InlineData(0b0100_0100, TlvType.RESOURCE_INSTANCE)]
        [InlineData(0b1010_1010, TlvType.MULTIPLE_RESOURCE)]
        [InlineData(0b1110_0110, TlvType.RESOURCE_VALUE)]
        public void Testl(byte input, TlvType expected)
        {
            TlvType result = _parser.ParseType(input);

            Assert.Equal(expected, result);
        }
    }
}
