using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MineField {
  public class Program {
    public static void Main(string[] args) {
      int[] boardMetaDim = new int[2];

      for (int i = 0; i < boardMetaDim.GetLength(0); i++) {
        int input;
        if (i == 0) {
          Console.Write("Field Height: ");
          input = valueRange(Console.ReadLine(), 10, 29);
        } else {
          Console.Write("Field Width: ");
          input = valueRange(Console.ReadLine(), 10, 65);
        }
        boardMetaDim[i] = input;
      }
      Console.Write("Number of Mines: ");

      int mines = valueRange(Console.ReadLine(), 10,
                  (int)Math.Floor((boardMetaDim[0] * boardMetaDim[1]) * 0.5));
      int[,] boardMeta = new int[boardMetaDim[0], boardMetaDim[1]];
      string[,] board = new string[boardMetaDim[0], boardMetaDim[1]];
      int[] cursor = {0, 0};

      placeMines(boardMeta, mines);
      firstPrint(boardMeta);
      keyListener(board, cursor);
    }

    public static int valueRange(string value, int minValue, int maxValue) {
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

    public static void placeMines(int[,] boardMeta, int mines) {
      int boardMetaSize = boardMeta.GetLength(0) * boardMeta.GetLength(1);
      mines = mines > boardMetaSize ? boardMetaSize : mines;
      int[] boardMeta1D = new int[boardMetaSize];
      for (int i = 0; i < mines; i++) {
        boardMeta1D[i] += 1;
      }

      Random range = GetRandom();
      for (int i = boardMetaSize -1; i >= 0; i--) {
        int dice = range.Next(i +1);
        int numOfDice = boardMeta1D[dice];

        List<int> newShfBoard = boardMeta1D.ToList();
        newShfBoard.RemoveAt(dice);
        boardMeta1D = newShfBoard.ToArray();

        int[] boardMetaPos = posIn2D(i, boardMeta.GetLength(0), boardMeta.GetLength(1));
        boardMeta[boardMetaPos[0],boardMetaPos[1]] = numOfDice;
      }
    }

    public static void firstPrint(int[,] boardMeta) {
      Console.Clear();
      for (int i = 0; i < boardMeta.GetLength(0); i++) {
        for (int j = 0; j < boardMeta.GetLength(1); j++) {
          if (boardMeta[i, j] == 1) {
            Console.Write("⎈ ");
          } else {
            Console.Write("○ ");
          }
          Thread.Sleep(1);
        }
        Console.Write(Environment.NewLine);
      }
      Console.Clear();
      for (int i = 0; i < boardMeta.GetLength(0); i++) {
        for (int j = 0; j < boardMeta.GetLength(1); j++) {
            Console.Write("◼ ");
        }
        Console.Write(Environment.NewLine);
      }
      Thread.Sleep(200);
    }

    public static void printBoard(string[,] board) {
      Console.Clear();
      for (int i = 0; i < board.GetLength(0); i++) {
        for (int j = 0; j < board.GetLength(1); j++) {
          if (board[i, j] == "◼") {
            Console.Write("◼ ");
          } else {
            Console.Write("□ ");
          }
        }
        Console.Write(Environment.NewLine);
      }
    }

    public static void moveCursor(string[,] board, int[] cursor, int column, int row) {
      int boardRows = board.GetLength(0);
      int boardColumns = board.GetLength(1);
      if (cursor[0] + row > boardRows && cursor[0] + row <= 0) return;
      if (cursor[1] + column > boardColumns && cursor[1] + column <= 0) return;

      board[cursor[0], cursor[1]] = "";
      Console.SetCursorPosition(cursor[1] * 2, cursor[0]);
      Console.Write("□ ");
      Console.SetCursorPosition((cursor[1] + row)* 2, cursor[0] + column);
      Console.Write("◼ ");

      board[cursor[0] + column, cursor[1] + row] = "◼";
      findPosOf(board, "◼", cursor);
    }

    public static int[] findPosOf(string[,] board, string element, int[] cursor = null) {
      for (int x = 0; x < board.GetLength(0); ++x) {
        for (int y = 0; y < board.GetLength(1); ++y) {
          if (board[x, y] == element) {
            if (cursor != null) {
              cursor[0] = x;
              cursor[1] = y;
            } else {
              int[] pos = {x, y};
              return pos;
            }
          }
        }
      }
      int[] r = {0, 0};
      return r;
    }

    public static void keyListener(string[,] board, int[] cursor) {
      ConsoleKeyInfo keyPress;
      board[(int)Math.Floor(board.GetLength(0) * 0.5),
            (int)Math.Floor(board.GetLength(1) * 0.5)] = "◼";
      findPosOf(board, "◼", cursor);
      printBoard(board);

      do {
        keyPress = Console.ReadKey();

        if(keyPress.Key == ConsoleKey.UpArrow) {
          moveCursor(board, cursor, -1, 0);
        }
        if(keyPress.Key == ConsoleKey.DownArrow) {
          moveCursor(board, cursor, 1, 0);
        }
        if(keyPress.Key == ConsoleKey.LeftArrow) {
          moveCursor(board, cursor, 0, -1);
        }
        if(keyPress.Key == ConsoleKey.RightArrow) {
          moveCursor(board, cursor, 0, 1);
        }
      } while (keyPress.Key != ConsoleKey.Escape);
    }
  }
}
