using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingUI : MonoBehaviour
{
    [SerializeField] List<Text> nameText;

    public void OnFinishClick()
    {
        List<Player> players = GameStats.GetPlayerList();
        players.Add(new Player(nameText[0].text, "Image/dock"));
        players.Add(new Player(nameText[1].text, "Image/egg_head"));
        players.Add(new Player(nameText[2].text, "Image/saugy"));
        players.Add(new Player(nameText[3].text, "Image/cones"));
    }
}
