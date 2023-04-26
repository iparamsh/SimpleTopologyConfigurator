using SimpleTopologyConfigurator;
using System.Collections.Generic;


public class MatrixTranslator
{
    private Dictionary<string, int> deviceIndexMap;
    private int[,] matrix;

    public MatrixTranslator(Device[] devices)
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

    public Dictionary<string, int> getDictionary()
    {
        return deviceIndexMap;
    }

    public int[,] GetMatrix()
    {
        return matrix;
    }
}
