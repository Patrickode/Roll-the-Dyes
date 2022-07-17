using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Dyeable : MonoBehaviour
{
    [Tooltip("The time it takes for this object to fully change color." +
        "Note that this object is considered to be dyed immediately, regardless of this.")]
    [SerializeField] protected float getDyedDuration;

    protected Renderer rendRef;
    protected Color initColor;
    protected bool[] levColorOptns;
    protected Vector4 hsvaCache;
    protected Coroutine changingColor;

    public bool IsDyed { get; private set; }

    public System.Action<bool> GetDyedCall;

    protected void OnDestroy() => GetDyedCall = null;

    protected virtual void Start()
    {
        rendRef = GetComponent<Renderer>();
        initColor = rendRef.material.color;

        levColorOptns = LevelColorHub.Instance.SetColorOptns;

        Color.RGBToHSV(rendRef.material.color, out hsvaCache.x, out hsvaCache.y, out hsvaCache.z);
        hsvaCache.w = rendRef.material.color.a;
    }

    public bool GetDyed(float duration = -1)
    {
        GetDyedCall?.Invoke(!IsDyed);
        if (IsDyed) return false;

        Coroutilities.TryStopCoroutine(this, ref changingColor);
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
            duration >= 0 ? duration : getDyedDuration));

        IsDyed = true;
        return true;
    }

    public void Undye(float duration = -1)
    {
        Coroutilities.TryStopCoroutine(this, ref changingColor);

        changingColor = StartCoroutine(ChangeColor(
            rendRef.material.color,
            initColor,
            duration >= 0 ? duration : getDyedDuration));

        IsDyed = false;
    }

    protected IEnumerator ChangeColor(Color startColor, Color endColor, float duration)
    {
        if (duration <= 0)
        {
            rendRef.material.color = endColor;
            yield break;
        }

        for (float progress = 0; progress <= 1; progress += Time.deltaTime / duration)
        {
            rendRef.material.color = Color.Lerp(startColor, endColor, progress);
            yield return null;
        }

        rendRef.material.color = endColor;
    }
}