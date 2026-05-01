using Godot;

public partial class InstanceAmountItem : HBoxContainer
{
    [Export] public Label DescLabel;
    [Export] public Label ValueLabel;

    public override void _Ready()
    {
        base._Ready();
    }

    public void SetDescription(string description)
    {
        DescLabel.Text = description;
    }

    public void SetValue(int value)
    {
        ValueLabel.Text = value.ToString();
    }
}