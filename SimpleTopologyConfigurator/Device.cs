using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTopologyConfigurator
{
    internal class Device
    {
        private List<string> neighbourDevice = new List<string>();
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
            neighbourDevice.Add(name);
        }
    }
}
