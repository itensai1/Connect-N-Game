namespace ConnectN;

public class Board
{
    private int columns, rows;
    private string[,] grid;
    private int[] colMap;
    
    public int Columns { get => columns; set => columns = value; }
    public int Rows { get => rows; set => rows = value; }
    public string[,] Grid { get => grid; set => grid = value; }
    public int[] ColMap { get => colMap; set => colMap = value; }
    


    public Board(int rows, int columns)
    {
        Columns = columns;
        Rows = rows;
        initGrid();
    }

    public void initGrid()
    {
        Grid = new string[Rows, Columns];
        for (int i = 0; i < Rows; i++)
        for (int j = 0; j < Columns; j++)
            Grid[i, j] = $"{char.ConvertFromUtf32((int)EmogiCode.empty)}";
            
        ColMap = new int[Columns];
        for (int i = 0; i < Columns; i++)
            ColMap[i] = Rows;
    }

    public void clearGrid()
    {
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Columns; j++)
                Grid[i, j] = char.ConvertFromUtf32((int)EmogiCode.empty);
        
        for (int i = 0; i < Columns; i++)
            ColMap[i] = Rows;
    }

    public bool placepiece(int col, Player player)
    {
        if (col < 0 || col >= Columns || ColMap[col] == 0)
            return false;

        int top = --ColMap[col];

        Grid[top, col] = player.Symbol;
        return true;
    }

    public bool isConnected(int col, int connectN)
    {
        int top = ColMap[col], count;
        string symbol = Grid[top, col];

        // Check horizontal
        count = 0;
        for (int i = 0; i < Columns; i++)
        {
            if (Grid[top, i] == symbol) count++;
            else count = 0;
            
            if (count == connectN) return true;
        }
        
        // Check vertical
        count = 0;
        for (int i = top; i < Rows; i++)
        {
            if (Grid[i, col] == symbol) count++;
            else count = 0;
            
            if (count == connectN) return true;
        }
        
        // Check diagonal
        count = 0;
        int r = top - Math.Min(top, col), c = col - Math.Min(top, col); // the start of the diagonal
        while (r < Rows && c < Columns)
        {
            if (Grid[r, c] == symbol) count++;
            else count = 0;
            
            if (count == connectN) return true;
            r++; c++;
        }
        
        // Check anti-diagonal
        count = 0; 
        r = top + Math.Min(Rows - top - 1, col); c = col - Math.Min(Rows - top - 1, col); // the start of the anti-diagonal
        while (r >= 0 && c < Columns)
        {
            if (Grid[r, c] == symbol) count++;
            else count = 0;
            
            if (count == connectN) return true;
            r--; c++;
        }
        
        return false;
    }



}