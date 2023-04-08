using System.Windows;
using System.Windows.Input;

namespace Minesweeper;

public partial class Win : Window
{
    public Win()
    {
        InitializeComponent();
    }

    private void OnRestartButtonClicked(object sender, MouseButtonEventArgs e)
    {
        GameField.InitializeGameField();
        GameField.ClearFieldContent();
        Visibility = Visibility.Hidden;
    }
}