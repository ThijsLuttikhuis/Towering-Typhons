using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : Billboard {
    public Slider slider;
    
    public SliderBar(float startValue = 100.0f, float maxValue = 100.0f) {
        slider.maxValue = maxValue;
        slider.value = startValue;
    }
    
    public void SetValue(float value, float maxValue) {
        slider.maxValue = maxValue;
        slider.value = value;
    }
    
    public void SetValue(float value) {
        slider.value = value;
    }
}
