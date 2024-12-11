using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwareConeDetector : ConeDetector
{
    [SerializeField, Min(0)] private float awareRadius, awareDistance;
    [SerializeField] private Color awareConeColor = Color.red;

    private void OnDrawGizmos() {
        DrawSpotlight(awareRadius, awareDistance, awareConeColor);
        base.OnDrawGizmosSelected();
    }
    public bool PlayerInAwareSpotlight(Transform playerTransform) => PlayerInSight(awareDistance, awareRadius, playerTransform);
}
