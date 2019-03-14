public class Binary {
    private int Length;

    /* Accessors */
    private string body;

    public string Body {
        get { return this.body; }
        set {
            this.body = value;
            this.Length = value.Length;
        }
    }
    public char this[int index] {
        get { return this.Body[index]; }
        set { this.Body = MainClass.ReplaceAt(this.Body, index, value.ToString()); }
    }
    /* End of Accessors */

    /* Constructors */
    public Binary() { // Default Constructor
        this.Body = "";
    }

    public Binary(string body) { // Body Constructor
        this.Body = body;
    }

    public Binary(int length) { // Bit-Length Constructor
        this.Body = new string('0', length);
    }
    /* End of Constructors */

    /* Methods */
    public override string ToString() {
        return this.Body;
    }

    public void Reverse() {
        char[] charArray = this.Body.ToCharArray();
        System.Array.Reverse(charArray);
        this.Body = new string(charArray);
    }
    /* End of Methods */

    /* Operators */
    /* -- Operators for Concatenation -- */
    public static Binary operator +(char m, Binary n) => new Binary(m + n.Body);
    public static Binary operator +(Binary m, char n) => new Binary(m.Body + n);
    public static Binary operator +(Binary m, string n) => new Binary(m.Body + n);
    public static Binary operator +(string m, Binary n) => new Binary(m + n.Body);
    /* -- End of Operators for Concatenation -- */

    public static Binary operator +(Binary m, Binary n) {
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
                if (i - 1 < 0) throw new System.Exception("Overflow Error!");
                else {
                    c[i] = (c[i] == '2' ? '0' : '1');
                    c[i - 1] = '1';
                }
            }
        }
        return c;
    }

    public static Binary operator -(Binary m, Binary n) {
        int l;
        if (m.Length > n.Length) {
            l = m.Length;
            for (int i = n.Length; i < m.Length; i++) n = (i == n.Length ? '1' : '0') + n;
        } else if (m.Length < n.Length) {
            l = n.Length;
            for (int i = m.Length; i < n.Length; i++) m = (i == m.Length ? '1' : '0') + m;
        } else {
            l = m.Length;
        }

        Binary c = new Binary(l);

        for (int i = l - 1; i >= 0; i--) {
            bool flag = true;
            do {
                int x = MainClass.ValueOf(m[i]) - MainClass.ValueOf(n[i]);
                if (x < 0) {
                    if (i - 1 < 0) throw new System.Exception("Subtrahend < Minuend, Underflow!");
                } else {
                    flag = false;
                    c[i] = MainClass.Character(x);
                }
            } while (flag);
        }

        return c;
    }

    public static Binary operator *(Binary m, Binary n) {
        Binary[] partials = new Binary[n.Length];
        for (int i = 0; i < partials.Length; i++) partials[i] = new Binary();

        for (int i = n.Length - 1; i >= 0; i--) {
            for (int k = 0; k < n.Length - (i + 1); k++) partials[i] += '0';
            for (int j = m.Length - 1; j >= 0; j--) {
                partials[i] += System.Convert.ToString(MainClass.ValueOf(n[i]) * MainClass.ValueOf(m[j]));
            }
            partials[i].Reverse();
        }

        Binary c = new Binary("0000");
        for (int i = 0; i < partials.Length; i++) c += partials[i];

        return c;
    }

    //public static Binary operator /(Binary m, Binary n) {

    //}
    /* End of Operators */
}