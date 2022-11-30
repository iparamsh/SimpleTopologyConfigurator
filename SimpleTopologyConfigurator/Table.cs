using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace SimpleTopologyConfigurator
{
    internal class Table
    {
        private IDictionary<string, Device> devices = new Dictionary<string, Device>();
        private string ip;
        private string name;
        string mask;
        
        public Table(IDictionary<string, Device> devices, string ip, string name)
        {
            this.devices = devices;
            this.ip = ip;
            this.name = name;
            this.mask = getMask();
        }

        private string getMask()
        {
            if (Int16.Parse(ip.Substring(0, 3)) <= 127)
            {
                return "255.0.0.0";
            }
            if (Int16.Parse(ip.Substring(0, 3)) <= 191)
            {
                return "255.255.0.0";
            }
            if (Int16.Parse(ip.Substring(0, 3)) <= 223)
            {
                return "255.255.255.0";
            }
            return "";
        }

        public void createTable()
        {
            
        }
    }
}
