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
            playerPosition = new int[]{ 5, 5};
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
                    int currentX = playerPosition[0] + col;
                    if (currentY < 0 || currentY >= currentMap.map.GetLength(0))
                    {
                        newGridView[row + 3, col + 4] = new RectangleUpdate();
                    }
                    else if (currentX < 0 || currentX >= currentMap.map.GetLength(1))
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
            biomeList = new string[] {"water", "desert", "forest", "grassland", "tundra", "city", "mountain", "volcano" };
        }
    }
}
