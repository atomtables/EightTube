using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EightTube.API
{
    [DataContract]
    public class Error
    {
        [DataMember(IsRequired = true)]
        public string error { get; set; }
    }

    public class DataLoadException : Exception
    {
        int status;

        public DataLoadException()
        {
        }

        public DataLoadException(int status, string message)
            : base(message)
        {
            this.status = status;
        }
    }
}
