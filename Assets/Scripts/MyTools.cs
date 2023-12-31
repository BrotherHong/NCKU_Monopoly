using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class MyTools
{
    public static float Distance2D(Vector3 a, Vector3 b)
    {
        return (float) Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.z - b.z, 2));
    }

    public static Color ColorFromHex(string colorCode)
    {
        string r_str = colorCode.Substring(1, 2);
        string g_str = colorCode.Substring(3, 2);
        string b_str = colorCode.Substring(5, 2);

        int r = int.Parse(r_str, NumberStyles.HexNumber);
        int g = int.Parse(g_str, NumberStyles.HexNumber);
        int b = int.Parse(b_str, NumberStyles.HexNumber);

        return new Color(r/255f, g/255f, b/255f);
    }
}
