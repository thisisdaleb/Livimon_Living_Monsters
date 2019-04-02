using System;

namespace LivimonTestWPF
{
    class WorldMapManager : MapManager
    {
        //A cool seed the last time I checked was -1674238469
        //is it still cool?

        //Map map;
        private static int sizeX = 100;
        private static int sizeY = sizeX;

        private float[,] elevationGrid;
        private float[,] percipitationGrid;
        private float[,] temperatureGrid;

        protected override Map makeMap()
        {
            Map worldMap = new Map(sizeY, sizeX);

            //some reasonable value examples are 0.1 for 50 with 9 types, 0.05 for 100,
            NoiseGenerator noiseGen = new NoiseGenerator(8f/sizeY);

            //DEFINE ELEVATION
            elevationGrid = new float[sizeY, sizeX];
            float[] nearEdgeList = { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
            for (int row = 0; row < sizeY; row++)
            {
                for (int col = 0; col < sizeX; col++)
                {
                    //else if (row < 5 && col < 5) multiplierForOceanBorder = Math.Min(1f - (5 - row) / 5f, 1f - (5 - col) / 5f);
                    //else if (row > (sizeY - 6) && col > (sizeX - 6)) multiplierForOceanBorder = Math.Min((sizeY - 1 - row) / 5f, (sizeX - 1 - col) / 5f);

                    //else if (row < 5 && col < 5) multiplierForOceanBorder = Math.Min(1f - (5 - row) / 5f, 1f - (5 - col) / 5f);
                    //else if (row < 5) multiplierForOceanBorder = 1f - (5 - row) / 5f;
                    //else if (col < 5) multiplierForOceanBorder = 1f - (5 - col) / 5f;

                    //else if (row > (sizeY - 6) && col > (sizeX - 6)) multiplierForOceanBorder = Math.Min((sizeY - 1 - row) / 5f, (sizeX - 1 - col) / 5f);
                    //else if (row > (sizeY - 6)) multiplierForOceanBorder = (sizeY - 1 - row)/5f;
                    //else if (col > (sizeX - 6)) multiplierForOceanBorder = (sizeX - 1 - col)/5f;

                    float rowMult = 1f;
                    if (row < 5) rowMult = nearEdgeList[row];
                    if (row > (sizeY - 6)) rowMult = nearEdgeList[sizeY - 1 - row];

                    float colMult = 1f;
                    if (col < 5) colMult = nearEdgeList[col];
                    if (col > (sizeX - 6)) colMult = nearEdgeList[sizeX - 1 - col];

                    elevationGrid[row, col] = noiseGen.getOctaveRand(row + 1000, col + 1000) * rowMult * colMult;
                }
            }

            //DEFINE PRECIPITATION
            percipitationGrid = new float[sizeY, sizeX];
            for (int row = 0; row < sizeY; row++)
            {
                for (int col = 0; col < sizeX; col++)
                {
                    float newRainValue = noiseGen.getOctaveRand(row + 2000, col + 2000);
                    percipitationGrid[row, col] = newRainValue;
                }
            }

            //DEFINE TEMPERATURE
            temperatureGrid = new float[sizeY, sizeX];
            for (int row = 0; row < sizeY; row++)
            {
                for (int col = 0; col < sizeX; col++)
                {
                    float newTempValue = noiseGen.getOctaveRand(row + 3000, col + 3000);
                    temperatureGrid[row, col] = newTempValue;
                }
            }

            //DEFINE FINAL BIOMES
            for (int row = 0; row < sizeY; row++)
            {
                for (int col = 0; col < sizeX; col++)
                {
                    //worldMap.map[row, col] = new MapTile(noiseGen.getOctaveRandInt(row, col, 9));

                    //low lying land is a water basin
                    if (elevationGrid[row, col] < 0.3)
                    {
                        worldMap.map[row, col] = new MapTile("water");
                    }
                    //high land is mountains
                    else if(elevationGrid[row, col] > 0.7)
                    {
                        worldMap.map[row, col] = new MapTile("mountain");
                    }
                    //low water is desert (unless it's cold, in which case it's tundra)
                    else if (percipitationGrid[row, col] < 0.3 && temperatureGrid[row, col] > 0.32)
                    {
                        worldMap.map[row, col] = new MapTile("desert");
                    }
                    //lots of rain creates a water basin
                    else if (percipitationGrid[row, col] > 0.8)
                    {
                        worldMap.map[row, col] = new MapTile("water");
                    }
                    //large temps is also desert
                    else if (temperatureGrid[row, col] > 0.7)
                    {
                        worldMap.map[row, col] = new MapTile("desert");
                    }
                    //low temps causes snow
                    else if (temperatureGrid[row, col] < 0.32)
                    {
                        worldMap.map[row, col] = new MapTile("tundra");
                    }
                    //a good amount of rain makes a forest
                    else if (percipitationGrid[row, col] > 0.6)
                    {
                        worldMap.map[row, col] = new MapTile("forest");
                    }
                    //the world's default ranges make grassland
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
