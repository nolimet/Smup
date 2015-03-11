using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Bar
{
    public RectTransform Normal, Overload;
    public Text valueText;
    Vector2 StartSize;
    float maxNormal, overloadFactor, frac;

    /// <summary>
    /// Start function for A bar
    /// </summary>
    /// <param name="maxNormal">Used to draw the overload at the right moment</param>
    /// <param name="maxOverloadFactor">How many times more can the overload hold than the Normal</param>
    public void init(float maxNormal, float overloadFactor)
    {
        this.overloadFactor = overloadFactor;
        this.maxNormal = maxNormal;

        StartSize = Normal.sizeDelta;
        frac = 1f / maxNormal;
    }

    /// <summary>
    /// Input the noneClamped value
    /// </summary>
    /// <param name="value">the level it should display</param>
    public void UpdateSize(float value)
    {
        if (valueText)
            valueText.text = Mathf.Floor(value).ToString() + " / " + Mathf.Floor(maxNormal).ToString();


        float calc = (frac * value);
        if (value < maxNormal)
        {
            Normal.sizeDelta = new Vector2(StartSize.x * calc, StartSize.y);
            Overload.sizeDelta = new Vector2(0.01f, StartSize.y);
        }
        else if (value <= maxNormal * overloadFactor)
        {
            Normal.sizeDelta = StartSize;
            Overload.sizeDelta = new Vector2(StartSize.x * ((calc - 1f) / overloadFactor), StartSize.y);
        }

    }


}
