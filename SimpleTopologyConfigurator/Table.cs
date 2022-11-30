using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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

        public void createTable(string filePath)
        {
            createFile(filePath);
            string content = "";
            int ipCtr = 1;
            content += "+---------------+---------------+---------------+---------------+\n";
            content += "|Device name    |Network name   |IP address     |Subnet mask    |\n";
            content += "+---------------+---------------+---------------+---------------+\n";
            foreach (var key in devices)
            {
                content += "|" + devices[key.Key].getName();
                for (int ctr = 0; ctr < 15 - devices[key.Key].getName().Length; ctr++)
                {
                    content += " ";
                }
                content += "|" + name;
                for (int ctr = 0; ctr < 15 - name.Length; ctr++)
                {
                    content += " ";
                }
                if (!devices[key.Key].getName().Contains("Switch"))
                {
                    content += "|" + ip.Substring(0, ip.Length - 1) + ipCtr;
                    ipCtr++;
                    for (int ctr = 0; ctr < 15 - ip.Length; ctr++)
                    {
                        content += " ";
                    }
                }
                else
                {
                    content += "|               ";
                }
                content += "|" + mask;
                for (int ctr = 0; ctr < 15 - mask.Length; ctr++)
                {
                    content += " ";
                }
                content += "|\n";
                content += "+---------------+---------------+---------------+---------------+\n";

                File.WriteAllText(filePath + "\\table.txt", content);
            }
        }

        private void createFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var myFile = File.CreateText(filePath + "\\table.txt");
                myFile.Close();
            }
        }
    }
}
