using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

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
        
        public Dictionary<string, MapTileType> biomeList;

        protected override Map makeMap()
        {
            Map worldMap = new Map(sizeY, sizeX);

            //make dict of biomes
            biomeList = new Dictionary<string, MapTileType>();
            MapTileType water = new MapTileType("water", Brushes.Blue);
            water.makeUnpassable();
            biomeList.Add("water", water);
            biomeList.Add("grassland", new MapTileType("grassland", Brushes.Lime));
            biomeList.Add("forest", new MapTileType("forest", Brushes.Green));
            biomeList.Add("desert", new MapTileType("desert", Brushes.Yellow));
            biomeList.Add("tundra", new MapTileType("tundra", Brushes.White));
            biomeList.Add("mountain", new MapTileType("mountain", Brushes.Brown));

            //some reasonable value examples are 0.1 for 50 with 9 types, 0.05 for 100,
            NoiseGenerator noiseGen = new NoiseGenerator(0.035f);


            //DEFINE ELEVATION
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
                                                        //your sample code
            elevationGrid = new float[sizeY, sizeX];
            float[] nearEdgeList = { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
            for (int row = 0; row < sizeY; row++)
            {
                for (int col = 0; col < sizeX; col++)
                {
                    float rowMult = 1f;
                    if (row < 5) rowMult = nearEdgeList[row];
                    if (row > (sizeY - 6)) rowMult = nearEdgeList[sizeY - 1 - row];

                    float colMult = 1f;
                    if (col < 5) colMult = nearEdgeList[col];
                    if (col > (sizeX - 6)) colMult = nearEdgeList[sizeX - 1 - col];

                    //okay, now to try something weird
                    //if row and col are within 25% to 75% of matrix size, don't change anything
                    //else if col within range, just get row distance and do math so that the edge tile is 100% water and it scales up to no change
                    //else if row etc.
                    //else if (this means we're on a corner tile), find the distance to the corner of the 25-75%, that's your multiplier
                    float multiplier = 1f;
                    int percent25 = sizeX / 4;
                    //making both sides the same halves the if statements
                    int distanceToVerticWall = row < percent25 ? row : sizeY - 1 - row;
                    int distanceToHorizWall = col < percent25 ? col : sizeX - 1 - col;

                    //if(row < percent25 || row > percent75)
                    //    if (col < percent25 || col > percent75)
                    if(distanceToVerticWall == 0 || distanceToHorizWall == 0)
                    {
                        multiplier = 0;
                    }
                    else if (distanceToVerticWall < percent25)
                    {
                        if (distanceToHorizWall < percent25)
                        {
                            //corners
                            float distToPerc25Vert = (float)(percent25 - distanceToVerticWall);
                            float distToPerc25Horiz = (float)(percent25 - distanceToHorizWall);
                            distToPerc25Vert /= percent25;
                            distToPerc25Horiz /= percent25;
                            multiplier = (float) Math.Max(1f - Math.Sqrt(distToPerc25Vert* distToPerc25Vert + distToPerc25Horiz*distToPerc25Horiz), 0f);
                        }
                        else
                        {
                            //top and bottom walls
                            multiplier = (float)(distanceToVerticWall) / percent25; //completely linear
                        }
                    }
                    else if (distanceToHorizWall < percent25)
                    {
                        //left and right walls
                        multiplier = (float)(distanceToHorizWall) / percent25; //completely linear
                    }

                    elevationGrid[row, col] = noiseGen.getOctaveRand(row + 1000, col + 1000, 1f, 4, 0.5f) * multiplier;
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
                    worldMap.map[row, col] = getMapTileForCell(row, col);
                }
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            return worldMap;
        }

        private MapTile getMapTileForCell(int row, int col)
        {
            //low lying land is a water basin
            if (elevationGrid[row, col] < 0.3)
            {
                return new MapTile(biomeList["water"]);
            }
            //high land is mountains
            if (elevationGrid[row, col] > 0.7)
            {
                return new MapTile(biomeList["mountain"]);
            }
            //low water is desert (unless it's cold, in which case it's tundra)
            if (percipitationGrid[row, col] < 0.3 && temperatureGrid[row, col] > 0.4)
            {
                return new MapTile(biomeList["desert"]);
            }
            //lots of rain creates a water basin
            if (percipitationGrid[row, col] > 0.8)
            {
                return new MapTile(biomeList["water"]);
            }
            //large temps is also desert
            if (temperatureGrid[row, col] > 0.7)
            {
                return new MapTile(biomeList["desert"]);
            }
            //low temps causes snow
            if (temperatureGrid[row, col] < 0.32)
            {
                return new MapTile(biomeList["tundra"]);
            }
            //a good amount of rain makes a forest
            if (percipitationGrid[row, col] > 0.6)
            {
                return new MapTile(biomeList["forest"]);
            }

            //the world's default ranges make grassland
            return new MapTile(biomeList["grassland"]);
        }

        public int[] getStartingPosition()
        {
            int notWater = 0;
            for (int row = 0; row < sizeY; row++)
            {
                for (int col = 0; col < sizeX; col++)
                {
                    if (map.map[row, col].getTypeName() != "water") notWater++;
                }
            }

            System.Diagnostics.Debug.WriteLine("not water: " + notWater);

            for (int row = 20; row < sizeY; row++)
            {
                for (int col = 0; col < sizeX; col++)
                {
                    if (map.map[row, col].getTypeName() == "grassland") return new int[2] { row, col };
                }
            }
            return new int[2] { 0, 0 };
        }
    }
}
