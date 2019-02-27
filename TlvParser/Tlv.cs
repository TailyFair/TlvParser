using System;
using System.Linq;
using System.Text;

namespace TlvParser
{
    public class Tlv
    {
        public readonly TlvType type;
        public readonly int identifier;
        public readonly Tlv[] children;
        public readonly byte[] value;

        public Tlv(TlvType type, int identifier, Tlv[] children, byte[] value)
        {
            this.type = type;
            this.identifier = identifier;
            this.children = children;
            this.value = value;

            if (type == TlvType.OBJECT_INSTANCE || type == TlvType.MULTIPLE_RESOURCE)
            {
                if (children == null)
                    throw new ArgumentNullException($"{type} must have children");

                if (value != null)
                    throw new ArgumentNullException($"{type} can't have value");
            }
            else
            {
                if (children != null)
                    throw new ArgumentNullException($"{type} can't have children");

                if (value == null)
                    throw new ArgumentNullException($"{type} must have value");
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("TLV <type: ").Append(type);
            sb.Append(", ID: ").Append(identifier);

            sb.Append(", value: ");
            if (value != null)
                sb.Append(Utilities.ByteArrayToString(value));
            else
                sb.Append("null");

            sb.Append(", children: ");
            if (children != null)
                foreach (Tlv child in children)
                    sb.Append(child.ToString());
            else
                sb.Append("null");

            sb.Append(">");

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Tlv tlv = (Tlv)obj;

            if (type != tlv.type || identifier != tlv.identifier)
            {
                return false;
            }

            if (children != null)
            {
                if (children.Length != tlv.children.Length)
                    return false;

                if (!Enumerable.SequenceEqual(children, tlv.children))
                    return false;
            }

            if (value != null)
            {
                if (!value.SequenceEqual(tlv.value))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int prime = 47;
            int result = 23;
            result = prime * result + children.GetHashCode();
            result = prime * result + identifier;
            result = prime * result + type.GetHashCode();
            result = prime * result + value.GetHashCode();
            return result;
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