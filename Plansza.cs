using System;

class BattleshipGame
{
    static void Main(string[] args)
    {
        // Rozmiar planszy
        const int boardSize = 10;

        // Utworzenie planszy gracza i przeciwnika
        char[,] playerBoard = new char[boardSize, boardSize];
        char[,] enemyBoard = new char[boardSize, boardSize];
        char[,] enemyDisplayBoard = new char[boardSize, boardSize];

        // Inicjalizacja plansz
        InitializeBoard(playerBoard);
        InitializeBoard(enemyBoard);
        InitializeBoard(enemyDisplayBoard);

        // Rozmieszczenie statków gracza
        PlaceShips(playerBoard);

        // Rozmieszczenie statków przeciwnika (prosta losowa logika)
        PlaceShips(enemyBoard);

        // Wyświetlenie plansz
        Console.WriteLine("Twoja plansza:");
        PrintBoard(playerBoard);

        Console.WriteLine("\nPlansza przeciwnika (ukryta):");
        PrintBoard(enemyDisplayBoard);

        // Możesz tutaj dodać logikę rozgrywki
    }

    static void InitializeBoard(char[,] board)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = '~'; // Woda
            }
        }
    }

    static void PrintBoard(char[,] board)
    {
        Console.WriteLine(" 1 2 3 4 5 6 7 8 9 10");
        for (int i = 0; i < board.GetLength(0); i++)
        {
            Console.Write(i + " ");
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void PlaceShips(char[,] board)
    {
        int[] shipSizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 }; // Rozmiary statków
        Random random = new Random();

        foreach (int size in shipSizes)
        {
            bool placed = false;

            while (!placed)
            {
                int row = random.Next(0, board.GetLength(0));
                int col = random.Next(0, board.GetLength(1));
                bool horizontal = random.Next(0, 2) == 0;

                if (CanPlaceShip(board, row, col, size, horizontal))
                {
                    PlaceShip(board, row, col, size, horizontal);
                    placed = true;
                }
            }
        }
    }

    static bool CanPlaceShip(char[,] board, int row, int col, int size, bool horizontal)
    {
        if (horizontal)
        {
            if (col + size > board.GetLength(1)) return false;
            for (int i = 0; i < size; i++)
            {
                if (board[row, col + i] != '~' || !IsSurroundingAreaClear(board, row, col + i)) return false;
            }
        }
        else
        {
            if (row + size > board.GetLength(0)) return false;
            for (int i = 0; i < size; i++)
            {
                if (board[row + i, col] != '~' || !IsSurroundingAreaClear(board, row + i, col)) return false;
            }
        }
        return true;
    }

    static bool IsSurroundingAreaClear(char[,] board, int row, int col)
    {
        int[] dx = { -1, -1, -1, 0, 1, 1, 1, 0 };
        int[] dy = { -1, 0, 1, 1, 1, 0, -1, -1 };

        for (int i = 0; i < dx.Length; i++)
        {
            int newRow = row + dx[i];
            int newCol = col + dy[i];

            if (newRow >= 0 && newRow < board.GetLength(0) && newCol >= 0 && newCol < board.GetLength(1))
            {
                if (board[newRow, newCol] == 'S')
                {
                    return false;
                }
            }
        }

        return true;
    }

    static void PlaceShip(char[,] board, int row, int col, int size, bool horizontal)
    {
        if (horizontal)
        {
            for (int i = 0; i < size; i++)
            {
                board[row, col + i] = 'S'; // 'S' oznacza statek
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                board[row + i, col] = 'S';
            }
        }
    }

    static bool TakeShot(char[,] enemyBoard, char[,] displayBoard, int row, int col)
    {
        if (enemyBoard[row, col] == 'S')
        {
            displayBoard[row, col] = 'X'; // Trafiony
            enemyBoard[row, col] = 'X';
            return true;
        }
        else if (enemyBoard[row, col] == '~')
        {
            displayBoard[row, col] = '*'; // Pudło
            enemyBoard[row, col] = '*';
            return false;
        }
        return false;
    }
}
