using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyeApplier : MonoBehaviour
{
    [SerializeField] private Dyeable dyeableComponent;
    [SerializeField] private float dyeCapacity = Mathf.Infinity;
    [SerializeField] [Min(0)] private float dyeCooldown = 0.1f;
    [SerializeField] [Min(0)] private float splatRadius;
    [SerializeField] private Bewildered.UHashSet<TagString> tagsToDye;

    private float initCapacity;
    private HashSet<GameObject> recentlyDyed = new HashSet<GameObject>();
    private Collider[] splattedCache;

    public bool AnyDyeLeft { get => dyeCapacity > 0; }
    public bool CanDye { get => !dyeableComponent || dyeableComponent.IsDyed; }

    private void Awake()
    {
        initCapacity = dyeCapacity;
        splattedCache = new Collider[50];
    }

    private void OnEnable()
    {
        if (dyeableComponent)
            dyeableComponent.GetDyedCall += OnGetDyedCall;
    }
    private void OnDisable()
    {
        if (dyeableComponent)
            dyeableComponent.GetDyedCall -= OnGetDyedCall;
    }

    private void OnGetDyedCall(bool _) => Refill();
    public void Refill(float amount = -1, bool overfill = false)
    {
        if (Mathf.Approximately(amount, 0)) return;

        if (amount < 0)
        {
            dyeCapacity = initCapacity;
            return;
        }

        if (overfill)
        {
            dyeCapacity += amount;
            return;
        }

        dyeCapacity = Mathf.Max(dyeCapacity + amount, initCapacity);
    }

    public bool SplatDye(float radius = -1)
    {
        if (!CanDye || !AnyDyeLeft) return false;

        radius = radius < 0 ? splatRadius : radius;
        if (radius <= 0) return false;

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            radius,
            splattedCache,
            Physics.AllLayers,
            QueryTriggerInteraction.Ignore);

        for (int i = 0; i < hitCount; i++)
        {
            if (ShouldDye(splattedCache[i], out Dyeable dyeHit))
            {
                DoDye(dyeHit);
            }
        }

        dyeableComponent.Undye(0.05f);
        return true;
    }

    private bool ShouldDye(Component objComp, out Dyeable result)
    {
        result = null;

        //If this dyer can still dye things,
        return CanDye && AnyDyeLeft
            //we haven't dyed objComp recently,
            && !recentlyDyed.Contains(objComp.gameObject)
            //objComp is a dyable thing,
            && tagsToDye.Contains(objComp.tag)
            && objComp.TryGetComponent(out result);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ShouldDye(collision.collider, out Dyeable dyeOther))
        {
            DoDye(dyeOther);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldDye(other, out Dyeable dyeOther))
        {
            DoDye(dyeOther);
        }
    }

    private void DoDye(Dyeable thingToDye)
    {
        if (!thingToDye.GetDyed()) return;

        Coroutilities.DoAfterDelay(this, () => recentlyDyed.Remove(thingToDye.gameObject), dyeCooldown);

        dyeCapacity--;
        if (dyeableComponent && !AnyDyeLeft)
            dyeableComponent.Undye();
    }
}
