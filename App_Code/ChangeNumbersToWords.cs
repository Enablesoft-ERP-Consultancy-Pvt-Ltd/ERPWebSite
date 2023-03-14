using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ChangeNumbersToWords
/// </summary>
public class ChangeNumbersToWords
{
    public static string NumberToWords(int number)
    {
        if (number == 0)
            return "zero";

        if (number < 0)
            return "minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
        }

        return words;
    }
    public static string ConvertMyword(int number)
    {
        int flag = 0;
        int lflag = 0;
        string words = String.Empty;
        string[] places = { "ones", "ten", "hundred", "thousand", "ten thousand", "lacs", "tenlacs", "crore", "tencrore", "Billon" };
        string rawnumber = number.ToString();
        char[] a = rawnumber.ToCharArray();
        Array.Reverse(a);
        if (a.Length >= 2)
        {
            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (i % 2 == 0 && i > 2)
                {
                    if (int.Parse(a[i].ToString()) > 1)
                    {
                        if (int.Parse(a[i - 1].ToString()) == 0)
                        {
                            words = words + getNumberStringty(int.Parse(a[i].ToString())) + " " + places[i - 1] + " ";
                        }
                        else
                        {
                            words = words + getNumberStringty(int.Parse(a[i].ToString())) + " ";
                        }
                    }
                    else if (int.Parse(a[i].ToString()) == 1)
                    {
                        if (int.Parse(a[i - 1].ToString()) == 0)
                        {
                            words = words + "Ten" + " ";
                        }
                        else
                        {
                            words = words + getNumberStringteen(int.Parse(a[i - 1].ToString())) + " ";
                        }
                        flag = 1;
                    }
                }
                else
                {
                    if (i == 1 || i == 0)
                    {
                        if (int.Parse(a[i].ToString()) > 1)
                        {
                            words = words + getNumberStringty(int.Parse(a[i].ToString())) + " " + getNumberString(int.Parse(a[0].ToString())) + " ";
                            break;
                        }
                        else if (int.Parse(a[i].ToString()) == 1)
                        {
                            if (int.Parse(a[i - 1].ToString()) == 0)
                            {
                                words = words + "Ten" + " ";
                            }
                            else
                            {
                                words = words + getNumberStringteen(int.Parse(a[i - 1].ToString())) + " ";
                            }

                            break;
                        }
                        else if (int.Parse(a[i - 1].ToString()) != 0)
                        {
                            words = words + getNumberString(int.Parse(a[i - 1].ToString())) + " ";
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (flag == 0)
                        {
                            for (int l = i; l >= 0; l--)
                            {
                                if (int.Parse(a[l].ToString()) != 0)
                                {
                                    lflag = 1;
                                }
                            }
                            if (lflag == 1 && int.Parse(a[i].ToString()) != 0)
                            {

                                words = words + getNumberString(int.Parse(a[i].ToString())) + " " + places[i] + " ";
                                lflag = 0;


                            }
                            else if (lflag == 0)
                            {

                                lflag = 0;
                                break;
                            }

                        }
                        else
                        {
                            words = words + " " + places[i] + " ";
                            flag = 0;
                        }

                    }
                }
            }
        }
        else
        {
            words = getNumberString(int.Parse(a[0].ToString()));
        }
        //Console.WriteLine(words);
        return words;
    }
    static string getNumberString(int num)
    {
        string Word = String.Empty;
        switch (num)
        {
            case 1:
                Word = "one";
                break;
            case 2:
                Word = "two";
                break;

            case 3:
                Word = "three";
                break;

            case 4:
                Word = "four";
                break;

            case 5:
                Word = "five";
                break;

            case 6:
                Word = "six";
                break;
            case 7:
                Word = "seven";
                break;

            case 8:
                Word = "eight";
                break;

            case 9:
                Word = "nine";
                break;


        }
        return Word;
    }
    static string getNumberStringty(int num)
    {
        string Word = String.Empty;
        switch (num)
        {

            case 2:
                Word = "twenty";
                break;

            case 3:
                Word = "thirty";
                break;

            case 4:
                Word = "fourty";
                break;

            case 5:
                Word = "fifty";
                break;

            case 6:
                Word = "sixty";
                break;
            case 7:
                Word = "seventy";
                break;

            case 8:
                Word = "eighty";
                break;

            case 9:
                Word = "ninty";
                break;


        }
        return Word;
    }
    static string getNumberStringteen(int num)
    {
        string Word = String.Empty;
        switch (num)
        {
            case 1:
                Word = "eleven";
                break;
            case 2:
                Word = "tewlve";
                break;

            case 3:
                Word = "thirteen";
                break;

            case 4:
                Word = "fourteen";
                break;

            case 5:
                Word = "fifteen";
                break;

            case 6:
                Word = "sixteen";
                break;
            case 7:
                Word = "seventeen";
                break;

            case 8:
                Word = "eighteen";
                break;

            case 9:
                Word = "ninteen";
                break;


        }
        return Word;
    }

}