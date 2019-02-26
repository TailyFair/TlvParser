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
                    reader.ReadByte();
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
    }
}