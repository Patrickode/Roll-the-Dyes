using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockWin : MonoBehaviour
{
    [SerializeField] private Renderer rendRef;
    [SerializeField] [Min(0)] private float changeColorDuration;
    [SerializeField] private Bewildered.UHashSet<TagString> winnerTags;
    private RequiredDyeable[] allRequiredDyeables;
    private HashSet<RequiredDyeable> currentRequiredDyeables = new HashSet<RequiredDyeable>();
    private bool canWin;

    private Color colorSwap;
    private Vector4 levColHSVCache;

    private void Start()
    {
        allRequiredDyeables = (RequiredDyeable[])FindSceneObjectsOfType(typeof(RequiredDyeable));
    }

    private void OnEnable()
    {
        RequiredDyeable.RequiredComponentDyed += OnRequiredComponentDyed;
    }
    private void OnDisable()
    {
        RequiredDyeable.RequiredComponentDyed -= OnRequiredComponentDyed;
    }

    private void OnRequiredComponentDyed(RequiredDyeable reqDyed)
    {
        currentRequiredDyeables.Add(reqDyed);
        if (currentRequiredDyeables.Count == allRequiredDyeables.Length)
        {
            WinUnlock();
        }
    }

    public void WinUnlock()
    {
        levColHSVCache = LevelColorHub.Instance.LevelColorHSV;
        colorSwap = Color.HSVToRGB(levColHSVCache.x, levColHSVCache.y, levColHSVCache.z);
        colorSwap.a = rendRef.material.color.a;

        StartCoroutine(ChangeColor(rendRef.material.color, colorSwap, changeColorDuration));
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
        canWin = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (canWin && winnerTags.Contains(other.tag))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}