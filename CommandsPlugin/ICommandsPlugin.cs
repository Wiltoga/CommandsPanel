using System;
using System.Collections.Generic;
using System.Text;

namespace CommandsPlugin
{
    public abstract class ICommandsPlugin
    {
        #region Public Properties

        public abstract IEnumerable<IPluginButton> ButtonsToAdd { get; }
        public abstract string ID { get; }
        public Dictionary<string, bool> SavedBools { get; set; }
        public Dictionary<string, double> SavedDoubles { get; set; }
        public Dictionary<string, int> SavedInts { get; set; }
        public Dictionary<string, string> SavedStrings { get; set; }

        #endregion Public Properties

        #region Public Methods

        public abstract void OnLoad();

        #endregion Public Methods
    }
}