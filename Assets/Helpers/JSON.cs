using UnityEngine;

struct Serializer<T> {
    public T data;
}

public static class JSON
{
    public static string SerializeArray<T>(T[] arr)
    {
        Serializer<T[]> serializer = new Serializer<T[]>();
        serializer.data = arr;
        return JsonUtility.ToJson(serializer);
    }

    public static T[] ParseArray<T>(string str)
    {
        return JsonUtility.FromJson<Serializer<T[]>>(str).data;
    }
}