//  The MIT License (MIT)
//  Copyright (c) 2014 Laz0r
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.

using System;
using System.Text;
using System.Collections.Generic;

namespace GameboyAdvance.Text
{
    public class SpecialChar : IPokeChar
    {
        #region Interface

        #region Fields


        string myChar;
        byte myByte;

        List<byte> list = new List<byte>() {
            0x34, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59,
            0x79, 0x7A, 0x7B, 0x7C, 0xB0, 0xB2, 0xB4, 0xB5,
            0xB6, 0xB9, 0xEF};
        List<string> list2 = new List<string>() {
            "[LV]", "[PK]", "[MN]", "[PO]", "[Ke]",
            "[BL]", "[OC]", "[K]" , "[u]" , "[d]" ,
            "[l]" , "[r]" , "[.]" , "[\"]", "[']" ,
            "[m]" , "[f]" , "[x]" , "[>]" };


        #endregion

        #region Properties


        /// <summary>
        /// Returns one char or sets one
        /// </summary>
        public string pokeChar
        {
            get
            {
                return myChar;
            }
            set
            {
                myChar = value;
            }
        }

        /// <summary>
        /// Returns one byte or sets one.
        /// </summary>
        public byte[] pokeByte
        {
            get
            {
                return new byte[] { myByte };
            }
            set
            {
                myByte = value[0];
            }
        }


        #endregion

        #endregion

        #region Constructor


        public SpecialChar(byte b)
        {
            myByte = b;
            int i = list.IndexOf(b);
            if (i != -1) myChar = list2[i];
            else myChar = "";
        }

        public SpecialChar(string str)
        {
            myChar = str;
            int i = list2.IndexOf(str);
            if (i != -1) myByte = list[i];
            else myByte = 0;
        }


        #endregion

        #region Destructor

        private bool disposed = false;

        ~SpecialChar()
        {
            Dispose(false);
        }

        // Implements IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Cleans up all managed resources & big objs
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free managed objects
                    list.Clear();
                    list2.Clear();
                }

                // Free unmanaged objects
                // and other structs (null)
                list = null;
                list2 = null;

                disposed = true;
            }
        }

        #endregion
    }
}
