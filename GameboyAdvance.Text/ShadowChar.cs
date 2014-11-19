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
    public class ShadowChar : IPokeChar
    {
        #region Interface

        #region Fields


        string myChar;
        byte[] myByte;

        List<byte> frlg = new List<byte> { 0x0, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9 };
        List<byte> rss = new List<byte>  { 0x1, 0x2, 0x3, 0x4, 0x5, 0xC, 0xD };

        List<string> frlg2 = new List<string> {
            "[SD_WEISS]", 
            "[SD_SCHWARZ]", 
            "[SD_GRAU]", 
            "[SD_ROT]",
            "[SD_ORANGE]",
            "[SD_GRÜN]",
            "[SD_CYAN]",
            "[SD_BLAU]",
            "[SD_HELLBLAU]" };
        List<string> rss2 = new List<string> {
            "[SD_WEISS]",
            "[SD_ROT]",
            "[SD_GRAU]",
            "[SD_BLAU]",
            "[SD_SCHWARZ]",
            "[SD_PINK]",
            "[SD_BRAUN]" };


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


        public ShadowChar(byte[] input)
        {
            myByte = input;

            //  We need a determination
            //  because different colors
            if (Code.isFireRed)
            {
                int i = frlg.IndexOf(input[2]);
                if (i != -1) myChar = frlg2[i];
                else myChar = "";
            }
            else
            {
                int i = rss.IndexOf(input[2]);
                if (i != -1) myChar = rss2[i];
                else myChar = "";
            }
        }

        public ShadowChar(string input)
        {
            myChar = input;

            //  We need a determination
            //  because different colors
            if (Code.isFireRed)
            {
                int i = frlg2.IndexOf(myChar);
                if (i != -1) myByte = new byte[] { 0xFC, 0x3, frlg[i] };
                else myChar = "";
            }
            else
            {
                int i = rss2.IndexOf(myChar);
                if (i != -1) myByte = new byte[] { 0xFC, 0x3, rss[i] };
                else myByte = new byte[] { };
            }
        }


        #endregion 

        #region Destructor

        private bool disposed = false;

        ~ShadowChar()
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
                    frlg.Clear();
                    frlg2.Clear();

                    rss.Clear();
                    rss2.Clear();
                }

                // Free unmanaged objects
                // and other structs (null)
                frlg = null;
                frlg2 = null;
                rss = null;
                rss2 = null;

                disposed = true;
            }
        }

        #endregion
    }
}
