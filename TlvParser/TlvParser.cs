using System;
using System.IO;
using System.Collections.Generic;

namespace TlvParser
{
    public class Parser
    {
        /// <summary>
        /// Parse Tlv packets from byte array
        /// </summary>
        public Tlv[] Parse(byte[] input_bytes)
        {
            List<Tlv> tlvs = new List<Tlv>();

            using (MemoryStream stream = new MemoryStream(input_bytes))
            using (ReversedBinaryReader reader = new ReversedBinaryReader(stream))
            {
                while ((stream.Length - stream.Position) > 0)
                {
                    byte typeByte = reader.ReadByte();

                    TlvType type = ParseType(typeByte);
                    int identifier = ParseIdentifier(reader, typeByte);
                    int length = ParseLength(reader, typeByte);

                    Tlv tlv = CreateTlv(reader, type, identifier, length);

                    tlvs.Add(tlv);
                }
            }

            return tlvs.ToArray();
        }

        public TlvType ParseType(byte input)
        {
            TlvType type;
            byte mask = 0b1100_0000; // Mask to check Bits 7-6

            switch (input & mask)
            {
                case 0b0000_0000:
                    type = TlvType.OBJECT_INSTANCE;
                    break;
                case 0b0100_0000:
                    type = TlvType.RESOURCE_INSTANCE;
                    break;
                case 0b1000_0000:
                    type = TlvType.MULTIPLE_RESOURCE;
                    break;
                case 0b1100_0000:
                    type = TlvType.RESOURCE_VALUE;
                    break;
                default:
                    throw new Exception("Unkown type");
            }

            return type;
        }

        public int ParseIdentifier(BinaryReader reader, byte input)
        {
            int identifier;
            byte mask = 0b0010_0000; // Mask to check Bit 5

            switch (input & mask)
            {
                case 0b0000_0000:
                    identifier = reader.ReadByte();
                    break;
                case 0b0010_0000:
                    identifier = reader.ReadUInt16();
                    break;
                default:
                    throw new Exception("Unkown identifier");
            }

            return identifier;
        }

        public int ParseLength(BinaryReader reader, byte input)
        {
            int length;
            byte mask = 0b0001_1000; // Mask to check Bits 4-3

            switch (input & mask)
            {
                case 0b0000_0000:
                    length = input & 0b0000_0111; // Select Bits 2-0 as length
                    break;
                case 0b0000_1000:
                    length = reader.ReadByte();
                    break;
                case 0b0001_0000:
                    length = reader.ReadUInt16();
                    break;
                case 0b0001_10000: // Read 24 bits unsinged integer
                    byte b = reader.ReadByte();
                    ushort s = reader.ReadUInt16();
                    length = (b << 16) | s;
                    break;
                default:
                    throw new Exception("Unknown length");
            }

            return length;
        }

        public Tlv CreateTlv(BinaryReader reader, TlvType type, int identifier, int length)
        {
            Tlv result;

            byte[] value = reader.ReadBytes(length);

            if (type == TlvType.OBJECT_INSTANCE || type == TlvType.MULTIPLE_RESOURCE)
            {
                Tlv[] children = Parse(value);

                result = new Tlv(type, identifier, children, null);
            }
            else
            {
                result = new Tlv(type, identifier, null, value);
            }

            return result;
        }

    }
}