#nullable enable

using Godot;
using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class GenerationDialogController : Control
{
    [Export] private InputPanelController InputPanel { get; set; }
    [Export] private InfoPanelController InfoPanel { get; set; }

    [Export] public required Container ButtonsContainer { get; set; }

    [Export] public required Container ProgressBarContainer { get; set; }

    [Export] public Button LoadButton { get; set; }


    public override void _Ready()
    {
        GenerationExecutor.Instance.GenerationSuccess += (o, args) => OnGenerationCompletedWithSuccess();
        GenerationExecutor.Instance.GenerationFailure += (o, args) => OnGenerationCompletedWithFailure();

        base._Ready();
    }

    public void OnGenerationStarted()
    {
        var executing = GlobalModel.Instance.ExecuteGeneration();

        if (executing)
        {
            ButtonsContainer.Hide();
            ProgressBarContainer.Show();
        }
    }

    public void OnGenerationProgressed(float progress)
    {
    }

    public void OnGenerationCompletedWithFailure()
    {
        ButtonsContainer.Show();
        ProgressBarContainer.Hide();
    }

    public void OnGenerationCompletedWithSuccess()
    {
        ButtonsContainer.Show();
        ProgressBarContainer.Hide();
        LoadButton.SetDisabled(false);
    }

    public void LoadGeneratedConfig()
    {
        LoadButton.SetDisabled(true);
        Hide();
        GlobalModel.Instance.LoadGeneratedFoliage();
    }

    public void OnGenerationCanceled()
    {
        GlobalModel.Instance.ClearTemporaryGenerationData();
        Hide();
    }
}