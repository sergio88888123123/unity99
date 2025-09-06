#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class AttachScriptsUtility
{
    [MenuItem("Tools/Interactive Book/Attach Scripts to Selection")]
    public static void AttachToSelection()
    {
        var selection = Selection.gameObjects;
        if (selection == null || selection.Length == 0)
        {
            EditorUtility.DisplayDialog("Attach Scripts", "No hay objetos seleccionados.", "OK");
            return;
        }

        int count = 0;
        foreach (var go in selection)
        {
            Undo.RegisterCompleteObjectUndo(go, "Attach Scripts");

            var img = go.GetComponent<Image>();
            if (img && go.GetComponent<DragSpriteUI>() == null) { go.AddComponent<DragSpriteUI>(); count++; }

            var sr = go.GetComponent<SpriteRenderer>();
            if (sr && go.GetComponent<DragSprite2D>() == null) { go.AddComponent<DragSprite2D>(); count++; }

            var canvas = go.GetComponent<Canvas>();
            if (canvas && go.GetComponent<NavigationBinder>() == null) { go.AddComponent<NavigationBinder>(); count++; }
        }

        EditorUtility.DisplayDialog("Attach Scripts", $"Se adjuntaron scripts a {count} componentes.", "OK");
    }
}
#endif
