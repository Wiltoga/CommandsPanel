using CommandsPlugin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CommandsPannel
{
    [Serializable]
    public class Data
    {
        #region Public Fields

        public List<ActionButton> Buttons;

        public double Left;
        public Dictionary<string, Plugin> PluginData;

        [JsonIgnore]
        public List<ICommandsPlugin> Plugins;

        public double Top;

        #endregion Public Fields

        #region Public Properties

        public static bool Portable => Directory.Exists("data");

        public static string WorkingDir => Portable ? Path.Combine(Directory.GetCurrentDirectory(), "data") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CommandsPannel");

        #endregion Public Properties

        #region Public Classes

        [Serializable]
        public class Plugin
        {
            #region Public Fields

            public Dictionary<string, bool> Bools;
            public Dictionary<string, double> Doubles;

            public Dictionary<string, int> Ints;

            public Dictionary<string, string> Strings;

            #endregion Public Fields

            #region Public Constructors

            public Plugin()
            {
                Doubles = new Dictionary<string, double>();
                Strings = new Dictionary<string, string>();
                Ints = new Dictionary<string, int>();
                Bools = new Dictionary<string, bool>();
            }

            #endregion Public Constructors
        }

        #endregion Public Classes

        //public Dictionary<string, object> toDic()
        //{
        //    var buttons = new List<Dictionary<string, object>>();
        //    var dic = new Dictionary<string, object>();
        //    dic.Add("Left", Left);
        //    dic.Add("Right", Top);
        //    dic.Add("Buttons", buttons);
        //    foreach (var item in Buttons)
        //    {
        //        var path = Guid.NewGuid().ToString();
        //        Console.WriteLine(item.imgPath);
        //        var butt = new Dictionary<string, object>();
        //        butt.Add("args", item.args);
        //        butt.Add("file", item.file);
        //        butt.Add("folder", item.folder);
        //        butt.Add("name", item.name);
        //        butt.Add("imgPath", path);
        //        buttons.Add(butt);
        //        using var stream = new FileStream("data/" + path, FileMode.Create);
        //        stream.Write(item.source);
        //        stream.Flush();
        //    }
        //    return dic;
        //}
    }
}