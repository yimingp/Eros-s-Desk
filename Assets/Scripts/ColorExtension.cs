using UnityEngine;

public static class ColorExtension
{
    public enum ColorChannelType
    {
        R,
        G,
        B,
        A
    }
    
    public static Color ModifiedRChannel(this Color c, float val)
    {
        return new Color(val, c.g, c.b, c.a);
    }
    public static Color ModifiedGChannel(this Color c, float val)
    {
        return new Color(c.r, val, c.b, c.a);
    }
    public static Color ModifiedBChannel(this Color c, float val)
    {
        return new Color(c.r, c.g, val, c.a);
    }
    public static Color ModifiedAlpha(this Color c, float val)
    {
        return new Color(c.r, c.g, c.b, val);
    }
}