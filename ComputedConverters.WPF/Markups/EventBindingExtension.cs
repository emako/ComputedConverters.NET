using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(Delegate))]
public sealed class EventBindingExtension : MarkupExtension
{
    private static readonly MethodInfo? EventHandlerImplMethod = typeof(EventBindingExtension).GetMethod(nameof(EventHandlerImpl), new Type[] { typeof(object), typeof(EventArgs), typeof(string) });

    public string Command { get; set; }

    public EventBindingExtension(string command)
    {
        if (string.IsNullOrEmpty(command))
        {
            throw new ArgumentException("Command cannot be null or empty", nameof(command));
        }

        Command = command;
    }

    public static void EventHandlerImpl(object sender, EventArgs eventArgs, string commandName)
    {
        if (sender is FrameworkElement frameworkElement
         && frameworkElement.DataContext is object dataContext
         && dataContext.GetType().GetProperty(commandName)?.GetValue(dataContext) is ICommand command)
        {
            RelayEventParameter commandParameter = new(sender, eventArgs);
            if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget targetProvider)
            if (targetProvider.TargetObject is FrameworkElement)
                if (targetProvider.TargetProperty is MemberInfo memberInfo)
                {
                    Type? eventHandlerType = null!;

                    if (memberInfo is EventInfo eventInfo)
                    {
                        eventHandlerType = eventInfo.EventHandlerType ?? throw new InvalidOperationException("Could not create event binding.");
                    }
                    else if (memberInfo is MethodInfo methodInfo)
                    {
                        eventHandlerType = methodInfo.GetParameters()[1].ParameterType;
                    }
                    else
                    {
                        return null;
                    }

                    MethodInfo handler = eventHandlerType.GetMethod("Invoke") ?? throw new InvalidOperationException("Could not create event binding.");
                    DynamicMethod method = new(string.Empty, handler.ReturnType, [typeof(object), typeof(object)]);

                    ILGenerator ilGenerator = method.GetILGenerator();
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                    ilGenerator.Emit(OpCodes.Ldstr, Command);
                    ilGenerator.Emit(OpCodes.Call, EventHandlerImplMethod ?? throw new InvalidOperationException("Could not create event binding."));
                    ilGenerator.Emit(OpCodes.Ret);

                    return method.CreateDelegate(eventHandlerType);
                }

        throw new InvalidOperationException("Could not create event binding.");
    }
}

public sealed class RelayEventParameter(object sender, EventArgs e) : Tuple<object, EventArgs>(sender, e)
{
    public object Sender => Item1;

    public EventArgs? EventArgs => Item2;

    public object GetSender()
    {
        return Item1;
    }

    public TSender GetSender<TSender>()
    {
        return (TSender)Item1;
    }

    public EventArgs GetEventArgs()
    {
        return Item2;
    }

    public TEventArgs GetEventArgs<TEventArgs>() where TEventArgs : EventArgs
    {
        return (TEventArgs)Item2;
    }

    public (object Sender, EventArgs EventArgs) Deconstruct()
    {
        return (Item1, Item2);
    }

    public (object Sender, TEventArgs EventArgs) Deconstruct<TEventArgs>() where TEventArgs : EventArgs
    {
        return (Item1, (TEventArgs)Item2);
    }

    public (TSender Sender, TEventArgs EventArgs) Deconstruct<TSender, TEventArgs>() where TEventArgs : EventArgs
    {
        return ((TSender)Item1, (TEventArgs)Item2);
    }

    public void Deconstruct(out EventArgs e)
    {
        e = Item2;
    }

    public void Deconstruct<TEventArgs>(out TEventArgs e) where TEventArgs : EventArgs
    {
        e = (TEventArgs)Item2;
    }

    public void Deconstruct(out object sender, out EventArgs e)
    {
        sender = Item1;
        e = Item2;
    }

    public void Deconstruct<TEventArgs>(out object sender, out TEventArgs e) where TEventArgs : EventArgs
    {
        sender = Item1;
        e = (TEventArgs)Item2;
    }
}
