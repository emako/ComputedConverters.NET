using Microsoft.Xaml.Behaviors;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ComputedBehaviors;

public class DropFileBehavior : Behavior<UIElement>
{
    public bool IsHandleDrop { get; set; } = false;
    public bool IsHandleMulti { get; set; } = false;

    public string[]? Data
    {
        get => (string[]?)GetValue(FilesProperty);
        set => SetValue(FilesProperty, value);
    }

    public static readonly DependencyProperty FilesProperty =
        DependencyProperty.Register(nameof(Data), typeof(string[]), typeof(DropFileBehavior), new UIPropertyMetadata(null));

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.AllowDrop = true;

        if (AssociatedObject is TextBox)
        {
            AssociatedObject.PreviewDragOver += OnDragOver;
            AssociatedObject.PreviewDrop += OnDrop;
        }
        else
        {
            AssociatedObject.Drop += OnDrop;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is TextBox)
        {
            AssociatedObject.PreviewDragOver -= OnDragOver;
            AssociatedObject.PreviewDrop -= OnDrop;
        }
        else
        {
            AssociatedObject.Drop -= OnDrop;
        }
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Copy;
        e.Handled = true;
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        bool isFileDrop = false;

        // Supported `DataFormats.FileDrop` only.
        // Unsupported `DataFormats.Text` and so on.
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] { } files)
            {
                Data = files;
                isFileDrop = true;
            }
        }

        if (IsHandleDrop && isFileDrop)
        {
            if (sender is TextBox textBox)
            {
                if (IsHandleMulti)
                {
                    textBox.Text = string.Join(Environment.NewLine, Data!);
                }
                else
                {
                    textBox.Text = Data!.FirstOrDefault();
                }
            }
        }
    }
}
