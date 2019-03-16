using System;

public static class MainClass {
    public static void Main(string[] args) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("---------- Regular Binary Math ----------\n");
        Console.ResetColor();

        Binary a = new Binary("1101");
        Binary b = new Binary("1010");
        Console.WriteLine("{0,4} + {1,4} = {2,8}", a, b, a + b);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("{0,4} + {1,4} = {2,8}\n", a.BaseChange(), b.BaseChange(), (a + b).BaseChange());
        Console.ResetColor();
        Console.WriteLine("{0,4} - {1,4} = {2,8}", a, b, a - b);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("{0,4} - {1,4} = {2,8}\n", a.BaseChange(), b.BaseChange(), (a - b).BaseChange());
        Console.ResetColor();
        Console.WriteLine("{0,4} * {1,4} = {2,8}", a, b, a * b);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("{0,4} * {1,4} = {2,8}\n", a.BaseChange(), b.BaseChange(), (a * b).BaseChange());
        Console.ResetColor();
        Console.WriteLine("{0,4} / {1,4} = {2,8}", a, b, a / b);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("{0,4} / {1,4} = {2,8}\n", a.BaseChange(), b.BaseChange(), (a / b).BaseChange());
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n---------- MiniFloat Math ---------------\n");

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("-------- Without Rounding --------");
        Console.ResetColor();
        MiniFloat x = new MiniFloat('1', "0111", "101");
        MiniFloat y = new MiniFloat('0', "1001", "010");
        Console.WriteLine("{0} + {1} = {2}", x, y, x + y);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("{0,10} + {1,10} = {2,10}\n", x.BaseChange(), y.BaseChange(), (x + y).BaseChange());
        Console.ResetColor();

        x = new MiniFloat('0', "1010", "010");
        y = new MiniFloat('1', "1001", "110");
        Console.WriteLine("{0,10} - {1,10} = {2,10}", x, y, x - y);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("{0,10} - {1,10} = {2,10}\n", x.BaseChange(), y.BaseChange(), (x - y).BaseChange());

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n-------- With Rounding -----------");
        Console.ResetColor();
        x = new MiniFloat('1', "0111", "101");
        y = new MiniFloat('0', "1001", "010");
        MiniFloat r = x + y;
        r.MinimizeMantissa();
        Console.WriteLine("{0} + {1} = {2}", x, y, r);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("{0,10} + {1,10} = {2,10}\n", x.BaseChange(), y.BaseChange(), r.BaseChange());
        Console.ResetColor();

        x = new MiniFloat('0', "1010", "010");
        y = new MiniFloat('1', "1001", "110");
        r = x - y;
        r.MinimizeMantissa();
        Console.WriteLine("{0,10} - {1,10} = {2,10}", x, y, r);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("{0,10} - {1,10} = {2,10}\n", x.BaseChange(), y.BaseChange(), r.BaseChange());
        Console.ResetColor();

        Console.ReadLine();
    }

    public static int ValueOf(char c) => Convert.ToInt32(char.GetNumericValue(c));
    public static char Character(int i) => i.ToString()[0];

    public static string ReplaceAt(string str, int index, string repText) {
        if (index + repText.Length < str.Length) {
            return string.Format("{0}{1}{2}",
                str.Substring(0, index), repText, str.Substring(index + repText.Length));
        } else {
            return string.Format("{0}{1}",
                str.Substring(0, index), repText);
        }
    }

    public static string ReplaceAt(string str, int index, char character) {
        return ReplaceAt(str, index, character.ToString());
    }
}