using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TMP_FontControlsUI : MonoBehaviour
{
    [Header("Objetivos")]
    public List<TextMeshProUGUI> targets = new List<TextMeshProUGUI>();

    [Header("Fuentes disponibles")]
    public List<TMP_FontAsset> fonts = new List<TMP_FontAsset>();
    public int currentFontIndex = 0;

    [Header("Tamaño")]
    public int step = 2;
    public int minSize = 12;
    public int maxSize = 72;

    public void NextFont()
    {
        if (fonts == null || fonts.Count == 0) return;
        currentFontIndex = (currentFontIndex + 1) % fonts.Count;
        ApplyFont(fonts[currentFontIndex]);
    }

    public void PrevFont()
    {
        if (fonts == null || fonts.Count == 0) return;
        currentFontIndex = (currentFontIndex - 1 + fonts.Count) % fonts.Count;
        ApplyFont(fonts[currentFontIndex]);
    }

    public void SetFontByIndex(int idx)
    {
        if (fonts == null || fonts.Count == 0) return;
        currentFontIndex = Mathf.Clamp(idx, 0, fonts.Count - 1);
        ApplyFont(fonts[currentFontIndex]);
    }

    public void IncreaseSize() => AdjustSize(step);
    public void DecreaseSize() => AdjustSize(-step);

    void ApplyFont(TMP_FontAsset f)
    {
        foreach (var t in targets) if (t) t.font = f;
    }

    void AdjustSize(int delta)
    {
        foreach (var t in targets) if (t)
        {
            t.fontSize = Mathf.Clamp(t.fontSize + delta, minSize, maxSize);
        }
    }
}
