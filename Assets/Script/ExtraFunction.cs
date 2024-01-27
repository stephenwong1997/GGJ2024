using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public static class ExtraFunction
{
#if UNITY_EDITOR
    public static T[] FindAssetsByType<T>(params string[] folders) where T : Object
    {
        string type = typeof(T).ToString().Replace("UnityEngine.", "");

        string[] guids;
        if (folders == null || folders.Length == 0)
        {
            guids = AssetDatabase.FindAssets("t:" + type);
        }
        else
        {
            guids = AssetDatabase.FindAssets("t:" + type, folders);
        }

        T[] assets = new T[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
        return assets;
    }
#endif

    public static int GetRandomArrayNum(this int[] array)
    {
        int[] levelCount = new int[array.Length];
        int totalChance = 0;


        for (int count = 0; count < array.Length; count++)
        {
            totalChance += array[count];
            levelCount[count] = totalChance;
        }

        int num = Random.Range(1, totalChance);

        for (int x = 0; x < array.Length; x++)
        {
            if (num <= levelCount[x])
            {
                return x;
            }
        }

        return -1;
    }

    public static int GetRandomArrayNum(this float[] array)
    {
        float[] levelCount = new float[array.Length];
        float totalChance = 0;


        for (int count = 0; count < array.Length; count++)
        {
            totalChance += array[count];
            levelCount[count] = totalChance;
        }

        float num = Random.Range(0, totalChance);

        for (int x = 0; x < array.Length; x++)
        {
            if (num <= levelCount[x])
            {
                return x;
            }
        }


        Debug.Log("Random Error");
        return -1;
    }


    public static int LimitValue(this int value, int min, int max)
    {
        if (value < min)
        {
            return min;
        }
        else if (value > max)
        {
            return max;
        }
        else
        {
            return value;
        }
    }

    public static float LimitValue(this float value, float min, float max)
    {
        if (value < min)
        {
            return min;
        }
        else if (value > max)
        {
            return max;
        }
        else
        {
            return value;
        }
    }

    public static List<T> GetRandomObjsInList<T>(this List<T> list, int count)
    {
        var newList = new List<T>();

        list.ShuffleList();

        for (int i = 0; i < count; i++)
        {
            newList.Add(list[i]);
        }

        return newList;
    }


    public static T GetRandomObjInList<T>(this List<T> list) where T : Object
    {
        if (list.Count <= 0)
            return null;
        return list[Random.Range(0, list.Count)];
    }


    public static T GetRandomObjInArray<T>(this T[] list) where T : Object
    {
        if (list.Length <= 0)
            return null;
        return list[Random.Range(0, list.Length)];
    }

    public static int GetArrayLoop<T>(this List<T> list, int index, bool next)
    {

        if (next && index + 1 >= list.Count)
        {
            return 0;
        }
        else if (!next && index - 1 < 0)
        {
            return list.Count - 1;
        }

        return index + (next ? 1 : -1);


    }


    public static int GetRandomArrayNum(this List<int> array)
    {
        return array.ToArray().GetRandomArrayNum();
    }

    public static int GetRandomArrayNum(this List<float> array)
    {
        return array.ToArray().GetRandomArrayNum();
    }

    public static void MoveItemInList<T>(this List<T> list, int oldIndex, int newIndex)
    {
        T item = list[oldIndex];
        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);

    }


    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static T DeepClone<T>(this T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
    public static T Pop<T>(this List<T> obj, int index = 0)
    {
        T member = obj[index];
        obj.RemoveAt(index);
        return member;
    }

    public static void ShuffleList<T>(this List<T> obj)
    {
        for (int i = 0; i < obj.Count; i++)
        {
            MoveItemInList(obj, i, Random.Range(i, obj.Count));
        }
        for (int i = obj.Count - 1; i > 0; i--)
        {
            MoveItemInList(obj, i, Random.Range(0, i));
        }

    }

    public static int Difference(int num1, int num2)
    {
        int cout;
        cout = Mathf.Max(num2, num1) - Mathf.Min(num1, num2);
        return cout;
    }

}
