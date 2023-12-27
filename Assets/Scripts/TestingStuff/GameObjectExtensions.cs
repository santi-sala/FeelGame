using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions 
{
    public static void SetRandomColor(this GameObject go)
    {
        if (go.TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = Random.ColorHSV();
        }
        else
        {
            Debug.Log("No sprite renderer");
        }
    }
}
