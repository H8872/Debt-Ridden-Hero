using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    Transform canvas;
    CanvasGroup fadeToWhiteGroup;
    [SerializeField] float previousDebt, tempDebt, totalDebt, healCost, eatCost, hospitalCost;
    TextMeshProUGUI summaryTitle, previousDebtText, bountyText, additionalText,
                    healAmountText, eatAmountText, debtRemainingText, totalDebtText,
                    hospitalVisitText, hospitalVisitAmountText;
    Toggle healToggle, eatToggle;
    Slider progress;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Shop);
        StartCoroutine(GameManager.instance.FadeOpacityTo(0,2));

        GameManager.instance.dayNumber++;
        totalDebt = GameManager.instance.totalDebt;
        previousDebt = GameManager.instance.previousDebt;
        healCost = GameManager.instance.healCost;
        eatCost = GameManager.instance.eatCost;
        hospitalCost = GameManager.instance.hospitalCost;

        canvas = GameObject.Find("Canvas").transform;
        fadeToWhiteGroup = GameObject.Find("FadeToWhite").GetComponent<CanvasGroup>();

        summaryTitle = canvas.Find("DaySummary").GetComponent<TextMeshProUGUI>();
        summaryTitle.text = $"Day {GameManager.instance.dayNumber} of {GameManager.instance.maxDays} Summary";
        previousDebtText = canvas.Find("PreviousDebtAmount").GetComponent<TextMeshProUGUI>();
        previousDebtText.text = previousDebt.ToString("N0") + "g";
        bountyText = canvas.Find("DebtPaidAmount").GetComponent<TextMeshProUGUI>();
        bountyText.text = GameManager.instance.debtPaid.ToString("N0") + "g";
        additionalText = canvas.Find("DebtGainedAmount").GetComponent<TextMeshProUGUI>();
        additionalText.text = GameManager.instance.debtGained.ToString("N0") + "g";
        healAmountText = canvas.Find("HealWoundsAmount").GetComponent<TextMeshProUGUI>();
        healAmountText.text = healCost.ToString("N0") + "g";
        healToggle = canvas.Find("HealWoundsToggle").GetComponent<Toggle>();
        healToggle.isOn = true;
        if(GameManager.instance.playerDied)
            healToggle.interactable = false;
        eatAmountText = canvas.Find("EatFoodAmount").GetComponent<TextMeshProUGUI>();
        eatAmountText.text = eatCost.ToString("N0") + "g";
        eatToggle = canvas.Find("EatFoodToggle").GetComponent<Toggle>();
        eatToggle.isOn = true;
        hospitalVisitText = canvas.Find("HospitalVisit").GetComponent<TextMeshProUGUI>();
        hospitalVisitAmountText = canvas.Find("HospitalVisitAmount").GetComponent<TextMeshProUGUI>();
        hospitalVisitAmountText.text = hospitalCost.ToString("N0") + "g";
        if(GameManager.instance.playerDied)
        {
            hospitalVisitText.enabled = false;
            hospitalVisitAmountText.enabled = false;
        }
        debtRemainingText = canvas.Find("DebtRemainingAmount").GetComponent<TextMeshProUGUI>();
        totalDebtText = canvas.Find("TotalDebtAmount").GetComponent<TextMeshProUGUI>();
        progress = canvas.Find("Progress").GetComponent<Slider>();


        RecalculateDebt();
        RefreshTextValues();
    }

    public void RecalculateDebt()
    {
        tempDebt = GameManager.instance.debtGained;
        if(healToggle.isOn)
            tempDebt += healCost;
        if(eatToggle.isOn)
            tempDebt += eatCost;
        if(GameManager.instance.playerDied)
            tempDebt += hospitalCost;
        
        totalDebt = GameManager.instance.totalDebt + tempDebt;
        tempDebt += previousDebt - GameManager.instance.debtPaid;
        if(tempDebt < 0) tempDebt = 0;

        RefreshTextValues();
    }

    void RefreshTextValues()
    {
        healAmountText.color = healToggle.isOn ? Color.red : Color.gray;
        eatAmountText.color = eatToggle.isOn ? Color.red : Color.gray;
        hospitalVisitText.enabled = GameManager.instance.playerDied;
        hospitalVisitAmountText.enabled = GameManager.instance.playerDied;

        debtRemainingText.text = tempDebt.ToString("N0") + "g";
        totalDebtText.text = totalDebt.ToString("N0") + "g";

        progress.maxValue = totalDebt;
        progress.value = tempDebt;
    }

    public void EndDay()
    {
        if(tempDebt == 0)
        {
            // Debt paid! Win the game!
            fadeToWhiteGroup.transform.GetComponent<Image>().color = Color.white;
            GameManager.instance.SwitchScene("WinScene");
        }
        else if(tempDebt > 0 && GameManager.instance.dayNumber == GameManager.instance.maxDays)
        {
            // Couldn't pay off debt before max days
            GameManager.instance.SwitchScene("LoseScene");
        }
        else
        {
            // Proceed to the next day
            if(healToggle.isOn)
                GameManager.instance.playerHp = 20;
            if(eatToggle.isOn)
                GameManager.instance.playerEat = true;
            previousDebt = tempDebt;
            GameManager.instance.debtGained = 0;
            GameManager.instance.debtPaid = 0;
            GameManager.instance.SwitchScene("BossScene");
        }
    }

}
