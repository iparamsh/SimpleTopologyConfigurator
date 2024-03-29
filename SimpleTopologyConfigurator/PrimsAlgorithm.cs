﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimpleTopologyConfigurator
{
    class PrimsAlgorithm
    {
        //code generated by ChatGPT
        //runs prims algorithm to find MST. 
        public int[,] primsAlgorithm(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int[] mstSet = new int[n];
            int[] key = new int[n];
            int[] parent = new int[n];

            for (int i = 0; i < n; i++)
            {
                key[i] = int.MaxValue;
                mstSet[i] = 0;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < n - 1; count++)
            {
                int u = minKey(key, mstSet);
                mstSet[u] = 1;

                for (int v = 0; v < n; v++)
                {
                    if (matrix[u, v] != 0 && mstSet[v] == 0 && matrix[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = matrix[u, v];
                    }
                }
            }

            int[,] mst = new int[n, n];

            for (int i = 1; i < n; i++)
            {
                mst[parent[i], i] = matrix[parent[i], i];
                mst[i, parent[i]] = matrix[parent[i], i];
            }

            return mst;
        }

        private static int minKey(int[] key, int[] mstSet)
        {
            int min = int.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < key.Length; v++)
            {
                if (mstSet[v] == 0 && key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

    }
}
