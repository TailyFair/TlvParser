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

        [Fact]
        public void TestResourceValue()
        {
            // Table 7.4.3.1-1 example. First Tlv
            Tlv[] result = _parser.Parse(Utilities.StringToByteArray("C800144F70656E204D6F62696C6520416C6C69616E6365"));

            Tlv expected = new Tlv(TlvType.RESOURCE_VALUE, 0x00, null, Utilities.StringToByteArray("4F70656E204D6F62696C6520416C6C69616E6365"));

            Assert.Equal(expected, result[0]);
        }

        [Fact]
        public void TestMultipleResource()
        {
            // Table 7.4.3.2-1 example. Taken from middle section
            Tlv[] result = _parser.Parse(Utilities.StringToByteArray("88070842000ED842011388"));

            Tlv expected = new Tlv(TlvType.MULTIPLE_RESOURCE, 0x07,
                new Tlv[] {
                    new Tlv(TlvType.RESOURCE_INSTANCE, 0x00, null, Utilities.StringToByteArray("0ED8")),
                    new Tlv(TlvType.RESOURCE_INSTANCE, 0x01, null, Utilities.StringToByteArray("1388")),
                }, null);

            Assert.Equal(expected, result[0]);
        }

        [Fact]
        public void TestObjectInstance()
        {
            // Table 7.4.3.2-3 example. With object children length fixed (0x0D -> 0x0F)
            Tlv[] result = _parser.Parse(Utilities.StringToByteArray("08000FC10001C40100015180C10601C10755"));

            Tlv expected = new Tlv(TlvType.OBJECT_INSTANCE, 0x00,
                new Tlv[] {
                    new Tlv(TlvType.RESOURCE_VALUE, 0x00, null, Utilities.StringToByteArray("01")),
                    new Tlv(TlvType.RESOURCE_VALUE, 0x01, null, Utilities.StringToByteArray("00015180")),
                    new Tlv(TlvType.RESOURCE_VALUE, 0x06, null, Utilities.StringToByteArray("01")),
                    new Tlv(TlvType.RESOURCE_VALUE, 0x07, null, Utilities.StringToByteArray("55")),
                }, null);

            Assert.Equal(expected, result[0]);
        }

        [Fact]
        public void TestObjectInstance2()
        {
            // Table 7.4.3.2-2 example. First object instance
            Tlv[] result = _parser.Parse(Utilities.StringToByteArray("08000EC10001C101008302417F07C1037F"));

            Tlv expected = new Tlv(TlvType.OBJECT_INSTANCE, 0x00,
                new Tlv[] {
                    new Tlv(TlvType.RESOURCE_VALUE, 0x00, null, Utilities.StringToByteArray("01")),
                    new Tlv(TlvType.RESOURCE_VALUE, 0x01, null, Utilities.StringToByteArray("00")),
                    new Tlv(TlvType.MULTIPLE_RESOURCE, 0x02, new Tlv[] {
                        new Tlv(TlvType.RESOURCE_INSTANCE, 0x7F, null, Utilities.StringToByteArray("07"))
                    }, null),
                    new Tlv(TlvType.RESOURCE_VALUE, 0x03, null, Utilities.StringToByteArray("7F"))
                }, null);

            Assert.Equal(expected, result[0]);
        }

        [Theory]
        [InlineData(TlvType.RESOURCE_VALUE, 0x00, null, null)]
        [InlineData(TlvType.RESOURCE_INSTANCE, 0x00, null, null)]
        [InlineData(TlvType.OBJECT_INSTANCE, 0x00, null, new byte[] { 0x01 })]
        public void TestTlvConstructorException(TlvType type, int identifier, Tlv[] children, byte[] value)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Tlv(type, identifier, children, value);
            });
        }
    }
}
