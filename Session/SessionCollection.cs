using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SessionDecoder.Session
{
    public class SessionCollection : CollectionBase
    {
        public Session this[int index]
        {
            get { return (Session)List[index]; }
            set { List[index] = value; }
        }

        public int Add(Session value)
        {
            return List.Add(value);
        }
    }
}
