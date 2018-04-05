using System;
[Serializable]
public class FloatRange
{
    public float min;
    public float max;

    public FloatRange(float mini, float maxi)
    {
        min = mini;
        max = maxi;
    }
    public float Random
    {
        get { return UnityEngine.Random.Range(min, max); }
    }
    public float Set(float mult)
    {
        return min + (max-min) * mult;
    }
}
