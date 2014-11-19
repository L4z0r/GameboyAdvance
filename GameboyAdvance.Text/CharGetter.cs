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

namespace GameboyAdvance.Text
{
    public static class CharGetter
    {
        public static IPokeChar GetType(string str)
        {
            if (str.Length == 1)
            {
                var sc = new SingleChar(str);
                if (sc.pokeByte[0] != 0xFF)
                    return sc;
                else
                    return null;
            }
            switch (str)
            {
                case "[WEISS]":
                case "[SCHWARZ]":
                case "[GRAU]":
                case "[ROT]":
                case "[ORANGE]":
                case "[GRÜN]":
                case "[CYAN]":
                case "[BLAU]":
                case "[HELLBLAU]":
                case "[PINK]":
                case "[BRAUN]":
                    return new ColorChar(str);
                case "[BG_WEISS]":
                case "[BG_SCHWARZ]":
                case "[BG_GRAU]":
                case "[BG_ROT]":
                case "[BG_ORANGE]":
                case "[BG_GRÜN]":
                case "[BG_CYAN]":
                case "[BG_BLAU]":
                case "[BG_HELLBLAU]":
                case "[BG_PINK]":
                case "[BG_BRAUN]":
                    return new BgColorChar(str);
                case "[SD_WEISS]":
                case "[SD_SCHWARZ]":
                case "[SD_GRAU]":
                case "[SD_ROT]":
                case "[SD_ORANGE]":
                case "[SD_GRÜN]":
                case "[SD_CYAN]":
                case "[SD_BLAU]":
                case "[SD_HELLBLAU]":
                case "[SD_PINK]":
                case "[SD_BRAUN]":
                    return new ShadowChar(str);
                case "[SPIELER]":
                case "[RIVALE]":
                case "[BUFFER1]":
                case "[BUFFER2]":
                case "[BUFFER3]":
                    return new BufferChar(str);
                case "[A]":
                case "[B]":
                case "[L]":
                case "[R]":
                case "[START]":
                case "[SELECT]":
                case "[?]":
                case "[KEINE]":
                    return new KeyChar(str);
                case "[OBEN]":
                case "[UNTEN]":
                case "[LINKS]":
                case "[RECHTS]":
                case "[+]":
                case "[Lv]":
                case "[AP]":
                case "[ID]":
                case "[NR]":
                case "[UNDERLINE]":
                case "[1]":
                case "[2]":
                case "[3]":
                case "[4]":
                case "[5]":
                case "[6]":
                case "[7]":
                case "[8]":
                case "[9]":
                case "[(]":
                case "[)]":
                case "[KREIS]":
                case "[DREIECK]":
                case "[X]":
                    return new SymbolChar(str);
                case "[GROSS]":
                case "[KLEIN]":
                    return new SizeChar(str);
                case "[LV]":
                case "[PK]":
                case "[MN]":
                case "[PO]":
                case "[Ke]":
                case "[BL]":
                case "[OC]":
                case "[K]":
                case "[u]":
                case "[d]":
                case "[l]":
                case "[r]":
                case "[.]":
                case "[\"]":
                case "[']":
                case "[m]":
                case "[f]":
                case "[>]":
                    return new SpecialChar(str);
                default:
                    return null;
            }
        }

        public static IPokeChar GetTypeDebug(string str)
        {
            if (str.Length == 1)
            {
                var sc = new SingleChar(str);
                if (sc.pokeByte[0] != 0xFF)
                    return sc;
                else
                    return null;
            }
            switch (str)
            {
                case "[WEISS]":
                case "[SCHWARZ]":
                case "[GRAU]":
                case "[ROT]":
                case "[ORANGE]":
                case "[GRÜN]":
                case "[CYAN]":
                case "[BLAU]":
                case "[HELLBLAU]":
                case "[PINK]":
                case "[BRAUN]":
                    return new ColorChar(str);
                case "[BG_WEISS]":
                case "[BG_SCHWARZ]":
                case "[BG_GRAU]":
                case "[BG_ROT]":
                case "[BG_ORANGE]":
                case "[BG_GRÜN]":
                case "[BG_CYAN]":
                case "[BG_BLAU]":
                case "[BG_HELLBLAU]":
                case "[BG_PINK]":
                case "[BG_BRAUN]":
                    return new BgColorChar(str);
                case "[SD_WEISS]":
                case "[SD_SCHWARZ]":
                case "[SD_GRAU]":
                case "[SD_ROT]":
                case "[SD_ORANGE]":
                case "[SD_GRÜN]":
                case "[SD_CYAN]":
                case "[SD_BLAU]":
                case "[SD_HELLBLAU]":
                case "[SD_PINK]":
                case "[SD_BRAUN]":
                    return new ShadowChar(str);
                case "[SPIELER]":
                case "[RIVALE]":
                case "[BUFFER1]":
                case "[BUFFER2]":
                case "[BUFFER3]":
                    return new BufferChar(str);
                case "[A]":
                case "[B]":
                case "[L]":
                case "[R]":
                case "[START]":
                case "[SELECT]":
                case "[?]":
                case "[KEINE]":
                    return new KeyChar(str);
                case "[OBEN]":
                case "[UNTEN]":
                case "[LINKS]":
                case "[RECHTS]":
                case "[+]":
                case "[Lv]":
                case "[AP]":
                case "[ID]":
                case "[NR]":
                case "[UNDERLINE]":
                case "[1]":
                case "[2]":
                case "[3]":
                case "[4]":
                case "[5]":
                case "[6]":
                case "[7]":
                case "[8]":
                case "[9]":
                case "[(]":
                case "[)]":
                case "[KREIS]":
                case "[DREIECK]":
                case "[X]":
                    return new SymbolChar(str);
                case "[GROSS]":
                case "[KLEIN]":
                    return new SizeChar(str);
                case "[LV]":
                case "[PK]":
                case "[MN]":
                case "[PO]":
                case "[Ke]":
                case "[BL]":
                case "[OC]":
                case "[K]":
                case "[u]":
                case "[d]":
                case "[l]":
                case "[r]":
                case "[.]":
                case "[\"]":
                case "[']":
                case "[m]":
                case "[f]":
                case "[>]":
                    return new SpecialChar(str);
                default:
                    return new InvalidChar(str);
            }
        }
    }
}
