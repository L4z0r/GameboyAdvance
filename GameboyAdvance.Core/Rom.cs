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
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameboyAdvance.Core
{
    public class Rom : IDisposable
    {
        #region Structures

        // Provides two dictionaries which handle
        // the users last actions to undo/redo them
        struct UndoRedoManager
        {
            public List<KeyValuePair<uint, byte[]>> undoList;
            public List<KeyValuePair<uint, byte[]>> redoList;

            public int capacity;
            public int position;

            public UndoRedoManager(int cap)
            {
                undoList = new List<KeyValuePair<uint, byte[]>>();
                redoList = new List<KeyValuePair<uint, byte[]>>();

                capacity = cap;
                position = -1;
            }

            public void Clear()
            {
                undoList.Clear();
                redoList.Clear();
                position = -1;
            }
        }

        #endregion

        #region Constants

        // For more readability
        const uint DEFAULT_OFF = 0x0;
        const uint ROM_POINTER = 0x8000000;
        const uint HEADER_POSI = 0xA0;

        // Error messages
        const int LOAD_SUCCESS = -1;
        const int FILE_NOT_FOUND = 0;
        const int FOLDER_NOT_FOUND = 1;
        const int ALREADY_USED = 2;
        const int DEFAULT_ERROR = 3;
        const int INVALID_ROM = 4;

        #endregion

        #region Fields

        // Default def
        string path;
        string code;
        byte[] data;
        bool firered;
        bool emerald;

        // Access def
        MemoryStream stream;
        BinaryReader reader;
        BinaryWriter writer;

        // Undo/Redo def
        UndoRedoManager manager;

        #endregion

        #region Properties

        /// <summary>
        /// Receives a bool whether the rom is FR or RS
        /// </summary>
        public bool FireRed
        {
            get { return firered; }
        }

        /// <summary>
        /// Receives a bool whether the rom is RS or EM
        /// </summary>
        public bool Emerald
        {
            get { return emerald; }
        }

        /// <summary>
        /// Receives or sets the location of the rom
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// Receives "POKEMON [CODE]"
        /// </summary>
        public string Code
        {
            get { return code; }
        }

        /// <summary>
        /// Receives "BPRD" or "BPED" etc.
        /// </summary>
        public string Version
        {
            get { return code.Substring(12, 4); }
        }

        /// <summary>
        /// Receives the rom-length
        /// </summary>
        public int Length
        {
            get { return data.Length; }
        }

        /// <summary>
        /// Receives the length of the max.offset(hex)
        /// </summary>
        public int HexLength
        {
            get { return (((data.Length) - 1).ToString("X").Length); }
        }

        /// <summary>
        /// Receives or sets the current offset
        /// </summary>
        public uint Offset
        {
            get { return (uint)stream.Position; }
            set { stream.Position = (long)value; }
        }

        /// <summary>
        /// Receives or sets the undo/redo capacity
        /// </summary>
        public int UndoRedoCapacity
        {
            get { return manager.capacity; }
            set { manager.capacity = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Only copies the filepath
        /// </summary>
        public Rom(string path)
        {
            this.path = path;
        }

        #endregion

        #region Destructor

        private bool disposed = false;

        // Destructor
        ~Rom()
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
                    Array.Clear(data, 0, data.Length);

                    reader.Dispose();
                    writer.Dispose();
                    stream.Dispose();

                    manager.undoList.Clear();
                    manager.redoList.Clear();
                }

                // Free unmanaged objects
                // and other structs (null)
                reader = null;
                writer = null;
                stream = null;

                manager.undoList = null;
                manager.redoList = null;
                data = null;

                disposed = true;
            }
        }

        #endregion

        #region Handling

        /// <summary>
        /// Loads the rom.
        /// </summary>
        public int Load()
        {
            FileMode mode = FileMode.Open;
            Stream temp;

            // Tries to open rom and returns error msgs
            try {
                temp = new FileStream(path, mode);
            } catch (DirectoryNotFoundException) {
                return FOLDER_NOT_FOUND;
            } catch (FileNotFoundException) {
                return FILE_NOT_FOUND;
            } catch (IOException) {
                return ALREADY_USED;
            } catch (Exception) {
                return DEFAULT_ERROR;
            }

            // Checks if rom is valid
            if (temp.Length < 0xB0)
                return INVALID_ROM;

            // Reads all the bytes to our byte[], creates
            // all the stream-types for easy access and
            // loads the undo/redo manager
            using (var br = new BinaryReader(temp))
                data = br.ReadBytes((int)temp.Length);

            temp.Close(); temp.Dispose();
            stream = new MemoryStream(data);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);

            stream.Position = HEADER_POSI;
            code = Encoding.Default.GetString(reader.ReadBytes(16));
            firered = (Version.Contains("BPR") | Version.Contains("BPG"));
            emerald = (Version.Contains("BPE"));

            manager = new UndoRedoManager(0xFF);
            stream.Position = DEFAULT_OFF;

            return LOAD_SUCCESS;
        }

        /// <summary>
        /// Saves the rom
        /// </summary>
        public void Save()
        {
            WriteRom();
        }

        /// <summary>
        /// Saves the rom at a new location
        /// </summary>
        /// <param name="pth"></param>
        public void SaveAs(string pth)
        {
            path = pth;
            WriteRom();
        }

        /// <summary>
        /// Writes all the stuff into the file
        /// </summary>
        private void WriteRom()
        {
            // Writes all our bytes at the
            // given rom-path and clears undo/redo
            File.WriteAllBytes(path, data);
            manager.Clear();
        }

        #endregion

        #region Methods

        // Those functions are used to read
        // bytes of different datasizes
        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public ushort ReadHword()
        {
            return reader.ReadUInt16();
        }

        public uint ReadWord()
        {
            return reader.ReadUInt32();
        }

        public ulong ReadDword()
        {
            return reader.ReadUInt64();
        }

        public byte[] ReadBytes(int count)
        {
            return reader.ReadBytes(count);
        }

        public uint ReadPointer()
        {
            return reader.ReadUInt32() - ROM_POINTER;
        }

        // And those are used for
        // writing and undo/redoing
        public void WriteByte(byte val)
        {
            writer.Write(new byte[] { val });
        }

        public void WriteHword(ushort val)
        {
            writer.Write(BitConverter.GetBytes(val));
        }

        public void WriteWord(uint val)
        {
            writer.Write(BitConverter.GetBytes(val));
        }

        public void WriteDword(ulong val)
        {
            writer.Write(BitConverter.GetBytes(val));
        }

        public void WriteBytes(byte[] val)
        {
            writer.Write(val);
        }

        public void WritePointer(uint offset)
        {
            writer.Write(BitConverter.GetBytes(offset + ROM_POINTER));
        }

        // 1. Reading old value
        // 2. Going back to orig. offset
        // 3. Writing old value to undo
        // 4. Writing new value to rom
        public void StackWriteByte(byte val)
        {
            AddStack(1);
            writer.Write(new byte[] { val });
        }

        public void StackWriteHword(ushort val)
        {
            AddStack(2);
            writer.Write(BitConverter.GetBytes(val));
        }

        public void StackWriteWord(uint val)
        {
            AddStack(4);
            writer.Write(BitConverter.GetBytes(val));
        }

        public void StackWriteDword(ulong val)
        {
            AddStack(8);
            writer.Write(BitConverter.GetBytes(val));
        }

        public void StackWriteBytes(byte[] val)
        {
            AddStack(val.Length);
            writer.Write(val);
        }

        public void StackWritePointer(uint offset)
        {
            AddStack(4);
            writer.Write(BitConverter.GetBytes(offset + ROM_POINTER));
        }

        #endregion

        #region Functions

        // Checks if you have undone some actions,
        // then remove newer undo-entries
        private void RemoveOld()
        {
            int count = manager.undoList.Count;
            while (manager.position != count - 1)
            {
                count -= 1;
                manager.undoList.RemoveAt(count);
            }
            if (count == manager.capacity)
            {
                manager.undoList.RemoveAt(0);
            }
            manager.redoList.Clear();
        }

        // Adds the given amount of bytes
        // to the manager-undo-stack
        private void AddStack(int count)
        {
            RemoveOld();
            uint off = this.Offset;
            byte[] old = reader.ReadBytes(count);

            manager.undoList.Add(new KeyValuePair<uint, byte[]>(off, old));
            manager.position += 1;

            this.Offset = off;
        }

        // Checks if there is an action
        // which can be undone
        public bool CanUndo()
        {
            return (manager.position > -1);
        }

        // Checks if there is an action
        // which can be redone
        public bool CanRedo()
        {
            return (manager.position != manager.undoList.Count - 1);
        }

        // Undos a recently made action
        public void Undo()
        {
            int cnt = (manager.undoList.Count - 1);
            var pair = manager.undoList[cnt];
            this.manager.undoList.RemoveAt(cnt);
            this.manager.redoList.Add(pair);

            this.Offset = pair.Key;
            WriteBytes(pair.Value);

            manager.position -= 1;
        }

        // Redos an undone action
        public void Redo()
        {
            int cnt = (manager.redoList.Count - 1);
            var pair = manager.redoList[cnt];
            this.manager.redoList.RemoveAt(cnt);
            this.manager.undoList.Add(pair);

            this.Offset = pair.Key;
            WriteBytes(pair.Value);
        }

        // Searches for freespace at the given start-offset
        // and the given amount of 0xFF bytes to be searched
        public uint FreeSpaceFinder(uint start, int count)
        {
            this.Offset = start;
            for (int i = 0; i < count; i++)
            {
                if (ReadByte() != 0xFF) {
                    i = -1;
                    start = Offset;
                } else {
                    if (start >= data.Length)
                    {
                        return 0;
                    }
                }
            } return start;
        }

        // Searches the given byte[] in the rom
        // and returns a list of all the offsets
        public List<int> FindBytes(byte[] src)
        {
            // Gets binary encoding
            List<int> bytes = new List<int>();
            var encoding = Encoding.GetEncoding("iso-8859-1");

            // Creates a string from this encoding
            string searchStack = encoding.GetString(src);
            string rawStack = encoding.GetString(data);
            string escape = Regex.Escape(searchStack);

            // Searches for it and returns the offsets
            for (var m = Regex.Match(rawStack, escape); m.Success; m = m.NextMatch())
            {
                bytes.Add(m.Index); 
            } Regex.CacheSize = 0;

            return new List<int>(bytes);
        }

        #endregion

        #region Decode

        /// <summary>
        /// Decompresses a picture or something
        /// else at a given offset location
        /// </summary>
        public byte[] Decompress(uint offset)
        {
            return LZ77.Decompress(data, (int)offset);
        }

        /// <summary>
        /// Compresses a byte[] and writes it
        /// at the given offset into the rom
        /// </summary>
        public void Compress(byte[] data, uint offset)
        {
            byte[] comp = LZ77.Compress(data);
            Offset = offset;
            WriteBytes(comp);
        }

        /// <summary>
        /// Compresses a byte[] and writes it
        /// into the rom. This action can be undone.
        /// </summary>
        public void StackCompress(byte[] data, uint offset)
        {
            byte[] comp = LZ77.Compress(data);
            Offset = offset;
            StackWriteBytes(comp);
        }

        /// <summary>
        /// Gets the compressed data
        /// of a given byte array
        /// </summary>
        public int GetCompressedLength(byte[] data)
        {
            return LZ77.Compress(data).Length;
        }

        /// <summary>
        /// Gets the decompressed data
        /// of a given location in rom
        /// </summary>
        public int GetDecompressedLength(uint offset)
        {
            return LZ77.Decompress(data, (int)offset).Length;
        }

        /// <summary>
        /// Checks whether the bytes at a
        /// given offset can be decompressed
        /// </summary>
        public bool CanDecompress(uint offset)
        {
            return LZ77.CanDecompress(data, (int)offset);
        }

        #endregion
    }
}