// Copyright (C) 2014 Laz0r

// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// In order to view the full description of the GNU GPL version 2.0(en),
// please visit http://www.gnu.org/licenses/old-licenses/gpl-2.0.en.html

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace GameboyAdvance.Core
{
    public unsafe class LZ77
    {
        #region Constants

        private const int SlidingWindowSize = 4096;
        private const int ReadAheadBufferSize = 18;
        private const int BlockSize = 8;

        #endregion

        #region Decompression

        /// <summary>
        /// Checks whether the data at a specific
        /// offset can be decompressed or not
        /// </summary>
        public static bool CanDecompress(byte[] data, int offset)
        {
            fixed (byte* ptr = &data[offset])
            {
                int length = GetCompressedLength(ptr);
                return (length != -1);
            }
        }

        /// <summary>
        /// Gets the length of the decompressed datastream
        /// </summary>
        private static int GetCompressedLength(byte* source)
        {
            if (*source != 0x10)
                return -1;

            uint deLength = ((*(uint*)(source)) >> 8);
            int dePosition = 0; int length = 4;

            while (dePosition < deLength)
            {
                byte isComp = source[length++];

                for (int i = 0; i < BlockSize && dePosition < deLength; i++)
                {
                    if ((isComp & 0x80) != 0)
                    {
                        byte amount = (byte)(((source[length] >> 4) & 0xF) + 3);
                        ushort copy = (ushort)((((source[length] & 0xF) << 8) + source[length + 1]) + 1);

                        if (copy < dePosition)
                            return -1;

                        dePosition += amount; length += 2;
                    }
                    else
                    {
                        dePosition++;
                        length++;
                    }
                    unchecked
                    {
                        isComp <<= 1;
                    }
                }
            }
            if (length % 4 != 0)
            {
                length += 4 - length % 4;
            }

            return length;
        }

        /// <summary>
        /// Decompresses data at a specific offset
        /// </summary>
        public static byte[] Decompress(byte[] data, int offset)
        {
            fixed (byte* ptr = &data[offset])
            {
                byte[] unCompData = new byte[(*(uint*)ptr) >> 8];

                if (unCompData.Length > 0)
                {
                    fixed (byte* dest = &unCompData[0])
                    {
                        if (!Decompress(ptr, dest))
                            unCompData = null;
                    }
                }
                return unCompData;
            }
        }

        /// <summary>
        /// Decompresses data and returns success
        /// </summary>
        private static bool Decompress(byte* source, byte* target)
        {
            if (*source++ != 0x10)
                return false;

            int positionUncomp = 0;
            int lenght = *(source++) + (*(source++) << 8) + (*(source++) << 16);

            while (positionUncomp < lenght)
            {
                byte isCompressed = *(source++);
                for (int i = 0; i < BlockSize; i++)
                {
                    if ((isCompressed & 0x80) != 0)
                    {
                        int amountToCopy = 3 + (*(source) >> 4);
                        int copyPosition = 1;
                        copyPosition += (*(source++) & 0xF) << 8;
                        copyPosition += *(source++);

                        if (copyPosition > lenght)
                            return false;

                        for (int u = 0; u < amountToCopy; u++)
                        {
                            target[positionUncomp] = target[positionUncomp - u - copyPosition + (u % copyPosition)];
                            positionUncomp++;
                        }
                    }
                    else
                    {
                        target[positionUncomp++] = *source++;
                    }
                    if (!(positionUncomp < lenght))
                        break;

                    unchecked
                    {
                        isCompressed <<= 1;
                    }
                }
            }
            return true;
        }

        #endregion

        #region Compression

        /// <summary>
        /// Compresses a byte[]
        /// </summary>
        public static byte[] Compress(byte[] data)
        {
            fixed (byte* pntr = &data[0])
            {
                return Compress(pntr, data.Length);
            }
        }

        /// <summary>
        /// Private compression
        /// </summary>
        private static byte[] Compress(byte* source, int lenght)
        {
            int position = 0;

            List<byte> CompressedData = new List<byte>();
            CompressedData.Add(0x10);

            {
                byte* pointer = (byte*)&lenght;
                for (int i = 0; i < 3; i++)
                    CompressedData.Add(*(pointer++));
            }

            while (position < lenght)
            {
                byte isCompressed = 0;
                List<byte> tempList = new List<byte>();

                for (int i = 0; i < BlockSize; i++)
                {
                    int[] searchResult = Search(source, position, lenght);

                    if (searchResult[0] > 2)
                    {
                        byte add = (byte)((((searchResult[0] - 3) & 0xF) << 4) + (((searchResult[1] - 1) >> 8) & 0xF));
                        tempList.Add(add);
                        add = (byte)((searchResult[1] - 1) & 0xFF);
                        tempList.Add(add);
                        position += searchResult[0];
                        isCompressed |= (byte)(1 << (BlockSize - (i + 1)));
                    }
                    else if (searchResult[0] >= 0)
                        tempList.Add(source[position++]);
                    else
                        break;
                }
                CompressedData.Add(isCompressed);
                CompressedData.AddRange(tempList);
            }
            while (CompressedData.Count % 4 != 0)
                CompressedData.Add(0);

            return CompressedData.ToArray();
        }

        /// <summary>
        /// Searches for a byte-row
        /// </summary>
        private static int[] Search(byte* source, int position, int lenght)
        {
            if (position >= lenght)
                return new int[] { -1, 0 };
            if ((position < 2) || ((lenght - position) < 2))
                return new int[] { 0, 0 };

            List<int> results = new List<int>();

            for (int i = 1; (i < SlidingWindowSize) && (i < position); i++)
            {
                if (source[position - (i + 1)] == source[position])
                {
                    results.Add(i + 1);
                }
            }
            if (results.Count == 0)
                return new int[2] { 0, 0 };

            int amountOfBytes = 0;

            bool Continue = true;
            while (amountOfBytes < ReadAheadBufferSize && Continue)
            {
                amountOfBytes++;
                for (int i = results.Count - 1; i >= 0; i--)
                {
                    if (source[position + amountOfBytes] != source[position - results[i] + (amountOfBytes % results[i])])
                    {
                        if (results.Count > 1)
                            results.RemoveAt(i);
                        else
                            Continue = false;
                    }
                }
            }
            return new int[2] { amountOfBytes, results[0] };
        }

        #endregion
    }
}