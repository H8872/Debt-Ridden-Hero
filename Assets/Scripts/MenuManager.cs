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

    bool quitConfirm = false;
    int currentScreen = 0;
    Transform canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
