#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Genera una escena base con:
/// - Canvas + CanvasScaler (ScaleWithScreenSize) y controlador de aspecto.
/// - Panel SafeArea para muescas/notches.
/// - 3 páginas de ejemplo con navegación (Prev/Next) y etiqueta de página.
/// - Barra inferior con botones anclados.
/// - Puntos de anclaje (esquinas y centro) para demostrar anchors.
/// 
/// USO:
/// Menú: Tools ▸ Interactive Book ▸ Create Starter Scene
/// </summary>
public static class InteractiveBookSceneBuilder
{
    const int RefWidth = 1080;
    const int RefHeight = 1920;

    [MenuItem("Tools/Interactive Book/Create Starter Scene")]
    public static void CreateStarterScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "InteractiveBook_Starter";

        // EventSystem
        var es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        Undo.RegisterCreatedObjectUndo(es, "Create EventSystem");

        // Canvas
        var canvasGO = new GameObject("Canvas",
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster),
            typeof(CanvasAspectController));
        Undo.RegisterCreatedObjectUndo(canvasGO, "Create Canvas");
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(RefWidth, RefHeight);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        var aspectCtrl = canvasGO.GetComponent<CanvasAspectController>();
        aspectCtrl.designAspect = (float)RefWidth / RefHeight;

        // FONDO raíz (opcional)
        var bg = CreateUIObject("Background", canvasGO.transform);
        var bgImg = bg.AddComponent<Image>();
        bgImg.color = new Color(0.1f, 0.1f, 0.12f, 1f);
        StretchFull(bg.GetComponent<RectTransform>());

        // SafeArea
        var safe = CreateUIObject("SafeArea", canvasGO.transform);
        StretchFull(safe.GetComponent<RectTransform>());
        safe.AddComponent<SafeArea>();

        // Contenido dentro de SafeArea
        var content = CreateUIObject("Content", safe.transform);
        StretchFull(content.GetComponent<RectTransform>());

        // Páginas
        var pagesRoot = CreateUIObject("Pages", content.transform);
        StretchFull(pagesRoot.GetComponent<RectTransform>());

        // Crear 3 páginas demo
        for (int i = 0; i < 3; i++)
        {
            var page = CreateUIObject($"Page_{i + 1}", pagesRoot.transform);
            StretchFull(page.GetComponent<RectTransform>());
            var img = page.AddComponent<Image>();
            img.color = Color.Lerp(new Color(0.15f, 0.18f, 0.22f), new Color(0.25f, 0.28f, 0.32f), i / 2f);

            // Demo: título en cada página
            var title = CreateUIObject("Title", page.transform);
            var titleRt = title.GetComponent<RectTransform>();
            titleRt.anchorMin = new Vector2(0.5f, 1f);
            titleRt.anchorMax = new Vector2(0.5f, 1f);
            titleRt.pivot = new Vector2(0.5f, 1f);
            titleRt.anchoredPosition = new Vector2(0, -60);
            var t = title.AddComponent<Text>();
            t.text = $"Página {i + 1}";
            t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            t.alignment = TextAnchor.MiddleCenter;
            t.fontSize = 48;
            t.color = Color.white;
            var layout = title.AddComponent<ContentSizeFitter>();
            layout.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            layout.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            if (i > 0) page.SetActive(false);
        }

        // Barra inferior (navegación)
        var navbar = CreateUIObject("Navbar", content.transform);
        var navRt = navbar.GetComponent<RectTransform>();
        navRt.anchorMin = new Vector2(0f, 0f);
        navRt.anchorMax = new Vector2(1f, 0f);
        navRt.pivot = new Vector2(0.5f, 0f);
        navRt.sizeDelta = new Vector2(0, 140f);
        navRt.anchoredPosition = Vector2.zero;

        var navBg = navbar.AddComponent<Image>();
        navBg.color = new Color(0f, 0f, 0f, .35f);

        // Botón Prev (izquierda)
        var prevBtn = CreateButton("PrevButton", navbar.transform, "<  Anterior");
        var prevRt = prevBtn.GetComponent<RectTransform>();
        prevRt.anchorMin = new Vector2(0f, 0f);
        prevRt.anchorMax = new Vector2(0f, 1f);
        prevRt.pivot = new Vector2(0f, 0.5f);
        prevRt.sizeDelta = new Vector2(340f, -40f);
        prevRt.anchoredPosition = new Vector2(20f, 0f);

        // Botón Next (derecha)
        var nextBtn = CreateButton("NextButton", navbar.transform, "Siguiente  >");
        var nextRt = nextBtn.GetComponent<RectTransform>();
        nextRt.anchorMin = new Vector2(1f, 0f);
        nextRt.anchorMax = new Vector2(1f, 1f);
        nextRt.pivot = new Vector2(1f, 0.5f);
        nextRt.sizeDelta = new Vector2(340f, -40f);
        nextRt.anchoredPosition = new Vector2(-20f, 0f);

        // Etiqueta de página (centro)
        var label = CreateUIObject("PageLabel", navbar.transform);
        var labelRt = label.GetComponent<RectTransform>();
        labelRt.anchorMin = new Vector2(0.5f, 0f);
        labelRt.anchorMax = new Vector2(0.5f, 1f);
        labelRt.pivot = new Vector2(0.5f, 0.5f);
        labelRt.sizeDelta = new Vector2(300f, -40f);
        var labelText = label.AddComponent<Text>();
        labelText.text = "1/3";
        labelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        labelText.alignment = TextAnchor.MiddleCenter;
        labelText.fontSize = 32;
        labelText.color = Color.white;

        // PageNavigator
        var nav = pagesRoot.AddComponent<PageNavigator>();
        nav.pagesRoot = pagesRoot.GetComponent<RectTransform>();
        nav.prevButton = prevBtn.GetComponent<Button>();
        nav.nextButton = nextBtn.GetComponent<Button>();
        nav.pageLabel = labelText;
        nav.loop = false;

        // Puntos de anclaje (gizmos) para mostrar anchors
        CreateAnchorDot("Anchor_TopLeft",  new Vector2(0f, 1f), new Vector2(20, -20), content.transform);
        CreateAnchorDot("Anchor_TopRight", new Vector2(1f, 1f), new Vector2(-20, -20), content.transform);
        CreateAnchorDot("Anchor_BotLeft",  new Vector2(0f, 0f), new Vector2(20, 20), content.transform);
        CreateAnchorDot("Anchor_BotRight", new Vector2(1f, 0f), new Vector2(-20, 20), content.transform);
        CreateAnchorDot("Anchor_Center",   new Vector2(.5f, .5f), Vector2.zero, content.transform, 28f);

        // Seleccionar canvas en jerarquía para que sea evidente
        Selection.activeObject = canvasGO;

        // Marcar escena como sucia para guardar
        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("Interactive Book: escena inicial creada.");
    }

    static GameObject CreateUIObject(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(go, "Create UI Object");
        go.transform.SetParent(parent, false);
        return go;
    }

    static void StretchFull(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0.5f);
    }

    static GameObject CreateButton(string name, Transform parent, string text)
    {
        var go = CreateUIObject(name, parent);
        var img = go.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, .12f);

        var btn = go.AddComponent<Button>();
        var colors = btn.colors;
        colors.highlightedColor = new Color(1f, 1f, 1f, .25f);
        colors.pressedColor = new Color(1f, 1f, 1f, .35f);
        colors.selectedColor = colors.highlightedColor;
        btn.colors = colors;

        var label = CreateUIObject("Text", go.transform);
        var rt = label.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(24, 12);
        rt.offsetMax = new Vector2(-24, -12);

        var t = label.AddComponent<Text>();
        t.text = text;
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = 28;
        t.color = Color.white;

        return go;
    }

    static void CreateAnchorDot(string name, Vector2 anchor, Vector2 offset, Transform parent, float size = 18f)
    {
        var dot = CreateUIObject(name, parent);
        var rt = dot.GetComponent<RectTransform>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.anchoredPosition = offset;

        var img = dot.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, .9f);

        var gizmo = dot.AddComponent<AnchorGizmo>();
        gizmo.size = size;
    }
}
#endif
