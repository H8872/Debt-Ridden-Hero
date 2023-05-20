using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    enum MenuType {
        Main,
        Loss,
        Win
    }
    [SerializeField] MenuType menuType;
    int currentScreen = 0;
    Transform canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;

        GameManager.instance.SetGameState(GameManager.GameState.Menu);
    }

    public void GoToMain()
    {
        GameManager.instance.SwitchScene("MainMenu");
    }

    public void QuitGame(bool confirm)
    {
        GameObject confirmBox = canvas.GetChild(currentScreen).Find("ConfirmBox").gameObject;
        if(confirmBox.activeSelf)
        {
            if(confirm)
                Application.Quit();
            else
                confirmBox.SetActive(false);
        }
        else
            confirmBox.SetActive(true);
    }

    public void GoToPlay()
    {
        canvas.GetChild(currentScreen).gameObject.SetActive(false);
        currentScreen = 1;
        canvas.GetChild(currentScreen).gameObject.SetActive(true);
    }

    public void StartGame()
    {
        GameManager.instance.SwitchScene("BossScene");
    }

    public void SetDefaultSettings()
    {
        GameManager.instance.InitializeToDefaultValues();
    }
}
