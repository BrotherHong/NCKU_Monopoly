using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHistCourse : MonoBehaviour
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
        sb.AppendLine("½Òµ{¾ú¥v¬ö¿ý: ");
        foreach (Course i in playerList[index].CourseHistory)
        {
            sb.AppendLine(i.ToString());
        }
        transform.GetComponent<Text>().text =  sb.ToString();
    }
}
