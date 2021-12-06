using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditApprovedLow : ICreditApproved
    {
        public double GetCreditApproved(CreditApplication creditApplication)
        {
            //Have a difference logic to caculate approval limit
            //We will hardcore to 50% of salary
            return creditApplication.Salary * .5;
        }
    }
}
