using System;
using UnityEngine;

public static class PointsDatabase
{
    public enum Field
    {
        Racer, Nonogram, Arcanoid, Frogger
    }

    public static int SumAllPoints()
    {
        int sum = 0;
        foreach(Field field in Enum.GetValues(typeof(Field)))
        {
            sum += Load(field);
        }

        return sum;
    }

    public static void Save(Field field, int value)
    {
        PlayerPrefs.SetInt(field.ToString(), value);
        PlayerPrefs.Save();
    }

    public static void SaveAdditively(Field field, int value) {
        Save(field, Load(field) + value);
    }

    public static int Load(Field field)
    {
        return PlayerPrefs.GetInt(field.ToString());
    }
}
