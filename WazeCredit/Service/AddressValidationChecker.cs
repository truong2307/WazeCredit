using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class AddressValidationChecker : IValidationChecker
    {
        public string Message => "Location validation failed";

        public bool ValidatorLogic(CreditApplication creditApplication)
        {
            if (creditApplication.PostalCode <=0 || creditApplication.PostalCode >= 99999)
            {
                return false;
            }

            return true;
        }
    }
}
