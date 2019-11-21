using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmxAppLib.Data
{
    public class DmxValue
    {

        public DmxChannel Channel { get; set; }

        public byte Value { get; set; }

        public DmxValue(DmxChannel channel, byte value)
        {
            Channel = channel;
            Value = value;
        }

    }
}
