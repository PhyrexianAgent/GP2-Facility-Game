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
}
