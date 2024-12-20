﻿using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ComputedBehaviors;

public sealed class LeftContextMenuBehavior : Behavior<FrameworkElement>
{
    public Point? PlacementOffset { get; set; } = null;
    public PlacementMode Placement { get; set; } = PlacementMode.Bottom;

    public double? PlacementOffsetX
    {
        get => PlacementOffset?.X;
        set => PlacementOffset = value != null ? new(value ?? 0d, PlacementOffset?.Y ?? 0d) : null;
    }

    public double? PlacementOffsetY
    {
        get => PlacementOffset?.Y;
        set => PlacementOffset = value != null ? new(PlacementOffset?.X ?? 0d, value ?? 0d) : null;
    }

    public LeftContextMenuBehavior()
    {
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        Register(AssociatedObject, PlacementOffset, Placement);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        Unregister(AssociatedObject);
    }

    public static void Register(FrameworkElement frameworkElement, Point? placementOffset = null, PlacementMode placement = PlacementMode.Bottom)
    {
        if (frameworkElement?.ContextMenu == null)
        {
            return;
        }
        frameworkElement.PreviewMouseRightButtonUp += (_, e) => e.Handled = true;
        frameworkElement.MouseRightButtonUp += (_, e) => e.Handled = true;
        frameworkElement.PreviewMouseLeftButtonDown += (_, _) =>
        {
            ContextMenu contextMenu = frameworkElement.ContextMenu;

            if (contextMenu != null)
            {
                if (contextMenu.PlacementTarget != frameworkElement)
                {
                    contextMenu.PlacementTarget = frameworkElement;
                    contextMenu.PlacementRectangle = new Rect(placementOffset ?? new Point(), new Size(frameworkElement.ActualWidth, frameworkElement.ActualHeight));
                    contextMenu.Placement = placement;
                    contextMenu.StaysOpen = false;
                }
                contextMenu.IsOpen = !contextMenu.IsOpen;
            }
        };
    }

    public static void Unregister(FrameworkElement frameworkElement)
    {
        _ = frameworkElement;
    }
}
