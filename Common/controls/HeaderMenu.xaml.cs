﻿using System;
using System.Collections.Generic;
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

namespace Common.Controls
{
    /// <summary>
    /// Interaction logic for HeaderMenu.xaml
    /// </summary>
    public partial class HeaderMenu : UserControl
    {
        public HeaderMenu()
        {
            InitializeComponent();
        }

        //private void ExitToMainMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        //    mainWindow.NavigateToGallery();
        //}

        public void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Window perentWindow = Window.GetWindow(this);
            if (perentWindow != null)
            {
                perentWindow.Close();
            }
        }
    }
}
