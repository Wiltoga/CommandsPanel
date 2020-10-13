using System;
using System.Collections.Generic;
using System.Text;

namespace CommandsPlugin
{
    /// <summary>
    /// Main plugin file. Loaded only when launching the Commands Pannel. Will be used to
    /// instanciate buttons on the command pannel. These buttons can then no longer be changed until
    /// the next restart.
    /// </summary>
    public abstract class ICommandsPlugin
    {
        #region Public Properties

        /// <summary>
        /// List of the buttons to add to the panel. Only the one added during the OnLoad() will
        /// take effect.
        /// </summary>
        public abstract IEnumerable<IPluginButton> ButtonsToAdd { get; }

        /// <summary>
        /// ID of the plugin. Must be unique in some way.
        /// </summary>
        public abstract string ID { get; }

        /// <summary>
        /// Map of the saved bool.
        /// </summary>
        public Dictionary<string, bool> SavedBools { get; set; }

        /// <summary>
        /// Map of the saved double.
        /// </summary>
        public Dictionary<string, double> SavedDoubles { get; set; }

        /// <summary>
        /// Map of the saved int.
        /// </summary>
        public Dictionary<string, int> SavedInts { get; set; }

        /// <summary>
        /// Map of the saved string.
        /// </summary>
        public Dictionary<string, string> SavedStrings { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Called once, and before launching the command panel. This is the only time you can
        /// create buttons.
        /// </summary>
        public abstract void OnLoad();

        #endregion Public Methods
    }
}