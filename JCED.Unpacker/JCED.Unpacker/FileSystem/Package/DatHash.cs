using System;

namespace JCED.Unpacker
{
    class DatHash
    {
        public static UInt32 iGetHash(String m_String)
        {
            Int32 i = 0;
            UInt32 dwHash = 0;
            Int32 dwLength = m_String.Length;

            if (dwLength == 0)
                return 0;

            if (dwLength != 1)
            {
                do
                {
                    Int32 j = i;
                    if (i < dwLength)
                    {
                        do
                        {
                            dwHash += (UInt32)(((Byte)m_String[i] + i) * ((Byte)m_String[i] + 7) * ((Byte)m_String[j] + 19) * ((Byte)m_String[j] + j));
                            ++j;
                        }
                        while (j < dwLength);
                    }
                    ++i;
                }
                while (i < dwLength - 1);
            }
            return dwHash % 0xEE6B2800;
        }
    }
}
