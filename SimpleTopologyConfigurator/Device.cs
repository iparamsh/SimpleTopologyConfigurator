using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SimpleTopologyConfigurator
{
    //class holds the information about the device
    public class Device
    {
        private List<string> neighbourDevices = new List<string>(); //list of all network devices
        private List<int> neighborsPing = new List<int>();          //list of all devices pings
        private string name = "";                                   //name of the current device
        private Point point;
        public Device (String name, Point point)
        {
            this.name = name;
            this.point = point;
        }

        //gets device name
        public string getName()
        {
            return this.name;
        }

        //gets devices position
        public Point getPos()
        {
            return this.point;
        }

        //changes devices position
        public void changePoint(Point point)
        {
            this.point = point;
        }

        //adds neighbours device
        public void addNeighbourDevice(string name)
        {
            neighbourDevices.Add(name);
        }

        //delets network connection with other device
        public void deleteNetworkConnection(string deviceName)
        {
           if (neighbourDevices.Contains(deviceName))
           {
                neighborsPing.RemoveAt(neighbourDevices.IndexOf(deviceName));
                neighbourDevices.Remove(deviceName);
           }
        }

        //adds ping of the connection with other device
        public void addNeighbourPing(int ping)
        {
            neighborsPing.Add(ping);
        }
        
        //gets devices neighbour count
        public int getNeighbourDeviceCount()
        {
            return neighbourDevices.Count;
        }

        //gets devices neighbours ping 
        public int[] getNeightboursPing()
        {
            return neighborsPing.ToArray();
        }

        //gets all neighbours and puts in the array of strings
        public string[] getNeighbours()
        {
            return neighbourDevices.ToArray();
        }
    }
}
