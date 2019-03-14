using System;

public static class MainClass {
    public static void Main(string[] args) {
        Console.WriteLine("Regular Binary:");
        Binary a = new Binary("1001");
        Binary b = new Binary("101");
        Console.WriteLine("{0} + {1}    = {2,8}", a, b, a + b);
        Console.WriteLine("{0} - {1}    = {2,8}", a, b, a - b);
        Console.WriteLine("{0} * {1}    = {2,8}", a, b, a * b);
        Console.WriteLine("{0} / {1}    = {2,8}", a, b, a / b);

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