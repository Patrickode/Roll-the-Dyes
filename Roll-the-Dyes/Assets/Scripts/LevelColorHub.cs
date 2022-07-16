using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColorHub : MonoBehaviour
{
    [SerializeField] private Color levelColor = Color.red;
    [Tooltip("White things should be dyed regardless if saturation isn't set. " +
        "Saturation values below/equal to this will be considered white.")]
    [SerializeField] [Range(0, 1)] private float whiteThreshold = 0;
    [SerializeField] private Material[] matsWithColor;
    [SerializeField] private bool setHue;
    [SerializeField] private bool setSaturation;
    [SerializeField] private bool setValue;
    [SerializeField] private bool setAlpha;

    [SerializeField] [HideInInspector] private Vector4 levelColHSV;
    private Vector3 switchHSVCache;

    private static LevelColorHub _inst = null;
    public static LevelColorHub Instance { get => _inst; private set => _inst = value; }
    public Color LevelColor { get => levelColor; }
    public Vector4 LevelColorHSV { get => levelColHSV; }
    public float WhiteThreshold { get => whiteThreshold; }
    public bool[] SetColorOptns { get => new[] { setHue, setSaturation, setValue, setAlpha }; }

    private void OnValidate() => ValidationUtility.DoOnDelayCall(this, Init);
    private void Awake() => Init();

    private void Init()
    {
        if (Application.isPlaying && Instance && Instance != this)
        {
            Debug.LogError($"There's already an instance of LevelColorHub in the scene ({Instance.name})!");
            Destroy(this);
        }
        else
            Instance = this;

        Color.RGBToHSV(levelColor, out levelColHSV.x, out levelColHSV.y, out levelColHSV.z);
        levelColHSV.w = levelColor.a;

        ApplyLevelColor();
    }

    private void ApplyLevelColor()
    {
        foreach (var mat in matsWithColor)
        {
            if (setHue && setSaturation && setValue && setAlpha)
            {
                mat.color = levelColor;
                continue;
            }

            Color.RGBToHSV(mat.color, out switchHSVCache.x, out switchHSVCache.y, out switchHSVCache.z);
            switchHSVCache.x = setHue
                ? switchHSVCache.x
                : levelColHSV.x;
            switchHSVCache.y = setSaturation || switchHSVCache.y <= whiteThreshold
                ? switchHSVCache.y
                : levelColHSV.y;
            switchHSVCache.z = setValue
                ? switchHSVCache.z
                : levelColHSV.z;

            var targetColor = Color.HSVToRGB(switchHSVCache.x, switchHSVCache.y, switchHSVCache.z);
            targetColor.a = setAlpha ? levelColor.a : mat.color.a;

            mat.color = targetColor;
        }
    }
}