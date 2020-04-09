using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommandsPannel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            /*Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "vlc";
            info.WorkingDirectory = "C:/Users/natha/Desktop";
            info.Arguments = "meow.mp3 --play-and-exit";
            p.StartInfo = info;
            p.Start();*/

            InitializeComponent();
            Window_MouseLeave(null, null);
            App.LoadData();
            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = App.Data.Top;
            Left = App.Data.Left;
            foreach (var item in App.Data.Buttons)
            {
                var current = new Button
                {
                    Width = 100,
                    Height = 100
                };
                var grid = new Grid();
                current.Tag = item;
                current.Content = grid;
                item.text = new TextBlock
                {
                    Text = item.name,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Center
                };
                grid.Children.Add(item.text);
                using var stream = new MemoryStream(item.source);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                item.image = new Image
                {
                    Source = bitmap
                };
                grid.Children.Add(item.image);
                icons.Children.Insert(icons.Children.Count - 1, current);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.SaveData();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new newButton();
            dialog.ShowDialog();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Topmost = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Topmost = false;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                for (int i = 0; i < e.Delta; i++)
                    (sender as ScrollViewer).LineLeft();
            else if (e.Delta < 0)
                for (int i = 0; i < -e.Delta; i++)
                    (sender as ScrollViewer).LineRight();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            Top -= 200;
            Height = 450;
            Width = 800;
            pannel.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Top += 200;
            Height = 250;
            Width = 20;
            pannel.Visibility = Visibility.Hidden;
        }
    }
}