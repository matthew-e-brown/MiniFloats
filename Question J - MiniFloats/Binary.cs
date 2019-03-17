﻿using System;

public class Binary { /* A class for binary numbers without decimal implementation */
    /* Fields */
    private int length;
    private string body;
    /* End of Fields */

    /* Properties */
    public int Length {
        get { return this.length; }
    }

    public string Body {
        get { return this.body; }
        set {
            this.body = value;
            this.length = value.Length;
        }
    }

    public char this[int index] {
        get { return this.Body[index]; }
        set { this.Body = MainClass.ReplaceAt(this.Body, index, value.ToString()); }
    }
    /* End of Properties */

    /* Constructors */
    public Binary() { // Default Constructor
        this.Body = "";
    }

    public Binary(string body) { // Body Constructor
        this.Body = body;
    }

    public Binary(char body) {
        this.Body = Convert.ToString(body);
    }

    public Binary(int length) { // Bit-Length Constructor
        this.Body = new string('0', length);
    }
    /* End of Constructors */

    /* Methods */
    public override string ToString() => this.Body;

    public void Reverse() {
        char[] charArray = this.Body.ToCharArray();
        Array.Reverse(charArray);
        this.Body = new string(charArray);
    }


    /* -- So I can modify Binary in operators without
     * it changing the passed value -- */
    public Binary Copy() {
        return new Binary(this.Body);
    }

    public string BaseChange() => BaseChange(this.Body);

    public static string BaseChange(string number, int fromBase = 2, int toBase = 10) {

        int[] RemainderDivide(int num, int den) {
            int quot = 0;
            while (num >= den) {
                num -= den;
                quot++;
            }
            return new int[] { quot, num % den };
        }

        string Reverse(string s) {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        char[] DigitOrder = new char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F'
        };

        string numString = Reverse(number);
        int decim = 0;
        for (int i = 0; i < numString.Length; i++) {
            int digit = Array.IndexOf(DigitOrder, Char.ToUpper(numString[i]));
            decim += digit * (int)Math.Pow(fromBase, i);
        }
        if (toBase == 10) return Convert.ToString(decim);

        string result = "";
        do {
            int[] division = RemainderDivide(decim, toBase);
            decim = division[0];
            result += DigitOrder[division[1]];
        } while (Math.Abs(decim) > 0);

        result = Reverse(result);
        while (result.Length < 4) result = '0' + result;
        return result;
    }

    public static string BaseChange(int number, int toBase = 2, int fromBase = 10) => BaseChange(Convert.ToString(number), toBase, fromBase);

    /* -- Autogenerated Methods to get rid of warnings -- */
    public override bool Equals(object obj) {
        return obj is Binary binary &&
               Length == binary.Length &&
               body == binary.body &&
               Body == binary.Body;
    }

    public override int GetHashCode() {
        var hashCode = 704898135;
        hashCode = hashCode * -1521134295 + Length.GetHashCode();
        hashCode = hashCode * -1521134295 + System.Collections.Generic.EqualityComparer<string>.Default.GetHashCode(body);
        hashCode = hashCode * -1521134295 + System.Collections.Generic.EqualityComparer<string>.Default.GetHashCode(Body);
        return hashCode;
    }
    /* -- End of autogenerated methods -- */
    /* End of Methods */

    /* Operators */
    public static bool operator >(Binary m, Binary n) {
        if (m == n) return false;
        int l;
        if (m.Length > n.Length) {
            l = m.Length;
            for (int i = n.Length; i < m.Length; i++) n = '0' + n;
        } else if (m.Length < n.Length) {
            l = n.Length;
            for (int i = m.Length; i < n.Length; i++) m = '0' + m;
        } else {
            l = m.Length;
        }

        for (int i = 0; i < l; i++) {
            bool mOne = false, nOne = false;
            if (m[i] == '1') mOne = true;
            if (n[i] == '1') nOne = true;
            if (mOne ^ nOne) return mOne; //If only one had a 1 found, return whether it was m.
        }
        return false;
    }

    public static bool operator <(Binary m, Binary n) => !(m > n) && !(m == n);

    public static bool operator ==(Binary m, Binary n) {
        if (m.Length != n.Length) return false;
        for (int i = 0; i < m.Length; i++) if (m[i] != n[i]) return false;
        return true;
    }

    public static bool operator !=(Binary m, Binary n) => !(m == n);
    public static bool operator >=(Binary m, Binary n) => (m > n) || (m == n);
    public static bool operator <=(Binary m, Binary n) => (m < n) || (m == n);

    /* -- Operators for Concatenation -- */
    public static Binary operator +(char m, Binary n) => new Binary(m + n.Body);
    public static Binary operator +(Binary m, char n) => new Binary(m.Body + n);
    public static Binary operator +(Binary m, string n) => new Binary(m.Body + n);
    public static Binary operator +(string m, Binary n) => new Binary(m + n.Body);
    /* -- End of Operators for Concatenation -- */

    public static Binary operator +(Binary Augend, Binary Addend) {
        Binary m = Augend.Copy();
        Binary n = Addend.Copy();

        /* Normalize to the longest one */
        int l;
        if (m.Length > n.Length) {
            l = m.Length;
            for (int i = n.Length; i < m.Length; i++) n = '0' + n;
        } else if (m.Length < n.Length) {
            l = n.Length;
            for (int i = m.Length; i < n.Length; i++) m = '0' + m;
        } else {
            l = m.Length;
        }

        Binary c = new Binary(l);

        for (int i = l - 1; i >= 0; i--) {
            int x = MainClass.ValueOf(m[i]) + MainClass.ValueOf(n[i]) + MainClass.ValueOf(c[i]);
            c[i] = MainClass.Character(x);
            if (c[i] == '2' || c[i] == '3') {
                c[i] = (c[i] == '2' ? '0' : '1');
                if (i - 1 < 0) { //If it's the last one...
                    c = '1' + c;
                } else {
                    c[i - 1] = '1';
                }
            }
        }
        return c;
    }

    public static Binary operator -(Binary Minuend, Binary Subtrahend) {
        Binary m = Minuend.Copy();
        Binary n = Subtrahend.Copy();

        void Borrow(ref Binary x, int p /* The index that caused a problem & must become a two */) {
            while (x[p] != '1' && x[p] != '2') {
                for (int i = p - 1 /**/ ; i >= 0; i--) {
                    if (x[i] == '1') {
                        x[i] = '0';
                        x[i + 1] = '2';
                        break;
                    } else if (x[i] == '2') {
                        x[i] = '1';
                        x[i + 1] = '2';
                        break;
                    }
                }
            }
        }

        int l;
        if (m.Length > n.Length) {
            l = m.Length;
            for (int i = n.Length; i < m.Length; i++) n = '0' + n;
        } else if (m.Length < n.Length) {
            l = n.Length;
            for (int i = m.Length; i < n.Length; i++) m = '0' + m;
        } else {
            l = m.Length;
        }

        Binary c = new Binary(l);

        for (int i = l - 1; i >= 0; i--) {
            bool flag = true;
            do {
                int x = MainClass.ValueOf(m[i]) - MainClass.ValueOf(n[i]);
                if (x < 0) {
                    if (i - 1 < 0) throw new BinaryUnderFlowException("Cannot borrow in last column, underflow error.");
                    else Borrow(ref m, i);
                } else {
                    flag = false;
                    c[i] = MainClass.Character(x);
                }
            } while (flag);
        }

        return c;
    }

    public static Binary operator *(Binary Multiplicand, Binary Multiplier) {
        Binary m = Multiplicand.Copy();
        Binary n = Multiplier.Copy();

        Binary[] partials = new Binary[n.Length]; // List of the partial sums
        for (int i = 0; i < partials.Length; i++) partials[i] = new Binary();

        for (int i = n.Length - 1; i >= 0; i--) {
            for (int k = 0; k < n.Length - (i + 1); k++) partials[i] += '0';
            for (int j = m.Length - 1; j >= 0; j--) {
                partials[i] += Convert.ToString(MainClass.ValueOf(n[i]) * MainClass.ValueOf(m[j]));
            }
            partials[i].Reverse();
        }

        Binary c = new Binary();
        for (int i = 0; i < partials.Length; i++) c += partials[i];

        return c;
    }

    public static Binary operator *(int Multiplicand, Binary Multiplier) => new Binary(BaseChange(Multiplicand, 10, 2)) * Multiplier;
    public static Binary operator *(Binary Multiplicand, int Multiplier) => Multiplicand * new Binary(BaseChange(Multiplier, 10, 2));

    public static Binary operator /(Binary Dividend /* m */, Binary Divisor /* n */) {
        Binary m = Dividend.Copy();
        Binary n = Divisor.Copy();

        /* Get the first set of digits of m where that set is the same length as n's digits */
        Binary quotient = new Binary(); //if N is 3 digits long, quotient starts with 000.

        Binary drop = new Binary();
        for (int i = 0; i < m.Length; i++) {
            drop += m[i];
            if (n <= drop) {
                quotient += '1';
            } else {
                quotient += '0';
            }
            drop = drop - n * MainClass.ValueOf(quotient[i]) /*0 or 1*/;
        }

        if (quotient.Body.IndexOf('1') != -1) { //If there's a one in it
            while (quotient[0] != '1') quotient.Body = quotient.Body.Substring(1);
        }

        return quotient;
    }
    /* End of Operators */
}

/* Just to be able to throw a more specific error, constructors generated autoamtically */
class BinaryUnderFlowException : Exception {
    public BinaryUnderFlowException() {
    }

    public BinaryUnderFlowException(string message) : base(message) {
    }
}