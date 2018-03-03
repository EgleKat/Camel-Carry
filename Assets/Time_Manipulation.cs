using UnityEngine;
using UnityEngine.UI;

public class Time_Manipulation : MonoBehaviour
{
    public Slider mainSlider;

    public void Start()
    {
        
        //Adds a listener to the main slider and invokes a method when the value changes.
        mainSlider.onValueChanged.AddListener(delegate
            {
                SetTimeScale(mainSlider.value);
            });

        mainSlider.value = 1;
        Time.timeScale = 1;
    }


    public void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

}