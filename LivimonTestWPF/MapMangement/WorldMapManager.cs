using System;

namespace LivimonTestWPF
{

    class WorldMapManager : MapManager
    {
        //Map map;
        protected override Map makeMap()
        {
            Map worldMap = new Map(100, 100);

            for (int row = 0; row < 100; row++)
            {
                for (int col = 0; col < 100; col++)
                {
                    if (row < 3 || row > 97) worldMap.map[row, col] = new MapTile("water");
                    else
                    {
                        worldMap.map[row, col] = new MapTile("grassland");
                    }
                }
            }
            map = worldMap;
            return worldMap;
        }
    }
}
