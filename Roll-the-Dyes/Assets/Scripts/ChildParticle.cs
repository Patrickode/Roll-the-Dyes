using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildParticle : MonoBehaviour
{
    private void Start() { transform.parent = null; }
}
