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
using GameboyAdvance.Text.Properties;
using System.Globalization;

namespace GameboyAdvance.Text
{
    public class InvalidChar : IPokeChar
    {
        #region Interface

        #region Fields


        string myChar;
        byte[] myByte;


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
                myByte = null;
            }
        }


        #endregion

        #endregion

        #region Constructor


        public InvalidChar(string str)
        {
            myChar = str;
            myByte = null;
        }


        #endregion

        #region Destructor

        private bool disposed = false;

        ~InvalidChar()
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
                    myByte = null;
                    myChar = null;
                }

                disposed = true;
            }
        }

        #endregion
    }
}
