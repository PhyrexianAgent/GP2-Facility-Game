using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyIconController : MonoBehaviour
{
    public Transform GamePoint {private get; set;}
    void Update()
    {
        if (GamePoint != null) transform.position = Camera.main.WorldToScreenPoint(GamePoint.position);
    }
}
