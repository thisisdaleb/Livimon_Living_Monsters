using System;

namespace LivimonWPF
{
    public static class Utility
    {
        static public float linearInterpolate(float _a, float _b, float _delta)
        {
            return (_a * (1 - _delta) + _b * _delta);
        }
        
        static public float smoothInterpolate(float _a, float _b, float _delta)
        {
            float deltaPi = _delta * 3.1415927f;
            float f = (float)(1 - Math.Cos(deltaPi)) * 0.5f;

            return (float)(_a * (1 - f) + _b * f);
        }
    }
}
