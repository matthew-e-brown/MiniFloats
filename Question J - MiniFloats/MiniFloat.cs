using System;

public class MiniFloat {
    /* Fields */
    private static readonly Binary BIAS = new Binary("0111");
    private static readonly int INTBIAS = 7;
    private char sign;
    private Binary expo;
    private Binary mant;

    /* Properties */
    private char Signbit { get { return this.sign; } set { this.sign = value; } }
    private Binary Exponent { get { return this.expo; } set { this.expo = value; } }
    private Binary Mantissa { get { return this.mant; } set { this.mant = value; } }

    /* Constructors */
    public MiniFloat() {
        this.Signbit = '0';
        this.Exponent = new Binary("0111");
        this.Mantissa = new Binary("000");
    }

    public MiniFloat(char sign) {
        this.Signbit = sign;
        this.Exponent = new Binary("0111");
        this.Mantissa = new Binary("000");
    }

    public MiniFloat(char sign, string exponent, string mantissa) {
        this.Signbit = sign;
        this.Exponent = new Binary(exponent);
        this.Mantissa = new Binary(mantissa);
    }

    public MiniFloat(char sign, Binary exponent, Binary mantissa) {
        this.Signbit = sign;
        this.Exponent = exponent;
        this.Mantissa = mantissa;
    }

    /* Methods */
    public float BaseChange() {
        int power = Convert.ToInt32((this.Exponent - BIAS).BaseChange());
        float significand = 1F;
        for (int i = 0; i < this.Mantissa.Length; i++) {
            if (this.Mantissa[i] == '1') significand += (float)(1 / Math.Pow(2, i + 1));
        }

        return significand * (int)Math.Pow(2, power) * (this.Signbit == '1' ? -1 : 1);
    }

    public MiniFloat Copy() {
        return new MiniFloat(this.Signbit, this.Exponent, this.Mantissa);
    }

    public override string ToString() => this.Reduce(true);

    public string Reduce(bool spaces) => string.Format(spaces ? "{0} {1} {2}" : "{0}{1}{2}",
            this.Signbit, this.Exponent, this.Mantissa);

    private void ShiftUp() {
        this.Mantissa = '0' + this.Mantissa;
        this.Exponent += new Binary("0001");
    }

    public void MinimizeMantissa() {
        if (this.Mantissa.Length >= 3) {
            this.Mantissa.Body = this.Mantissa.Body.Substring(0, 3);
        } else {
            while (this.Mantissa.Length < 3) {
                this.Mantissa += '0';
            }
        }
    }

    /* 10.111 -> 1.0111 */
    private void ShiftDown() {
        this.Mantissa.Body = this.Mantissa.Body.Substring(1); //Chop off the first thing
        this.Exponent -= new Binary("0001");
    }

    private static void Normalize(ref MiniFloat a, ref MiniFloat b) {
        int diff = Convert.ToInt32(a.Exponent.BaseChange()) - Convert.ToInt32(b.Exponent.BaseChange()); //If b's exponent is bigger, will be < 0.
        if (diff < 0) {
            for (int i = 0; i < Math.Abs(diff); i++) a.ShiftUp();
        } else if (diff > 0) {
            for (int i = 0; i < Math.Abs(diff); i++) b.ShiftUp();
        } //If there's no difference then don't bother shifting

        diff = a.Mantissa.Length - b.Mantissa.Length; //Difference in their lengths //Reuse 'diff'
        // I.E. 1.010 vs 0.01101 will be 4 - 6 = 2, therefore a needs 2 zeros added if diff = -2
        if (diff < 0) {
            for (int i = 0; i < Math.Abs(diff); i++) a.Mantissa += '0';
        } else if (diff > 0) {
            for (int i = 0; i < Math.Abs(diff); i++) b.Mantissa += '0';
        }
    }

    /* Helper method, ignores sign-bits */
    private static MiniFloat add(MiniFloat m, MiniFloat n) {
        MiniFloat result = new MiniFloat();

        /* Need to be able to tell if it wraps over
         * I.E. 1.101 + 1.010 = 10.111
         * Will have wrapped over if their lengths before hand are less than after */
        result.Mantissa = m.Mantissa + n.Mantissa;
        result.Exponent = m.Exponent; //They've been normalized to the same exponent, doesn't matter
        /* If result is more bits than the longer of the two summands */
        int longest = (m.Mantissa.Length >= n.Mantissa.Length) ? m.Mantissa.Length : n.Mantissa.Length;
        if (result.Mantissa.Length > longest) {
            int diff = result.Mantissa.Length - longest; //Should only ever be one but why not
            for (int i = 0; i < diff; i++) result.Exponent += new Binary('1');
        } //else { //If the place before the decimal isn't two long
          //Take off leading one
        result.Mantissa.Body = result.Mantissa.Body.Substring(1);
        //}

        /* MiniFloat format can only hold 3 digits */
        //result.MinimizeMantissa();

        return result;
    }

    private static MiniFloat sub(MiniFloat m, MiniFloat n) {
        MiniFloat result;
        if (m.Mantissa > n.Mantissa) {
            result = new MiniFloat((m.Signbit == '1') ? '1' : '0');
        } else if (n.Mantissa > m.Mantissa) {
            result = new MiniFloat((n.Signbit == '1') ? '1' : '0');
        } else result = new MiniFloat('0');

        try {
            result.Mantissa = m.Mantissa - n.Mantissa;
            result.Exponent = m.Exponent;
        } catch (BinaryUnderFlowException) {
            result.Signbit = (result.Signbit == '0') ? '1' : '0'; //Flip the sign
            result.Mantissa = n.Mantissa - m.Mantissa;
        }

        if (result.Mantissa.Body.IndexOf('1') != -1) { // If there's a one in the mantissa
            while (result.Mantissa[0] != '1') {
                result.ShiftDown();
            }

            /* Take off leading one */
            result.Mantissa.Body = result.Mantissa.Body.Substring(1);
        }

        //result.MinimizeMantissa();

        return result;
    }

    /* Operators */
    public static MiniFloat operator +(MiniFloat Augend, MiniFloat Addend) {
        MiniFloat m = Augend.Copy();
        MiniFloat n = Addend.Copy();

        m.Mantissa = '1' + m.Mantissa;
        n.Mantissa = '1' + n.Mantissa;
        Normalize(ref m, ref n);

        if (m.Signbit == '0' && n.Signbit == '0') return add(m, n);
        else if ((m.Signbit == '1' && n.Signbit == '0') || (n.Signbit == '1' && m.Signbit == '0')) {
            if (m.Mantissa > n.Mantissa) return sub(m, n);
            else if (n.Mantissa > m.Mantissa) return sub(n, m);
            else /* Equal */ return new MiniFloat(); //Zero
        } else { //Both are negative
            MiniFloat r = add(m, n);
            r.Signbit = '1';
            return r;
        }
    }

    public static MiniFloat operator -(MiniFloat Minuend, MiniFloat Subtrahend) {
        MiniFloat m = Minuend.Copy();
        MiniFloat n = Subtrahend.Copy();
        /* To subtract, you add the negative */
        n.Signbit = (n.Signbit == '0') ? '1' : '0'; //flip dat bit
        return m + n; //+ operator deals with the signbit stuff
    }

    private static char NewSignbit(char s1, char s2) {
        bool one = (s1 == '1'), two = (s2 == '1');
        //XOR: one but not both is 1, then the new is 1
        return ((one || two) && !(one && two)) ? '1' : '0';
    }

    public static MiniFloat operator *(MiniFloat Multiplicand, MiniFloat Multiplier) {
        /* M * N = (M_mant * N_mant) * 2^(M_sign + N_sign) */
        MiniFloat m = Multiplicand.Copy();
        MiniFloat n = Multiplier.Copy();

        /* Add leading ones */
        m.Mantissa = '1' + m.Mantissa;
        n.Mantissa = '1' + n.Mantissa;

        /* Multipliy the mantissas */
        Binary newMantissa = m.Mantissa * n.Mantissa;

        /* Add the non-biased exponents, create new biased exponent */
        int e = (Convert.ToInt32(m.Exponent.BaseChange()) - 7) +
            (Convert.ToInt32(n.Exponent.BaseChange()) - 7);
        Binary newExponent = new Binary(Binary.BaseChange(e + 7, 10, 2));

        char newSignbit = NewSignbit(m.Signbit, n.Signbit);

        MiniFloat result = new MiniFloat(newSignbit, newExponent, newMantissa);

        // Will have "11.[...]" or "10" at the start
        // OR, if both were 000 000 -> 1.000 * 1.000 = 1.000
        if (!(m.Mantissa.Body == "1000" && n.Mantissa.Body == "1000")) {
            result.Exponent += new Binary("0001");
        }

        /* Remove leading one */
        result.Mantissa.Body = result.Mantissa.Body.Substring(1);

        return result;
    }

    //public static MiniFloat operator /(MiniFloat m, MiniFloat n) {

    //}

}