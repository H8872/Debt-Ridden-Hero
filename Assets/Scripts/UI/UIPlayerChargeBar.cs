using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerChargeBar : MonoBehaviour
{
    Slider slider;


    void Awake() 
    {
        slider = GetComponentInChildren<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetChargeBarInactive();
    }

    public void SetChargeBarActive()
    {
        slider.gameObject.SetActive(true);
    }

    public void SetChargeBarInactive()
    {
        slider.gameObject.SetActive(false);
    }

    public void SetChargeBarValues(float maxCharge, float minCharge)
    {
        slider.maxValue = maxCharge;
        slider.minValue = minCharge;
        slider.value = minCharge;
    }

    public void SetCurrentBarValue(float chargeTime)
    {
        slider.value = chargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
