using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Bar
{
    public RectTransform bar1, bar2;
    public Text valueText;
    public Color[] barColours;
    Vector2 StartSize;
    float maxValue, overloadFactor, frac, maxBarValue;
    bool useMutlibars;

    /// <summary>
    /// Start function for A bar
    /// </summary>
    /// <param name="maxNormal">Used to draw the overload at the right moment</param>
    /// <param name="maxOverloadFactor">How many times more can the overload hold than the Normal</param>
    public void init(float maxNormal, float overloadFactor)
    {
        this.overloadFactor = overloadFactor;
        maxValue = maxNormal;

        StartSize = bar1.sizeDelta;
        frac = 1f / maxNormal;

        useMutlibars = false;
    }

    /// <summary>
    /// Start function for a bar
    /// </summary>
    /// <param name="maxValue">max value of the system</param>
    /// <param name="maxBarValue">max value of a single bar</param>
    public void init(float maxValue, int maxBarValue)
    {
        this.maxValue = maxValue;
        this.maxBarValue = maxBarValue;

        if (maxBarValue * barColours.Length < maxValue)
            this.maxBarValue = maxValue / barColours.Length;

        StartSize = bar1.sizeDelta;
        if (maxValue > this.maxBarValue)
            frac = 1f / this.maxBarValue;
        else
            frac = 1f / maxValue;

        useMutlibars = true;
    }
    /// <summary>
    /// Input the noneClamped value
    /// </summary>
    /// <param name="value">the level it should display</param>
    public void UpdateSize(float value)
    {
        if (valueText)
            valueText.text = Mathf.Floor(value).ToString() + " / " + Mathf.Floor(maxValue).ToString();

        if (useMutlibars)
            MultiBarSystem(value);
        else
            OverloadSystem(value);
    }

    private void MultiBarSystem(float value)
    {
        int index = Mathf.FloorToInt(value / maxBarValue);
        if (index == barColours.Length)
            index--;
        float calc = (frac * value);

        if (index > 0)
        {
            bar1.GetComponent<Image>().color = barColours[index - 1];
            bar2.GetComponent<Image>().color = barColours[index];

            bar1.sizeDelta = StartSize;
            bar2.sizeDelta = new Vector2(StartSize.x * (calc - index), StartSize.y);
        }
        else
        {
            bar1.GetComponent<Image>().color = barColours[index];

            bar1.sizeDelta = new Vector2(StartSize.x * calc, StartSize.y);
            bar2.sizeDelta = new Vector2(0.01f, StartSize.y);
        }
    }

    private void OverloadSystem(float value)
    {
        float calc = (frac * value);
        if (value < maxValue)
        {
            bar1.sizeDelta = new Vector2(StartSize.x * calc, StartSize.y);
            bar2.sizeDelta = new Vector2(0.01f, StartSize.y);
        }
        else if (value <= maxValue * overloadFactor)
        {
            bar1.sizeDelta = StartSize;
            bar2.sizeDelta = new Vector2(StartSize.x * ((calc - 1f) / overloadFactor), StartSize.y);
        }
    }


}
