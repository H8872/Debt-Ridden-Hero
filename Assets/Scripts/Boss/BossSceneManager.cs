using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneManager : MonoBehaviour
{
    public float Bounty = 10000;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Playing);
        StartCoroutine(GameManager.instance.FadeOpacityTo(0, 2));
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void BossEnd(bool victory)
    {
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
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(2f);
        GameManager.instance.SwitchScene("ShopScene");
    }
}
