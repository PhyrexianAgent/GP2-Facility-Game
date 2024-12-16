using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DialogSource{
    mission_control,
    player
}

public class GameManager
{
    public static bool PauseInput;
    public static GameDialogController CurrentDialogGUI;
    public static PlayerDeadController PlayerDeadGUI;
    public static ElevatorEntranceController CurrentElevatorEntrance;
    public static PlayerWonController PlayerWonGUI;
    public static Transform PlayerInterface;
    public static SceneFader CurrentSceneFader;
    public static int WorkbotsCollectedCount {get; private set;}
    private static Transform playerTrans;
    private static ViewPane playerPane;
    private static SimpleHealth playerHealth;
    private static Dictionary<DialogSource, Sprite> characterHeads = new Dictionary<DialogSource, Sprite>();
    private static int currentLevelCollectionCount = 0;
    
    public static void SetPlayer(Transform playerTransform) => playerTrans = playerTransform;
    public static Transform GetPlayerTransform() => playerTrans;
    public static void SetPlayerHealth(SimpleHealth health) => playerHealth = health;
    public static Sprite GetCharacterHead(DialogSource character) => characterHeads[character];
    public static Quaternion GetRotationToPointOverTime(Transform target, Vector3 point, float turnRate){
        Vector3 diff = point - target.position;
        Vector3 dir = Vector3.RotateTowards(target.forward, diff, turnRate * Time.deltaTime, 0.0f);
        return Quaternion.LookRotation(dir);
    }
    public static ViewPane SetPlayerPane(ViewPane pane) => playerPane = pane;
    public static GameObject GetObjectBetweenPlayerAndTarget(Transform target){
        RaycastHit hit;
        Vector3 dirFromPlayerToTarget = (target.position - playerTrans.position).normalized;
        if (!Physics.Raycast(playerTrans.position, dirFromPlayerToTarget, out hit, 500)) return null;
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
        Debug.DrawLine(position, hit.point, Color.red, 0);
        return hit.collider.tag == "Player";
    }
    public static bool PlayerInView(Vector3 position){
        if (playerPane == null) return false;
        return playerPane.PaneVisibleToPoint(position);
    }
    public static void CollectWorkBot() {
        WorkbotsCollectedCount++;
        currentLevelCollectionCount++;
    } 
    public static void MakePlayerTakeDamage(float damage) => playerHealth.DealDamage(damage);
    public static void ChangeScene(string sceneName, bool resetSceneCollectedCount = false){
        SceneManager.LoadSceneAsync(sceneName);
        if (resetSceneCollectedCount) WorkbotsCollectedCount -= currentLevelCollectionCount;
        currentLevelCollectionCount = 0;
    }
    public static void RestartScene() => ChangeScene(SceneManager.GetActiveScene().name, true);
    public static void PlayerWon(){
        EndPlayerControl();
        PlayerWonGUI.PlayerWon();
        WorkbotsCollectedCount = 0;
    }
    public static void KillPlayer(){
        EndPlayerControl();
        PlayerDeadGUI.PlayerDied();
    }
    private static void EndPlayerControl(){
        PauseInput = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        MonoBehaviour.Destroy(playerTrans.GetComponent<PlayerControl>());
        MonoBehaviour.Destroy(playerTrans.GetComponent<CharacterController>());
    }
}
