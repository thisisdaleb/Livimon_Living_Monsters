using System;

namespace LivimonTestWPF
{
    class WorldMapManager : MapManager
    {
        //Map map;
        private int sizeX = 50;
        private int sizeY = 50;

        protected override Map makeMap()
        {
            Map worldMap = new Map(sizeY, sizeX);

            NoiseGenerator noiseGen = new NoiseGenerator();

            for (int row = 0; row < sizeY; row++)
            {
                for (int col = 0; col < sizeX; col++)
                {
                    if (row < 3 || row > (sizeY - 4)) worldMap.map[row, col] = new MapTile("water");
                    else if ((row == 3 || row == (sizeY - 4)) && noiseGen.getRand(row, col, 3) > 1)
                    {
                        worldMap.map[row, col] = new MapTile("water");
                    }
                    else
                    {
                        worldMap.map[row, col] = new MapTile(noiseGen.getRand(row, col, 6));
                    }
                }
            }
            map = worldMap;
            return worldMap;
        }
    }
}
