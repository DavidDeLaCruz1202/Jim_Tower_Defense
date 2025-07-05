using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorShifter : MonoBehaviour
{
    /* -- SOLELY USED FOR 'START WAVE' BUTTON! -- */
    public Button button;
    public Color newColor;
    public float frequency = 0.8f;
    private float darkenRatio;
    private ColorBlock cb;
    private ColorBlock original_cb;

    // Start is called before the first frame update
    void Start()
    {
        // Save the original color block so we can reset it to this if this button goes inactive
        original_cb = button.colors;

        // Set a color block to have the same colors as the button's
        cb = button.colors;
    }

    // Update is called once per frame
    void Update()
    {
        // If the button is active, keep updating color
        if (button.gameObject.activeSelf)
        {
            // This value alternates between 0 and 1, at a speed dependent on 'frequency'
            darkenRatio = Mathf.Abs(Mathf.Cos(Time.fixedTime * frequency));

            // The new color should go from white --> black --> white
            newColor = new Color(darkenRatio, darkenRatio, darkenRatio, 1f);

            // Set the "normalColor" part of the color block variable to this new darker / lighter white
            cb.normalColor = newColor;

            // Set the button's color block to this new one, replacing its "normalColor"
            button.colors = cb;
        }
        // Otherwise, reset both the button's color block and our 'cb' variable
        else
        {
            //
            button.colors = original_cb;
            cb = original_cb;
        }
    }
}
