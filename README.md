# Loan-Calculator #

Group project: team of 3.

This project is a console application that accepts at four command line arguments, in this order: 
 (1) the original loan amount 
 (2) the loan’s annual interest rate
 (3) the number of months in the loan term
 (4) an extra monthly payment to apply to principal (this last parameter is optional - so if the user omits the 4th command line argument, we assume there is no additional principal included in the monthly payment)

Most loans are payed monthly, so the project automatically assumes that the loan term is specified in number of months and that there will be a payment made each month. 

The program also creates an amortization table that includes every monthly payment.

