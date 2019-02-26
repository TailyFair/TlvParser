using System;
using Xunit;
using System.IO;
using TlvParser;

namespace TlvParser.Tests
{
    public class TlvIdentifierTest
    {
        private readonly Parser _parser;

        public TlvIdentifierTest()
        {
            _parser = new Parser();
        }

        [Theory]
        [InlineData("C80014", 0)]
        [InlineData("61013601", 310)]
        public void Test1(string input, int expected)
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

    }
}
