using System;

namespace JCED.Unpacker
{
    class DatEntry
    {
        public UInt32 dwHash { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwChunksCount { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwDecompressedSize { get; set; } // Max chunk size 0x8000 (32768)
    }
}
