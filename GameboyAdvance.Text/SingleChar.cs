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
    public class SingleChar : IPokeChar
    {
        #region Interface

        #region Fields


        string myChar;
        byte myByte;

        static List<byte> list;
        static List<string> list2;


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
                return new byte[] {myByte};
            }
            set
            {
                myByte = value[0];
            }
        }


        #endregion

        #endregion

        #region Constructor


        public SingleChar(byte b)
        {
            myByte = b;
            int i = list.IndexOf(b);
            if (i != -1) myChar = list2[i];
            else myChar = "";
        }

        public SingleChar(string str)
        {
            myChar = str;
            int i = list2.IndexOf(str);
            if (i != -1) myByte = list[i];
            else myByte = 0xFF;
        }

        public static void Initialize()
        {
            //  Initializes the lists
            list = new List<byte>();
            list2 = new List<string>();

            //  Splits the table into lines
            string[] arr = Resources.SingleTable.Split(new[]
            {Environment.NewLine}, StringSplitOptions.None);
            int x = arr.Length;

            //  Loops through each entry
            for (int i = 0; i < x; i++)
            {
                string[] tmp = arr[i].Split('=');
                list.Add(byte.Parse(tmp[0], NumberStyles.HexNumber));
                list2.Add(tmp[1]);
            }

            list.AddRange(new byte[]{0xFA, 0xFB, 0xFE});
            list2.AddRange(new string[] { @"\l", @"\p", @"\n" });
        }


        #endregion

        #region Destructor

        private bool disposed = false;

        ~SingleChar()
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
                    myByte = 0;
                    myChar = String.Empty;
                }

                disposed = true;
            }
        }

        public static void Denitialize()
        {
            list.Clear();
            list2.Clear();
            list = null;
            list2 = null;
        }

        #endregion
    }
}