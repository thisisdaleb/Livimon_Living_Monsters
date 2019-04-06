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
        public Map currentMap;
        public WorldMapManager mainWorldMap;
        int[] playerPosition;
        bool mapChanged = false;

        public MapGridSystem()
        {
            playerPosition = new int[]{ 0, 0 };
        }

        public void initializeWorldMap()
        {
            mainWorldMap = new WorldMapManager();
            currentMap = mainWorldMap.getMap();
            playerPosition = mainWorldMap.getStartingPosition();
            mapChanged = true;
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
                        newGridView[row + 3, col + 4] = new RectangleUpdate(currentMapTile.getColor(), null, currentMapTile.tileUnpassable());
                    }
                }
            }
            return newGridView;
        }

        private int oldX = 0;
        private int oldY = 0;
        public bool getFullMapListNeedsUpdate()
        {
            if (!mapChanged) return false;
            return Math.Round((double)playerPosition[0] / 25) * 25 != oldY || (int)Math.Round((double)playerPosition[1] / 25) * 25 != oldX;
        }

        private bool worldGenDebug = true;
        public RectangleUpdate[,] getFullMapListView()
        {
            if (worldGenDebug == true)
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

            oldY = (int)Math.Round((double)playerPosition[0] / 25) * 25;
            oldX = (int)Math.Round((double)playerPosition[1] / 25) * 25;

            int startRow = Math.Max(0, Math.Min(currentMap.map.GetLength(0) - 51, oldY - 25));
            int startCol = Math.Max(0, Math.Min(currentMap.map.GetLength(1) - 51, oldX - 25));
            int rowCount = Math.Min(currentMap.map.GetLength(0), 50);
            int colCount = Math.Min(currentMap.map.GetLength(1), 50);
            RectangleUpdate[,] miniMapGrid = new RectangleUpdate[rowCount, colCount];
            for (int row = startRow; row < rowCount + startRow; row++)
            {
                for (int col = startCol; col < colCount + startCol; col++)
                {
                    if (row < 0 || row > (currentMap.map.GetLength(0) - 1))
                    {
                        miniMapGrid[row - startRow, col - startCol] = new RectangleUpdate();
                    }
                    else if (col < 0 || col > (currentMap.map.GetLength(1) - 1))
                    {
                        miniMapGrid[row - startRow, col - startCol] = new RectangleUpdate();
                    }
                    else
                    {
                        MapTile currentMapTile = currentMap.map[row, col];
                        miniMapGrid[row - startRow, col - startCol] = new RectangleUpdate(currentMapTile.getColor());
                    }
                }
            }
            return miniMapGrid;
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
