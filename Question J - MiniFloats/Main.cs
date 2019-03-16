using System;

public static class MainClass {
    public static void Main(string[] args) {
        Console.WriteLine("Regular Binary:");
        Binary a = new Binary("1101");
        Binary b = new Binary("1010");
        Console.WriteLine("{0} + {1}    = {2,16}", a, b, a + b);
        Console.WriteLine("{0} - {1}    = {2,16}", a, b, a - b);
        Console.WriteLine("{0} * {1}    = {2,16}", a, b, a * b);
        Console.WriteLine("{0} / {1}    = {2,16}", a, b, a / b);

        Console.WriteLine("\nBase Ten:");
        Console.WriteLine("{0} + {1}    = {2,16}", a.BaseChange(), b.BaseChange(), (a + b).BaseChange());
        Console.WriteLine("{0} - {1}    = {2,16}", a.BaseChange(), b.BaseChange(), (a - b).BaseChange());
        Console.WriteLine("{0} * {1}    = {2,16}", a.BaseChange(), b.BaseChange(), (a * b).BaseChange());
        Console.WriteLine("{0} / {1}    = {2,16}", a.BaseChange(), b.BaseChange(), (a / b).BaseChange());

        Console.WriteLine("\nMinifloat Time!");

        MiniFloat x = new MiniFloat('0', "0111", "101");
        MiniFloat y = new MiniFloat('0', "1001", "010");
        Console.WriteLine("{0} + {1} = {2}", x, y, x + y);

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