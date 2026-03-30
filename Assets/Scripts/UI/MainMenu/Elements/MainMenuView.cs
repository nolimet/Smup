using System;
using UnityEngine.UIElements;

namespace Smup.UI.MainMenu.Elements
{
    public class MainMenuView : VisualElement
    {
        public const string ussClassName = "main-menu";
        public const string ussContainerClassName = ussClassName + "__container";

        public const string ussStartButtonClassName = ussClassName + "__start-button";
        public const string ussUpgradesButtonClassName = ussClassName + "__upgrades-button";
        public const string ussExitButtonClassName = ussClassName + "__exit-button";

        public const string ussTitleClassName = ussClassName + "__title";

        public event Action StartClicked;
        public event Action UpgradesClicked;
        public event Action ExitClicked;

        public MainMenuView()
        {
            AddToClassList(ussClassName);
            style.position = Position.Absolute;
            style.top = 0;
            style.left = 0;
            style.right = 0;
            style.bottom = 0;
            style.justifyContent = Justify.Center;

            var container = new VisualElement();
            container.AddToClassList(ussContainerClassName);
            Add(container);

            container.Add(new Button(() => StartClicked?.Invoke()) { text = "Start" });
            container.Add(new Button(() => UpgradesClicked?.Invoke()) { text = "Upgrades" });
            container.Add(new Button(() => ExitClicked?.Invoke()) { text = "Exit" });
        }
    }
}
