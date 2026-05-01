using Godot;

namespace ProceduralFoliageGenerator.ViewModel;

public partial class FileDialogController : Control
{
    private PathInput _currentPathInput;

    [Export] public FileDialog FileDialog { get; set; }

    public override void _Ready()
    {
        if (FileDialog is null)
        {
            FileDialog = new FileDialog();
            FileDialog.Access = FileDialog.AccessEnum.Filesystem;
            FileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
            AddChild(FileDialog);
        }
        // FileDialog.FileSelected += OnFileSelected;
        // FileDialog.CloseRequested += OnFileDialogCloseRequested;

        base._Ready();
    }

    public virtual void OnFileDialogWriteRequested(string extensions, string description, PathInput input)
    {
        FileDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
    }

    /// <summary>
    ///     Signal handler for handling requests of opening the file explorer node.
    /// </summary>
    /// <param name="extensions"></param>
    /// <param name="description"></param>
    /// <param name="input"></param>
    public virtual void OnFileDialogReadRequested(string extensions, string description, PathInput input)
    {
        FileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
    }

    public virtual void OnFileDialogOpenRequested(string extensions, string description, PathInput input)
    {
        _currentPathInput = input;
        FileDialog.AddFilter(extensions, description);
        FileDialog.CloseRequested += OnFileDialogCloseRequested;
        FileDialog.FileSelected += OnFileSelected;
        FileDialog.Canceled += OnFileDialogCloseRequested;

        FileDialog.Show();
    }

    public void OnFileSelected(string path)
    {
        FileDialog.ClearFilters();
        _currentPathInput.Text = path;
        _currentPathInput.EmitSignal(LineEdit.SignalName.TextSubmitted, path);
        FileDialog.EmitSignal(FileDialog.SignalName.CloseRequested);
        FileDialog.Hide();
    }


    /// <summary>
    ///     Signal handler for handling the requests of closing the file explorer node.
    /// </summary>
    public virtual void OnFileDialogCloseRequested()
    {
        FileDialog.ClearFilters();
        FileDialog.CurrentFile = "";
        FileDialog.FileSelected -= OnFileSelected;
        FileDialog.CloseRequested -= OnFileDialogCloseRequested;
        FileDialog.Hide();
    }
}