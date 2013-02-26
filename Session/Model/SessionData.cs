using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;

namespace SessionDecoder.Session.Model
{
    [Table(Name="ASPStateTempSessions")]
    public class SessionData
    {
        [Column(IsPrimaryKey = true)]
        public string SessionId;
        [Column]
        public byte[] SessionItemShort;
    }
}
