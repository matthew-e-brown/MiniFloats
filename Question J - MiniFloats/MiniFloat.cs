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
    public string Reduce(bool spaces = false) {
        return string.Format(spaces ? "{0} {1} {2}" : "{0}{1}{2}",
            this.Signbit, this.Exponent, this.Mantissa);
    }

    /* Operators */
    //public MiniFloat operator +(MiniFloat m, MiniFloat n) {

    //}

    //public MiniFloat operator -(MiniFloat m, MiniFloat n) {

    //}

    //public MiniFloat operator *(minifloat m, minifloat n) {

    //}

    //public MiniFloat operator /(MiniFloat m, MiniFloat n) {

    //}

}