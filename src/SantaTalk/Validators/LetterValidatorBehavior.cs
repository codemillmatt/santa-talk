using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SantaTalk.Validators
{
    public class LetterValidatorBehavior : Behavior<Editor>
    {
        public static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly(nameof(IsValid), typeof(bool), typeof(LetterValidatorBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(Editor bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += HandleTextChanged;
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            var newLetter = e.NewTextValue;
            IsValid = !String.IsNullOrWhiteSpace(newLetter) && (newLetter != Defaults.LetterText);
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= HandleTextChanged;
        }
    }
}
