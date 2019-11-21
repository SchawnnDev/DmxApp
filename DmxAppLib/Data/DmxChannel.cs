using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DmxAppLib.Interfaces;

namespace DmxAppLib.Data
{
    public class DmxChannel : IChannel
    {

        public int Value { get; set; }

        public DmxChannel(int value)
        {
            Value = value;
        }

    }
}
