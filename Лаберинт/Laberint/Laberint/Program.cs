using System;
using System.Linq;

class Program
{
    const int Width = 20;
    const int Height = 10;

    static char[,] maze = new char[Height * 2 + 1, Width * 2 + 1];
    static int playerX = 1, playerY = 1;

    static void Main()
    {
        GenerateMaze();
        PlayGame();
    }

    static void GenerateMaze()
    {
        var rand = new Random();

        // Заполняем лабиринт стенами
        for (int y = 0; y < maze.GetLength(0); y++)
            for (int x = 0; x < maze.GetLength(1); x++)
                maze[y, x] = (y % 2 == 0 || x % 2 == 0) ? '│' : '─';

        // Начинаем алгоритм с центрального узла
        CarvePath(rand, 1, 1);

        // Добавляем вход и выход
        maze[1, 0] = ' ';
        maze[maze.GetLength(0) - 2, maze.GetLength(1) - 1] = ' ';
    }

    static void CarvePath(Random rand, int x, int y)
    {
        int[] directions = { 0, 1, 2, 3 };
        directions = directions.OrderBy(d => rand.Next()).ToArray(); // Случайный порядок

        foreach (var direction in directions)
        {
            int dx = 0, dy = 0;

            switch (direction)
            {
                case 0: dx = 1; break; // Вправо
                case 1: dx = -1; break; // Влево
                case 2: dy = 1; break; // Вниз
                case 3: dy = -1; break; // Вверх
            }

            int newX = x + dx * 2, newY = y + dy * 2;

            if (newX > 0 && newX < Width * 2 && newY > 0 && newY < Height * 2 && maze[newY, newX] == '─')
            {
                maze[y + dy, x + dx] = ' '; // Убираем стену
                maze[newY, newX] = ' '; // Убираем узел
                CarvePath(rand, newX, newY);
            }
        }
    }

    static void PlayGame()
    {
        while (true)
        {
            Console.Clear();
            DisplayMaze();

            if (playerX == maze.GetLength(1) - 1 && playerY == maze.GetLength(0) - 2)
            {
                Console.WriteLine("Вы нашли выход! Поздравляем!");
                break;
            }

            Console.WriteLine("Используйте WASD для перемещения. Выйти из игры - Q.");
            var key = Console.ReadKey(true).Key;

            int newX = playerX, newY = playerY;

            switch (key)
            {
                case ConsoleKey.W: newY--; break;
                case ConsoleKey.S: newY++; break;
                case ConsoleKey.A: newX--; break;
                case ConsoleKey.D: newX++; break;
                case ConsoleKey.Q: return;
            }

            if (newX >= 0 && newX < maze.GetLength(1) && newY >= 0 && newY < maze.GetLength(0) && maze[newY, newX] == ' ')
            {
                playerX = newX;
                playerY = newY;
            }
        }
    }

    static void DisplayMaze()
    {
        // Верхняя рамка
        Console.WriteLine('┌' + new string('─', maze.GetLength(1)) + '┐');

        for (int y = 0; y < maze.GetLength(0); y++)
        {
            Console.Write("│"); // Левая рамка
            for (int x = 0; x < maze.GetLength(1); x++)
            {
                if (x == playerX && y == playerY)
                    Console.Write('O'); // Кружок
                else
                    Console.Write(maze[y, x]);
            }
            Console.WriteLine("│"); // Правая рамка
        }

        // Нижняя рамка
        Console.WriteLine('└' + new string('─', maze.GetLength(1)) + '┘');
    }
}
