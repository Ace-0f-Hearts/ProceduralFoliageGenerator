using Godot;
using System;

namespace ProceduralFoliageGenerator.ViewModel
{
    /// <summary>
    /// Custom class for handling user inputs regarding options.
    /// </summary>
    public partial class OptionsButton : MenuButton
    {
        public override void _Ready()
        {
            var popup = this.GetPopup();

            popup.IdPressed += _OnMenuSelected;
        }

        private void _OnMenuSelected(long id)
        {
            switch (id)
            {
                case 0: //Exit
                    GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
                    GetTree().Quit();
                    break;
            }
        }

    }
    
}
