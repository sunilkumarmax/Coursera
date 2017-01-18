namespace Coursera.Algorithms.ClassLibrary
{
    using System;

    public class NumberString
    {
        // Assumption:: The numbers given for Addition or multiplication are of the same length
        // TODO:: Implement for numbers that are not of the same length
        private string number;
        private const string ZERO = "0";
        private char[] NUMBERS = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public NumberString(string number)
        {
            int indexOfNumber = number.IndexOfAny(NUMBERS);
            if (indexOfNumber == -1)
            {
                this.number = "0";
            }
            else
            {
                bool isNegative = (number[0] - '-' == 0);
                number = number.Substring(indexOfNumber);
                this.number = string.IsNullOrEmpty(number) ? "0" : (isNegative ? "-" + number : number);
            }
        }

        public NumberString(char[] number)
        {
            this.number = new string(number);
        }

        public NumberString(int number)
        {
            this.number = number.ToString();
        }

        public bool IsNegative()
        {
            return this.number.StartsWith("-");
        }

        public string PrettyPrint()
        {
            int negativeCount = this.IsNegative() ? 1 : 0;
            int length = this.number.Length - negativeCount;
            char[] str = new char[length + ((length - 1) / 3) + negativeCount];
            for (int iter = negativeCount + length - 1, finalIter = str.Length - 1, counter = 0; iter >= 0; iter--, finalIter--, counter++)
            {
                str[finalIter] = this.number[iter];
                if (counter % 3 == 2)
                {
                    if (iter == 0 || (iter == 1 && negativeCount == 1))
                    {
                        continue;
                    }
                    finalIter--;
                    str[finalIter] = ',';
                }
            }
            return new string(str);
        }
        private bool isTwoMultiple(int num)
        {
            while (num > 0)
            {
                if (num % 2 != 0)
                {
                    return false;
                }
                num /= 2;
                if (num == 1)
                {
                    return true;
                }
            }
            return true;
        }
        public string Number { get { return this.number; } }

        public void PadRight(int totalWidth, char paddingCharacter)
        {
            if (this.number[0] != 48)
            {
                string paddingString = string.Empty.PadRight(totalWidth, paddingCharacter);
                this.number = this.number + paddingString;
            }
        }

        public void PadLeft(int totalWidth, char paddingCharacter)
        {
            if (paddingCharacter != 48)
            {
                string paddingString = string.Empty.PadRight(totalWidth, paddingCharacter);
                this.number = paddingString + this.number;
            }
        }

        public NumberString Add(NumberString anotherNumber)
        {
            char[] x, y;
            if (this.number.Length > anotherNumber.Number.Length)
            {
                x = this.number.ToCharArray();
                y = anotherNumber.Number.ToCharArray();
            }
            else
            {
                y = this.number.ToCharArray();
                x = anotherNumber.Number.ToCharArray();
            }
            char[] sum = new char[x.Length];
            int carry = 0;
            for (int iterX = x.Length - 1, iterY = y.Length - 1; iterX >= 0; iterX--, iterY--)
            {
                if (x[iterX] < 48 || x[iterX] > 57)
                {
                    throw new Exception("The input number is not in number format! Please enter valid digits from 0 - 9");
                }
                if (iterY >= 0 && (y[iterY] > 57 || y[iterY] < 48))
                {
                    throw new Exception("The input number is not in number format! Please enter valid digits from 0 - 9");
                }
                int miniSum = x[iterX] + carry - 48; // Removing Ascii
                if (iterY >= 0)
                {
                    miniSum += y[iterY] - 48; // Removing Ascii
                }
                carry = miniSum / 10;
                sum[iterX] = Convert.ToChar((miniSum % 10) + 48);
            }
            if (carry > 0)
            {
                return new NumberString(carry.ToString() + new string(sum));
            }
            else
            {
                return new NumberString(sum);
            }
        }

        public NumberString Subtract(NumberString anotherNumber)
        {
            char[] x, y;
            bool swap = true;
            if (this.number.Length > anotherNumber.Number.Length)
            {
                swap = false;
            }
            if (this.number.Length == anotherNumber.Number.Length)
            {
                for (int iter = 0; iter < this.number.Length; iter++)
                {
                    if (this.number[iter] > anotherNumber.Number[iter])
                    {
                        swap = false;
                        break;
                    }
                }
            }
            x = swap ? anotherNumber.Number.ToCharArray() : this.number.ToCharArray();
            y = swap ? this.number.ToCharArray() : anotherNumber.Number.ToCharArray();
            char[] sum = new char[x.Length];
            int borrow = 0;
            bool stillBorrow = false;
            for (int iterX = x.Length - 1, iterY = y.Length - 1; iterX >= 0; iterX--, iterY--)
            {
                if (x[iterX] < 48 || x[iterX] > 57)
                {
                    throw new Exception("The input number is not in number format! Please enter valid digits from 0 - 9");
                }
                if (iterY >= 0 && (y[iterY] > 57 || y[iterY] < 48))
                {
                    throw new Exception("The input number is not in number format! Please enter valid digits from 0 - 9");
                }
                int firstDigit, secondDigit;
                if (x[iterX] == 48 && borrow > 0)
                {
                    firstDigit = 9; // Assiging 9 directly in case of borrow
                    stillBorrow = true;
                }
                else
                {
                    firstDigit = x[iterX] - borrow - 48; // Removing Ascii
                    borrow = 0;
                }
                int miniDifference = firstDigit;
                if (iterY >= 0)
                {
                    secondDigit = y[iterY] - 48;
                    if (firstDigit >= secondDigit)
                    {
                        miniDifference -= secondDigit;
                        borrow = stillBorrow ? borrow : 0;
                    }
                    else
                    {
                        miniDifference = miniDifference + 10 - secondDigit;
                        borrow = 1;
                    }
                }
                sum[iterX] = Convert.ToChar(miniDifference + 48);
            }
            string finalDiff = new string(sum);
            int indexOfNumber = finalDiff.IndexOfAny(NUMBERS);
            finalDiff = finalDiff.Substring(indexOfNumber);
            if (swap)
            {
                return new NumberString("-" + finalDiff);
            }
            else
            {
                return new NumberString(finalDiff);
            }
        }

        public NumberString Multiply(NumberString anotherNumber)
        {
            string x, y = anotherNumber.Number;
            short negativeCount = 0;
            if (this.IsNegative())
            {
                x = this.number.Substring(1);
                negativeCount++;
            }
            else
            {
                x = this.number;
            }
            if (anotherNumber.IsNegative())
            {
                y = anotherNumber.Number.Substring(1);
                negativeCount++;
            }
            else
            {
                y = anotherNumber.Number;
            }
            if (x.Equals(ZERO) || y.Equals(ZERO))
            {
                return new NumberString(ZERO);
            }
            if (x.Length == 1 && y.Length == 1)
            {
                int miniProduct = Convert.ToInt32(x) * Convert.ToInt32(y);
                if (negativeCount % 2 == 1)
                {
                    return new NumberString("-" + miniProduct);
                }
                return new NumberString(miniProduct);
            }
            bool applyEfficiency = false;
            if(x.Length == y.Length && this.isTwoMultiple(x.Length))
            {
                applyEfficiency = true;
            }
            int xFirstHalfLength = x.Length / 2;
            int xSecondHalfLength = (x.Length + 1) / 2;
            int yFirstHalfLength = y.Length / 2;
            int ySecondHalfLength = (y.Length + 1) / 2;
            NumberString a = new NumberString(x.Substring(0, xFirstHalfLength));
            NumberString b = new NumberString(x.Substring(xFirstHalfLength));
            NumberString c = new NumberString(y.Substring(0, yFirstHalfLength));
            NumberString d = new NumberString(y.Substring(yFirstHalfLength));
            NumberString firstProduct = a.Multiply(c);
            firstProduct.PadRight(xSecondHalfLength + ySecondHalfLength, '0');
            NumberString fourthProduct = b.Multiply(d), product;
            if (applyEfficiency)
            {
                NumberString secondProduct = a.Add(b).Multiply(c.Add(d));
                secondProduct.PadRight(xSecondHalfLength, '0');
                product = firstProduct.Add(secondProduct).Add(fourthProduct);
            }
            else
            {
                NumberString secondProduct = a.Multiply(d);
                secondProduct.PadRight(xSecondHalfLength, '0');
                NumberString thirdProduct = b.Multiply(c);
                thirdProduct.PadRight(ySecondHalfLength, '0');
                product = firstProduct.Add(secondProduct).Add(thirdProduct).Add(fourthProduct);
            }
            if (negativeCount % 2 == 1)
            {
                return new NumberString("-" + product.Number);
            }
            return product;
        }
    }
}
