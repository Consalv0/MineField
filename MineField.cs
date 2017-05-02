using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MineField {
  public class Program {
    public static int heigth = Console.WindowHeight;
    public static int width = Console.WindowWidth;
    public static int halfWidth = (int)Math.Floor(width * 0.5);
    public static int halfHeigth = (int)Math.Floor(heigth * 0.99);
    public static void Main(string[] args) {
      Console.OutputEncoding = Encoding.UTF7;
      int[] boardMetaDim = new int[2];
      int input;
      for (int i = 0; i < boardMetaDim.GetLength(0); i++) {
        if (i == 0) {
          Console.Write("Field Height: ");
          input = valueRange(Console.ReadLine(), 10, halfHeigth);
        } else {
          Console.Write("Field Width: ");
          input = valueRange(Console.ReadLine(), 10, halfWidth);
        }
        boardMetaDim[i] = input;
      }
      Console.Write("Number of Mines: ");

      int mines = valueRange(Console.ReadLine(), 10,
                  (int)Math.Floor((boardMetaDim[0] * boardMetaDim[1]) * 0.3));
      int[,] boardMeta = new int[boardMetaDim[0], boardMetaDim[1]];
      string[,] board = new string[boardMetaDim[0], boardMetaDim[1]];
      int[] cursor = {(int)Math.Floor(boardMetaDim[0] * 0.5),
                      (int)Math.Floor(boardMetaDim[1] * 0.5)};

      placeMines(boardMeta, mines);
      firstPrint(boardMeta);
      keyListener(boardMeta, board, cursor, mines);
    }

    public static int valueRange(string value, int minValue, int maxValue) {
      int number;
      bool result = Int32.TryParse(value, out number);
      if (result && number >= minValue) {
        if (number <= maxValue) {
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

    public static void placeMines(int[,] boardMeta, int mines) {
      int boardMetaSize = boardMeta.GetLength(0) * boardMeta.GetLength(1);
      mines = mines > boardMetaSize ? boardMetaSize : mines;
      int[] boardMeta1D = new int[boardMetaSize];
      for (int i = 0; i < mines; i++) {
        boardMeta1D[i] += 10;
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
          if (boardMeta[i, j] == 10) {
            for (int k = i -1; k < i + 2; k++) {
            for (int l = j -1; l < j + 2; l++) {
              if (k >= 0 && k < boardMeta.GetLength(0)) {
              if (l >= 0 && l < boardMeta.GetLength(1)) {
                  if (boardMeta[k, l] != 10) boardMeta[k, l] += 1;
              }}
            }}
            Console.Write("\u2388 ");
          } else {
            Console.Write("\u25CB ");
          }
          Thread.Sleep(1);
        }
        Console.Write(Environment.NewLine);
      }
    }

    public static void printBoard(string[,] board, int[] cursor = null) {
      Console.Clear();
      for (int i = 0; i < board.GetLength(0); i++) {
        for (int j = 0; j < board.GetLength(1); j++) {
          if (cursor != null && cursor[0] == i && cursor[1] == j) {
            Console.Write("\u25FC ");
          } else {
            refreshPos(board, i, j);
          }
        }
        Console.Write(Environment.NewLine);
      }
    }

    public static void refreshPos(string[,] board, int x, int y, int[] cursor = null) {
      Console.SetCursorPosition(y * 2, x);
      if (cursor != null && cursor[0] == x && cursor[1] == y) {
        Console.Write("\u25FC ");
      } else {
        string element = board[x, y];
        if (element == "0") {
          element = " ";
        }
        if (element == "1") {
          Console.ForegroundColor = ConsoleColor.Blue;
        }
        if (element == "2") {
          Console.ForegroundColor = ConsoleColor.Magenta;
        }
        if (element == "3") {
          Console.ForegroundColor = ConsoleColor.Yellow;
        }
        if (element == "4") {
          Console.ForegroundColor = ConsoleColor.Cyan;
        }
        if (element == "5") {
          Console.ForegroundColor = ConsoleColor.DarkBlue;
        }
        if (element == "6") {
          Console.ForegroundColor = ConsoleColor.DarkMagenta;
        }
        if (element == "7") {
          Console.ForegroundColor = ConsoleColor.DarkYellow;
        }
        if (element == "8") {
          Console.ForegroundColor = ConsoleColor.DarkCyan;
        }
        if (element == "9") {
          element = "\u16B9";
          Console.ForegroundColor = ConsoleColor.Red;
        }
        if (element == "10") {
          element = "\u2388";
          Console.ForegroundColor = ConsoleColor.Red;
        }
        if (element == "11") {
          element = "\u2388";
          Console.ForegroundColor = ConsoleColor.Green;
        }
        Console.Write(element + " ");
        Console.ResetColor();
      }
    }

    public static void moveCursor(int[,] boardMeta, string[,] board, int[] cursor, int columns, int rows) {
      int boardRows = board.GetLength(0);
      int boardColumns = board.GetLength(1);
      if (cursor[0] + columns > boardRows -1 || cursor[0] + columns < 0) return;
      if (cursor[1] + rows > boardColumns -1 || cursor[1] + rows < 0) return;

      cursor[0] += columns;
      cursor[1] += rows;
      refreshPos(board, cursor[0] - columns, cursor[1] - rows);
      refreshPos(board, cursor[0], cursor[1], cursor);
    }

    public static void reveal(int[,] boardMeta, string[,] board, int[] pos) {
      if (boardMeta[pos[0], pos[1]] == 0) {
        for (int i = pos[0] -1; i < pos[0] + 2; i++) {
        for (int j = pos[1] -1; j < pos[1] + 2; j++) {
          if (i >= 0 && i < boardMeta.GetLength(0)) {
          if (j >= 0 && j < boardMeta.GetLength(1)) {
            if (boardMeta[i, j] < 10) {
              if (board[i, j] == "\u25A1") {
                board[i, j] = boardMeta[i, j] == 0 ? " " : "" + boardMeta[i, j];
                int[] newPos = {i, j};
                reveal(boardMeta, board, newPos);
              }
            refreshPos(board, i, j);
            }
          }}
        }}
      } else if (boardMeta[pos[0], pos[1]] < 9) {
        board[pos[0], pos[1]] = "" + boardMeta[pos[0], pos[1]];
      } else if (boardMeta[pos[0], pos[1]] == 10) {
        for (int i = 0; i < board.GetLength(0); i++) {
          for (int j = 0; j < board.GetLength(1); j++) {
            board[i, j] = "" + boardMeta[i, j];
          }
        }
        printBoard(board);
      }
    }

    public static void placeFlag(int[,] boardMeta, string[,] board, int[] cursor, ref int flags, int mines) {
      int concurrences = 0;
      if (board[cursor[0], cursor[1]] == "\u25A1" && flags > 0) {
        flags -= 1;
        board[cursor[0], cursor[1]] = "9";
        if (boardMeta[cursor[0], cursor[1]] == 10) {
          boardMeta[cursor[0], cursor[1]] = 11;
        }
      } else if (board[cursor[0], cursor[1]] == "9") {
        flags += 1;
        board[cursor[0], cursor[1]] = "\u25A1";
        if (boardMeta[cursor[0], cursor[1]] == 11) {
          boardMeta[cursor[0], cursor[1]] = 10;
        }
      }
      if (flags <= 0) {
        for (int i = 0; i < board.GetLength(0); i++) {
        for (int j = 0; j < board.GetLength(1); j++) {
          if (boardMeta[i, j] == 11) concurrences++;
        }}
        if (concurrences == mines) {
          for (int i = 0; i < board.GetLength(0); i++) {
          for (int j = 0; j < board.GetLength(1); j++) {
            board[i, j] = "" + boardMeta[i, j];
          }}
          printBoard(board);
        }
      }
      Console.SetCursorPosition(halfWidth -8, 0);
      Console.Write("Flags: " + flags + "   ");
      Console.SetCursorPosition(halfWidth -8, 1);
      Console.Write("Mines: " + mines);
      // Console.SetCursorPosition(100, 2);
      // Console.Write("Acerted Flags: " + concurrences);
    }

    public static void keyListener(int[,] boardMeta, string[,] board, int[] cursor, int mines) {
      Console.CursorVisible = false;
      ConsoleKeyInfo keyPress;
      for (int i = 0; i < board.GetLength(0); i++) {
        for (int j = 0; j < board.GetLength(1); j++) {
          board[i, j] = "\u25A1";
        }
      }
      printBoard(board, cursor);

      int flags = mines;
      Console.SetCursorPosition(halfWidth -8, 0);
      Console.Write("Flags: " + flags + " ");
      Console.SetCursorPosition(halfWidth -8, 1);
      Console.Write("Mines: " + mines);

      do {
        keyPress = Console.ReadKey(true);

        if(keyPress.Key == ConsoleKey.UpArrow) {
          moveCursor(boardMeta, board, cursor, -1, 0);
        }
        if(keyPress.Key == ConsoleKey.DownArrow) {
          moveCursor(boardMeta, board, cursor, 1, 0);
        }
        if(keyPress.Key == ConsoleKey.LeftArrow) {
          moveCursor(boardMeta, board, cursor, 0, -1);
        }
        if(keyPress.Key == ConsoleKey.RightArrow) {
          moveCursor(boardMeta, board, cursor, 0, 1);
        }

        if(keyPress.Key == ConsoleKey.Spacebar) {
          reveal(boardMeta, board, cursor);
        }

        if(keyPress.Key == ConsoleKey.F) {
          placeFlag(boardMeta, board, cursor, ref flags, mines);
        }
      } while (keyPress.Key != ConsoleKey.Escape);
    }
  }
}
