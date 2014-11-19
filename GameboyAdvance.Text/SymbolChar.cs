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
    public class SymbolChar : IPokeChar
    {
        #region Interface

        #region Fields


        string myChar;
        byte[] myByte;

        List<byte> list = new List<byte>() { 
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 
            0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0x10, 0x11,
            0x12, 0x13, 0x14, 0x15, 0x16, 0x17 };
        List<string> list2 = new List<string>() { 
            "[OBEN]", "[UNTEN]", "[LINKS]", "[RECHTS]", "[+]",
            "[Lv]", "[AP]", "[ID]", "[NR]", "[UNDERLINE]", "[1]",
            "[2]", "[3]", "[4]", "[5]", "[6]", "[7]", "[8]", "[9]",
            "[(]", "[)]", "[KREIS]", "[DREIECK]", "[X]" };


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
                return myByte;
            }
            set
            {
                myByte = value;
            }
        }


        #endregion

        #endregion

        #region Constructor


        public SymbolChar(byte[] input)
        {
            myByte = input;
            int i = list.IndexOf(input[1]);
            if (i != -1) myChar = list2[i];
            else myChar = "";
        }

        public SymbolChar(string input)
        {
            myChar = input;
            int i = list2.IndexOf(myChar);
            if (i != -1) myByte = new byte[] { 0xF9, list[i] };
            else myByte = new byte[] { };
        }


        #endregion 

        #region Destructor

        private bool disposed = false;

        ~SymbolChar()
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
