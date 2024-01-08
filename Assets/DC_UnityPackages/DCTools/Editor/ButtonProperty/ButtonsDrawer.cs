using System.Collections.Generic;
using System.Reflection;

namespace DC.Tools.Editor
{
    public class ButtonsDrawer
    {
        public readonly List<Button> Buttons = new List<Button>();

        public ButtonsDrawer(object target)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var methods = target.GetType().GetMethods(flags);

            foreach (MethodInfo method in methods)
            {
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();

                if (buttonAttribute == null)
                    continue;

                Buttons.Add(Button.Create(method, buttonAttribute));
            }
        }

        public void DrawButtons(object[] targets)
        {
            foreach (Button button in Buttons)
            {
                button.Draw(targets);
            }
        }
    }
}