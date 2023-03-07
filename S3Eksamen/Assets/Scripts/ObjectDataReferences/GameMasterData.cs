using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMasterData
{
    public static GameMaster GameMasterInstance;

    public static void SetGMInstance()
    {
        GameMaster.Instance = GameMasterInstance;
    }
}
