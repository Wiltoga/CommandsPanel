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
                current.MouseRightButtonDown += ButtonEdition;
                current.Click += playAction;
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
                if (item.source != null)
                {
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
                }
                else
                    item.image = new Image();
                grid.Children.Add(item.image);
                grid.Children.Add(item.text);
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
            if (dialog.ShowDialog().Value)
            {
                var act = new ActionButton
                {
                    folder = dialog.folder.Text,
                    file = dialog.file.Text,
                    args = dialog.args.Text,
                    name = dialog.actionName.Text,
                    source = dialog.imageData
                };
                App.Data.Buttons.Add(act);
                var current = new Button
                {
                    Width = 100,
                    Height = 100
                };
                current.MouseRightButtonDown += ButtonEdition;
                current.Click += playAction;
                var grid = new Grid();
                current.Tag = act;
                current.Content = grid;
                act.text = new TextBlock
                {
                    Text = act.name,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Center
                };
                if (act.source != null)
                {
                    using var stream = new MemoryStream(act.source);
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    act.image = new Image
                    {
                        Source = bitmap
                    };
                }
                else
                    act.image = new Image();
                grid.Children.Add(act.image);
                grid.Children.Add(act.text);
                icons.Children.Insert(icons.Children.Count - 1, current);
            }
        }

        private void ButtonEdition(object sender, RoutedEventArgs e)
        {
            var dialog = new newButton(sender as Button);
            if (dialog.ShowDialog().Value)
            {
                var act = ((Button)sender).Tag as ActionButton;
                if (dialog.remove)
                {
                    App.Data.Buttons.Remove(act);
                    icons.Children.Remove(sender as Button);
                }
                else
                {
                    if (dialog.imageData != null)
                    {
                        act.source = dialog.imageData;
                        act.image.Source = dialog.usedImage;
                    }
                    else
                    {
                        act.source = null;
                        act.image.Source = null;
                    }
                    act.name = dialog.actionName.Text;
                    act.folder = dialog.folder.Text;
                    act.file = dialog.file.Text;
                    act.args = dialog.args.Text;

                    act.text.Text = act.name;
                }
            }
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

        private void playAction(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var act = button.Tag as ActionButton;
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = act.file;
            info.WorkingDirectory = act.folder;
            info.Arguments = act.args;
            p.StartInfo = info;
            p.Start();
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