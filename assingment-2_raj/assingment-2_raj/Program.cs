using System;
using System.Collections.Generic;

public class Position
{

    
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position positionValue)
    {
        Name = name;
        Position = positionValue;
        GemCount = 0;
    }

    public void Move(char direction)
    {
        if (direction == 'U')
        {
            Position = new Position(Position.X, Position.Y - 1);
        }
        else if (direction == 'D')
        {
            Position = new Position(Position.X, Position.Y + 1);
        }
        else if (direction == 'L')
        {
            Position = new Position(Position.X - 1, Position.Y);
        }
        else if (direction == 'R')
        {
            Position = new Position(Position.X + 1, Position.Y);
        }
    }
}

public class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant)
    {
        Occupant = occupant;
    }
}

public class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell("-");
            }
        }

        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";

        Random randValue = new Random();
        for (int i = 0; i < 4; i++)
        {
            int x = randValue.Next(0, 6);
            int y = randValue.Next(0, 6);
            if (Grid[x, y].Occupant == "-")
            {
                Grid[x, y].Occupant = "G";
            }
            else
            {
                i--;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            int x = randValue.Next(0, 6);
            int y = randValue.Next(0, 6);
            if (Grid[x, y].Occupant == "-")
            {
                Grid[x, y].Occupant = "O";
            }
            else
            {
                i--;
            }
        }
    }

    public void Display()
    {
      
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(Player playerpos, char direction)
    {
        if (direction == 'U')
        {
            return playerpos.Position.Y > 0 && Grid[playerpos.Position.X, playerpos.Position.Y - 1].Occupant != "O";
        }
        else if (direction == 'D')
        {
            return playerpos.Position.Y < 5 && Grid[playerpos.Position.X, playerpos.Position.Y + 1].Occupant != "O";
        }
        else if (direction == 'L')
        {
            return playerpos.Position.X > 0 && Grid[playerpos.Position.X - 1, playerpos.Position.Y].Occupant != "O";
        }
        else if (direction == 'R')
        {
            return playerpos.Position.X < 5 && Grid[playerpos.Position.X + 1, playerpos.Position.Y].Occupant != "O";
        }
        else
        {
            return false;
        }
    }

    public void CollectGem(Player player)
    {
        if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
        {
            player.GemCount++;
            Grid[player.Position.X, player.Position.Y].Occupant = "-";
        }
    }
}

public class Game
{
    public Board Board { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player CurrentTurn { get; private set; }
    public int TotalTurns { get; private set; }

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }

    public void Start()
    {

        Console.Write("Game developed by Raj Gandhi\n\n");
        while (!IsGameOver())
        {
            Console.WriteLine($"Turn {TotalTurns + 1}: {CurrentTurn.Name}'s turn");
            Board.Display();

            Console.Write("Enter direction (U/D/L/R): ");
            char direction = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (Board.IsValidMove(CurrentTurn, direction))
            {
                CurrentTurn.Move(direction);
                Board.CollectGem(CurrentTurn);
                Board.Grid[Player1.Position.X, Player1.Position.Y].Occupant = "P1";
                Board.Grid[Player2.Position.X, Player2.Position.Y].Occupant = "P2";
                TotalTurns++;
                SwitchTurn();
            }
            else
            {
                Console.WriteLine("Invalid move! Try again.");
            }
        }

        AnnounceWinner();
    }

    public void SwitchTurn()
    {
        CurrentTurn = CurrentTurn == Player1 ? Player2 : Player1;
    }

    public bool IsGameOver()
    {
        return TotalTurns >= 30;
    }

    public void AnnounceWinner()
    {
        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine("Player 1 wins!");
        }
        else if (Player1.GemCount < Player2.GemCount)
        {
            Console.WriteLine("Player 2 wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}
