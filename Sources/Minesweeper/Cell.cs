using System.Windows.Controls;

namespace Minesweeper;

public class Cell
{
    public string ButtonName { get; set; }
    public int BombsNear { get; set; }

    public bool IsBomb { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    
    public Cell(string buttonName)
    {
        ButtonName = buttonName;
        BombsNear = 0;
        IsBomb = false;
        IsRevealed = false;
        IsFlagged = false;
    }
}
