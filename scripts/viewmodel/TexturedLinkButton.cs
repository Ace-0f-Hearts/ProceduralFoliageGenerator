using Godot;
using System;

namespace ViewModel
{
    /// <summary>
    /// Custom textured button class for handling URL opening.
    /// </summary>
    public partial class TexturedLinkButton : TextureButton
    {
        [Export]
        public string Url { get; set; }
        
        /// <summary>
        /// Handler function of the <c>pressed</c> signal. Responsible for opening a custom URL.
        /// </summary>
        public void OnPressed()
        {
            OS.ShellOpen(Url);
        }
    }
}
