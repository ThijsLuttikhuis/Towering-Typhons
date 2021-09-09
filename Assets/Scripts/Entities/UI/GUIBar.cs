using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIBar : MonoBehaviour {
    public Slider slider;
    public Slider referenceSlider;
    
    private void LateUpdate() {
        slider.maxValue = referenceSlider.maxValue;
        slider.value = referenceSlider.value;
    }
}
