using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GameboyAdvance.Core;
using GameboyAdvance.Text;

namespace LibraryTest
{
    class Program
    {
        // For testing all the error messages
        const string path1 = @"C:\Users\Admin\Desktop\Pokemon Feuerrot (D).gba";
        const string path2 = @"C:\InvalidFolder\invalid.txt";
        const string path3 = @"C:\Users\InvalidFile.gba";
        const string path4 = @"C:\Users\Admin\Desktop\somefile.gba";

        static void Main(string[] args)
        {
            while (true)
            {
                SingleChar.Initialize();

                var rom = new Rom(path1);
                int msg = rom.Load();
                Console.WriteLine("=============================");

                /*  Error messages =====>
                    LOAD_SUCCESS = -1;
                    FILE_NOT_FOUND = 0;
                    FOLDER_NOT_FOUND = 1;
                    ALREADY_USED = 2;
                    DEFAULT_ERROR = 3;
                    INVALID_ROM = 4;    */
                switch (msg)
                {
                    case -1:
                        Console.WriteLine("Rom: Loading successful");
                        break;
                    case 0:
                        Console.WriteLine("Rom: File was not found");
                        break;
                    case 1:
                        Console.WriteLine("Rom: Folder was not found");
                        break;
                    case 2:
                        Console.WriteLine("Rom: File is already used by another program");
                        break;
                    case 3:
                        Console.WriteLine("Rom: An unknown error appeared");
                        break;
                    case 4:
                        Console.WriteLine("Rom: File is invalid. Please open another one");
                        break;
                } Console.WriteLine("=============================");

                Console.WriteLine("Bitte gib den zu suchenden Text ein!");
                var poke = new PokeString(Console.ReadLine());
                var list = rom.FindBytes(poke.GetBytes());
                poke.Dispose();

                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine("Offset: 0x" + list[i].ToString("X"));
                    Console.WriteLine("----------------------------------");

                    var str = new PokeString(rom, (uint)list[i]);
                    Console.WriteLine(str.GetString());
                    str.Dispose();

                    Console.WriteLine("----------------------------------");
                    Console.WriteLine(" ");
                }

                SingleChar.Denitialize();
                list.Clear();
                rom.Dispose();
                rom = null;

                Console.WriteLine("Drücke F5 um neuzustarten!");
                if (Console.ReadKey().Key != ConsoleKey.F5)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.Clear();
                }
            }
        }
    }
}