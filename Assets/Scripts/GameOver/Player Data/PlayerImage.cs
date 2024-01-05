using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImage : MonoBehaviour
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
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(playerList[index].ImagePath);
    }
}
