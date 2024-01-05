using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerClass : MonoBehaviour
{
    List<Player> playerList;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        playerList = GameStats.GetPlayerList();
    }

    // Update is called once per frame
    void Update()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("�ثe�ת��ҵ{: ");
        foreach (Course i in playerList[index].CurrentCourse)
        {
            sb.AppendLine(i.ToString());
        }
        transform.Find("Scroll View").Find("Viewport").Find("Content").GetComponent<Text>().text =  sb.ToString();
    }
}
