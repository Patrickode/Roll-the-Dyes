using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Dyeable : MonoBehaviour
{
    [Tooltip("The time it takes for this object to fully change color." +
        "Note that this object is considered to be dyed immediately, regardless of this.")]
    [SerializeField] private float getDyedDuration;

    private Renderer rendRef;
    private bool[] levColorOptns;
    private Vector4 hsvaCache;
    private Coroutine changingColor;

    public bool IsDyed { get; private set; }

    public static System.Action NotDyedResponse;

    private void Start()
    {
        rendRef = GetComponent<Renderer>();

        levColorOptns = LevelColorHub.Instance.SetColorOptns;

        Color.RGBToHSV(rendRef.material.color, out hsvaCache.x, out hsvaCache.y, out hsvaCache.z);
        hsvaCache.w = rendRef.material.color.a;
    }

    public void GetDyed()
    {
        Coroutilities.TryStopCoroutine(this, changingColor);
        var levCol = LevelColorHub.Instance.LevelColorHSV;

        for (int i = 0; i < levColorOptns.Length; i++)
            if (levColorOptns[i]
                || (hsvaCache[1] <= LevelColorHub.Instance.WhiteThreshold && i == 1))
            {
                hsvaCache[i] = levCol[i];
            }

        changingColor = StartCoroutine(ChangeColor(
            rendRef.material.color,
            UtilFunctions.HSVAtoRGBA(hsvaCache),
            getDyedDuration));

        IsDyed = true;
    }

    private IEnumerator ChangeColor(Color startColor, Color endColor, float duration)
    {
        if (duration <= 0)
        {
            rendRef.material.color = endColor;
            yield break;
        }

        for (float progress = 0; progress < 1; progress += Time.deltaTime / duration)
        {
            rendRef.material.color = Color.Lerp(startColor, endColor, progress);
            yield return null;
        }
    }
}