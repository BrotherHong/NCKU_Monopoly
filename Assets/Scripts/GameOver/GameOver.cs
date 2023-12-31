using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    List<Player> playerList;
    void Start()
    {
        playerList = GameStats.GetPlayerList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
