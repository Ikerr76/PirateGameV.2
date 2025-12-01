using UnityEngine;
using UnityEngine.UI;

public class CraftingProgress : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        if (slider != null)
            slider.value = 0;

        gameObject.SetActive(false);
    }

    public void Show()
    {
        if (slider != null)
            slider.value = 0;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetProgress(float value)
    {
        if (slider != null)
            slider.value = Mathf.Clamp01(value);
    }
}