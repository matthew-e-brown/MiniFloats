# MiniFloats
A repository to work with a custom floating point format of binary numbers. This project is an over-kill version of a single question on the second assignment of the Trent University course, COIS-2300H.

A minifloat is an adaption of the `IEEE 754` standard for storing floating point numbers in binary. In that standard, a floating point number is stored as follows:
1. The first of 32 bits is a sign-bit: 0 for positive, 1 for negative
2. The next 8 bits store the exponent, biased with -127.
3. The final 23 bits stores the mantissa.

From Wikipedia:

![Wikipedia Image](https://upload.wikimedia.org/wikipedia/commons/thumb/d/d2/Float_example.svg/1180px-Float_example.svg.png)

becomes:

![Wikipedia Image](https://wikimedia.org/api/rest_v1/media/math/render/svg/15f92e12d6d0a7c02be4f12c83007940c432ba87)

***

Our MiniFloat system from **COIS-2300H** is very similar, and works exactly the same. However, it has:
1. The first bit is once again a sign-bit.
2. The next four are used to store the exponent.
3. The final three store the mantissa/fraction of the number.

This allows for a smaller number, making it easier to perform arithmetic by hand on paper, but at the cost of accuracy.

## Disclaimer
This 'calculator' is not 100% accurate, and fails on certain edge cases, particularly with larger powers. It is only a proof of concept meant to demonstrate an understanding of the process of floating point computations.
