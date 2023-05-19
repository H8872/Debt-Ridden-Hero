using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState{
        MainMenu,
        Playing,
        Shop
    }
    public GameState gameState;

    GameObject player, boss;
    public float totalDebt = 1000000f, previousDebt = 1000000f, debtPaid, debtGained,
                dayNumber, maxDays = 5f;
    [SerializeField] string currentScene;
    public bool playerDied = false;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log($"Only one {this} allowed. Deleting this {this}.");
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if(gameState == GameState.Playing)
            player = GameObject.FindWithTag("Player");

        currentScene = SceneManager.GetActiveScene().name;
    }

    public void AddDebt(float amount)
    {
        if(amount > 0)
        {
            debtGained += amount;
        }
        else
        {
            debtPaid += amount;
        }

        totalDebt += amount;
    }

    public void SwitchScene(string sceneName)
    {
        if(currentScene != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            currentScene = sceneName;
        }
        else
            Debug.LogWarning($"Do not load the same scene. ({sceneName})");
    }

    public void SetGameState(GameState state)
    {
        if(state != gameState)
        {
            gameState = state;
            player = null;
            boss = null;
            if(gameState == GameState.Playing)
            {
                player = GameObject.FindWithTag("Player");
                boss = GameObject.FindWithTag("Boss");
            }
            else if(gameState == GameState.MainMenu)
            {

            }
            else if(gameState == GameState.Shop)
            {

            }
        }
        else
        {
            Debug.LogWarning($"GameState is already {state}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
