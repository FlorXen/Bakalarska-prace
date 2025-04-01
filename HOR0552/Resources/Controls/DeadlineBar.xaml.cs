using CommunityToolkit.Mvvm.ComponentModel;

namespace HOR0552.Controls;

public partial class DeadlineBar : ContentView
{
    private bool isSizeAllocated = false;

    public DeadlineBar()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty MaxValueProperty =
        BindableProperty.Create(nameof(MaxValue), typeof(int), typeof(DeadlineBar), 1, propertyChanged: OnValuesChanged);

    public int MaxValue
    {
        get => (int)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    public static readonly BindableProperty CurrentValueProperty =
        BindableProperty.Create(nameof(CurrentValue), typeof(int), typeof(DeadlineBar), 1, propertyChanged: OnValuesChanged);

    public int CurrentValue
    {
        get => (int)GetValue(CurrentValueProperty);
        set => SetValue(CurrentValueProperty, value);
    }

    private static void OnValuesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (DeadlineBar)bindable;
        control.TryUpdateValues();
    }

    private void TryUpdateValues()
    {
        if (isSizeAllocated)
        {
            UpdateDeadlineBar(this);
        }
    }

    private void UpdateDeadlineBar(DeadlineBar control)
    {
        double value = 0;

        if (control.CurrentValue <= 0)
        {
            value = 0;
        }
        else if (control.CurrentValue >= control.MaxValue)
        {
            value = 1;
        }
        else
        {
            value = control.CurrentValue / (double)control.MaxValue;
        }

        control.backgroundBox.WidthRequest = (control.Width - 4) * value;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (!isSizeAllocated && width != -1 && height != -1)
        {
            isSizeAllocated = true;
            TryUpdateValues();
        }
    }
}

