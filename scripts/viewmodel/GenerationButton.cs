using Godot;
using System;
using ViewModel;

namespace ViewModel
{
    /// <summary>
    /// Custom class for handling the user request for foliage generation. 
    /// </summary>
    public partial class GenerationButton : Button
    {
        
        [Export] 
        public FileDialogController Dialog  { get; set; }
        /// <summary>
        /// Signal handler function.
        /// Responsible for prompting the start of the foliage generation and the input of additional arguments for the generator.
        /// </summary>
        public void OnPressed()
        {
            Dialog.Show();
        }
    }
    
}
