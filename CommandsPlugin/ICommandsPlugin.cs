using System;
using System.Collections.Generic;
using System.Text;

namespace CommandsPlugin
{
    public interface ICommandsPlugin
    {
        #region Public Properties

        byte[] Image { get; }
        string Name { get; }

        #endregion Public Properties

        #region Public Methods

        void LeftClick();

        void RightClick();

        #endregion Public Methods
    }
}