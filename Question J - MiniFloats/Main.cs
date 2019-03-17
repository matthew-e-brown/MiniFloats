using System;

public static class MainClass {
    public static void Main(string[] args) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("-------------- Regular Binary Math --------------\n");
        Console.ResetColor();

        Binary a = new Binary("1101");
        Binary b = new Binary("1011");
        Binary[] binaryResults = new Binary[] { a + b, a - b, a * b, a / b };
        char[] signs = { '+', '-', '*', '/' };

        for (int i = 0; i < binaryResults.Length; i++) {
            Console.WriteLine("{0,4} {1} {2,4} = {3,8}",
                a, signs[i], b, binaryResults[i]);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("{0,4} {1} {2,4} = {3,8}\n",
                a.BaseChange(), signs[i], b.BaseChange(), binaryResults[i].BaseChange());
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n-------------- MiniFloat Math -------------------\n");
        for (int k = 0; k < 3; k++) {
            for (int i = 0; i < 2; i++) {

                MiniFloat x;
                MiniFloat y;

                switch (k) {
                case 1:
                    /* Example 2 */
                    x = new MiniFloat('0', "0111", "101");
                    y = new MiniFloat('0', "1001", "010");
                    break;
                case 2:
                    /* Example 3 */
                    x = new MiniFloat('0', "1010", "110");
                    y = new MiniFloat('0', "1001", "110");
                    break;
                default:
                    /* Example 1*/
                    x = new MiniFloat('1', "1011", "101");
                    y = new MiniFloat('0', "0111", "011");
                    break;
                }

                if (i == 0) {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("-------- {0,10} and {1,10} -----------\n", x, y);
                    Console.ResetColor();
                }

                MiniFloat[] miniFloatResults = new MiniFloat[] { x + y, x - y, x * y, x / y };

                Console.ForegroundColor = ConsoleColor.Blue;
                if (i == 0) Console.WriteLine("------------ Without Rounding ------------\n");
                else {
                    Console.WriteLine("\n------------ With Rounding ---------------\n");
                    x.MinimizeMantissa();
                    y.MinimizeMantissa();
                    foreach (MiniFloat m in miniFloatResults) m.MinimizeMantissa();
                }
                Console.ResetColor();

                for (int j = 0; j < miniFloatResults.Length; j++) {
                    Console.WriteLine("{0} {1} {2} = {3}",
                        x, signs[j], y, miniFloatResults[j]);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("{0,10} {1} {2,10} = {3,10}\n",
                        x.BaseChange(), signs[j], y.BaseChange(), miniFloatResults[j].BaseChange());
                    Console.ResetColor();
                }
            }
        }

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