using System.Windows;
using ComplexCompressor.Core;
using ComplexCompressor.ViewModel;

namespace ComplexCompressor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}