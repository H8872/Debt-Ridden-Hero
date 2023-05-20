using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState{
        Menu,
        Playing,
        Shop
    }
    public GameState gameState;
    CanvasGroup fadeToWhiteGroup;

    GameObject player, boss;
    public float totalDebt = 100000f, previousDebt = 100000f, debtPaid, debtGained,
                dayNumber, maxDays = 5f, healCost, eatCost, hospitalCost, playerHp = 20, playerMaxHp = 20;
    [SerializeField] string currentScene;
    public bool playerDied = false, playerEat = false;
    

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log($"Only one {this} allowed. Deleting this {this}.");
            Destroy(gameObject);
        }
        else
            instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(gameState == GameState.Playing)
            player = GameObject.FindWithTag("Player");

        currentScene = SceneManager.GetActiveScene().name;
    }

    public void InitializeToDefaultValues()
    {
        totalDebt = 100000f;
        previousDebt = totalDebt;
        playerMaxHp = 20;
        playerHp = playerMaxHp;
        debtPaid = 0;
        debtGained = 0;
        dayNumber = 0;
        maxDays = 5f;
        healCost = 2000f;
        eatCost = 1000f;
        hospitalCost = 7500f;
        playerDied = false;
        playerEat = true;
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
            currentScene = sceneName;
            StartCoroutine(FadeOpacityTo(1,2,true));
        }
        else
            Debug.LogWarning($"Do not load the same scene. ({sceneName})");
    }

    public void SetGameState(GameState state)
    {
        if(state != gameState)
        {
            gameState = state;
            fadeToWhiteGroup = GameObject.Find("FadeToWhite").GetComponent<CanvasGroup>();
            if(fadeToWhiteGroup == null)
                Debug.LogWarning("Add FadeToWhite CanvasGroup, thanks");
            player = null;
            boss = null;
            if(gameState == GameState.Playing)
            {
                player = GameObject.FindWithTag("Player");
                boss = GameObject.FindWithTag("Boss");
            }
            else if(gameState == GameState.Menu)
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
    
    public IEnumerator FadeOpacityTo(float opacity, float seconds = 1f, bool switchScene = false)
    {
        fadeToWhiteGroup = GameObject.Find("FadeToWhite").GetComponent<CanvasGroup>();
        fadeToWhiteGroup.blocksRaycasts = true;
        float initialAlpha = fadeToWhiteGroup.alpha;
        float elapsedTime = 0f;

        while (fadeToWhiteGroup.alpha != opacity && elapsedTime < seconds)
        {
            fadeToWhiteGroup.alpha = Mathf.Lerp(initialAlpha, opacity, elapsedTime / seconds);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
        fadeToWhiteGroup.alpha = opacity;
        fadeToWhiteGroup.blocksRaycasts = false;
        if(switchScene)
            SceneManager.LoadScene(currentScene);
    }
}
