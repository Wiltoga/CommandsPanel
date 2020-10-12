using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CommandsPannel
{
    [Serializable]
    public class ActionButton : ISerializable
    {
        #region Public Fields

        public string args;
        public string file;
        public string folder;

        public Image image;

        public string imgPath;

        public string name;

        public byte[] source;

        public TextBlock text;

        #endregion Public Fields

        #region Public Constructors

        public ActionButton()
        {
        }

        public ActionButton(SerializationInfo info, StreamingContext context)
        {
            args = App.GetInfo<string>(info, "args");
            file = App.GetInfo<string>(info, "file");
            folder = App.GetInfo<string>(info, "folder");
            imgPath = App.GetInfo<string>(info, "imgPath");
            name = App.GetInfo<string>(info, "name");
            using var stream = new FileStream(Path.Combine(Data.WorkingDir, imgPath), FileMode.Open);
            source = new byte[stream.Length];
            if (source.Length == 0)
                source = null;
            else
                stream.Read(source, 0, source.Length);
        }

        #endregion Public Constructors

        #region Public Methods

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (imgPath == null)
                imgPath = Guid.NewGuid().ToString();
            info.AddValue("args", args);
            info.AddValue("file", file);
            info.AddValue("folder", folder);
            info.AddValue("imgPath", imgPath);
            info.AddValue("name", name);
            using var stream = new FileStream(Path.Combine(Data.WorkingDir, imgPath), FileMode.Create);
            if (source != null)
                stream.Write(source, 0, source.Length);
            stream.Flush();
        }

        #endregion Public Methods
    }
}