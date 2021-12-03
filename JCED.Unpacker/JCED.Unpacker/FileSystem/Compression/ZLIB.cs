using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace JCED.Unpacker
{
    class ZLIB
    {
        static List<DatChunk> m_Chunks = new List<DatChunk>();

        public static Byte[] iDecompress(Byte[] lpBuffer)
        {
            var TOutMemoryStream = new MemoryStream();
            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer) { Position = 2 })
            {
                using (DeflateStream TDeflateStream = new DeflateStream(TMemoryStream, CompressionMode.Decompress, false))
                {
                    TDeflateStream.CopyTo(TOutMemoryStream);
                    TDeflateStream.Dispose();
                }
                TMemoryStream.Dispose();
            }

            return TOutMemoryStream.ToArray();
        }

        public static Byte[] iDecompressChunks(FileStream TDatStream, DatEntry TEntry)
        {
            Byte[] lpDstBuffer = new Byte[TEntry.dwDecompressedSize];

            TDatStream.Seek(TEntry.dwOffset, SeekOrigin.Begin);

            m_Chunks.Clear();
            for (Int32 i = 0; i < TEntry.dwChunksCount; i++)
            {
                UInt32 dwOffset = TDatStream.ReadUInt32();
                Int32 dwCompressedSize = TDatStream.ReadInt32();

                var TChunk = new DatChunk
                {
                    dwOffset = dwOffset,
                    dwCompressedSize = dwCompressedSize,
                };

                m_Chunks.Add(TChunk);
            }

            Int32 dwTempPos = 0;
            foreach (var m_Chunk in m_Chunks)
            {
                TDatStream.Seek(m_Chunk.dwOffset, SeekOrigin.Begin);

                var lpChunkBuffer = TDatStream.ReadBytes(m_Chunk.dwCompressedSize);
                var lpTemp = iDecompress(lpChunkBuffer);

                Array.Copy(lpTemp, 0, lpDstBuffer, dwTempPos, lpTemp.Length);
                dwTempPos += lpTemp.Length;
            }

            return lpDstBuffer;
        }
    }
}