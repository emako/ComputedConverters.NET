### 5. Computed Behaviors

Behaviors that make non bindable property to be bindable property.

```xaml
<Button Height="50" Content="Please mouse over here">
    <i:Interaction.Triggers>
        <i:EventTrigger EventName="MouseEnter">
            <!-- Originally, IsMouseOver is not a bindable property -->
            <b:ButtonSetStateToSourceAction Source="{Binding ButtonMouseOver, Mode=TwoWay}" Property="IsMouseOver" />
        </i:EventTrigger>
        <i:EventTrigger EventName="MouseLeave">
            <!--  Originally, IsMouseOver is a not bindable property  -->
            <b:ButtonSetStateToSourceAction Source="{Binding ButtonMouseOver, Mode=TwoWay}" Property="IsMouseOver" />
        </i:EventTrigger>
    </i:Interaction.Triggers>
</Button>
```

```xaml
<TextBox>
    <i:Interaction.Behaviors>
        <b:TextBoxBindingSupportBehavior
            SelectedText="{Binding SelectedText}"
            SelectionLength="{Binding SelectionLength}"
            SelectionStart="{Binding SelectionStart}" />
    </i:Interaction.Behaviors>
</TextBox>
```

```xaml
<PasswordBox>
    <i:Interaction.Behaviors>
        <b:PasswordBoxBindingSupportBehavior Password="{Binding Password}" />
    </i:Interaction.Behaviors>
</PasswordBox>
```
