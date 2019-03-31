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

            //instead of directly writing map tiles using a number, we should make a list of mapTiles
            //that way we can tell water tiles to not enter each other
            //we could then find out the "groups", then we can give each group a randomly chosen mapTile
            //The group would also be handed a name
            //We could even add crazier rules like "mountain groups are often near deserts" because of blocked rain,
            //and put tundra next to mountains as wekk
            //volcanos could only go on groups < 5 tiles
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
                        worldMap.map[row, col] = new MapTile(noiseGen.getRand(row, col, 7));
                    }
                }
            }
            map = worldMap;
            return worldMap;
        }
    }
}
