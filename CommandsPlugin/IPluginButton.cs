using System;
using System.Collections.Generic;
using System.Text;

namespace CommandsPlugin
{
    /// <summary>
    /// Interface used to creates custom buttons. Loaded only when launching the Commands Pannel.
    /// </summary>
    public interface IPluginButton
    {
        #region Public Properties

        /// <summary>
        /// The data of the image for the button. Can be any common image format.
        /// </summary>
        byte[] Image { get; }

        /// <summary>
        /// The name displayed on the button.
        /// </summary>
        string Name { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Function called when performing a left click on the button.
        /// </summary>
        void LeftClick();

        /// <summary>
        /// Function called when performing a right click on the button.
        /// </summary>
        void RightClick();

        #endregion Public Methods
    }
}