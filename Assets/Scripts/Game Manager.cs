using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static Transform player;

    public static void SetPlayer(Transform playerTrans) => player = playerTrans;
}
