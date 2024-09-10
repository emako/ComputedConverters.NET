using Microsoft.Xaml.Behaviors;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ComputedBehaviors;

public sealed class TextBoxInvalidFileNameBehavior : Behavior<TextBox>
{
    private Regex InvalidFileNameRegex { get; } = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.TextChanged += OnTextChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.TextChanged -= OnTextChanged;
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        string newText = InvalidFileNameRegex.Replace(AssociatedObject.Text, string.Empty);

        if (newText != AssociatedObject.Text)
        {
            int selectionStart = AssociatedObject.SelectionStart;
            AssociatedObject.Text = newText;
            AssociatedObject.SelectionStart = selectionStart > newText.Length ? newText.Length : selectionStart;
        }
    }
}
