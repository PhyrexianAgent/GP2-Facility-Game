using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogSource{
    mission_control,
    player
}

public class GameManager
{
    public static bool PauseInput;
    public static GameDialogController CurrentDialogGUI;
    public static Transform PlayerInterface;
    private static Transform player;
    private static Dictionary<DialogSource, Sprite> characterHeads = new Dictionary<DialogSource, Sprite>();
    public static void SetPlayer(Transform playerTrans) => player = playerTrans;
    public static Transform GetPlayerTransform() => player;
    public static Sprite GetCharacterHead(DialogSource character) => characterHeads[character];
    public static Quaternion GetRotationToPointOverTime(Transform target, Vector3 point, float turnRate){
        Vector3 diff = point - target.position;
        Vector3 dir = Vector3.RotateTowards(target.forward, diff, turnRate * Time.deltaTime, 0.0f);
        return Quaternion.LookRotation(dir);
    }
}
