using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRolling : MonoBehaviour
{

    [SerializeField] float rollTime;
    [SerializeField] float stopTime;

    float timeElapsed;
    Quaternion targetRotation;

    void Start()
    {
        timeElapsed = 0;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (GameStats.currentState == GameState.DICE_ROLLING)
        {

            if (timeElapsed >= stopTime)
            {
                targetRotation = GetPointRotation(GameStats.UI.DiceResult);
            } else
            {
                targetRotation = Random.rotation;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.3f);

            if (timeElapsed >= rollTime)
            {
                timeElapsed = 0;
                transform.rotation = Quaternion.identity;
                GameStats.currentState = GameState.MOVE;
            } else
            {
                timeElapsed += Time.deltaTime;
            }
        }
    }

    private Quaternion GetPointRotation(int point)
    {
        switch (point)
        {
            case 1: return Quaternion.identity;
            case 2: return Quaternion.Euler(0, 0, -90);
            case 3: return Quaternion.Euler(-90, 0, 0);
            case 4: return Quaternion.Euler(90, 0, 0);
            case 5: return Quaternion.Euler(0, 0, 90);
            case 6: return Quaternion.Euler(180, 0, 0);
        }
        return Quaternion.identity;
    }
}
