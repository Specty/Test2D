using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer render;
    void Start()
    {
       
    }

    public void DrawLines(string[] arr)
    {
        int x, y;
        Vector3 vec3;
        for (int i = 0; i < arr.Length; i++)
        {
            x = Convert.ToInt32(arr[i].Substring(0, arr[i].IndexOf(","))) / 100;
            y = Convert.ToInt32(arr[i].Substring(arr[i].IndexOf(",") + 1)) / 100;
            vec3 = new Vector3(x, y, 0);
        }
    }
}
