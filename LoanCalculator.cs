/*
   * Show Me The Money Project
   *   Creates loan statistics and an amortization table for a given loan.
   *
   *   file:   ShowMeTheMoney.cs
   *   author: Tamsyn Evezard
   */

using System;

namespace LoanCalculator
{
    class Loan
    {
        private double p; // original loan amount
        private double r; //interest rate (per month)
        private int n; // number of months in the loan term
        private double o; //owed balance
        private double a; // APR: annual interest rate
        private double e; // extra monthly payment

        public Loan(double amountBorrowed = 200000, double apr = 4.25, int numberOfMonths = 360, double extra = 0.0)
        {
            p = amountBorrowed;
            a = apr;
            r = apr / 100 / 12;
            n = numberOfMonths;
            o = amountBorrowed; // borrowed amount = owed amount at beginning of loan period
            e = extra;
        }

        public double AmountBorrowed
        {
            get { return Math.Round(p, 2); }
        }

        public double RemainingBalance
        {
            get { return Math.Round(o, 2); }
        }

        public double InterestRate
        {
            get { return Math.Round(a, 2); }
        }

        public int NumberOfMonths
        {
            get { return n; }
        }

        public double Extra
        {
            get { return Math.Round(e, 2); }
        }

        //--Calculate the Monthly Payment--//

        public double CalculateMonthlyPayment()
        {
            double expTerm = Math.Pow(1 + r, n);
            double numerator = p * r * expTerm;
            double denominator = expTerm - 1;
            return numerator / denominator;
        }

        //--Calculate the Interest Portion--//

        public double InterestPortion()
        {
            double ip;
            ip = r * o;
            return ip;
        }

        //--Calculate the Principal Portion--//

        public double PrincipalPortion()
        {
            double payment = CalculateMonthlyPayment();
            double interest = InterestPortion();
            double principal = payment - interest + e;
            return Math.Round(principal, 2);
        }

        //-- Make the Monthly Payment (subtract it from the Remaining Balance) --//

        public void MakeMonthlyPayment()
        {
            double principal = PrincipalPortion();
            o -= principal;
        }

        //-- Calculate the Total Amount that is Paid at the end of the loan period --//

        public double TotalPaid(double[,] at)
        {
            double totalPaid = 0;
            for(int t = 0; t < at.GetLength(0); t++)
            {
                totalPaid += at[t, 0];
            }
            return totalPaid;
        }

        //-- Create Amortization Table for the given loan --//

        public double[,] AmortizationTable() // 2D Array
        {
            double[,] at = new double[n + 1, 4]; // first row in table before any payments occur

            at[0, 0] = 0.0;
            at[0, 1] = 0.0;
            at[0, 2] = 0.0;
            at[0, 3] = AmountBorrowed;


            int m;
            for (m = 1; m < at.GetLength(0) && RemainingBalance > 0; m++) // until remaining balance reaches 0
            {
                if (RemainingBalance > CalculateMonthlyPayment()) // full payments
                {
                    at[m, 0] = CalculateMonthlyPayment() + e;
                    at[m, 1] = InterestPortion();
                    at[m, 2] = PrincipalPortion();
                    MakeMonthlyPayment();
                    at[m, 3] = RemainingBalance;
                }

                else // last payment (if less than others)
                {
                    at[m, 0] = InterestPortion() + RemainingBalance;
                    at[m, 1] = InterestPortion();
                    at[m, 2] = RemainingBalance;
                    at[m, 3] = 0;

                    o = 0; //set Remaining Balance to 0
                }
            }

            double[,] nb = new double[m, 4];

            int x;
            for(x = 0; x < m; x++)  // allows us to print the table until the loan is repayed
                                    // NB for when loan is repayed BEFORE the number of Months inputed is reached (due to extra monthly payment)
            {
                nb[x, 0] = at[x, 0];
                nb[x, 1] = at[x, 1];
                nb[x, 2] = at[x, 2];
                nb[x, 3] = at[x, 3];
            }

            return nb;
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            //-- Step 1: Validate Length --//

            if (args.Length < 4)
            {
                Console.WriteLine("Error: Not enough arguements");
                Console.WriteLine("Usage: mono ShowMeTheMoney.exe <Loan Amount> <Interest Rate> <Number of Months> <Extra Payment per Month>");
                return;
            }

            if (args.Length > 4)
            {
                Console.WriteLine("Error: Too many arguements");
                Console.WriteLine("Usage: mono ShowMeTheMoney.exe <Loan Amount> <Interest Rate> <Number of Months> <Extra Payment per Month>");
                return;
            }

            //-- Step 2: Validate Parsing --//

            if (!double.TryParse(args[0], out double amountBorrowed))
            {
                Console.WriteLine("Error: Invalid Loan Amount");
                Console.WriteLine("Usage: mono ShowMeTheMoney.exe <Loan Amount> <Interest Rate> <Number of Months> <Extra Payment per Month>");
                return;
            }
            
            if (!double.TryParse(args[1], out double apr))
            {
                Console.WriteLine("Error: Invalid Interest Rate");
                Console.WriteLine("Usage: mono ShowMeTheMoney.exe <Loan Amount> <Interest Rate> <Number of Months> <Extra Payment per Month>");
                return;
            }

            if (!int.TryParse(args[2], out int numberOfMonths))
            {
                Console.WriteLine("Error: Invalid Number of Months");
                Console.WriteLine("Usage: mono ShowMeTheMoney.exe <Loan Amount> <Interest Rate> <Number of Months> <Extra Payment per Month>");
                return;
            }

            if(!double.TryParse(args[3], out double extra))
            {
                Console.WriteLine("Error: Invalid Extra Amount");
                Console.WriteLine("Usage: mono ShowMeTheMoney.exe <Loan Amount> <Interest Rate> <Number of Months> <Extra Payment per Month>");
                return;
            }

            //-- Step 3: Validate Range --//

            if (amountBorrowed > 50000000 || amountBorrowed < 50)
            {
                Console.WriteLine("Error: Loan Amount is outside of Range");
                return;
            }

            if (apr > 20 || apr < 0)
            {
                Console.WriteLine("Error: Interest Rate is outside of Range");
                return;
            }

            if (numberOfMonths > 600)
            {
                Console.WriteLine("Error: Number of Months is outside of Range");
                return;
            }

            if (extra > amountBorrowed || extra < 0)
            {
                Console.WriteLine("Error: Extra Payment per Month is outside of Range");
                return;
            }

            //-- Loan Calculator Construction --//

            Loan myLoan = new Loan(amountBorrowed, apr, numberOfMonths, extra);
            double monthlyPayment = myLoan.CalculateMonthlyPayment();

            Console.WriteLine($"You have borrowed {amountBorrowed:C2} at an APR of {apr}");
            Console.WriteLine($"You have {numberOfMonths} months to pay back the loan");
            Console.WriteLine($"Your default monthly payment is {monthlyPayment:C2}");
            Console.WriteLine($"You have chosen to pay {extra:C2} extra per month");

            double[,] at = myLoan.AmortizationTable();
            string[] tableHeadings = { "Payment", "Amount", "Interest", "Principal", "Balance" };
            Console.WriteLine($"{tableHeadings[0]} {tableHeadings[1],15} {tableHeadings[2],15} {tableHeadings[3],15} {tableHeadings[4],15}");

            for (int i = 0; i < 6; i++) // prints out first 6 rows (first 5 payments)
            {
                int col0 = i;
                string col1 = $"{at[i, 0],15:C2}";
                string col2 = $"{at[i, 1],15:C2}";
                string col3 = $"{at[i, 2],15:C2}";
                string col4 = $"{at[i, 3],15:C2}";
                Console.WriteLine($"{col0, 7} {col1} {col2} {col3} {col4}");
            }

            Console.WriteLine("    ...");

            for(int i = at.GetLength(0) - 5; i < at.GetLength(0); i++) // prints out last 5 payments
            {
                    int col0 = i;
                    string col1 = $"{at[i, 0],15:C2}";
                    string col2 = $"{at[i, 1],15:C2}";
                    string col3 = $"{at[i, 2],15:C2}";
                    string col4 = $"{at[i, 3],15:C2}";
                    Console.WriteLine($"{col0,7} {col1} {col2} {col3} {col4}");
            }

            double totalPaid = myLoan.TotalPaid(at);
            double originalRepaymentAmount = monthlyPayment * numberOfMonths;
            double interestSaved = originalRepaymentAmount - totalPaid;

            Console.WriteLine($"You will pay a total of {totalPaid:C2}");
            Console.WriteLine($"The original repayment amount was {originalRepaymentAmount:C2}");
            Console.WriteLine($"By paying extra, you saved a total of {interestSaved:C2} in interest");
        }
    }
}
