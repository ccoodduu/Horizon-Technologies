using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T Random<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
}
