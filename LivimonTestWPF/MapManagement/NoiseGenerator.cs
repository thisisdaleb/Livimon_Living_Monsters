using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivimonTestWPF
{
    //this is just an interface for the Simplex Noise Generator, as it's
    public class NoiseGenerator
    {
        private SimplexNoiseGenerator simplex;
        private List<float[,]> noiseMat = new List<float[,]>();
        private int[] matSizes;
        private int layersNeeded;
        private float persistence;

        public NoiseGenerator(int _maxX, int _maxY, int _seed = 0, float _persistance = 0.8f)
        {
            int seed = _seed;
            if (seed == 0) seed = (new Random()).Next(1, 5000);
            System.Diagnostics.Debug.WriteLine("Seed: " + seed);
            persistence = _persistance;

            //set list of matrix sizes used for math
            layersNeeded = Convert.ToInt32(Math.Ceiling( Math.Max(Math.Log(_maxX, 2), Math.Log(_maxY, 2)) ));
            matSizes = new int[layersNeeded];
            for (int mats = 0; mats < (matSizes.Length); mats++)
            {
                matSizes[mats] = 2 * ((int)Math.Pow(2, mats));
            }

            //get Simplex Matrices
            simplex = new SimplexNoiseGenerator(seed);
            noiseMat = new List<float[,]>();
            for (int noiseNumber = 0; noiseNumber < layersNeeded; noiseNumber++)
            {
                noiseMat.Add(new float[matSizes[noiseNumber], matSizes[noiseNumber]]);
                for (int y = 0; y < matSizes[noiseNumber]; y++)
                {
                    for (int x = 0; x < matSizes[noiseNumber]; x++)
                    {
                        noiseMat[noiseNumber][y, x] = (float)((simplex.Evaluate(x, y) + 1) / 2);
                        //the extra parts around it normalize the -1 to 1 to 0 to 1
                    }
                }
            }

        }

        //helper function
        private float smoothInterpolate(float a, float b, float x)
        {
            float ft = x * 3.1415927f;
            float f = (float)(1 - Math.Cos(ft)) * 0.5f;
            return (float)(a * (1 - f) + b * f);
        }

        public float getNoise(int _x, int _y, float _maxValue = 1f)
        {

            float noiseValue = 0f;

            for (int noiseNumber = 0; noiseNumber < layersNeeded; noiseNumber++)
            {
                float valueToAdd = 0f;
                int sizeOfCurrentLayer = matSizes[noiseNumber];
                if(noiseNumber == layersNeeded - 1)
                {
                    valueToAdd  = noiseMat[noiseNumber][_y, _x];
                }
                else
                {
                    int lengthOfLongestY = noiseMat[layersNeeded - 1].GetLength(0);
                    int lengthOfLongestX = noiseMat[layersNeeded - 1].GetLength(1);
                    int lengthOfCurrentY = noiseMat[noiseNumber].GetLength(0);
                    int lengthOfCurrentX = noiseMat[noiseNumber].GetLength(1);

                    int divisorY = lengthOfLongestY / lengthOfCurrentY;
                    int divisorX = lengthOfLongestX / lengthOfCurrentX;

                    if (_x % divisorX == 0 && _y % divisorY == 0)
                    {
                        //the exact square exists in the smaller matrix and can be directly grabbed
                        valueToAdd = noiseMat[noiseNumber][_y / divisorY, _x / divisorX];
                    }
                    else
                    {
                        //need to grab the 4 nearest cells, and interpolate between them all
                        //get the 2 y-axis interpolations, then interpolate along the x those 2 values

                        int lowerY = (int)Math.Floor((double)_y / divisorY);
                        int lowerX = (int)Math.Floor((double)_x / divisorX);
                        int higherY = lowerY + 1;
                        int higherX = lowerX + 1;

                        float distanceBetweenY = (float)(_y % divisorY) / divisorY;
                        if (distanceBetweenY == 0) distanceBetweenY = 1;

                        float lowerXlowerYValue    = noiseMat[noiseNumber][lowerY, lowerX];
                        float lowerXHigherYValue   = noiseMat[noiseNumber][higherY, lowerX];

                        float higherXlowerYValue = noiseMat[noiseNumber][lowerY, higherX];
                        float higherXHigherYValue = noiseMat[noiseNumber][higherY, higherY];

                        float lowerXInterpolation  = smoothInterpolate(lowerXlowerYValue, lowerXHigherYValue, distanceBetweenY);
                        float higherXInterpolation = smoothInterpolate(higherXlowerYValue, higherXHigherYValue, distanceBetweenY);

                        float distanceBetweenX = (float)(_x % divisorX)/ divisorX;
                        if (distanceBetweenX == 0) distanceBetweenX = 1;

                        valueToAdd = smoothInterpolate(lowerXInterpolation, higherXInterpolation, distanceBetweenX);
                    }
                }

                //we divide the number by layersNeeded because this normalizes all the values
                //i.e if all 8 values grabbed were 1, then you'd have 8, so dividing it by 8 gives you you the
                //noiseValue += ((valueToAdd * (float)(Math.Pow(persistence, noiseNumber))) / layersNeeded);
                if (valueToAdd > 1)
                {
                    System.Diagnostics.Debug.WriteLine("Single step number above 1: " + valueToAdd);
                }
                noiseValue += ((valueToAdd * (float)(Math.Pow(persistence, noiseNumber))));
                //noiseValue += valueToAdd/layersNeeded;
            }
            if(noiseValue > 1)
            {
                System.Diagnostics.Debug.WriteLine("full number above num: " + noiseValue);
            }
            return noiseValue * _maxValue * 0.4f;
        }

        public int getDiscreteNoise(int _x, int _y, int _maxValue )
        {
            float noise = getNoise(_x, _y, _maxValue);
            //System.Diagnostics.Debug.WriteLine("" + noise);
            return (int) Math.Floor(noise); //fun fact, every value has a 1 in a million chance of being the value below it, except 0
        }
    }
}
