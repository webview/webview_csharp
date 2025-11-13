using Avalonia.Controls;
using SharpWebview;
using SharpWebview.Content;
using System;
using System.Threading;

namespace AvaloniaDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}