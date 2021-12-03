using System;

namespace JCED.Unpacker
{
    class DatHeader
    {
        public Int32 dwTotalFiles { get; set; }
        public UInt32 dwTableOffset { get; set; }
    }
}
