using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommandsPannel
{
    [Serializable]
    public class Data
    {
        #region Public Fields

        public List<ActionButton> Buttons;
        public double Left;
        public double Top;

        #endregion Public Fields

        #region Public Properties

        public static bool Portable => Directory.Exists("data");
        public static string WorkingDir => Portable ? "data" : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CommandsPannel");

        #endregion Public Properties

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