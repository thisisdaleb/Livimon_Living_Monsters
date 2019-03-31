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

        public NoiseGenerator(int _seed)
        {
            System.Diagnostics.Debug.WriteLine("Seed: " + _seed);
            simplex = new SimplexNoiseGenerator(_seed);
        }

        public NoiseGenerator()
        {
            int seed = (new Random()).Next(1, 500000);
            System.Diagnostics.Debug.WriteLine("Random Seed: " + seed);
            simplex = new SimplexNoiseGenerator(seed);
        }

        public int getRand(int _x, int _y, int _maxValue)
        {
            return (int)Math.Floor((simplex.Evaluate(_x * 0.1, _y * 0.1) + 1) / 2 * (_maxValue + 1));
        }
    }
}
