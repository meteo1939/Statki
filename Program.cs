static void EnemyTurn(char[,] playerBoard)
{
    Random random = new Random();
    int row, col;
    bool validShot = false;

    while (!validShot)
    {
        row = random.Next(0, playerBoard.GetLength(0));
        col = random.Next(0, playerBoard.GetLength(1));

        if (playerBoard[row, col] == 'S')
        {
            playerBoard[row, col] = 'X';
            Console.WriteLine($"Przeciwnik trafił w twoje pole: {(char)('A' + row)}{col + 1}");
            validShot = true;
        }
        else if (playerBoard[row, col] == '~')
        {
            playerBoard[row, col] = '*';
            Console.WriteLine($"Przeciwnik spudłował: {(char)('A' + row)}{col + 1}");
            validShot = true;
        }
    }
}