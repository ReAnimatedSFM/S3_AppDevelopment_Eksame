using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
            GameMasterData.GameMasterInstance = Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        FirebaseManager.Instance.SignOut();
    }

}
