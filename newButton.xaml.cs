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
        public byte[] imageData;
        public bool remove;
        public ImageSource usedImage;

        public newButton()
        {
            InitializeComponent();
            imageData = null;
            usedImage = null;
            Title = "Ajouter un bouton";
            removeButton.Visibility = Visibility.Hidden;
        }

        public newButton(Button b)
        {
            InitializeComponent();
            var act = b.Tag as ActionButton;
            remove = false;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            remove = true;
            DialogResult = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Choisir le fichier à éxecuter";
            dialog.Filter = "Tous les fichiers|*.*";
            if (dialog.ShowDialog().Value)
            {
                folder.Text = System.IO.Path.GetDirectoryName(dialog.FileName);
                file.Text = System.IO.Path.GetFileName(dialog.FileName);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Choisir le fichier à éxecuter";
            dialog.Filter = "Tous les fichiers|*.*";
            if (dialog.ShowDialog().Value)
            {
                var folderName = System.IO.Path.GetDirectoryName(dialog.FileName);
                var fileName = System.IO.Path.GetFileName(dialog.FileName);
                dialog.Title = "Choisir le logiciel";
                dialog.Filter = "Executables|*.exe|Tous les fichiers|*.*";
                if (dialog.ShowDialog().Value)
                {
                    folder.Text = folderName;
                    file.Text = dialog.FileName;
                    args.Text = fileName;
                }
            }
        }

        private void iconImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp|Tous les fichiers|*.*";
            dialog.Title = "Choisir une icône";
            if (dialog.ShowDialog().Value)
            {
                var img = new BitmapImage();
                MemoryStream stream = new MemoryStream();
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
                        MemoryStream stream = new MemoryStream();
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
                        MemoryStream stream = new MemoryStream();
                        App.Client.OpenRead(file).CopyTo(stream);
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
    }
}