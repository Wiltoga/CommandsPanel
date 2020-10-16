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
        #region Public Constructors

        public MainWindow()
        {
            InitializeComponent();
            App.Window = this;
            Expanded = false;
            Window_MouseLeave(null, null);
            App.LoadData();
            OpeningFlow = 0;
            windowFlow.SelectedIndex = App.Data.RightToLeft ? 1 : 0;
            windowFlow_DropDownClosed(null, null);
            if (!Data.Portable)
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
                Top = App.Data.Top;
                Left = App.Data.Left;
            }
            foreach (var plugin in App.Data.Plugins)
            {
                plugin.SavedDoubles = App.Data.PluginData[plugin.ID].Doubles;
                plugin.SavedStrings = App.Data.PluginData[plugin.ID].Strings;
                plugin.SavedInts = App.Data.PluginData[plugin.ID].Ints;
                plugin.SavedBools = App.Data.PluginData[plugin.ID].Bools;
                try
                {
                    plugin.OnLoad(System.IO.Path.Combine(Data.WorkingDir, "plugins"));
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.ToString(), e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }
                foreach (var item in plugin.ButtonsToAdd)
                {
                    var current = new Button
                    {
                        Width = 100,
                        Height = 100
                    };
                    current.MouseRightButtonDown += (sender, e) => item.RightClick();
                    current.Click += (sender, e) => item.LeftClick();
                    var grid = new Grid();
                    current.Tag = item;
                    current.Content = grid;
                    var text = new TextBlock
                    {
                        Text = item.Name,
                        TextAlignment = TextAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    Image image;
                    if (item.Image != null)
                    {
                        using var stream = new MemoryStream(item.Image);
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();
                        image = new Image
                        {
                            Source = bitmap
                        };
                    }
                    else
                        image = new Image();
                    grid.Children.Add(image);
                    grid.Children.Add(text);
                    icons.Children.Insert(icons.Children.Count - 1, current);
                }
            }
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

        #endregion Public Constructors

        #region Public Properties

        public bool Expanded { get; private set; }

        public int OpeningFlow { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void ButtonEdition(object sender, RoutedEventArgs e)
        {
            var dialog = new newButton(sender as Button);
            dialog.Show();
        }

        public void playAction(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var act = button.Tag as ActionButton;
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = act.file;
            info.WorkingDirectory = act.folder;
            info.Arguments = act.args;
            info.UseShellExecute = true;
            p.StartInfo = info;
            p.Start();
        }

        #endregion Public Methods

        #region Private Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new newButton();
            dialog.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            App.SaveData();
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

        private void Window_Closing(object sender, EventArgs e)
        {
            App.SaveData();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            Expanded = true;
            Height = 450;
            Width = 800;
            Top -= 200;
            if (OpeningFlow == 1)
                Left -= 780;
            pannel.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Expanded = false;
            Top += 200;
            Height = 250;
            Width = 20;
            if (OpeningFlow == 1)
                Left += 780;
            pannel.Visibility = Visibility.Hidden;
        }

        private void windowFlow_DropDownClosed(object sender, EventArgs e)
        {
            if (windowFlow.SelectedIndex != OpeningFlow)
                if (windowFlow.SelectedIndex == 1)
                {
                    OpeningFlow = 1;
                    leftContent.Content = movepad;
                    rightContent.Content = pannel;
                    leftColumn.Width = new GridLength(1, GridUnitType.Star);
                    rightColumn.Width = new GridLength(20);
                    blueLine.HorizontalAlignment = HorizontalAlignment.Left;
                }
                else if (windowFlow.SelectedIndex == 0)
                {
                    OpeningFlow = 0;
                    leftContent.Content = pannel;
                    rightContent.Content = movepad;
                    rightColumn.Width = new GridLength(1, GridUnitType.Star);
                    leftColumn.Width = new GridLength(20);
                    blueLine.HorizontalAlignment = HorizontalAlignment.Right;
                }
        }

        #endregion Private Methods
    }
}