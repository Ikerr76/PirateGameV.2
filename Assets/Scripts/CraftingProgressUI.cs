using UnityEngine;
using UnityEngine.UI;

public class CraftingProgressUI : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        gameObject.SetActive(false);
        slider.value = 0;
    }

    public void Show()
    {
        slider.value = 0;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetProgress(float value)
    {
        slider.value = value;
    }
}
