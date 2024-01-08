using System;

namespace DC.Tools
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class ButtonAttribute : Attribute
    {
        public readonly string Name;

        public ButtonAttribute() { }

        public ButtonAttribute(string name) => Name = name;

        public ButtonMode Mode { get; set; } = ButtonMode.AlwaysEnabled;

        public ButtonSpacing Spacing { get; set; } = ButtonSpacing.None;

        public bool Expanded { get; set; }
    }

    public enum ButtonMode
    {
        AlwaysEnabled,
        EnabledInPlayMode,
        DisabledInPlayMode
    }

    [Flags]
    public enum ButtonSpacing
    {
        None = 0,
        Before = 1,
        After = 2
    }
}