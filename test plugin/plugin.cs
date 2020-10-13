using CommandsPlugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace test_plugin
{
    public class plugin : ICommandsPlugin
    {
        #region Public Properties

        public byte[] Image => null;

        public string Name => "test plugin";

        #endregion Public Properties

        #region Public Methods

        public void LeftClick()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("test", "value");
            new test() { Text = Newtonsoft.Json.JsonConvert.SerializeObject(dic) }.ShowDialog();
        }

        public void RightClick()
        {
        }

        #endregion Public Methods
    }
}