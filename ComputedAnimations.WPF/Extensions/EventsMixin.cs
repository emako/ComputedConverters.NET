using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace System.Windows
{
    internal static class EventsMixin
    {
        internal static FrameworkElementEvents Events(this FrameworkElement element) => new FrameworkElementEvents(element);

        internal class FrameworkElementEvents
        {
            private readonly FrameworkElement _element;

            internal FrameworkElementEvents(FrameworkElement element) => _element = element;

            internal IObservable<EventPattern<RoutedEventArgs>> Loaded =>
                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    h => _element.Loaded += h,
                    h => _element.Loaded -= h);

            internal IObservable<EventPattern<RoutedEventArgs>> Unloaded =>
                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    h => _element.Unloaded += h,
                    h => _element.Unloaded -= h);

            internal IObservable<EventPattern<RoutedEventArgs>> LoadedUntilUnloaded =>
                Loaded
                    .DistinctUntilChanged()
                    .TakeUntil(Unloaded);

            internal IObservable<EventPattern<RoutedEventArgs>> GotFocus =>
                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    h => _element.GotFocus += h,
                    h => _element.GotFocus -= h);

            internal IObservable<EventPattern<RoutedEventArgs>> LostFocus =>
                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    h => _element.LostFocus += h,
                    h => _element.LostFocus -= h);

            internal IObservable<EventPattern<SizeChangedEventArgs>> SizeChanged =>
                Observable.FromEventPattern<SizeChangedEventHandler, SizeChangedEventArgs>(
                    h => _element.SizeChanged += h,
                    h => _element.SizeChanged -= h);

            internal IObservable<EventPattern<MouseEventArgs>> PointerEntered =>
                Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
                    h => _element.MouseEnter += h,
                    h => _element.MouseEnter -= h);

            internal IObservable<EventPattern<MouseEventArgs>> PointerExited =>
                Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
                    h => _element.MouseLeave += h,
                    h => _element.MouseLeave -= h);

            // Specific section for DataContextChanged for each platform
            // =========================================================
            internal IObservable<EventPattern<DependencyPropertyChangedEventArgs>> DataContextChanged =>
                Observable.FromEventPattern<DependencyPropertyChangedEventHandler, DependencyPropertyChangedEventArgs>(
                    h => _element.DataContextChanged += h,
                    h => _element.DataContextChanged -= h);

            // =========================================================
        }
    }
}

namespace System.Windows.Controls
{
    internal static class EventsMixin
    {
        internal static ItemContainerGeneratorEvents Events(this ItemContainerGenerator element) => new ItemContainerGeneratorEvents(element);

        internal class ItemContainerGeneratorEvents
        {
            private readonly ItemContainerGenerator _element;

            internal ItemContainerGeneratorEvents(ItemContainerGenerator element) => _element = element;

            internal IObservable<EventPattern<EventArgs>> ContainersGenerated =>
                Observable.FromEventPattern<EventHandler, EventArgs>(
                    h => _element.StatusChanged += h,
                    h => _element.StatusChanged -= h);
        }

        internal static ListBoxItemEvents Events(this ListBoxItem element) => new ListBoxItemEvents(element);

        internal class ListBoxItemEvents
        {
            private readonly ListBoxItem _element;

            internal ListBoxItemEvents(ListBoxItem element) => _element = element;

            internal IObservable<EventPattern<RoutedEventArgs>> Loaded =>
                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    h => _element.Loaded += h,
                    h => _element.Loaded -= h);
        }
    }
}
