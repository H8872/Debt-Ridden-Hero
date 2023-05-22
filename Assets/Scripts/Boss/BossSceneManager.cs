using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneManager : MonoBehaviour
{
    public float Bounty = 10000;
    PlayerController player;
    bool gameEnding;
    public bool Paused;
    GameObject pauseScreen;
    GameObject confirmBox;
    public AudioClip BgMusic;
    // Start is called before the first frame update
    void Start()
    {
        gameEnding = false;
        GameManager.instance.SetGameState(GameManager.GameState.Playing);
        StartCoroutine(GameManager.instance.FadeOpacityTo(0, 2));
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        pauseScreen = GameObject.Find("PauseScreen");
        confirmBox = GameObject.Find("ConfirmBox");
        confirmBox.SetActive(false);
        pauseScreen.SetActive(false);
    }

    public void BossEnd(bool victory)
    {
        gameEnding = true;
        StartCoroutine(SlomoToSwitch());
        GameManager.instance.playerHp = player.Hp;
        GameManager.instance.debtGained = player.debt;
        if(victory)
        {
            GameManager.instance.debtPaid = Bounty;
        }
        else
        {
            GameManager.instance.playerDied = true;
        }
    }


    
    public IEnumerator SlomoToSwitch()
    {
        Time.timeScale = 0.25f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(2f);
        GameManager.instance.SwitchScene("ShopScene");
    }

    public void TogglePause()
    {
        if(pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
            Paused = false;
        }
        else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
            Paused = true;
        }
    }

    public void QuitToMenu(bool confirm)
    {
        if(confirmBox.activeSelf)
        {
            if(confirm)
            {
                GameManager.instance.SwitchScene("MainMenu");
                Time.timeScale = 1;
            }
            else
                confirmBox.SetActive(false);
        }
        else
            confirmBox.SetActive(true);
    }

    void Update() {
        if(Input.GetButtonDown("Pause") && !gameEnding)
        {
            Debug.Log("Pressed");
            TogglePause();
        }
    }
}
