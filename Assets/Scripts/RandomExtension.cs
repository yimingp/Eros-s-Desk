using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class RandomExtension
{
    public static T TakeRandomElementFromList<T>(this List<T> list)
    {
        if (list is null || list.Count <= 0)
        {
            throw new Exception("list broken");
        }
        else
            return list[Random.Range(0, list.Count)];
    }      
}