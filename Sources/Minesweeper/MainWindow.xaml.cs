using System;
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

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GameField.MainWindow = this;
            GameField.InitializeGameField();
        }

        private void RestartGame(object sender, RoutedEventArgs e)
        {
            GameField.InitializeGameField();
            GameField.ClearFieldContent();
        }

        private void OnCellClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            
            int column = int.Parse(button.Name.Substring(1, 1));
            int row = int.Parse(button.Name.Substring(2));
            GameField.RevealCell(column, row, button);
        }

        private void OnFlagPlaced(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            
            int column = int.Parse(button.Name.Substring(1, 1));
            int row = int.Parse(button.Name.Substring(2));
            GameField.PlaceFlag(column, row, button);
        }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}