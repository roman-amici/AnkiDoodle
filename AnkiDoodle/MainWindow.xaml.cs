using System.Windows;
using AnkiDoodle.Database;

namespace AnkiDoodle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var service = new DbService();

        }
    }
}
