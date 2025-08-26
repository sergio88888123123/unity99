
// ResponsiveWindow.cs
// Coloca este script en una escena vacía (o en un GameObject vacío) y dale Play.
// Crea una ventana UI adaptable a cualquier resolución con:
// - Un título
// - Cuatro iconos
// - Tres botones
// - Tres enlaces por cada costado (izquierdo y derecho)
// Puedes cambiar textos, URLs e íconos en el método ConfigureContent().

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[DefaultExecutionOrder(-1000)]
public class ResponsiveWindow : MonoBehaviour
{
    [Header("Opcional: Sprites para iconos (si están en Resources/Icons)")]
    [Tooltip("Si dejas vacío, se usarán cuadrados de color como placeholders.")]
    public string[] iconResourceNames = new[] { "icon1", "icon2", "icon3", "icon4" };

    private Canvas canvas;
    private CanvasScaler scaler;
    private EventSystem eventSystem;

    void Awake()
    {
        BuildCanvasIfNeeded();
        BuildUI();
    }

    // ---------- Construcción general ----------
    void BuildCanvasIfNeeded()
    {
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var goCanvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = goCanvas.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            scaler = goCanvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }
        else
        {
            scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler == null) scaler = canvas.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }

        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            var goES = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem = goES.GetComponent<EventSystem>();
        }
    }

    void BuildUI()
    {
        // Safe Area root
        var safeAreaGO = CreateUIObject("SafeArea", canvas.transform);
        var safeAreaRT = safeAreaGO.GetComponent<RectTransform>();
        StretchToParent(safeAreaRT);
        var safeArea = safeAreaGO.AddComponent<SafeAreaFitter>();

        // Ventana principal (panel)
        var window = CreateUIObject("Window", safeAreaRT);
        var windowRT = window.GetComponent<RectTransform>();
        StretchToParent(windowRT, new Vector2(0.08f, 0.08f), new Vector2(0.92f, 0.92f)); // márgenes porcentuales
        var windowImg = window.AddComponent<Image>();
        windowImg.color = new Color(0.12f, 0.12f, 0.14f, 0.95f); // fondo oscuro
        window.AddComponent<Mask>().showMaskGraphic = true;

        var windowPad = window.AddComponent<LayoutElement>();
        var windowVLG = window.AddComponent<VerticalLayoutGroup>();
        windowVLG.padding = new RectOffset(32, 32, 32, 32);
        windowVLG.spacing = 24;
        windowVLG.childAlignment = TextAnchor.UpperCenter;

        // Contenido central (título, grid de iconos, fila de botones)
        var content = CreateUIObject("Content", windowRT);
        var contentRT = content.GetComponent<RectTransform>();
        StretchToParent(contentRT);
        var contentVLG = content.AddComponent<VerticalLayoutGroup>();
        contentVLG.spacing = 24;
        contentVLG.childAlignment = TextAnchor.UpperCenter;

        // Título
        var title = CreateText("Título de la Ventana", 44, FontStyle.Bold, contentRT);
        var titleLE = title.gameObject.AddComponent<LayoutElement>();
        titleLE.minHeight = 64;

        // Grid de 4 iconos
        var iconGrid = CreateUIObject("IconGrid", contentRT);
        var gridRT = iconGrid.GetComponent<RectTransform>();
        SetSize(gridRT, 0, 120); // altura mínima
        var grid = iconGrid.AddComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 4;
        grid.cellSize = new Vector2(96, 96);
        grid.spacing = new Vector2(20, 20);
        grid.childAlignment = TextAnchor.MiddleCenter;

        // Iconos
        for (int i = 0; i < 4; i++)
        {
            var icon = CreateUIObject($"Icon_{i + 1}", gridRT);
            var img = icon.AddComponent<Image>();
            img.color = new Color(0.25f, 0.6f, 1f, 1f); // placeholder azul
            var sprite = TryLoadIconSprite(i);
            if (sprite != null) img.sprite = sprite;
            var iconLE = icon.AddComponent<LayoutElement>();
            iconLE.preferredWidth = 96;
            iconLE.preferredHeight = 96;
            // Borde suave
            var outline = icon.AddComponent<Outline>();
            outline.effectDistance = new Vector2(2, -2);
            outline.effectColor = new Color(0, 0, 0, 0.4f);
        }

        // Fila de 3 botones
        var buttonsRow = CreateUIObject("ButtonsRow", contentRT);
        var rowRT = buttonsRow.GetComponent<RectTransform>();
        SetSize(rowRT, 0, 80);
        var hlg = buttonsRow.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = 16;
        hlg.childForceExpandHeight = false;
        hlg.childForceExpandWidth = true;
        hlg.childAlignment = TextAnchor.MiddleCenter;
        for (int i = 0; i < 3; i++)
        {
            var btn = CreateButton($"Botón {i + 1}", buttonsRow.transform);
            var btnLE = btn.GetComponent<LayoutElement>();
            btnLE.minHeight = 56;
            btnLE.minWidth = 220;
        }

        // Barras laterales de enlaces (tres por costado)
        var leftBar = CreateSidebar(windowRT, "LeftLinks", anchorMin: new Vector2(0, 0), anchorMax: new Vector2(0, 1), pivot: new Vector2(0, 0.5f));
        var rightBar = CreateSidebar(windowRT, "RightLinks", anchorMin: new Vector2(1, 0), anchorMax: new Vector2(1, 1), pivot: new Vector2(1, 0.5f));

        // Enlaces izquierda
        CreateLinkButton(leftBar, "U. de Guadalajara", "https://www.udg.mx/");
        CreateLinkButton(leftBar, "Unity Learn", "https://learn.unity.com/");
        CreateLinkButton(leftBar, "Documentación UI", "https://docs.unity3d.com/Manual/UISystem.html");

        // Enlaces derecha
        CreateLinkButton(rightBar, "MongoDB", "https://www.mongodb.com/");
        CreateLinkButton(rightBar, "C# Docs", "https://learn.microsoft.com/dotnet/csharp/");
        CreateLinkButton(rightBar, "UI Samples", "https://github.com/UnityTechnologies/UnityCsReference");

        // Ajustar contenido para no solaparse con barras laterales
        var contentLE = content.AddComponent<LayoutElement>();
        contentLE.minWidth = 800;

        // Configuración de textos/URLs opcional en un lugar centralizado
        ConfigureContent(title.GetComponent<Text>());
    }

    // ---------- Helpers de construcción ----------
    GameObject CreateSidebar(RectTransform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
    {
        var bar = CreateUIObject(name, parent);
        var rt = bar.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.sizeDelta = new Vector2(280, 0); // ancho fijo, alto estira
        rt.anchoredPosition = Vector2.zero;

        var bg = bar.AddComponent<Image>();
        bg.color = new Color(1f, 1f, 1f, 0.06f);

        var vlg = bar.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(16, 16, 24, 24);
        vlg.spacing = 12;
        vlg.childAlignment = TextAnchor.UpperCenter;

        var title = CreateText("Enlaces", 22, FontStyle.Bold, rt);
        var sep = CreateSeparator(rt);

        return bar;
    }

    void CreateLinkButton(GameObject parent, string text, string url)
    {
        var btnGO = CreateButton(text, parent.transform);
        var link = btnGO.AddComponent<LinkOpener>();
        link.url = url;

        // Aspecto secundario para diferenciar enlaces de botones principales
        var img = btnGO.GetComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.24f, 1f);

        var txt = btnGO.GetComponentInChildren<Text>();
        txt.fontSize = 20;
        txt.color = new Color(0.8f, 0.9f, 1f, 1f);
    }

    Sprite TryLoadIconSprite(int index)
    {
        // Carga opcional desde Resources/Icons
        if (iconResourceNames != null && index < iconResourceNames.Length)
        {
            var name = iconResourceNames[index];
            if (!string.IsNullOrEmpty(name))
            {
                var s = Resources.Load<Sprite>($"Icons/{name}");
                return s;
            }
        }
        return null;
    }

    GameObject CreateSeparator(Transform parent)
    {
        var sep = CreateUIObject("Separator", parent);
        var rt = sep.GetComponent<RectTransform>();
        SetSize(rt, 0, 2);
        var img = sep.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0.08f);
        var le = sep.AddComponent<LayoutElement>();
        le.minHeight = 2;
        return sep;
    }

    GameObject CreateButton(string text, Transform parent)
    {
        var go = CreateUIObject(text, parent);
        var rt = go.GetComponent<RectTransform>();
        SetSize(rt, 220, 56);

        var img = go.AddComponent<Image>();
        img.color = new Color(0.24f, 0.45f, 0.95f, 1f);

        var btn = go.AddComponent<Button>();
        var colors = btn.colors;
        colors.fadeDuration = 0.08f;
        colors.highlightedColor = new Color(0.28f, 0.52f, 1f, 1f);
        colors.pressedColor = new Color(0.18f, 0.36f, 0.8f, 1f);
        colors.selectedColor = colors.highlightedColor;
        btn.colors = colors;

        var label = CreateText(text, 24, FontStyle.Bold, rt);
        var t = label.GetComponent<Text>();
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        var labelRT = label.GetComponent<RectTransform>();
        StretchToParent(labelRT);

        var le = go.AddComponent<LayoutElement>();
        le.minHeight = 56;
        le.minWidth = 200;

        return go;
    }

    GameObject CreateUIObject(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        var rt = go.GetComponent<RectTransform>();
        go.transform.SetParent(parent, false);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot    = new Vector2(0.5f, 0.5f);
        return go;
    }

    GameObject CreateText(string text, int size, FontStyle style, Transform parent)
    {
        var go = CreateUIObject("Text", parent);
        var t = go.AddComponent<Text>();
        t.text = text;
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.fontSize = size;
        t.fontStyle = style;
        t.color = new Color(0.95f, 0.98f, 1f, 1f);
        t.alignment = TextAnchor.MiddleCenter;
        var rt = go.GetComponent<RectTransform>();
        SetSize(rt, 600, 64);
        return go;
    }

    void SetSize(RectTransform rt, float w, float h)
    {
        rt.sizeDelta = new Vector2(w, h);
    }

    void StretchToParent(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    void StretchToParent(RectTransform rt, Vector2 anchorMin, Vector2 anchorMax)
    {
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    // Personaliza textos/URLs desde aquí si quieres centralizar cambios
    void ConfigureContent(Text titleText)
    {
        titleText.text = "Interfaz Adaptativa (Unity UI)";
    }
}

// ---------- Abrir URL ----------
public class LinkOpener : MonoBehaviour
{
    [TextArea] public string url;

    void Reset()
    {
        url = "https://unity.com/";
        var btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(Open);
    }

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveListener(Open);
            btn.onClick.AddListener(Open);
        }
    }

    public void Open()
    {
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
    }
}

// ---------- Ajuste a Safe Area (iOS/Android) ----------
public class SafeAreaFitter : MonoBehaviour
{
    RectTransform rt;
    Rect lastSafe;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void OnRectTransformDimensionsChange()
    {
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
#if UNITY_EDITOR
        // En editor no hay notches; usaremos la pantalla completa.
        var safe = new Rect(0, 0, Screen.width, Screen.height);
#else
        var safe = Screen.safeArea;
#endif
        if (safe == lastSafe) return;
        lastSafe = safe;

        var anchorMin = safe.position;
        var anchorMax = safe.position + safe.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
```
