using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    List<Player> playerList;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        playerList = GameStats.GetPlayerList();
    }

    private void Update()
    {
        gameObject.GetComponent<Text>().text = $"體力: {playerList[index].Power}\n心情: {playerList[index].Emotion}";
    }
}
