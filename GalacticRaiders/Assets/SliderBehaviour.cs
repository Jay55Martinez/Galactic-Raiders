using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderBehaviour : MonoBehaviour
{
    TMP_Text sliderText;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        sliderText = GetComponentInChildren<TMP_Text>();
        slider = GetComponent<Slider>();

        slider.value = GameManager.sensitivity;
        sliderText.text = slider.value.ToString("0.0");
    }

    public void UpdateSensitivity(System.Single val) {
        sliderText.text = val.ToString("0.0");
        GameManager.UpdateSens(val);
    }
}
