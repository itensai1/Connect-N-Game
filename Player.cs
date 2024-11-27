namespace ConnectN;

public enum EmogiCode
{
    red = 0x1F534,
    blue = 0x1F535,
    empty = 0x26AA 
}
public class Player
{
    private string name;
    private int score;
    private EmogiCode code;
    
    public string Name { get => name; set => name = value; }
    public EmogiCode Code { get => code; set => code = value; }
    public int Score { get => score; set => score = value; }
    public string Symbol{get => char.ConvertFromUtf32((int)Code);}

    public Player(string name, EmogiCode code)
    {
        Name = name;
        Score = 0;
        Code = code;
    }

    public void addPoint()
    {
        Score++;
    }
}