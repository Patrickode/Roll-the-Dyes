using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyeApplier : MonoBehaviour
{
    [SerializeField] private Dyeable dyeableComponent;
    [SerializeField] private Bewildered.UHashSet<TagString> tagsToDye;

    public bool CanDye { get => !dyeableComponent || dyeableComponent.IsDyed; }

    private void OnCollisionEnter(Collision collision)
    {
        if (CanDye && tagsToDye.Contains(collision.gameObject.tag)
            && collision.gameObject.TryGetComponent(out Dyeable dyeOther))
        {
            dyeOther.GetDyed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanDye && tagsToDye.Contains(other.tag)
            && other.TryGetComponent(out Dyeable dyeOther))
        {
            dyeOther.GetDyed();
        }
    }
}
