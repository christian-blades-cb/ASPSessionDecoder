using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace SessionDecoder.Session
{
    public class SessionItemCollection : CollectionBase
    {
        public SessionItem this[int index]
        {
            get { return (SessionItem)List[index]; }
            set { List[index] = value; }
        }

        public int Add(SessionItem value)
        {
            return List.Add(value);
        }

        public SessionItem this[string key]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Key.Equals(key))
                    {
                        return this[i];
                    }
                }
                return null;
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Key.Equals(key))
                    {
                        this[i] = value;
                        break;
                    }
                }
            }
        }

        public SessionItem FindBy(int index)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Index == index)
                {
                    return this[i];
                }
            }
            return null;
        }
    }
}
