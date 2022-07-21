using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class UtilityStatic
{
    public static GameObject GetFirstChild(this GameObject obj)
    {
        foreach (Transform t in obj.transform)
        {
            return t.gameObject;
        }
        return null;  // Will never happen.
    }

    public static T ChooseRandomObject<T>(this List<T> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }

    public static string ToStringBetter<T>(this List<T> list)
    {
        StringBuilder sb = new StringBuilder();
        foreach(T item in list)
        {
            sb.Append(item);
        }

        return sb.ToString();
    }
}
