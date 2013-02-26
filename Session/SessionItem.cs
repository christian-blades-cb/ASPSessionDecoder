using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace SessionDecoder.Session
{
    public class SessionItem
    {
        #region properties
        private int _seekPosition;

        public int SeekPosition
        {
            get { return _seekPosition; }
            set { _seekPosition = value; }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        private string _key;

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private object _value;

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        #endregion

        public SessionItem() { }

        public SessionItem(int index, int seekPosition, string key, object value)
        {
            Index = index;
            SeekPosition = seekPosition;
            Key = key;
            Value = value;
        }
    }
}
