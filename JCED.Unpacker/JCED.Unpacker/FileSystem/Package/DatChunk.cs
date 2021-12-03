using System;

namespace JCED.Unpacker
{
    class DatChunk
    {
        public UInt32 dwOffset { get; set; }
        public Int32 dwCompressedSize { get; set; }
    }
}
