using System;
using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditValidationChecker : IValidationChecker
    {
        public string Message => "You did not meet Age/Salary/Credit requiurements";

        public bool ValidatorLogic(CreditApplication creditApplication)
        {
            if (DateTime.Now.AddYears(-18) < creditApplication.DOB)
            {
                return false;
            }
            if (creditApplication.Salary < 10000)
            {
                return false;
            }

            return true;
        }
    }
}
