using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Net;

namespace CommandsPannel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ImageSource Blank = ConvertImage(CommandsPannel.Properties.Resources.blank);
        public static WebClient Client = new WebClient();
        public static MainWindow Window;
        public static Data Data { get; set; }

        public static ImageSource ConvertImage(Bitmap bitmap)
        {
            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = stream;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            img.Freeze();
            return img;
        }

        public static void LoadData()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CommandsPannel");
            var path = Path.Combine(dir, "buttons.cpser");
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                Data = (Data)formatter.Deserialize(stream);
            }
            else
            {
                Data = new Data { Buttons = new List<ActionButton>(), Left = Current.MainWindow.Left, Top = Current.MainWindow.Top };
                Data.Buttons.Add(new ActionButton
                {
                    file = "steam://open/main",
                    name = "Steam"
                });
                Data.Buttons.Add(new ActionButton
                {
                    file = "https://google.com",
                    name = "Google"
                });
                Data.Buttons.Add(new ActionButton
                {
                    file = "Explorer",
                    name = "explorer"
                });
                Directory.CreateDirectory(dir);
                var formatter = new BinaryFormatter();
                using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, Data);
            }
        }

        public static void SaveData()
        {
            Data.Top = Current.MainWindow.Top + 200;
            Data.Left = Current.MainWindow.Left;
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CommandsPannel");
            var path = Path.Combine(dir, "buttons.cpser");
            if (!File.Exists(path))
                Directory.CreateDirectory(dir);
            var formatter = new BinaryFormatter();
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, Data);
        }
    }
}