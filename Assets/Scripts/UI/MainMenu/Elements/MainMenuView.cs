using System;
using UnityEngine.UIElements;

namespace Smup.UI.MainMenu.Elements
{
    public class MainMenuView : VisualElement
    {
        public event Action StartClicked;
        public event Action UpgradesClicked;
        public event Action ExitClicked;

        public MainMenuView()
        {
            Add(new Button(() => StartClicked?.Invoke()) { text = "Start" });
            Add(new Button(() => UpgradesClicked?.Invoke()) { text = "Upgrades" });
            Add(new Button(() => ExitClicked?.Invoke()) { text = "Exit" });
        }
    }
}
