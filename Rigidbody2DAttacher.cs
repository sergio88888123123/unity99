#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class Rigidbody2DAttacher
{
    [MenuItem("Tools/Interactive Book/Physics2D/Ensure Rigidbody2D + Collider2D")]
    public static void EnsurePhysics2D()
    {
        var sel = Selection.gameObjects;
        if (sel == null || sel.Length == 0)
        {
            EditorUtility.DisplayDialog("Physics2D", "Selecciona uno o más objetos.", "OK");
            return;
        }

        int changed = 0;
        foreach (var go in sel)
        {
            Undo.RegisterCompleteObjectUndo(go, "Ensure Physics2D");

            var rb = go.GetComponent<Rigidbody2D>();
            if (!rb) { rb = Undo.AddComponent<Rigidbody2D>(go); rb.bodyType = RigidbodyType2D.Kinematic; changed++; }

            var sr = go.GetComponent<SpriteRenderer>();
            if (sr)
            {
                if (!go.GetComponent<Collider2D>())
                {
                    Undo.AddComponent<BoxCollider2D>(go);
                    changed++;
                }
            }
        }

        EditorUtility.DisplayDialog("Physics2D", $"Actualizados {changed} componentes.", "OK");
    }
}
#endif
