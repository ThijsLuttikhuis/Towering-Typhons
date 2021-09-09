using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour {
    private Slider slider;
    private Ability ability;

    public void SetSlider(Slider slider_) {
        slider = slider_;
    }
    
    public void SetAbility(Ability ability_) {
        ability = ability_;
    }
    
    private void LateUpdate() {
        slider.maxValue = ability.GetCooldownTime();
        slider.value = ability.GetCooldownRemaining();
    }
}