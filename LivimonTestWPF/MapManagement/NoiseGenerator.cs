using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivimonTestWPF
{
    //this is just an interface for the Simplex Noise Generator
    //currently it doesn't use octaves, which could reduce the straight edges you can find in the map
    public class NoiseGenerator
    {
        private SimplexNoiseGenerator simplex;
        private float frequency;

        public NoiseGenerator(int _seed, float _frequency = 0.5f)
        {
            System.Diagnostics.Debug.WriteLine("Seed: " + _seed);
            frequency = _frequency;
            simplex = new SimplexNoiseGenerator(_seed);
        }

        public NoiseGenerator(float _frequency = 0.5f)
        {
            int seed = (new Random()).Next(int.MinValue, int.MaxValue);
            System.Diagnostics.Debug.WriteLine("Random Seed: " + seed);
            frequency = _frequency;
            simplex = new SimplexNoiseGenerator(seed);
        }

        public double lowestNum = 1f;
        public double largestNum = -1f;
        public bool debugging = false;

        //simplex does not return full values between -1 and 1, somewhere around -0.864 and 0.864
        //my values here return between around 0.006 and 0.994
        public float getSimplexNoise(float _x, float _y)
        {
            if (!debugging)
            {
                return ((float) simplex.Evaluate(_x, _y) + 0.875f) / 1.75f;
            }
            else
            {
                double eval = simplex.Evaluate(_x, _y);
                double newSample = (eval + 0.875) / 1.75;
                if (eval == 0)
                {
                    System.Diagnostics.Debug.WriteLine("EVAL 0: X=" + _x + ", Y=" + _y);
                }
                if (eval < lowestNum)
                {
                    lowestNum = eval;
                    System.Diagnostics.Debug.WriteLine("Lowest num is " + eval + " === " + "MOD LOW IS " + newSample);
                }
                if (eval > largestNum)
                {
                    largestNum = eval;
                    System.Diagnostics.Debug.WriteLine("Largest num is " + eval + " === " + "MOD HIGH is " + newSample);
                }
                if (newSample < 0 || newSample > 1)
                {
                    throw new ArithmeticException("RANDOM NOISE FAILED " + newSample);
                }
                return (float) newSample;
            }
        }

        //https://cmaher.github.io/posts/working-with-simplex-noise/
        public float getOctaveRand(int _x, int _y, float _maxValue = 1, int _octaves = 4, float _persistence = 0.5f)
        {
            float maxAmp = 0;
            float amp = 1;
            float noise = 0;
            float freq = frequency;

            //add successively smaller, higher-frequency terms
            for (int i = 0; i < _octaves; ++i)
            {
                noise += getSimplexNoise(_x * freq, _y * freq) * amp;
                maxAmp += amp;
                amp *= _persistence;
                freq *= 2;
            }

            //take the average value of the iterations
            noise = noise / maxAmp;

            //normalize the result
            noise = noise * _maxValue;

            //System.Diagnostics.Debug.WriteLine(noise);
            return noise;
        }
        public int getOctaveRandInt(int _x, int _y, int _maxValue = 1, int _octaves = 4, float _persistence = 0.5f)
        {
            return (int) Math.Floor(getOctaveRand(_x, _y, _maxValue + 1, _octaves, _persistence));
        }

        public float getRand(int _x, int _y, int _maxValue = 1, int _octave = 1)
        {
            //no max value will return a boolean 0 or 1
            int octAdd = _octave * 1000 - 999; //+1 if no octave passed in, +1001 if 2, +2001 if 3, etc.
            return getSimplexNoise(_x * frequency + octAdd, _y * frequency + octAdd) * _maxValue;
        }
        public int getRandInt(int _x, int _y, int _maxValue = 1, int _octave = 1)
        {
            return (int) Math.Floor(getRand(_x, _y, _maxValue + 1, _octave));
        }
    }
}
