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

using GameboyAdvance.Core;

namespace GameboyAdvance.Text
{
    public class PokeString : IDisposable
    {
        #region Properties


        public List<IPokeChar> pokeChars { get; set; }


        #endregion

        #region Constructor


        public PokeString() { }

        public List<string> DebugString(string input)
        {
            // First removes special chars
            var arr = new char[] { '"', '§', '$', '%', '&', '/', '´', '`', '²', 
                '³', '{', '}', '\\', '@', '>', '<', '|', 'µ', ';', '*', '~', '#' };

            input = input.TrimStart(arr);

            List<string> pos = new List<string>();
            pokeChars = new List<IPokeChar>();

            //  Splits the whole string into
            //  several lines, to make a loop
            string[] lines = input.Split(new string[] {
                Environment.NewLine }, StringSplitOptions.None);
            int linecnt = lines.Length;

            for (int x = 0; x < linecnt; x++)
            {
                //  Declares essential loop vars
                //  and a substring of the lines[x]
                string sub = lines[x];
                int position = 0;
                int length = sub.Length;
                int line = x;

                while (position < length)
                {
                    //  Reads a char
                    int copy = position;
                    string tmp = sub.Substring(position, 1);

                    //  If it's a bracket, read whole bracket
                    if (tmp == "[")
                    {
                        int len = (sub.IndexOf("]", position) - position) + 1;
                        tmp = sub.Substring(position, len);
                        position += len;
                    }
                    else
                    {
                        //  Else just increase the pos
                        position++;
                    }

                    //  Gets the interface-type and adds it
                    IPokeChar chr = CharGetter.GetTypeDebug(tmp);
                    if (chr is InvalidChar)
                    {
                        copy++;
                        line++;
                        pos.Add(copy + "=" + line);
                        pokeChars.Add(chr);
                    }
                }
                if (length > 36)
                {
                    pos.Add("#" + length + "=" + line); 
                }
            }

            return pos;
        }

        public PokeString(string input)
        {
            pokeChars = new List<IPokeChar>();

            //  Splits the whole string into
            //  several lines, to make a loop
            string[] lines = input.Split(new string[] {
                Environment.NewLine }, StringSplitOptions.None);
            int linecnt = lines.Length;

            for (int x = 0; x < linecnt; x++)
            {
                //  After every line, we add a newline
                //  or an \l which is used for scrolling
                if (x == 1)
                    pokeChars.Add(new SingleChar(@"\n"));
                else if (x > 1 && x < linecnt)
                    pokeChars.Add(new SingleChar(@"\l"));

                //  Declares essential loop vars
                //  and a substring of the lines[x]
                string sub = lines[x];
                int position = 0;
                int length = sub.Length;

                while (position < length)
                {
                    //  Reads a char
                    string tmp = sub.Substring(position, 1);

                    //  If it's a bracket, read whole bracket
                    if (tmp == "[")
                    {
                        int len = (sub.IndexOf("]", position) - position) + 1;
                        tmp = sub.Substring(position, len);
                        position += len;
                    }
                    else
                    {
                        //  Else just increase the pos
                        position++;
                    }

                    //  Gets the interface-type and adds it
                    IPokeChar chr = CharGetter.GetType(tmp);
                    if (chr != null)
                        pokeChars.Add(chr);
                }
            }
        }
        
        public PokeString(Rom rom, uint offset)
        {
            //  Declares a list which holds the
            //  string-bytes without 0xFF (end).
            List<byte> pokeBytes = new List<byte>();
            pokeChars = new List<IPokeChar>();
            rom.Offset = offset;
            Code.isFireRed = rom.FireRed;

            //  Loops until it hits a 0xFF byte
            byte chr = rom.ReadByte();
            while (chr != 0xFF)
            {
                //  Adds the byte, if not 0xFF
                //  and reads another one.
                pokeBytes.Add(chr);
                chr = rom.ReadByte();

                if (pokeBytes.Count == 500)
                    break;
            }

            //  Decoding process
            int count = pokeBytes.Count;
            for (int i = 0; i < count; i++)
            {
                //  Gets the byte at current pos
                byte b = pokeBytes[i];

                //  Determines whether it is
                //  a color-char, buffer-char, etc
                #region Charswitch

                switch (b)
                {
                    case 0xFC:
                        if (i + 2 >= count)
                        {
                            i += 2;
                            break;
                        }

                        //  Foreground, background,
                        //  shadow and font-size
                        if (i + 1 == pokeBytes.Count) return;
                        byte b2 = pokeBytes[i + 1];

                        if (b2 == 1)
                        {
                            pokeChars.Add(new ColorChar(new byte[]
                                { b, b2, pokeBytes[i + 2] }));
                        }
                        else if (b2 == 2)
                        {
                            pokeChars.Add(new BgColorChar(new byte[] 
                                { b, b2, pokeBytes[i + 2] }));
                        }
                        else if (b2 == 3)
                        {
                            pokeChars.Add(new ShadowChar(new byte[]
                                { b, b2, pokeBytes[i + 2] }));
                        } 
                        else if (b2 == 6)
                        {
                            pokeChars.Add(new SizeChar(new byte[]
                            { b, b2, pokeBytes[i + 2] }));
                        } i += 2;
                        break;

                    case 0xFD:
                        //  Player, rival, buffers, etc
                        if (i + 1 == pokeBytes.Count) return;
                        pokeChars.Add(new BufferChar(new byte[]
                            { b, pokeBytes[i + 1] })); i++;
                        break;

                    case 0xF8:
                        //  Keypresses like A, B, L, R, etc
                        if (i + 1 == pokeBytes.Count) return;
                        pokeChars.Add(new KeyChar(new byte[]
                            { b, pokeBytes[i + 1] })); i++;
                        break;

                    case 0xF9:
                        //  Symbols like left-arrow, circle, etc
                        if (i + 1 == pokeBytes.Count) return;
                        pokeChars.Add(new SymbolChar(new byte[]
                            { b, pokeBytes[i + 1] })); i++;
                        break;

                    case 0x34: case 0x53: case 0x54: case 0x55:
                    case 0x56: case 0x57: case 0x58: case 0x59:
                    case 0x79: case 0x7A: case 0x7B: case 0x7C:
                    case 0xB0: case 0xB2: case 0xB4: case 0xB5:
                    case 0xB6: case 0xB9: case 0xEF:
                        //  Special chars are only 1 byte,
                        //  but multiple representative chars
                        pokeChars.Add(new SpecialChar(b));
                        break;

                    default:
                        //  If char is not special, add
                        //  a normal singlechar.
                        var sc = new SingleChar(b);
                        if (sc.pokeByte[0] != 0xFF)
                            pokeChars.Add(new SingleChar(b));
                        break;
                }

                #endregion
            }
        }

        public PokeString(Rom rom, uint offset, bool start)
        {
            if (!start)
            {
                //  Declares a list which holds the
                //  string-bytes without 0xFF (end).
                List<byte> pokeBytes = new List<byte>();
                pokeChars = new List<IPokeChar>();
                rom.Offset = offset;
                Code.isFireRed = rom.FireRed;

                //  Loops until it hits a 0xFF byte
                byte chr = rom.ReadByte();
                while (chr != 0xFF)
                {
                    //  Adds the byte, if not 0xFF
                    //  and reads another one.
                    pokeBytes.Add(chr);
                    chr = rom.ReadByte();

                    if (pokeBytes.Count == 500)
                        break;
                }

                //  Decoding process
                int count = pokeBytes.Count;
                for (int i = 0; i < count; i++)
                {
                    //  Gets the byte at current pos
                    byte b = pokeBytes[i];

                    //  Determines whether it is
                    //  a color-char, buffer-char, etc
                    #region Charswitch

                    switch (b)
                    {
                        case 0xFC:
                            if (i + 2 > count)
                            {
                                i += 2;
                                break;
                            }

                            //  Foreground, background,
                            //  shadow and font-size
                            if (i + 1 == pokeBytes.Count) return;
                            byte b2 = pokeBytes[i + 1];

                            if (b2 == 1)
                            {
                                pokeChars.Add(new ColorChar(new byte[] { b, b2, pokeBytes[i + 2] }));
                            }
                            else if (b2 == 2)
                            {
                                pokeChars.Add(new BgColorChar(new byte[] { b, b2, pokeBytes[i + 2] }));
                            }
                            else if (b2 == 3)
                            {
                                pokeChars.Add(new ShadowChar(new byte[] { b, b2, pokeBytes[i + 2] }));
                            }
                            else if (b2 == 6)
                            {
                                pokeChars.Add(new SizeChar(new byte[] { b, b2, pokeBytes[i + 2] }));
                            } i += 2;
                            break;

                        case 0xFD:
                            //  Player, rival, buffers, etc
                            if (i + 1 == pokeBytes.Count) return;
                            pokeChars.Add(new BufferChar(new byte[] { b, pokeBytes[i + 1] })); i++;
                            break;

                        case 0xF8:
                            //  Keypresses like A, B, L, R, etc
                            if (i + 1 == pokeBytes.Count) return;
                            pokeChars.Add(new KeyChar(new byte[] { b, pokeBytes[i + 1] })); i++;
                            break;

                        case 0xF9:
                            //  Symbols like left-arrow, circle, etc
                            if (i + 1 == pokeBytes.Count) return;
                            pokeChars.Add(new SymbolChar(new byte[] { b, pokeBytes[i + 1] })); i++;
                            break;

                        case 0x34:
                        case 0x53:
                        case 0x54:
                        case 0x55:
                        case 0x56:
                        case 0x57:
                        case 0x58:
                        case 0x59:
                        case 0x79:
                        case 0x7A:
                        case 0x7B:
                        case 0x7C:
                        case 0xB0:
                        case 0xB2:
                        case 0xB4:
                        case 0xB5:
                        case 0xB6:
                        case 0xB9:
                        case 0xEF:
                            //  Special chars are only 1 byte,
                            //  but multiple representative chars
                            pokeChars.Add(new SpecialChar(b));
                            break;

                        default:
                            //  If char is not special, add
                            //  a normal singlechar.
                            var sc = new SingleChar(b);
                            if (sc.pokeByte[0] != 0xFF)
                                pokeChars.Add(new SingleChar(b));
                            break;
                    }

                    #endregion
                }
            }
            else
            {
                //  Declares a list which holds the
                //  string-bytes without 0xFF (end).
                List<byte> pokeBytes = new List<byte>();
                pokeChars = new List<IPokeChar>();
                rom.Offset = offset - 1;

                // Loops back until it hits 0xFF
                while (rom.ReadByte() != 0xFF)
                {
                    rom.Offset -= 2;
                    offset--;
                }

                rom.Offset = offset;
                Code.isFireRed = rom.FireRed;

                //  Loops until it hits a 0xFF byte
                byte chr = rom.ReadByte();
                while (chr != 0xFF)
                {
                    //  Adds the byte, if not 0xFF
                    //  and reads another one.
                    pokeBytes.Add(chr);
                    chr = rom.ReadByte();

                    if (pokeBytes.Count == 500)
                        break;
                }

                //  Decoding process
                int count = pokeBytes.Count;
                for (int i = 0; i < count; i++)
                {
                    //  Gets the byte at current pos
                    byte b = pokeBytes[i];

                    //  Determines whether it is
                    //  a color-char, buffer-char, etc
                    #region Charswitch

                    switch (b)
                    {
                        case 0xFC:
                            //  Foreground, background,
                            //  shadow and font-size
                            if (i + 1 == pokeBytes.Count) return;
                            byte b2 = pokeBytes[i + 1];

                            if (b2 == 1)
                            {
                                pokeChars.Add(new ColorChar(new byte[] { b, b2, pokeBytes[i + 2] }));
                            }
                            else if (b2 == 2)
                            {
                                pokeChars.Add(new BgColorChar(new byte[] { b, b2, pokeBytes[i + 2] }));
                            }
                            else if (b2 == 3)
                            {
                                pokeChars.Add(new ShadowChar(new byte[] { b, b2, pokeBytes[i + 2] }));
                            }
                            else if (b2 == 6)
                            {
                                pokeChars.Add(new SizeChar(new byte[] { b, b2, pokeBytes[i + 2] }));
                            } i += 2;
                            break;

                        case 0xFD:
                            //  Player, rival, buffers, etc
                            if (i + 1 == pokeBytes.Count) return;
                            pokeChars.Add(new BufferChar(new byte[] { b, pokeBytes[i + 1] })); i++;
                            break;

                        case 0xF8:
                            //  Keypresses like A, B, L, R, etc
                            if (i + 1 == pokeBytes.Count) return;
                            pokeChars.Add(new KeyChar(new byte[] { b, pokeBytes[i + 1] })); i++;
                            break;

                        case 0xF9:
                            //  Symbols like left-arrow, circle, etc
                            if (i + 1 == pokeBytes.Count) return;
                            pokeChars.Add(new SymbolChar(new byte[] { b, pokeBytes[i + 1] })); i++;
                            break;

                        case 0x34:
                        case 0x53:
                        case 0x54:
                        case 0x55:
                        case 0x56:
                        case 0x57:
                        case 0x58:
                        case 0x59:
                        case 0x79:
                        case 0x7A:
                        case 0x7B:
                        case 0x7C:
                        case 0xB0:
                        case 0xB2:
                        case 0xB4:
                        case 0xB5:
                        case 0xB6:
                        case 0xB9:
                        case 0xEF:
                            //  Special chars are only 1 byte,
                            //  but multiple representative chars
                            pokeChars.Add(new SpecialChar(b));
                            break;

                        default:
                            //  If char is not special, add
                            //  a normal singlechar.
                            var sc = new SingleChar(b);
                            if (sc.pokeByte[0] != 0xFF)
                                pokeChars.Add(new SingleChar(b));
                            break;
                    }

                    #endregion
                }
            }
        }


        #endregion

        #region Destructor

        private bool disposed = false;

        ~PokeString()
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
                    pokeChars.Clear();

                    if (pokeChars != null)
                    {
                        foreach (var c in pokeChars)
                        {
                            c.Dispose();
                        }
                    }
                }

                disposed = true;
            }
        }

        #endregion

        #region Functions


        public string GetString()
        {
            //  Builds the string
            StringBuilder sb = new StringBuilder();
            if (pokeChars.Count > 5000)
            {
                throw new OutOfMemoryException("Um den Computer zu schützen, wurde das Programm unterbrochen.");
            }

            foreach (IPokeChar c in pokeChars)
            {
                //  Copies the string
                //  for performance purposes
                if (c != null)
                {
                    string b = c.pokeChar;

                    //  Checks whether it's a break in Pokémon
                    //  if yes, add an Environment.NewLine
                    if (b == @"\l" | b == @"\p" | b == @"\n")
                        sb.Append(Environment.NewLine);
                    else
                        sb.Append(b);
                }
            }

            //  Returns the string
            return sb.ToString();
        }

        public byte[] GetBytes()
        {
            //  Creates a bytelist
            List<byte> output = new List<byte>();
            if (pokeChars.Count > 5000)
            {
                throw new OutOfMemoryException("Um den Computer zu schützen, wurde das Programm unterbrochen.");
            }

            foreach (IPokeChar c in pokeChars)
            {
                if (c != null)
                    output.AddRange(c.pokeByte);
            }

            //  Returns the list as array
            return output.ToArray();
        }

        #endregion
    }

    public static class Code
    {
        public static bool isFireRed;
    }
}
