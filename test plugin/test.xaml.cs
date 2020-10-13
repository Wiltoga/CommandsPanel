using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace test_plugin
{
    /// <summary>
    /// Logique d'interaction pour test.xaml
    /// </summary>
    public partial class test : Window
    {
        #region Public Constructors

        public test()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Text { get => text.Text; set => text.Text = value; }

        #endregion Public Properties
    }
}