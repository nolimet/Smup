using Data;
using UnityEngine;

namespace OverrideTest
{
    public class OverrideableDisplayingTest : MonoBehaviour
    {
        [SerializeField] private Overrideable<int> someInterger = 6;
    }
}
