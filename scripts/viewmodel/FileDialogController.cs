using Godot;
using System;


namespace ViewModel;

public partial class FileDialogController : Control
{
    [Export]
    public FileDialog FileDialog { get; set; }

    public override void _Ready()
    {
        FileDialog.CloseRequested += () => OnFileDialogCloseRequested();
    }
    
    public void OnFileDialogOpenRequested(string extensions,string description,PathInput input)
    {
        FileDialog.AddFilter(extensions,description);
        FileDialog.FileSelected += (path) => OnFileSelected(path,input);
        FileDialog.Show();
    }

    public void OnFileDialogCloseRequested()
    {
        FileDialog.ClearFilters();
        FileDialog.CurrentFile = "";
        FileDialog.Hide();
    }

    public void OnFileSelected(string path,PathInput input)
    {
        FileDialog.ClearFilters();
        input.Text = path;
        FileDialog.Hide();
    }

    public void OnGenerateStarted()
    {
        
        GenerationExecutor.Instance.ExecuteGeneration();
        this.Hide();
    }
}



