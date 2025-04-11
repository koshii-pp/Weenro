using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private PlayerControllerTakeUno player;
    [SerializeField] private Slider staminaSlider;
    
    void Start()
    {
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = player.MaxStamina;
        }
    }
    
    void Update()
    {
        if (staminaSlider != null && player != null)
        {
            staminaSlider.value = player.CurrentStamina;
        }
    }
} 