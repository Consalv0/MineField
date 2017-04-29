using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MineField {
  public class Program {
    public static void Main(string[] args) {
      int[,] board = new int[20, 65];
      int mines = 300;

      placeMines(board, mines);
      print(board);
    }

    public static Random GetRandom() {
      return new Random();
    }

    public static int[] posIn2D(int num, int lines, int columns) {
      if (num >= lines * columns) {
        return null;
      }
      float line = num / columns;
      int j = (int)Math.Floor(line);
      int k = (num - columns * j);
      int[] coords = {j, k};
      return coords;
    }

    public static void placeMines(int[,] board, int mines) {
      int[] shfBoard = new int[board.GetLength(0) * board.GetLength(1)];
      for (int i = 0; i < mines; i++) {
        shfBoard[i] += 1;
      }

      Random rng = GetRandom();
      for (int i = shfBoard.GetLength(0) -1; i > 0; i--) {
        int dice = rng.Next(i);
        int num = shfBoard[dice];

        List<int> newShfBoard = shfBoard.ToList();
        newShfBoard.RemoveAt(dice);
        shfBoard = newShfBoard.ToArray();

        int[] boardCoords = posIn2D(i, board.GetLength(0), board.GetLength(1));
        board[boardCoords[0],boardCoords[1]] = num;
      }
    }

    public static void print(int[,] board) {
      Console.Write(Environment.NewLine);
      for (int i = 0; i < board.GetLength(0); i++) {
        for (int j = 0; j < board.GetLength(1); j++) {
          Console.Write(string.Format("{0} ", board[i, j]));
        }
        Console.Write(Environment.NewLine);
      }
      Console.ReadLine();
    }
  }
}
