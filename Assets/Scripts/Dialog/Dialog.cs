using System.Collections;
using System.Collections.Generic;
using UnityEngine;






[CreateAssetMenu(menuName="Dialog/Dialog", fileName="Dialog X")]
public class Dialog : ScriptableObject
{
    [TextArea(15, 20)]public string Text;
    public DialogSource Source;
    public bool TriggerKillAnimation = false;
    
}
