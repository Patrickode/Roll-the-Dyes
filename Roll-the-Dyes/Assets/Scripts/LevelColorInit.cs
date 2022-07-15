using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColorInit : MonoBehaviour
{
    [SerializeField] private Color levelColor = Color.red;
    [SerializeField] private Material[] matsWithColor;
    [SerializeField] private bool setSaturation;
    [SerializeField] private bool setValue;
    [SerializeField] private bool setAlpha;

    [SerializeField] [HideInInspector] private Vector3 levelColHSV;
    private Vector3 switchHSVCache;

    private void OnValidate() => ValidationUtility.DoOnDelayCall(this, Init);
    private void Start() => Init();

    private void Init()
    {
        Color.RGBToHSV(levelColor, out levelColHSV.x, out levelColHSV.y, out levelColHSV.z);
        ApplyLevelColor();
    }

    private void ApplyLevelColor()
    {
        foreach (var mat in matsWithColor)
        {
            if (setSaturation && setAlpha)
            {
                mat.color = levelColor;
                continue;
            }

            Color.RGBToHSV(mat.color, out switchHSVCache.x, out switchHSVCache.y, out switchHSVCache.z);
            switchHSVCache.y = setSaturation ? switchHSVCache.y : levelColHSV.y;
            switchHSVCache.z = setValue ? switchHSVCache.z : levelColHSV.z;

            var targetColor = Color.HSVToRGB(switchHSVCache.x, switchHSVCache.y, switchHSVCache.z);
            targetColor.a = setAlpha ? levelColor.a : mat.color.a;

            mat.color = targetColor;
        }
    }
}