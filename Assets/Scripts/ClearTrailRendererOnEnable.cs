using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrailRendererOnEnable : MonoBehaviour
{
    [SerializeField] TrailRenderer trailRenderer;

    private void OnEnable()
    {
        if (trailRenderer)
            trailRenderer.Clear();
    }
}
