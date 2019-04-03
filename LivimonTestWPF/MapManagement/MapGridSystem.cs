using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LivimonTestWPF
{
    class MapGridSystem
    {
        Map currentMap;
        WorldMapManager mainWorldMap;
        int[] playerPosition;

        public MapGridSystem()
        {
            playerPosition = new int[]{ 20, 20 };
        }

        public void initializeWorldMap()
        {
            mainWorldMap = new WorldMapManager();
            currentMap = mainWorldMap.getMap();
        }

        public RectangleUpdate[,] getCurrentMapListView()
        {
            RectangleUpdate[,] newGridView = new RectangleUpdate[7, 9];
            for (int row = -3; row <= 3; row++)
            {
                for (int col = -4; col <= 4; col++)
                {
                    int currentY = playerPosition[0] + row;
                    int currentX = playerPosition[1] + col;
                    if (currentY < 0 || currentY > (currentMap.map.GetLength(0) - 1))
                    {
                        newGridView[row + 3, col + 4] = new RectangleUpdate();
                    }
                    else if (currentX < 0 || currentX > (currentMap.map.GetLength(1) - 1))
                    {
                        newGridView[row + 3, col + 4] = new RectangleUpdate();
                    }
                    else
                    {
                        MapTile currentMapTile = currentMap.map[currentY, currentX];
                        newGridView[row + 3, col + 4] = new RectangleUpdate(currentMapTile.getColor());
                    }
                }
            }
            return newGridView;
        }

        public RectangleUpdate[,] getFullMapListView()
        {
            RectangleUpdate[,] fullMapGrid = new RectangleUpdate[currentMap.map.GetLength(0), currentMap.map.GetLength(1)];
            for (int row = 0; row < fullMapGrid.GetLength(0); row++)
            {
                for (int col = 0; col < fullMapGrid.GetLength(1); col++)
                {
                    if (row < 0 || row > (currentMap.map.GetLength(0) - 1))
                    {
                        fullMapGrid[row, col] = new RectangleUpdate();
                    }
                    else if (col < 0 || col > (currentMap.map.GetLength(1) - 1))
                    {
                        fullMapGrid[row, col] = new RectangleUpdate();
                    }
                    else
                    {
                        MapTile currentMapTile = currentMap.map[row, col];
                        fullMapGrid[row, col] = new RectangleUpdate(currentMapTile.getColor());
                    }
                }
            }
            return fullMapGrid;
        }

        internal string getTileName()
        {
            return currentMap.map[playerPosition[0], playerPosition[1]].getTypeName();
        }

        internal void moveLeft()
        {
            if (playerPosition[1] > 0) playerPosition[1] -= 1;
        }

        internal void moveRight()
        {
            if (playerPosition[1] < currentMap.map.GetLength(1)) playerPosition[1] += 1;
        }

        internal void moveUp()
        {
            if (playerPosition[0] > 0) playerPosition[0] -= 1;
        }

        internal void moveDown()
        {
            if (playerPosition[0] < currentMap.map.GetLength(0)) playerPosition[0] += 1;
        }
    }
}
