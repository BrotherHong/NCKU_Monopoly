using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeUserName : MonoBehaviour
{
    public InputField inputField;
    public Text resultText;
    public int index;
    public void EnterPlayer()
    {
        string input = inputField.text;
        resultText.text = input;
    }
}
