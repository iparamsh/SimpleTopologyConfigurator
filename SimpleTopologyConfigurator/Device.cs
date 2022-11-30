using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTopologyConfigurator
{
    public class Device
    {
        private List<string> neighbourDevices = new List<string>();
        private string name = "";
        private Point point;
        public Device (String name, Point point)
        {
            this.name = name;
            this.point = point;
        }

        public string getName()
        {
            return this.name;
        }

        public Point getPos()
        {
            return this.point;
        }

        public void changePoint(Point point)
        {
            this.point = point;
        }

        public void addNeighbourDevice(string name)
        {
            neighbourDevices.Add(name);
        }
        
        public int getNeighbourDeviceCount()
        {
            return neighbourDevices.Count;
        }

        public string[] getNeighbours()
        {
            return neighbourDevices.ToArray();
        }
    }
}
