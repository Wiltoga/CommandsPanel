using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CommandsPannel
{
    [Serializable]
    public class ActionButton
    {
        public string commands;

        [NonSerialized]
        public Image image;

        public string name;
        public byte[] source;

        [NonSerialized]
        public TextBlock text;
    }
}