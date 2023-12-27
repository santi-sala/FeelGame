using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
public class MenuColorRandomizer : MonoBehaviour
{
    [MenuItem("Tools/Colorize")]
    static void Colorize()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.TryGetComponent(out Renderer renderer))
            {
                renderer.sharedMaterial.color = Random.ColorHSV();
            }
        }
    }

    [MenuItem("Tools/Organize Hierarchy")]
    static void OrganizeHierarchy()
    {
        new GameObject("--- ENVIRONEMNT ---");
        new GameObject(" ");

        new GameObject("--- GAMEPLAY ---");
        new GameObject(" ");

        new GameObject("--- UI ---");
        new GameObject(" ");

        new GameObject("--- MANAGERS ---");
    }

    [MenuItem("Tools/Reload Scene")]
    static void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

}
#endif