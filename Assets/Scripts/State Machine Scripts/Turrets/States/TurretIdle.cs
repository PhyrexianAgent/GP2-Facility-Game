using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIdle : State // Add part to return to initial rotation when deactivated by camera
{
    public TurretIdle(GameObject agent) : base(agent){

    }
}
