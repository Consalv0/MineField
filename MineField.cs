using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MineField {
  public class Program {
    public static void Main(string[] args) {
      int[] boardDim = new int[2];
      for (int i = 0; i < boardDim.GetLength(0); i++) {
        int input;
        if (i == 0) {
          Console.Write("Field Height: ");
          input = StringtoInt(Console.ReadLine(), 10, 25);
        } else {
          Console.Write("Field Width: ");
          input = StringtoInt(Console.ReadLine(), 10, 65);
        }
        boardDim[i] = input;
      }
      Console.Write("Number of Mines: ");
      int mines = StringtoInt(Console.ReadLine(), 10,
                  (int)Math.Floor((boardDim[0] * boardDim[1]) * 0.5));
      int[,] board = new int[boardDim[0], boardDim[1]];

      placeMines(board, mines);
      print(board);
    }

    public static int StringtoInt(string value, int minValue, int maxValue) {
      int number;
      bool result = Int32.TryParse(value, out number);
      if (result && number > minValue) {
        if (number < maxValue) {
         return number;
        } else {
         return maxValue;
        }
      } else {
        Random range = GetRandom();
        return range.Next(minValue, maxValue);
      }
    }

    public static Random GetRandom() {
      return new Random();
    }

    public static int[] posIn2D(int num, int lines, int columns) {
      if (num >= lines * columns) return null;

      float line = num / columns;
      int j = (int)Math.Floor(line);
      int k = (num - columns * j);
      int[] coords = {j, k};
      return coords;
    }

    public static void placeMines(int[,] board, int mines) {
      int boardSize = board.GetLength(0) * board.GetLength(1);
      mines = mines > boardSize ? boardSize : mines;
      int[] board1D = new int[boardSize];
      for (int i = 0; i < mines; i++) {
        board1D[i] += 1;
      }

      Random range = GetRandom();
      for (int i = boardSize -1; i >= 0; i--) {
        int dice = range.Next(i +1);
        int numOfDice = board1D[dice];

        List<int> newShfBoard = board1D.ToList();
        newShfBoard.RemoveAt(dice);
        board1D = newShfBoard.ToArray();

        int[] boardPos = posIn2D(i, board.GetLength(0), board.GetLength(1));
        board[boardPos[0],boardPos[1]] = numOfDice;
      }
    }

    public static void print(int[,] board) {
      Console.Write(Environment.NewLine);
      for (int i = 0; i < board.GetLength(0); i++) {
        for (int j = 0; j < board.GetLength(1); j++) {
          if (board[i, j] == 1) {
            Console.Write("● ");
          } else {
            Console.Write("○ ");
          }
          Thread.Sleep(1);
        }
        Console.Write(Environment.NewLine);
      }
      Console.ReadLine();
    }
  }
}
