using CommandsPlugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPlugin
{
    public class ButtonPlug : IPluginButton
    {
        #region Public Fields

        public string display;

        #endregion Public Fields

        #region Public Properties

        public byte[] Image => null;
        public string Name => "Wpf plugin";

        #endregion Public Properties

        #region Public Methods

        public void LeftClick()
        {
            var dialog = new Window1();
            dialog.textblock.Text = display;
            dialog.Show();
        }

        public void RightClick()
        {
        }

        #endregion Public Methods
    }

    public class Plugin : ICommandsPlugin
    {
        #region Private Fields

        private List<ButtonPlug> buttons;

        #endregion Private Fields

        #region Public Properties

        public override IEnumerable<IPluginButton> ButtonsToAdd => buttons;

        public override string ID => "WpfPlugin";

        #endregion Public Properties

        #region Public Methods

        public override void OnLoad()
        {
            if (!SavedStrings.ContainsKey("display"))
                SavedStrings.Add("display", "test");
            buttons = new List<ButtonPlug>();
            buttons.Add(new ButtonPlug() { display = SavedStrings["display"] });
        }

        #endregion Public Methods
    }
}