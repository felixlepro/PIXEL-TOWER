using System;
[Serializable]
public class IntRange {
    public int min;
    public int max;

    public IntRange(int mini, int maxi)
    {
        min = mini;
        max = maxi;
    }
    public int Random
    {
        get { return UnityEngine.Random.Range(min, max); }
    }
}
