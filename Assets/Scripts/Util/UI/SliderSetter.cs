using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Util.UI
{
    /// <summary>
    /// A classes that is used to create a slider for the Scrollrect thatdoes not change size
    /// </summary>
    public class SliderSetter : MonoBehaviour
    {
        public Slider slider;
        public ScrollRect scrollRect;
        [FormerlySerializedAs("StartValue")] public float startValue;

        private void Start()
        {
            slider.value = startValue;
            scrollRect.verticalNormalizedPosition = startValue;
        }
    }
}
