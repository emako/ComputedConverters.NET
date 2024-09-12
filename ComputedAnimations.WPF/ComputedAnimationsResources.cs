using System;
using System.Windows;

namespace ComputedAnimations;

public sealed class ComputedAnimationsResources : ResourceDictionary
{
    public ComputedAnimationsResources()
    {
        Source = new Uri("pack://application:,,,/ComputedAnimations;component/ComputedAnimations/DefaultAnimations.xaml");
    }
}
