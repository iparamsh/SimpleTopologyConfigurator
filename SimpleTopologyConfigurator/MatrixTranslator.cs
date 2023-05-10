using SimpleTopologyConfigurator;
using System.Collections.Generic;


public class MatrixTranslator
{
    private Dictionary<string, int> deviceIndexMap; //device and it's index map
    private int[,] matrix;                          //matrix of the graph 

    //updates the matrix
    public void update(Device[] devices)
    {
        int deviceCount = devices.Length;
        deviceIndexMap = new Dictionary<string, int>();
        matrix = new int[deviceCount, deviceCount];

        for (int i = 0; i < deviceCount; i++)
        {
            string deviceName = devices[i].getName();
            deviceIndexMap.Add(deviceName, i);
        }

        for (int i = 0; i < deviceCount; i++)
        {
            Device device = devices[i];
            string[] neighbors = device.getNeighbours();
            int[] pingValues = device.getNeightboursPing();

            for (int j = 0; j < neighbors.Length; j++)
            {
                string neighborName = neighbors[j];
                int neighborPing = pingValues[j];
                int neighborIndex = deviceIndexMap[neighborName];

                matrix[i, neighborIndex] = neighborPing;
                matrix[neighborIndex, i] = neighborPing; 
            }
        }
    }

    //gets the device index map
    public Dictionary<string, int> getDictionary()
    {
        return deviceIndexMap;
    }

    //gets matrix
    public int[,] GetMatrix()
    {
        return matrix;
    }
}
