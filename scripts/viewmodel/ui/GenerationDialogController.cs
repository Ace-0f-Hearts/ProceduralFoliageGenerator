#nullable enable
using Godot;
using System;

using ProceduralFoliageGenerator.Model;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class GenerationDialogController : Control
{
    [Export]
    InputPanelController InputPanel { get; set; }
    [Export]
    InfoPanelController InfoPanel { get; set; }
    
    [Export]
    public required Container ButtonsContainer { get; set; }
    [Export]
    public required Container ProgressBarContainer { get; set; }
    
    [Export]
    public Button LoadButton { get; set; }
    

    public override void _Ready()
    {
        GenerationExecutor.Instance.GenerationSuccess += (object o, EventArgs args) => OnGenerationCompletedWithSuccess(); 
        GenerationExecutor.Instance.GenerationFailure += (object o, EventArgs args) => OnGenerationCompletedWithFailure(); 
        
        base._Ready();
    }
    
    public void OnGenerationStarted()
    {
        GlobalModel.Instance.ExecuteGeneration();
        ButtonsContainer.Hide();
        ProgressBarContainer.Show();
    }

    public void OnGenerationProgressed(float progress)
    {
        //TODO: Update progressbar   
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
        this.Hide();
        GlobalModel.Instance.LoadGeneratedFoliage();
    }

    public void OnGenerationCanceled()
    {
        GlobalModel.Instance.ClearTemporaryGenerationData();
        this.Hide();
    }

    

}




