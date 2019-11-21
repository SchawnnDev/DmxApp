using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DmxAppLib.Data;
using Sniper.Lighting.DMX;

namespace DmxAppLib
{
    public class Lib
    {

        private List<DmxValue> SetValues { get; set; } = new List<DmxValue>();

        //shared boolean to track if you're connected
        public bool IsConnected => DmxController<DMXProUSB>.Connected;

        void DMXProUSB_StateChangedHandler(object sender, StateChangedEventArgs e)
        {
            //this handler is called by a different (non UI) thread so if you want to update
            //ui controls you'll need to BeginInvoke
            //get the current DMX values
            var values = DmxController<DMXProUSB>.GetCurrentValues();
        }
        public void TurnLightsOn()
        {
            //connect to controller if not connected
            ConnectToController();
            Guid queueId = Guid.Empty;
            //queues allow you to segregate events that run in parallel; use Guid.Empty for the shared queue
            //when all effects on a given queue ID (other than Empty) are complete, the queue is deleted 
            //and those channels return to off.
            //the Guid.Empty queue is never deleted so allows you set permanent state
            int priority = 1;
            //priority allows you to control the order in which parallel queues are stacked

            //create a random helper
            Random random = new Random();
            //pick a random channel number (note some lights such as DMX RGB lights, smoke machines) etc. 
            //require multiple channels to turn the light on
            //for example rgbwae lights have a base address which you set on the back of the light
            //which is RED then you have GREEN, BLUE, WHITE, AMBER, LUMA/BRIGHTNESS, EFFECTS
            //to set the light to aqua, you need to set Base+1 [Green], Base+2 [Blue] and Base+5 [Luma] to 255
           // int channel = random.Next(0, 512);
            int channel = 0; 
           //set value to 255 (full on)
            byte channelValue = 18;
            //call the controller to set the value on the queue; this is a permanent set (will stay until you change it)
            DmxController<DMXProUSB>.SetDmxValue(queue: queueId, channel: channel, priority: priority, value: channelValue);
        }
        public void ConnectToController()
        {
            if (IsConnected) return;
            //you only need to set up limits once; this allows you to lock specific channels to max values, e.g. if they are behind a wall or under the floor and you don't want them heating up and burning

            DmxController<DMXProUSB>.SetLimits(new DmxLimits
            {
                Min = Enumerable.Range(0, 512).Select(c => (byte)0).ToArray(),
                Max = Enumerable.Range(0, 512).Select(c => (byte)255).ToArray(),
            });

            //in a try/catch (can throw)
            try
            {
                //attempt a start
                if (DmxController<DMXProUSB>.Start())
                {
                    //if now connected
                    if (DmxController<DMXProUSB>.Connected)
                    {
                        //attach to handler
                        //note: don't forget to detach this later if you lose connection
                        DmxController<DMXProUSB>.StateChanged += DMXProUSB_StateChangedHandler;
                    }
                    else
                    {
                        Console.WriteLine("DmxController.Start returned [true] but not connected");
                    }
                }
                else
                {
                    Console.WriteLine("DmxController.Start returned [false]");
                }
            }
            catch (Exception ex)
            {
                //exception occurred, failed to connect
                Console.WriteLine("DmxController.Start threw {0}", ex);
            }
        }

        public void SetDmxValue(Guid queueId, DmxValue value, int priority)
        {
            DmxController<DMXProUSB>.SetDmxValue(queueId, value.Channel.Value, priority, value.Value);
        }

        public bool ContainsDmxValue(DmxValue value) => ContainsDmxValue(value.Channel.Value, value.Value);

        public bool ContainsDmxValue(int channel, byte value) => GetDmxValue(channel, value) != null;

        public DmxValue GetDmxValue(DmxValue value) => GetDmxValue(value.Channel.Value, value.Value);

        public DmxValue GetDmxValue(int channel, byte value)
        {
            foreach (var dmxValue in SetValues)
            {
                if (dmxValue.Channel.Value == channel)
                    return dmxValue;
            }

            return null;
        }


        public void Disconnect()
        {
            if (IsConnected)
            {
                DmxController<DMXProUSB>.SetDefaults(new DmxDefaults());
                //shut down DMX controller
                DmxController<DMXProUSB>.StateChanged -= DMXProUSB_StateChangedHandler;
                DmxController<DMXProUSB>.Dispose();
            }
        }

    }
}
