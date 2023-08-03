using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperCheat : MonoBehaviour
{
    [SerializeField] GameObject childern;
    [SerializeField] ScrollRect scroll;
    [SerializeField] TMP_InputField input;
    [SerializeField] TextMeshProUGUI content;

    string commandErrorString = "Invalid command\n";
    string paramErrorString = "Invalid param\n";
    string textColorNotification = "<color=#00ff00ff>";
    string textColorWarning = "<color=#ff0000ff>";
    string textColorNormal = "<color=#ffffffff>";
    string helpString = 
        "-----------------Command List------------------\n" +
        "clear (Clear the console window)\n" +
        "superspeed [speed multiplier] (Modify the run speed of player)\n" +
        "-----------------------------------------------\n";

    float originalPlayerSpeed;

    public static DeveloperCheat Instance;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = FindObjectOfType<DeveloperCheat>();
        content.text = "Type in \'?\' for help\n";
        input.text = string.Empty;

        InitVariables();
    }

    private void InitVariables()
    {
        originalPlayerSpeed = GameManager.Instance.playerMovement.runSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals) && GameManager.Instance.activeMenu == null)
        {
            GameManager.Instance.statePaused();
            GameManager.Instance.activeMenu = childern;
            GameManager.Instance.activeMenu.SetActive(GameManager.Instance.isPaused);
        }
        else if (Input.GetKeyDown(KeyCode.Equals) && GameManager.Instance.activeMenu == childern)
        {
            input.text = string.Empty;
            GameManager.Instance.stateUnpaused();
        }
    }

    public void SuperSpeed(float multiplier)
    {
        if (multiplier >= 0)
        {
            GameManager.Instance.playerMovement.runSpeed = originalPlayerSpeed * multiplier;
            AddContent("Speed multiplier now is " + multiplier + '\n', textColorNotification);
        }
        else
        {
            AddContent(commandErrorString, textColorWarning);
        }
    }

    public void ComputeInput()
    {
        string str = input.text;
        if (str.EndsWith("\n"))
        {
            if (!string.IsNullOrWhiteSpace(str.Remove(str.Length - 1)))
            {
                content.text += str;
                string[] serparators = new string[] { " ", "\n" };
                string[] tokens = str.Split(serparators, System.StringSplitOptions.None);
                switch (tokens[0].ToLower())
                {
                    case "?":
                        AddContent(helpString);
                        break;
                    case "clear":
                        content.text = string.Empty;
                        AddContent("Type in \'?\' for help\n");
                        break;
                    case "superspeed":
                        if (tokens.Length >= 1)
                        {
                            float multiplier = 0;
                            if (float.TryParse(tokens[1], out multiplier))
                            {
                                SuperSpeed(multiplier);   
                            }
                            else
                            {
                                AddContent(paramErrorString, textColorWarning);
                            }
                        }
                        break;
                    default:
                        AddContent(commandErrorString, textColorWarning);
                        break;
                }
            }
            input.text = string.Empty;
        }
    }

    void AddContent(string text, string textColor = null)
    {
        if (textColor == null)
        {
            textColor = textColorNormal;
        }
        string txt = textColor + text + "</color>";
        content.text += txt;
    }
}
