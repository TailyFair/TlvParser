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
            using (BinaryReader reader = new BinaryReader(stream))
            {
                while ((stream.Length - stream.Position) > 0)
                {
                    reader.ReadByte();
                }
            }

            return tlvs.ToArray();
        }
    }
}