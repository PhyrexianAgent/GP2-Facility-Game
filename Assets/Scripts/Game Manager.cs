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
    public static int WorkbotsCollectedCount {get; private set;}
    private static Transform player;
    private static ViewPane playerPane;
    private static Dictionary<DialogSource, Sprite> characterHeads = new Dictionary<DialogSource, Sprite>();
    public static void SetPlayer(Transform playerTrans) => player = playerTrans;
    public static Transform GetPlayerTransform() => player;
    public static Sprite GetCharacterHead(DialogSource character) => characterHeads[character];
    public static Quaternion GetRotationToPointOverTime(Transform target, Vector3 point, float turnRate){
        Vector3 diff = point - target.position;
        Vector3 dir = Vector3.RotateTowards(target.forward, diff, turnRate * Time.deltaTime, 0.0f);
        return Quaternion.LookRotation(dir);
    }
    public static ViewPane SetPlayerPane(ViewPane pane) => playerPane = pane;
    public static GameObject GetObjectBetweenPlayerAndTarget(Transform target){
        RaycastHit hit;
        Vector3 dirFromPlayerToTarget = (target.position - player.position).normalized;
        if (!Physics.Raycast(player.position, dirFromPlayerToTarget, out hit, 500)) return null;
        return hit.collider.gameObject;
    }
    public static GameObject GetObjectFromRay(Vector3 position, Vector3 direction){
        RaycastHit hit;
        if (!Physics.Raycast(position, direction, out hit, 500)) return null;
        return hit.collider.gameObject;
    }
    public static bool PlayerInView(Vector3 position, Vector3 direction){
        RaycastHit hit;
        if (!Physics.Raycast(position, direction, out hit)) return false;
        //Debug.DrawLine(position, hit.point, Color.red, 0);
        return hit.collider.tag == "Player";
    }
    public static bool PlayerInView(Vector3 position){
        if (playerPane == null) return false;
        return playerPane.PaneVisibleToPoint(position);
    }
    public static int CollectWorkBot() => WorkbotsCollectedCount++;
}
