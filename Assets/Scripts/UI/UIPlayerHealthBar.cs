using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealthBar : MonoBehaviour
{
    Slider slider;


    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetHealthBarActive()
    {
        slider.gameObject.SetActive(true);
    }
    public void SetHealthBarInactive()
    {
        slider.gameObject.SetActive(false);
    }

    public void SetPlayerMaxHealth(float healthMax)
    {
        slider.maxValue = healthMax;
    }

    public void SetPlayerCurrentHealth(float health)
    {
        slider.value = health;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
