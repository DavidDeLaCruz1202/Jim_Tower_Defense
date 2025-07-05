using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cameraRef;
    public bool Pickle;

    public void UpdateHealthBar(float healthValue, float maxHealthValue)
    {
        slider.maxValue = maxHealthValue;
        slider.value = healthValue;
    }

    private void Update()
    {
        if (!Pickle)
            transform.rotation = cameraRef.transform.rotation; // Used this to keep enemy health bar from flipping left and right
    }
}
