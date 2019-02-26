using System;

namespace TlvParser
{
    public class Tlv
    {
        private TlvType type;
        private int identifier;
        Tlv[] children;
        private byte[] value;

        public Tlv(TlvType type, int identifier, Tlv[] children, byte[] value)
        {
            this.type = type;
            this.identifier = identifier;
            this.children = children;
            this.value = value;
        }
    }

    public enum TlvType
    {
        OBJECT_INSTANCE, // 00
        RESOURCE_INSTANCE, // 01
        MULTIPLE_RESOURCE, // 10
        RESOURCE_VALUE // 11
    }
}