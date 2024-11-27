using System.Globalization;

namespace ConnectN;

public class Game
{
    private Board board;
    private int connectN, roundsNumber;
    private Player player1, player2;

    public Game()
    {
        welcome();
    }
    private void welcome()
    {
        title("Welcome to Connect N Game!");
    }

    private void title(string text)
    {
        int start = (Console.WindowWidth - text.Length) / 2;
        Console.SetCursorPosition(start, Console.CursorTop);
        Console.WriteLine($"{text}\n");

    }

    private void gameOptions()
    {
        title("Game Options");

        Console.Write("How many pieces should be connected to win ? ");
        int pieces = getIntInput();
        while (pieces < 3 || pieces > Math.Max(board.Columns, board.Rows))
        {
            Console.Write($"Choose a number in range [ 3 ~ {Math.Max(board.Columns, board.Rows)} ] : ");
            pieces = getIntInput();
        }
        connectN = pieces;

        Console.Write("\nHow many rounds would you like to play ? ");
        int rounds = getIntInput();
        while (rounds < 1)
        {
            Console.Write("Enter a number greater than 0 : ");
            rounds = getIntInput();
        }

        roundsNumber = rounds;

        Console.WriteLine($"The game will continue until one of you scores {roundsNumber + 1 / 2} points or all rounds end.");

    }

    private void boardOptions()
    {
        title("Board Options");
        Console.WriteLine("Please choose the size of the board");
        Console.Write("Number of rows : ");
        int rows = getIntInput();
        while (rows < 4 || rows > 10)
        {
            Console.Write("Choose a number in range [ 4 ~ 10 ] : ");
            rows = getIntInput();
        }
        Console.Write("Number of columns : ");
        int columns = getIntInput();
        while (columns < 4 || columns > 10)
        {
            Console.Write("Choose a number in range [ 4 ~ 10 ] : ");
            columns = getIntInput();
        }
        board = new Board(rows: rows, columns: columns);

    }

    private void playerOptions()
    {
        title("Player 1");
        Console.Write("Enter your name: ");
        string name1 = Console.ReadLine() ?? "";
        while (name1.Trim().Length < 2)
        {
            Console.Write("Name must be at least 2 characters! : ");
            name1 = Console.ReadLine() ?? "";
        }
        
        Console.WriteLine("Choose a color : ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1) Red ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("2) Blue ");
        Console.ResetColor();
        Console.Write("1 or 2 ? ");
        string color1 = Console.ReadLine() ?? "";
        while (color1.Trim() != "1" && color1.Trim() != "2")
        {
            Console.Write("Invalid choice! : ");
            color1 = Console.ReadLine() ?? "";
        }
        player1 = new Player(name: name1, code: color1.Trim() == "1" ? EmogiCode.red : EmogiCode.blue);
        Console.Clear();
        
        title("Player 2");
        Console.Write("Enter your name: ");
        string name2 = Console.ReadLine() ?? "";
        while (name2.Trim().Length < 2)
        {
            Console.Write("Name must be at least 2 characters! : ");
            name2 = Console.ReadLine() ?? "";
        }
        player2 = new Player(name: name2, code: color1.Trim() == "1" ? EmogiCode.blue : EmogiCode.red);
        Console.ForegroundColor = (color1.Trim() == "1") ? ConsoleColor.Blue : ConsoleColor.Red;
        Console.WriteLine($"\nYou will be {(color1.Trim() == "1" ? "Blue" : "Red")} since {name1} chooses {(color1.Trim() == "1" ? "Red" : "Blue")} :)");
        Console.ResetColor();
        Console.WriteLine("\nPress any key to continue ...");
        Console.ReadKey();
    }

    private void printBoard()
    {
        Console.Write("\t+");
        for (int j = 0; j < board.Columns; j++)
        {
            Console.Write($"----+");
        }
        Console.WriteLine();
        for (int i = 0; i < board.Rows; i++)
        {
            Console.Write("\t| ");
            for (int j = 0; j < board.Columns; j++)
            {
                Console.Write($"{board.Grid[i, j]} | ");
            }
            Console.WriteLine();
            Console.Write("\t+");
            for (int j = 0; j < board.Columns; j++)
            {
                Console.Write($"----+");
            }
            Console.WriteLine();
        }

        Console.Write("\t| ");
        for (int j = 1; j <= board.Columns; j++)
        {
            Console.Write($"{(j < 10 ? "0" : "")}{j} | ");
        }
        Console.WriteLine();
        Console.Write("\t+");
        for (int j = 0; j < board.Columns; j++)
        {
            Console.Write($"----+");
        }
        Console.WriteLine("\n");
    }

    private bool playMove(Player player)
    {
        title($"{player1.Name} [{player1.Score}] <> [{player2.Score}] {player2.Name}");
        Console.ForegroundColor = (player.Code == EmogiCode.red) ? ConsoleColor.Red : ConsoleColor.Blue;
        Console.WriteLine($"{player.Name} turn!\n");
        Console.ResetColor();

        printBoard();
        
        Console.Write($"Choose a column to place your piece [1 ~ {board.Columns}] : ");
        int input = getIntInput();
        while (!board.placepiece(col: input - 1, player: player))
        {
            Console.Write("Invalid Column: ");
            input = getIntInput();
        }

        return board.isConnected(col: input - 1, connectN: connectN);

    }

    private void playRound()
    {
        Random randomNumber = new Random();
        Player curruntPlayer = (randomNumber.Next(1, 100) % 2 == 0) ? player1 : player2;
        bool isTie = true;

        for (int i = 0; i < board.Rows * board.Columns; i++)
        {
            bool move = playMove(curruntPlayer);
            Console.Clear();

            if (move)
            {
                curruntPlayer.addPoint();
                isTie = false;
                celebrate(curruntPlayer);
                break;
            }

            if (curruntPlayer == player1) curruntPlayer = player2;
            else curruntPlayer = player1;
        }

        if (isTie) celebrate();

    }

    private void celebrate(Player player = null)
    {
        Console.Clear();
        if (player == null) Console.WriteLine("\nIt's a tie!");
        else
        {
            Console.ForegroundColor = (player.Code == EmogiCode.red) ? ConsoleColor.Red : ConsoleColor.Blue;
            Console.WriteLine($"\n{player.Name} WINS!");
            Console.ResetColor();
        }
        Console.WriteLine("\nPress any key to continue ...");
        Console.ReadKey();
    }

    public void play()
    {
        boardOptions();
        Console.Clear();
        playerOptions();
        Console.Clear();
        gameOptions();
        Console.Clear();
        
        for (int i = 1; i <= roundsNumber; i++)
        {
            playRound();
            board.clearGrid();
            if (player1.Score > roundsNumber / 2 || player2.Score > roundsNumber / 2) break;
            Console.Clear();
        }
        if (player1.Score > player2.Score) celebrate(player: player1);
        else if (player2.Score > player1.Score) celebrate(player: player2);
        else celebrate();
    }

    private int getIntInput()
    {
        return int.TryParse(Console.ReadLine(), out int num) ? num : 0;
    }
}