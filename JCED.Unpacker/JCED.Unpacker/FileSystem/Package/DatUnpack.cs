using System;
using System.IO;
using System.Collections.Generic;

namespace JCED.Unpacker
{
    class DatUnpack
    {
        static List<DatEntry> m_EntryTable = new List<DatEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            DatHashList.iLoadProject();
            using (FileStream TDatStream = File.OpenRead(m_Archive))
            {
                var lpHeader = TDatStream.ReadBytes(8);
                var m_Header = new DatHeader();

                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwTotalFiles = THeaderReader.ReadInt32();
                    m_Header.dwTableOffset = THeaderReader.ReadUInt32();

                    if (m_Header.dwTableOffset >= TDatStream.Length)
                    {
                        throw new Exception("[ERROR]: Invalid DAT archive!");
                    }

                    THeaderReader.Dispose();
                }

                TDatStream.Seek(m_Header.dwTableOffset, SeekOrigin.Begin);
                var lpTable = TDatStream.ReadBytes(m_Header.dwTotalFiles * 20);

                m_EntryTable.Clear();
                using (MemoryStream TEntryReader = new MemoryStream(lpTable))
                {
                    for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                    {
                        UInt32 dwHash = TEntryReader.ReadUInt32();
                        UInt32 dwOffset = TEntryReader.ReadUInt32();
                        Int32 dwChunksCount = TEntryReader.ReadInt32();
                        Int32 dwCompressedSize = TEntryReader.ReadInt32();
                        Int32 dwDecompressedSize = TEntryReader.ReadInt32();

                        var TEntry = new DatEntry
                        {
                            dwHash = dwHash,
                            dwOffset = dwOffset,
                            dwChunksCount = dwChunksCount,
                            dwCompressedSize = dwCompressedSize,
                            dwDecompressedSize = dwDecompressedSize,
                        };

                        m_EntryTable.Add(TEntry);
                    }

                    TEntryReader.Dispose();
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = DatHashList.iGetNameFromHashList(m_Entry.dwHash);
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    var lpBuffer = ZLIB.iDecompressChunks(TDatStream, m_Entry);

                    File.WriteAllBytes(m_FullPath, lpBuffer);
                }

                TDatStream.Dispose();
            }
        }
    }
}
