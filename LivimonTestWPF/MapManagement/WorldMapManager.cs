using System;

namespace LivimonTestWPF
{
    class WorldMapManager : MapManager
    {
        private int sizeX = 50;
        private int sizeY = 50;
        //Map map;
        protected override Map makeMap()
        {
            Map worldMap = new Map(20, 20);

            NoiseGenerator noiseGen = new NoiseGenerator(sizeX, sizeY);

            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; col++)
                {
                    if (row < 3 || row > 17) worldMap.map[row, col] = new MapTile("water");
                    else
                    {
                        worldMap.map[row, col] = new MapTile(noiseGen.getDiscreteNoise(row, col, 6));
                    }
                }
            }
            map = worldMap;
            return worldMap;
        }
    }
}
