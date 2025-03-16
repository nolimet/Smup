using System;
using UnityEngine;
using UnityEngine.UI;

namespace ValueClasses
{
    [Serializable]
    public class Bar
    {
        public RectTransform bar1, bar2;
        public Text valueText;
        public Color[] barColours;
        private Vector2 _startSize;
        private float _maxValue, _overloadFactor, _frac, _maxBarValue;
        private bool _useMutlibars;

        /// <summary>
        /// Start function for A bar
        /// </summary>
        /// <param name="maxNormal">Used to draw the overload at the right moment</param>
        /// <param name="maxOverloadFactor">How many times more can the overload hold than the Normal</param>
        public void Init(float maxNormal, float overloadFactor)
        {
            _overloadFactor = overloadFactor;
            _maxValue = maxNormal;

            _startSize = bar1.sizeDelta;
            _frac = 1f / maxNormal;

            _useMutlibars = false;
        }

        /// <summary>
        /// Start function for a bar
        /// </summary>
        /// <param name="maxValue">max value of the system</param>
        /// <param name="maxBarValue">max value of a single bar</param>
        public void Init(float maxValue, int maxBarValue)
        {
            _maxValue = maxValue;
            _maxBarValue = maxBarValue;

            if (maxBarValue * barColours.Length < maxValue)
                _maxBarValue = maxValue / barColours.Length;

            _startSize = bar1.sizeDelta;
            if (maxValue > _maxBarValue)
                _frac = 1f / _maxBarValue;
            else
                _frac = 1f / maxValue;

            _useMutlibars = true;
        }

        /// <summary>
        /// Input the noneClamped value
        /// </summary>
        /// <param name="value">the level it should display</param>
        public void UpdateSize(float value)
        {
            if (valueText)
                valueText.text = Mathf.Floor(value).ToString() + " / " + Mathf.Floor(_maxValue).ToString();

            if (_useMutlibars)
                MultiBarSystem(value);
            else
                OverloadSystem(value);
        }

        private void MultiBarSystem(float value)
        {
            var index = Mathf.FloorToInt(value / _maxBarValue);
            if (index == barColours.Length)
                index--;

            var calc = _frac * value;

            if (index > 0)
            {
                bar1.GetComponent<Image>().color = barColours[index - 1];
                bar2.GetComponent<Image>().color = barColours[index];

                bar1.sizeDelta = _startSize;
                bar2.sizeDelta = new Vector2(_startSize.x * (calc - index), _startSize.y);
            }
            else
            {
                bar1.GetComponent<Image>().color = barColours[index];

                bar1.sizeDelta = new Vector2(_startSize.x * calc, _startSize.y);
                bar2.sizeDelta = new Vector2(0.01f, _startSize.y);
            }
        }

        private void OverloadSystem(float value)
        {
            var calc = _frac * value;
            if (value < _maxValue)
            {
                bar1.sizeDelta = new Vector2(_startSize.x * calc, _startSize.y);
                bar2.sizeDelta = new Vector2(0.01f, _startSize.y);
            }
            else if (value - _maxValue <= _maxValue * _overloadFactor)
            {
                bar1.sizeDelta = _startSize;
                bar2.sizeDelta = new Vector2(_startSize.x * ((calc - 1f) / _overloadFactor), _startSize.y);
            }
        }
    }
}
