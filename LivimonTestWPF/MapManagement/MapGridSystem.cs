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
            MapTileDefs.initializeMapTileDefs();
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
            return currentMap.map[playerPosition[0], playerPosition[1]].getBiome();
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

    abstract class MapManager
    {
        protected Map map;
        public MapManager()
        {
            map = makeMap();
        }

        protected abstract Map makeMap();

        public Map getMap()
        {
            return map;
        }

    }

    class Map
    {
        public MapTile[,] map;
        
        public Map(int _y, int _x) {
            map = new MapTile[_y, _x];
        }

    }

    class MapTile
    {
        private string biome;
        private SolidColorBrush tileColor;
        
        public MapTile(string _biomeType)
        {
            biome = _biomeType;
            tileColor = MapTileDefs.biomeToBrushList[biome];
        }

        public MapTile(int _biomeNum)
        {
            biome = MapTileDefs.biomeList[_biomeNum];
            tileColor = MapTileDefs.biomeToBrushList[biome];
        }

        public SolidColorBrush getColor()
        {
            return tileColor;
        }
        public string getBiome()
        {
            return biome;
        }
    }

    static class MapTileDefs
    {
        static public Dictionary<string, SolidColorBrush> biomeToBrushList = new Dictionary<string, SolidColorBrush>();
        static public string[] biomeList;

        public static void initializeMapTileDefs()
        {
            biomeToBrushList.Add("desert", System.Windows.Media.Brushes.Yellow);
            biomeToBrushList.Add("forest", Brushes.Green);
            biomeToBrushList.Add("water", Brushes.Blue);
            biomeToBrushList.Add("grassland", Brushes.Lime);
            biomeToBrushList.Add("tundra", Brushes.White);
            biomeToBrushList.Add("city", Brushes.DarkGray);
            biomeToBrushList.Add("mountain", Brushes.Brown);
            biomeToBrushList.Add("volcano", Brushes.Red);
            biomeList = new string[] {"water", "grassland", "water", "forest", "mountain", "desert", "grassland", "tundra", "city", "volcano"};
        }
    }
}
