using System;
using System.Collections.Generic;

public class BattleshipBot
{
    private static Random random = new Random();
    private const int BoardSize = 10;
    private char[,] board;
    private HashSet<(int, int)> hitCells; 
    private HashSet<(int, int)> attackHistory;

    public BattleshipBot()
    {
        board = new char[BoardSize, BoardSize];
        hitCells = new HashSet<(int, int)>();
        attackHistory = new HashSet<(int, int)>();
        InitializeBoard();
    }


    private void InitializeBoard()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                board[i, j] = '~';
            }
        }
    }

  
    public (int, int) MakeRandomShot()
    {
        int x, y;
        do
        {
            x = random.Next(0, BoardSize);
            y = random.Next(0, BoardSize);
        } while (attackHistory.Contains((x, y))); 

        attackHistory.Add((x, y));
        return (x, y);
    }

    
    public (int, int) MakeShot()
    {
        if (hitCells.Count == 0)
        {
            
            return MakeRandomShot();
        }
        else
        {
            var lastHit = new List<(int, int)>(hitCells);
            var lastHitCell = lastHit[lastHit.Count - 1];
            var adjacentCells = GetAdjacentCells(lastHitCell);

            foreach (var cell in adjacentCells)
            {
                if (!attackHistory.Contains(cell) && IsValidCell(cell))
                {
                    attackHistory.Add(cell);
                    return cell;
                }
            }

            // Jeśli nie udało się znaleźć nieatakowanego sąsiedniego pola, strzelamy losowo
            return MakeRandomShot();
        }
    }

    // Sprawdzanie sąsiednich komórek
    private List<(int, int)> GetAdjacentCells((int, int) cell)
    {
        List<(int, int)> adjacentCells = new List<(int, int)>
        {
            (cell.Item1 - 1, cell.Item2), // górna komórka
            (cell.Item1 + 1, cell.Item2), // dolna komórka
            (cell.Item1, cell.Item2 - 1), // lewa komórka
            (cell.Item1, cell.Item2 + 1), // prawa komórka
        };

        // Filtrowanie komórek, które wychodzą poza planszę
        adjacentCells.RemoveAll(c => !IsValidCell(c));
        return adjacentCells;
    }

    // Sprawdzanie, czy komórka jest w obrębie planszy
    private bool IsValidCell((int, int) cell)
    {
        return cell.Item1 >= 0 && cell.Item1 < BoardSize && cell.Item2 >= 0 && cell.Item2 < BoardSize;
    }

    // Funkcja do oznaczenia trafienia
    public void MarkHit(int x, int y)
    {
        hitCells.Add((x, y));
    }

    // Funkcja do sprawdzenia, czy trafiliśmy statek
    public bool IsHit(int x, int y)
    {
        // Przyjmujemy, że statek jest oznaczony przez 'X'
        return board[x, y] == 'X';
    }

    // Funkcja do ustawienia statków na planszy (symulacja)
    public bool PlaceShip(int x, int y, int length, bool horizontal)
    {
        if (horizontal)
        {
            if (y + length > BoardSize) return false; // Statek wychodzi poza planszę

            for (int i = 0; i < length; i++)
            {
                if (board[x, y + i] == 'X') return false; // Sprawdzamy, czy nie ma już statku
            }

            for (int i = 0; i < length; i++)
            {
                board[x, y + i] = 'X'; // Umieszczamy statek
            }
        }
        else
        {
            if (x + length > BoardSize) return false; // Statek wychodzi poza planszę

            for (int i = 0; i < length; i++)
            {
                if (board[x + i, y] == 'X') return false; // Sprawdzamy, czy nie ma już statku
            }

            for (int i = 0; i < length; i++)
            {
                board[x + i, y] = 'X'; // Umieszczamy statek
            }
        }

        return true;
    }

    // Funkcja do wyświetlania planszy
    public void DisplayBoard(bool showShips = false)
    {
        Console.Clear();
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                if (!showShips && board[i, j] == 'X') // Jeśli nie pokazujemy statków, zamień je na 'O'
                    Console.Write("O ");
                else
                    Console.Write($"{board[i, j]} ");
            }
            Console.WriteLine();
        }
    }

    public static void Main()
    {
        BattleshipBot bot = new BattleshipBot();

        // Rozmieszczamy statki na planszy
        bot.PlaceShip(5, 5, 3, true); // Statek o długości 3 poziomo
        bot.PlaceShip(2, 3, 2, false); // Statek o długości 2 pionowo
        bot.PlaceShip(7, 1, 4, true); // Statek o długości 4 poziomo

        // Wyświetlenie planszy
        Console.WriteLine("Plansza z rozmieszczonymi statkami:");
        bot.DisplayBoard(showShips: true); // Pokażemy statki na planszy (tylko dla debugowania)

        // Symulacja gry
        for (int i = 0; i < 30; i++) // Symulujemy 30 tur strzałów
        {
            var shot = bot.MakeShot();
            Console.WriteLine($"Bot strzelił na pozycję: ({shot.Item1}, {shot.Item2})");

            if (bot.IsHit(shot.Item1, shot.Item2))
            {
                Console.WriteLine("Trafienie!");
                bot.MarkHit(shot.Item1, shot.Item2);
            }
            else
            {
                Console.WriteLine("Chybienie!");
            }

            // Wyświetlenie planszy po każdym strzale
            bot.DisplayBoard();
            System.Threading.Thread.Sleep(500); // Opóźnienie na wyświetlanie kolejnych tur
        }
    }
}

