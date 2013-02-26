using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.Linq;
using SessionDecoder.Session.Model;

namespace SessionDecoder.Session
{
    public class Session
    {
        #region properties
        private string _sessionId;

        public string SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }

        private SessionItemCollection _items;

        public SessionItemCollection Items
        {
            get { return _items; }
            set { _items = value; }
        }

        private BinaryReader _br;
        #endregion

        public Session() { }

        public Session(string sessionId, byte[] buffer)
        {
            Items = new SessionItemCollection();
            SessionId = sessionId;

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                Decode(new MemoryStream(buffer));
                ms.Close();
            }
        }

        public static IEnumerable<Session> Retrieve(string connectionString)
        {
            DataContext db = new DataContext(connectionString);
            Table<SessionData> sessionTable = db.GetTable<SessionData>();

            var datas = sessionTable.Select(row => new Session(row.SessionId, row.SessionItemShort));

            return datas;
        }

        private void Decode(MemoryStream ms)
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                int timeout = br.ReadInt32(); // session timeout (i think)

                bool state = br.ReadBoolean(); // we'll be using this
                bool _static = br.ReadBoolean(); // don't need this

                if (state)
                {
                    DecodeStateItems(br);
                }
            }
        }

        private void DecodeStateItems(BinaryReader br)
        {
            int items = br.ReadInt32(); // items in session
            if (items > 0)
            {
                int num2 = br.ReadInt32(); // another check flag
                int currentItem = 0;

                while (currentItem < items) // loop through each item
                {
                    string key;
                    if (currentItem == num2) { key = null; }
                    else // found the key
                    {
                        key = br.ReadString();
                        Items.Add(new SessionItem(currentItem, 0, key, null)); // add it to our collection
                    }
                    currentItem++;
                }

                // need this loop so we can find the seek position
                for (currentItem = 0; currentItem < items; currentItem++)
                {
                    if (currentItem != 0)
                    {
                        Items.FindBy(currentItem).SeekPosition = br.ReadInt32(); // seek to so we can find the value
                    }
                }

                int lastOffset = br.ReadInt32();
                byte[] buffer = new byte[lastOffset];
                int length = br.BaseStream.Read(buffer, 0, lastOffset);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    _br = new BinaryReader(ms);
                    SetValues();
                }
            }
        }

        enum DataType : byte
        {
            // Fields
            Boolean = 3,
            Byte = 6,
            Char = 7,
            DateTime = 4,
            Decimal = 5,
            Double = 9,
            Guid = 0x11,
            Int16 = 11,
            Int32 = 2,
            Int64 = 12,
            IntPtr = 0x12,
            Null = 0x15,
            Object = 20,
            SByte = 10,
            Single = 8,
            String = 1,
            TimeSpan = 0x10,
            UInt16 = 13,
            UInt32 = 14,
            UInt64 = 15,
            UIntPtr = 0x13
        }

        private void SetValues()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                _br.BaseStream.Seek((long)this.Items[i].SeekPosition, SeekOrigin.Begin);
                this.Items[i].Value = ReadValueFromStream();
            }

            _br.Close();
        }

        object ReadValueFromStream()
        {
            object obj1 = null;
            int[] numArray1;
            int num1;
            switch (((DataType)_br.ReadByte()))
            {
                case DataType.String:
                    {
                        return _br.ReadString();
                    }
                case DataType.Int32:
                    {
                        return _br.ReadInt32();
                    }
                case DataType.Boolean:
                    {
                        return _br.ReadBoolean();
                    }
                case DataType.DateTime:
                    {
                        return new DateTime(_br.ReadInt64());
                    }
                case DataType.Decimal:
                    {
                        numArray1 = new int[4];
                        num1 = 0;
                        goto Label_00CA;
                    }
                case DataType.Byte:
                    {
                        return _br.ReadByte();
                    }
                case DataType.Char:
                    {
                        return _br.ReadChar();
                    }
                case DataType.Single:
                    {
                        return _br.ReadSingle();
                    }
                case DataType.Double:
                    {
                        return _br.ReadDouble();
                    }
                case DataType.SByte:
                    {
                        return _br.ReadSByte();
                    }
                case DataType.Int16:
                    {
                        return _br.ReadInt16();
                    }
                case DataType.Int64:
                    {
                        return _br.ReadInt64();
                    }
                case DataType.UInt16:
                    {
                        return _br.ReadUInt16();
                    }
                case DataType.UInt32:
                    {
                        return _br.ReadUInt32();
                    }
                case DataType.UInt64:
                    {
                        return _br.ReadUInt64();
                    }
                case DataType.TimeSpan:
                    {
                        return new TimeSpan(_br.ReadInt64());
                    }
                case DataType.Guid:
                    {
                        byte[] buffer1 = _br.ReadBytes(0x10);
                        return new Guid(buffer1);
                    }
                case DataType.IntPtr:
                    {
                        if (IntPtr.Size == 4)
                        {
                            return new IntPtr(_br.ReadInt32());
                        }
                        return new IntPtr(_br.ReadInt64());
                    }
                case DataType.UIntPtr:
                    {
                        if (UIntPtr.Size == 4)
                        {
                            return new UIntPtr(_br.ReadUInt32());
                        }
                        return new UIntPtr(_br.ReadUInt64());
                    }
                case DataType.Object:
                    {
                        try
                        {
                            BinaryFormatter formatter1 = new BinaryFormatter();
                            return formatter1.Deserialize(_br.BaseStream);
                        }
                        catch
                        {
                            return "Blob";
                        }                        
                    }
                case DataType.Null:
                    {
                        return null;
                    }
                default:
                    {
                        return obj1;
                    }
            }
        Label_00CA:
            if (num1 >= 4)
            {
                return new decimal(numArray1);
            }
            numArray1[num1] = _br.ReadInt32();
            num1++;
            goto Label_00CA;
        }
    }
}

