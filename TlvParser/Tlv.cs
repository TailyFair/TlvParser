using System;

namespace TlvParser
{
    public class Tlv
    {
        private TlvType type;
        private int identifier;
        private byte[] value;
    }

    public enum TlvType
    {
        OBJECT_INSTANCE, // 00
        RESOURCE_INSTANCE, // 01
        MULTIPLE_RESOURCE, // 10
        RESOURCE_VALUE // 11
    }
}