using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHealthBar : MonoBehaviour
{
    Slider slider;


    void Awake() 
    {
        slider = GetComponentInChildren<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //SetHealthBarInactive();
    }

    public void SetHealthBarActive()
    {
        slider.gameObject.SetActive(true);
    }

    public void SetHealthBarInactive()
    {
        slider.gameObject.SetActive(false);
    }

    public void SetBossMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetBossCurrentHealth(float health)
    {
        slider.value = health;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
