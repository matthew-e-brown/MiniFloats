using System;

public class MiniFloat {
    /* Fields */
    private static readonly Binary BIAS = new Binary("0111");
    private Binary sign;
    private Binary expo;
    private Binary mant;

    /* Properties */
    private Binary Signbit { get { return this.sign; } set { this.sign = value; } }
    private Binary Exponent { get { return this.expo; } set { this.expo = value; } }
    private Binary Mantissa { get { return this.mant; } set { this.mant = value; } }

    /* Constructors */
    public MiniFloat() {
        this.Signbit = new Binary("0");
        this.Exponent = new Binary("0111");
        this.Mantissa = new Binary("000");
    }

    public MiniFloat(char sign, string exponent, string mantissa) {
        this.Signbit = new Binary(sign);
        this.Exponent = new Binary(exponent);
        this.Mantissa = new Binary(mantissa);
    }

    public MiniFloat(Binary sign, Binary exponent, Binary mantissa) {
        this.Signbit = sign;
        this.Exponent = exponent;
        this.Mantissa = mantissa;
    }

    /* Methods */
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

    /* 10.111 -> 1.0111 */
    private void ShiftDown() {
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

    /* Helper method, since the - and + operator will call this,
     * to help deal with sign-bits. */
    private static MiniFloat add(MiniFloat m, MiniFloat n) {
        MiniFloat result = new MiniFloat();

        /* Leading ones */
        m.Mantissa = '1' + m.Mantissa;
        n.Mantissa = '1' + n.Mantissa;

        Normalize(ref m, ref n);
        /* Need to be able to tell if it wraps over
         * I.E. 1.101 + 1.010 = 10.111
         * Will have wrapped over if their lengths before hand are less than after */
        result.Mantissa = m.Mantissa + n.Mantissa;
        result.Exponent = m.Exponent; //They've been normalized to the same exponent, doesn't matter
        /* If result is more bits than the longer of the two summands */
        int longest = (m.Mantissa.Length >= n.Mantissa.Length) ? m.Mantissa.Length : n.Mantissa.Length;
        if (result.Mantissa.Length > longest) {
            int diff = result.Mantissa.Length - longest; //Should only ever be one but why not
            for (int i = 0; i < diff; i++) result.ShiftDown();
        } else { //If the place before the decimal isn't two long
            //Take off leading one
            result.Mantissa.Body = result.Mantissa.Body.Substring(1);
        }

        /* MiniFloat format can only hold 3 digits */
        result.Mantissa.Body = result.Mantissa.Body.Substring(0, 3);
        return result;
    }

    /* Operators */
    public static MiniFloat operator +(MiniFloat Augend, MiniFloat Addend) {
        MiniFloat m = Augend.Copy();
        MiniFloat n = Addend.Copy();
        return add(m, n);
    }

    //public static MiniFloat operator -(MiniFloat m, MiniFloat n) {

    //}

    //public static MiniFloat operator *(minifloat m, minifloat n) {

    //}

    //public static MiniFloat operator /(MiniFloat m, MiniFloat n) {

    //}

}