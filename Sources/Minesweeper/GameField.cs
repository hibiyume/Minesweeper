using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Minesweeper;

public static class GameField
{
    private static readonly Cell[,] Field = new Cell[9, 9];
    private static readonly string[] BombsCellsNames = new string[10];

    private static int BombsLeftForPlayer { get; set; } = 10; // bombs left which player changed by placing flags
    const int BombsLeftActually = 10; // bombs actually left
    static int CellsRevealed { get; set; } = 0;
    const int CellsAmount = 81; // cells that are unrevealed
    private static bool IsNotLost { get; set; } = true;

    public static Window MainWindow { get; set; }
    private static Window WinWindow = new Win();
    
    public static void InitializeGameField() // Spawning game field, placing bombs
    {
        ((TextBlock)MainWindow.FindName("tbBombs")).Text = (10).ToString();
        // Initializing game field
        for (int column = 0; column < 9; column++)
        {
            for (int row = 0; row < 9; row++)
            {
                Field[column, row] = new Cell("_" + column.ToString() + row.ToString());
            }
        }

        // Spawning bombs
        CellsRevealed = 0;
        BombsLeftForPlayer = 10;
        int bombsCount = 10;
        Random random = new Random();
        for (int i = 0; i < bombsCount; i++)
        {
            while (true) //until bomb is placed in empty cell
            {
                int column = random.Next(0, 9);
                int row = random.Next(0, 9);
                
                if (!Field[column, row].IsBomb)
                {
                    Field[column, row].IsBomb = true;
                    BombsCellsNames[i] = "_" + column + row;

                    if (column - 1 >= 0) //checking left side cells
                    {
                        Field[column - 1, row].BombsNear++;
                        if (row - 1 >= 0)
                            Field[column - 1, row - 1].BombsNear++;
                        if (row + 1 < Field.GetLength(1))
                            Field[column - 1, row + 1].BombsNear++;
                    }

                    //checking middle upper and lower cells
                    if (row - 1 >= 0)
                        Field[column, row - 1].BombsNear++;
                    if (row + 1 < Field.GetLength(1))
                        Field[column, row + 1].BombsNear++;

                    //checking right side cells
                    if (column + 1 < Field.GetLength(0))
                    {
                        Field[column + 1, row].BombsNear++;
                        if (row - 1 >= 0)
                            Field[column + 1, row - 1].BombsNear++;
                        if (row + 1 < Field.GetLength(1))
                            Field[column + 1, row + 1].BombsNear++;
                    }

                    break;
                }
            }
        }
    }

    public static void ClearFieldContent()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                IsNotLost = true;
                ((Button)MainWindow.FindName(Field[i, j].ButtonName)).Content = null;
                ((Button)MainWindow.FindName(Field[i, j].ButtonName)).IsEnabled = true;
                ((Button)MainWindow.FindName("bRestart")).Content = new Image
                {
                    Source = new BitmapImage(new Uri(
                        @"Images\Smile.png", UriKind.Relative))
                };
            }
        }
    }

    public static void RevealCell(int column, int row, Button sender)
    {
        if (IsNotLost)
        {
            sender.IsEnabled = false; //disable button
            sender.Content = null;
            Field[column, row].IsRevealed = true;
            CellsRevealed++;
            
            // Checking for lose
            if (Field[column, row].IsBomb)
            {
                // Changing restart button view
                ((Button)MainWindow.FindName("bRestart")).Content = new Image
                {
                    Source = new BitmapImage(new Uri(
                        @"Images\LooserSmile.png", UriKind.Relative))
                };

                // Revealing bombs
                foreach (var item in BombsCellsNames)
                {
                    ((Button)MainWindow.FindName(item)).Content = new Image
                    {
                        Source = new BitmapImage(new Uri(
                            @"Images\Bomb.png", UriKind.Relative))
                    };
                }

                IsNotLost = false;
                return;
            }

            // Drawing number of near bombs on the button
            if (Field[column, row].BombsNear > 0)
            {
                var textBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 32,
                    FontWeight = FontWeights.Medium,
                    Text = Field[column, row].BombsNear.ToString()
                };

                if (Field[column, row].BombsNear > 3)
                    textBlock.Foreground = Brushes.Black;
                else
                {
                    switch (Field[column, row].BombsNear)
                    {
                        case 1:
                            textBlock.Foreground = Brushes.Blue;
                            break;
                        case 2:
                            textBlock.Foreground = Brushes.Green;
                            break;
                        case 3:
                            textBlock.Foreground = Brushes.Red;
                            break;
                    }
                }

                sender.Content = textBlock;
            }
            // Revealing all near cells
            else
            {
                if (column - 1 >= 0) //checking left side cells
                {
                    if (!Field[column - 1, row].IsRevealed)
                        RevealCell(column - 1, row, (Button)MainWindow.FindName("_" + (column - 1) + row));
                    if (row - 1 >= 0 && !Field[column - 1, row - 1].IsRevealed)
                        RevealCell(column - 1, row - 1, (Button)MainWindow.FindName("_" + (column - 1) + (row - 1)));
                    if (row + 1 < Field.GetLength(1) && !Field[column - 1, row + 1].IsRevealed)
                        RevealCell(column - 1, row + 1, (Button)MainWindow.FindName("_" + (column - 1) + (row + 1)));
                }

                //checking middle upper and lower cells
                if (row - 1 >= 0 && !Field[column, row - 1].IsRevealed)
                    RevealCell(column, row - 1, (Button)MainWindow.FindName("_" + column + (row - 1)));
                if (row + 1 < Field.GetLength(1) && !Field[column, row + 1].IsRevealed)
                    RevealCell(column, row + 1, (Button)MainWindow.FindName("_" + column + (row + 1)));

                //checking right side cells
                if (column + 1 < Field.GetLength(0))
                {
                    if (!Field[column + 1, row].IsRevealed)
                        RevealCell(column + 1, row, (Button)MainWindow.FindName("_" + (column + 1) + row));
                    if (row - 1 >= 0 && !Field[column + 1, row - 1].IsRevealed)
                        RevealCell(column + 1, row - 1, (Button)MainWindow.FindName("_" + (column + 1) + (row - 1)));
                    if (row + 1 < Field.GetLength(1) && !Field[column + 1, row + 1].IsRevealed)
                        RevealCell(column + 1, row + 1, (Button)MainWindow.FindName("_" + (column + 1) + (row + 1)));
                }
            }

            // Checking for win
            if (CellsRevealed == CellsAmount - BombsLeftActually) // all non-bomb cells are revealed
            {
                // Changing restart button view
                ((Button)MainWindow.FindName("bRestart")).Content = new Image
                {
                    Source = new BitmapImage(new Uri(
                        @"Images\WinnerSmile.png", UriKind.Relative))
                };
                for (int i = 0; i < BombsLeftActually; i++)
                {
                    ((Button)MainWindow.FindName(BombsCellsNames[i])).Content = new Image
                    {
                        Source = new BitmapImage(new Uri(
                            @"Images\Bomb.jpg", UriKind.Relative))
                    };
                }
                WinWindow.Show();

                return;
            }
        }
    }

    public static void PlaceFlag(int column, int row, Button sender)
    {
        if (IsNotLost)
        {
            if (!Field[column, row].IsFlagged)
            {
                Field[column, row].IsFlagged = true;
                BombsLeftForPlayer--;

                sender.Content = new Image
                {
                    Source = new BitmapImage(new Uri(
                        @"Images\Flag.png", UriKind.Relative))
                };
            }
            else
            {
                Field[column, row].IsFlagged = false;
                BombsLeftForPlayer++;

                sender.Content = null;
            }

            if (BombsLeftForPlayer == 10 || BombsLeftForPlayer < 0)
                ((TextBlock)MainWindow.FindName("tbBombs")).Text = BombsLeftForPlayer.ToString();
            else
            {
                ((TextBlock)MainWindow.FindName("tbBombs")).Text = "0" + BombsLeftForPlayer.ToString();
            }
        }
    }
}