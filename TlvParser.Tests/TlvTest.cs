using System;
using System.IO;
using Xunit;
using TlvParser;

namespace TlvParser.Tests
{
    public class TlvTest
    {
        private readonly Parser _parser;

        public TlvTest()
        {
            _parser = new Parser();
        }

        [Theory]
        [InlineData(0b0001_0000, TlvType.OBJECT_INSTANCE)]
        [InlineData(0b0100_0100, TlvType.RESOURCE_INSTANCE)]
        [InlineData(0b1010_1010, TlvType.MULTIPLE_RESOURCE)]
        [InlineData(0b1110_0110, TlvType.RESOURCE_VALUE)]
        public void TestTlvType(byte input, TlvType expected)
        {
            TlvType result = _parser.ParseType(input);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("C80014", 0)]
        [InlineData("61013601", 310)]
        public void TestTlvIdentifier(string input, int expected)
        {
            int result;

            using (MemoryStream stream = new MemoryStream(Utilities.StringToByteArray(input)))
            using (ReversedBinaryReader reader = new ReversedBinaryReader(stream))
            {
                byte typeByte = reader.ReadByte();
                result = _parser.ParseIdentifier(reader, typeByte);
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("C80209333435303030313233", 9)]
        [InlineData("C800144F70656E204D6F62696C6520416C6C69616E6365", 20)]
        public void TestTlvLength(string input, int expected)
        {
            int result;

            using (MemoryStream stream = new MemoryStream(Utilities.StringToByteArray(input)))
            using (ReversedBinaryReader reader = new ReversedBinaryReader(stream))
            {
                byte typeByte = reader.ReadByte();
                TlvType type = _parser.ParseType(typeByte);
                int identifier = _parser.ParseIdentifier(reader, typeByte);

                result = _parser.ParseLength(reader, typeByte);
            }

            Assert.Equal(expected, result);
        }
    }
}
