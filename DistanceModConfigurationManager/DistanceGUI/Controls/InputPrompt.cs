using DistanceModConfigurationManager.DistanceGUI.Data;
using DistanceModConfigurationManager.DistanceGUI.Menu;
using System;

namespace DistanceModConfigurationManager.DistanceGUI.Controls
{
    public class InputPrompt : MenuItemBase
    {
        public string Title { get; set; }
        public string CurrentValue { get; set; }

        public Func<string> DefaultValue { get; set; }
        public Func<string, string> Validator { get; set; }
        public Action<string> SubmitAction { get; set; }
        public Action<InputPrompt> CloseAction { get; set; }
        public InputPrompt(MenuDisplayMode mode, string id, string name) : base(mode, id, name) { }
        public InputPrompt WithTitle(string title)
        {
            Title = title ?? string.Empty;
            return this;
        }
        public InputPrompt WithCurrentValue(string currentValue)
        {
            CurrentValue = currentValue ?? string.Empty;
            return this;
        }
        public InputPrompt WithDefaultValue(string defaultValue)
        {
            DefaultValue = () => defaultValue;
            return this;
        }
        public InputPrompt WithDefaultValue(Func<string> defaultValue)
        {
            DefaultValue = defaultValue ?? (() => string.Empty);
            return this;
        }
        public InputPrompt WithSubmitAction(Action<string> submitAction)
        {
            SubmitAction = submitAction ?? throw new ArgumentNullException("Submit action cannot be null.");
            return this;
        }
        public InputPrompt WithCloseAction(Action<InputPrompt> closeAction)
        {
            CloseAction = closeAction;
            return this;
        }
        public InputPrompt ValidatedBy(Func<string, string> validator)
        {
            Validator = validator;
            return this;
        }
        protected virtual bool OnSubmit(out string error, string input)
        {
            error = Validator?.Invoke(input);

            if (error == null)
            {
                SubmitAction?.Invoke(input);
                CurrentValue = input;
                return true;
            }

            return false;
        }
        protected virtual void OnCancel()
        {
            CloseAction?.Invoke(this);
        }
        protected virtual void OnTweak()
        {
            InputPromptPanel.Create(
                new InputPromptPanel.OnSubmit(OnSubmit),
                new InputPromptPanel.OnPop(OnCancel),
                Title,
                CurrentValue == string.Empty ? DefaultValue() : CurrentValue
            );
        }
        public override void Tweak(ModdingMenu menu)
        {
            menu.TweakAction(Name, () =>
            {
                OnTweak();
                base.Tweak(menu);
            }, Description);
        }
    }
}
