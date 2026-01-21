using Godot;
using System;
using ProceduralFoliageGenerator.ViewModel;

namespace ProceduralFoliageGenerator.ViewModel
{
    /// <summary>
    /// Custom class for handling the user request for foliage generation. 
    /// </summary>
    public partial class DialogPopupButton : Button
    {
        
        [Export] 
        public FileDialogController Dialog  { get; set; }

        public override void _Ready()
        {
            Pressed += Dialog.Show;
            base._Ready();
        }
    }
    
}
