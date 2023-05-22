using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    TMP_InputField StartDebt, MaxHp, MaxDays, HealCost, EatCost, HospitalCost, ArrowCost;
    public AudioClip BgMusic;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
        GameManager.instance.SetGameState(GameManager.GameState.Menu);

        if(menuType == MenuType.Loss)
        {
            GameObject.Find("DebtGained").GetComponent<TextMeshProUGUI>().text = "Total Debt Gained: " + GameManager.instance.totalDebt.ToString("N0") + "g";
            StartCoroutine(GameManager.instance.FadeOpacityTo(0));
        }
        else if(menuType == MenuType.Win)
        {
            GameObject.Find("DebtGained").GetComponent<TextMeshProUGUI>().text = "Total Debt Paid: " + GameManager.instance.totalDebt.ToString("N0") + "g";
            StartCoroutine(GameManager.instance.FadeOpacityTo(0,2));
        }
        else if(menuType == MenuType.Main)
        {
            StartDebt = canvas.GetChild(3).GetChild(0).GetComponent<TMP_InputField>();
            MaxHp = canvas.GetChild(3).GetChild(1).GetComponent<TMP_InputField>();
            MaxDays = canvas.GetChild(3).GetChild(2).GetComponent<TMP_InputField>();
            HealCost = canvas.GetChild(3).GetChild(3).GetComponent<TMP_InputField>();
            EatCost = canvas.GetChild(3).GetChild(4).GetComponent<TMP_InputField>();
            HospitalCost = canvas.GetChild(3).GetChild(5).GetComponent<TMP_InputField>();
            ArrowCost = canvas.GetChild(3).GetChild(6).GetComponent<TMP_InputField>();

            SetDefaultSettings();

            canvas.GetChild(1).gameObject.SetActive(false);
            canvas.GetChild(2).gameObject.SetActive(false);
            canvas.GetChild(3).gameObject.SetActive(false);
        }
        
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

    public void GoToSettings()
    {
        canvas.GetChild(currentScreen).gameObject.SetActive(false);
        currentScreen = 2;
        canvas.GetChild(currentScreen).gameObject.SetActive(true);
        SetFieldValues();
    }

    public void StartGame()
    {
        GameManager.instance.SwitchScene("BossScene");
    }

    public void SetDefaultSettings()
    {
        GameManager.instance.InitializeToDefaultValues();
        SetFieldValues();
    }

    public void SetFieldValues()
    {
            StartDebt.text = GameManager.instance.totalDebt.ToString();
            MaxHp.text = GameManager.instance.playerMaxHp.ToString();
            MaxDays.text = GameManager.instance.maxDays.ToString();
            HealCost.text = GameManager.instance.healCost.ToString();
            EatCost.text = GameManager.instance.eatCost.ToString();
            HospitalCost.text = GameManager.instance.hospitalCost.ToString();
            ArrowCost.text = GameManager.instance.arrowCost.ToString();
    }
}
