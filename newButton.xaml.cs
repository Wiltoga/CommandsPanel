using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Windows.Shapes;

namespace CommandsPannel
{
    /// <summary>
    /// Logique d'interaction pour newButton.xaml
    /// </summary>
    public partial class newButton : Window
    {
        #region Public Fields

        public byte[] imageData;
        public Button initialButton;
        public ImageSource usedImage;

        #endregion Public Fields

        #region Public Constructors

        public newButton()
        {
            InitializeComponent();
            initialButton = null;
            imageData = null;
            usedImage = null;
            Title = "Ajouter un bouton";
            removeButton.Visibility = Visibility.Hidden;
        }

        public newButton(Button b)
        {
            InitializeComponent();
            initialButton = b;
            var act = b.Tag as ActionButton;
            imageData = act.source;
            usedImage = act.image.Source;
            actionName.Text = act.name;
            folder.Text = act.folder;
            file.Text = act.file;
            args.Text = act.args;
            if (act.source != null)
            {
                iconImage.Source = usedImage;
                removeIcon.Visibility = Visibility.Visible;
            }
            Title = "Modifier un bouton";
        }

        #endregion Public Constructors

        #region Private Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Window.Dispatcher.Invoke(() =>
            {
                if (initialButton != null)
                {
                    var act = initialButton.Tag as ActionButton;
                    if (imageData != null)
                    {
                        act.source = imageData;
                        act.image.Source = usedImage;
                    }
                    else
                    {
                        act.source = null;
                        act.image.Source = null;
                    }
                    act.name = actionName.Text;
                    act.folder = folder.Text;
                    act.file = file.Text;
                    act.args = args.Text;

                    act.text.Text = act.name;
                }
                else
                {
                    var act = new ActionButton
                    {
                        folder = folder.Text,
                        file = file.Text,
                        args = args.Text,
                        name = actionName.Text,
                        source = imageData
                    };
                    App.Data.Buttons.Add(act);
                    var current = new Button
                    {
                        Width = 100,
                        Height = 100
                    };
                    current.MouseRightButtonDown += App.Window.ButtonEdition;
                    current.Click += App.Window.playAction;
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
                        act.image = new System.Windows.Controls.Image
                        {
                            Source = bitmap
                        };
                    }
                    else
                        act.image = new System.Windows.Controls.Image();
                    grid.Children.Add(act.image);
                    grid.Children.Add(act.text);
                    App.Window.icons.Children.Insert(App.Window.icons.Children.Count - 1, current);
                }
            });
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App.Window.Dispatcher.Invoke(() =>
            {
                var act = initialButton.Tag as ActionButton;
                App.Data.Buttons.Remove(act);
                App.Window.icons.Children.Remove(initialButton);
            });
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Choisir le fichier à éxecuter";
            dialog.Filter = "Tous les fichiers|*.*";
            if (dialog.ShowDialog().Value)
            {
                var directory = System.IO.Path.GetDirectoryName(dialog.FileName);
                if (Data.Portable && System.IO.Path.GetPathRoot(Directory.GetCurrentDirectory()) == System.IO.Path.GetPathRoot(dialog.FileName))
                {
                    var l = System.IO.Path.GetPathRoot(dialog.FileName).Length;
                    directory = '\\' + directory.Substring(l);
                }
                folder.Text = directory;
                file.Text = System.IO.Path.GetFileName(dialog.FileName);
                args.Text = "";
                if (actionName.Text == "")
                    actionName.Text = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
                if (usedImage == null && System.IO.Path.GetExtension(dialog.FileName) == ".exe")
                {
                    var img = new BitmapImage();
                    using var stream = new MemoryStream();
                    System.Drawing.Icon.ExtractAssociatedIcon(dialog.FileName).ToBitmap().Save(stream, ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    img.BeginInit();
                    img.StreamSource = stream;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    img.Freeze();
                    iconImage.Source = img;
                    imageData = stream.GetBuffer();
                    usedImage = iconImage.Source;
                    removeIcon.Visibility = Visibility.Visible;
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Choisir le fichier à ouvrir";
            dialog.Filter = "Tous les fichiers|*.*";
            if (dialog.ShowDialog().Value)
            {
                var folderName = System.IO.Path.GetDirectoryName(dialog.FileName);
                var fileName = System.IO.Path.GetFileName(dialog.FileName);
                dialog.Title = "Choisir le logiciel";
                dialog.Filter = "Executables|*.exe|Tous les fichiers|*.*";
                if (dialog.ShowDialog().Value)
                {
                    string exe = dialog.FileName;
                    if (Data.Portable && System.IO.Path.GetPathRoot(Directory.GetCurrentDirectory()) == System.IO.Path.GetPathRoot(dialog.FileName))
                    {
                        var l = System.IO.Path.GetPathRoot(folderName).Length;
                        folderName = '\\' + folderName.Substring(l);
                        l = System.IO.Path.GetPathRoot(exe).Length;
                        exe = '\\' + exe.Substring(l);
                    }
                    folder.Text = folderName;
                    file.Text = exe;
                    args.Text = fileName;
                    if (actionName.Text == "")
                        actionName.Text = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    if (usedImage == null && System.IO.Path.GetExtension(dialog.FileName) == ".exe")
                    {
                        var img = new BitmapImage();
                        using var stream = new MemoryStream();
                        System.Drawing.Icon.ExtractAssociatedIcon(dialog.FileName).ToBitmap().Save(stream, ImageFormat.Png);
                        stream.Seek(0, SeekOrigin.Begin);
                        img.BeginInit();
                        img.StreamSource = stream;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();
                        img.Freeze();
                        iconImage.Source = img;
                        imageData = stream.GetBuffer();
                        usedImage = iconImage.Source;
                        removeIcon.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void iconImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp|Tous les fichiers|*.*";
            dialog.Title = "Choisir une icône";
            if (dialog.ShowDialog().Value)
            {
                var img = new BitmapImage();
                using MemoryStream stream = new MemoryStream();
                App.Client.OpenRead(dialog.FileName).CopyTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                img.BeginInit();
                img.StreamSource = stream;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                img.Freeze();
                iconImage.Source = img;
                imageData = stream.GetBuffer();
                usedImage = iconImage.Source;
                removeIcon.Visibility = Visibility.Visible;
            }
        }

        private void Image_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
                e.Effects = DragDropEffects.Link;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link;
            e.Handled = true;
        }

        private void Image_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
                e.Effects = DragDropEffects.Link;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link;
            e.Handled = true;
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                Task.Run(() =>
                {
                    try
                    {
                        var uri = (string)e.Data.GetData(DataFormats.StringFormat);
                        var img = new BitmapImage();
                        using MemoryStream stream = new MemoryStream();
                        App.Client.OpenRead(uri).CopyTo(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        img.BeginInit();
                        img.StreamSource = stream;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();
                        img.Freeze();
                        Dispatcher.Invoke(() =>
                        {
                            iconImage.Source = img;
                            imageData = stream.GetBuffer();
                            usedImage = iconImage.Source;
                            removeIcon.Visibility = Visibility.Visible;
                        });
                    }
                    catch (Exception) { }
                });
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Task.Run(() =>
                {
                    try
                    {
                        var file = ((string[])e.Data.GetData(DataFormats.FileDrop)).First();
                        var img = new BitmapImage();
                        using MemoryStream stream = new MemoryStream();
                        using var fileStream = App.Client.OpenRead(file);
                        fileStream.CopyTo(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        img.BeginInit();
                        img.StreamSource = stream;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();
                        img.Freeze();
                        Dispatcher.Invoke(() =>
                        {
                            iconImage.Source = img;
                            imageData = stream.GetBuffer();
                            usedImage = iconImage.Source;
                            removeIcon.Visibility = Visibility.Visible;
                        });
                    }
                    catch (Exception) { }
                });
            }
        }

        private void removeIcon_Click(object sender, RoutedEventArgs e)
        {
            removeIcon.Visibility = Visibility.Hidden;
            imageData = null;
            iconImage.Source = App.Blank;
            usedImage = null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                DialogResult = false;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
            {
                if (Clipboard.ContainsImage())
                {
                    var img = Clipboard.GetImage();
                    using var stream = new MemoryStream();
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(img));
                    encoder.Save(stream);
                    iconImage.Source = img;
                    imageData = stream.GetBuffer();
                    usedImage = iconImage.Source;
                    removeIcon.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion Private Methods
    }
}