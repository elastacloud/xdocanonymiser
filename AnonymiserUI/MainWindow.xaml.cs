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
using System.Xml.Linq;

namespace AnonymiserUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void tvXml_Loaded(object sender, RoutedEventArgs e)
        {

            XDocument doc = XDocument.Load(@"C:\tmp\ilr\ILR-A-99999999-1415-20140601-164401-03.xml");
            tvXml.ItemsSource = doc.Nodes();
        }
    }
}
