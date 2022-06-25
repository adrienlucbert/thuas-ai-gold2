using System;

public static class RNG
{
    private static Random _rng = null;
    private static Random rng
    {
        get
        {
            if (RNG._rng == null)
                RNG._rng = new Random();
            return RNG._rng;
        }
        set => RNG._rng = value;
    }

    public static void Seed(int seed)
    {
        RNG.rng = new Random(seed);
    }

    public static int Range(int min, int max)
    {
        return RNG.rng.Next(min, max);
    }

    public static float Range(float min, float max, uint precision = 4)
    {
        float factor = (float)Math.Pow(10f, precision);
        return RNG.Range((int)(min * factor), (int)(max * factor)) / factor;
    }

    public static float Next()
    {
        return (float)RNG.rng.NextDouble();
    }
}
