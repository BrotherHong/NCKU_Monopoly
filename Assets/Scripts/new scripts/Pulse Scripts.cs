using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseScripts : MonoBehaviour
{
    public Canvas canvas;
    public Canvas pulseCanvas;
    
    public int isPulse = 0;
    // Start is called before the first frame update
    void Start()
    {
        isPulse = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickPulseButton()
    {
        if(isPulse == 0)
        {           
            PauseGame();
        }else
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0; //暫停

        canvas.enabled = false;
        pulseCanvas.enabled = true;
        isPulse = 1;
    }

    void ResumeGame()
    {
        Time.timeScale = 1; //正常速度執行遊戲

        canvas.enabled = true;
        pulseCanvas.enabled = false;
        isPulse = 0;
    }
}
