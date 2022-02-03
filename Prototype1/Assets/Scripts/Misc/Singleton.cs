using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get {
            if (instance == null) {
                GenerateInstance();
            }
            return instance;
        }
    }

    void Start() {
        if (instance == null) {
            GenerateInstance();
        }
    }

    private static void GenerateInstance()
    {
        instance = FindObjectOfType<T>();

        if (instance == null) {
            GameObject obj = new GameObject();
            obj.name = typeof(T).Name;
            instance = obj.AddComponent<T>();
        }        
    }
}
