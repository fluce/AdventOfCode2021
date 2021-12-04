using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualize.Shared
{
    public class GridData
    {
        public bool[][]? Data { get; set; }
        public int Width => Data?.GetLength(0) ?? 0;
        public int Height => Data?[0]?.GetLength(0) ?? 0;


        public static GridData From(int width, int height, Func<int,int,bool> func)
        {
            var gridData=new GridData();
            gridData.Data = new bool[width][];
            for (int i = 0; i < width; i++)
            {
                gridData.Data[i] = new bool[height];
                for (int j = 0; j < height; j++)
                    gridData.Data[i][j] = func(i, j);
            }
            return gridData;
        }

    }


}
