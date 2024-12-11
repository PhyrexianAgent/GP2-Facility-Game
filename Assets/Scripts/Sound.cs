using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    public readonly Vector3 Location;
    public readonly float Range;
    public readonly float Strength;
    public Sound(Vector3 location, float range, float strength){
        Location = location;
        Range = range;
        Strength = strength;
    }

    public bool CanHearSound(Vector3 targetPos) => Vector3.Distance(targetPos, Location) <= Range;
}
