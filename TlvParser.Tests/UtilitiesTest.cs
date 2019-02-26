using System;
using Xunit;
using TlvParser;

namespace TlvParser.Tests
{
    public class UtilitiesTest
    {
        [Fact]
        public void TestConvertStringToBytes()
        {
            string input = "C800144F";
            var expected = new byte[] { 200, 00, 20, 79 };

            var result = Utilities.StringToByteArray(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestConvertBytesToString()
        {
            var input = new byte[] { 200, 00, 20, 79 };
            string expected = "C800144F";

            var result = Utilities.ByteArrayToString(input);

            Assert.Equal(expected, result);
        }
    }
}
