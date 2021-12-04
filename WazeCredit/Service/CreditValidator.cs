using System.Collections.Generic;
using System.Threading.Tasks;
using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditValidator : ICreditValidator
    {
        private readonly IEnumerable<IValidationChecker> _validators;
        public CreditValidator(IEnumerable<IValidationChecker> validations)
        {
            _validators = validations;
        }


        public async Task<(bool, IEnumerable<string>)> PassAllValidator(CreditApplication creditApplication)
        {
            bool validationsPassed = true;

            List<string> errorMessages = new List<string>();

            foreach (var validator in _validators)
            {
                if (!validator.ValidatorLogic(creditApplication))
                {
                    errorMessages.Add(validator.Message);
                    validationsPassed = false;
                }
            }
            return (validationsPassed, errorMessages);
        }
    }
}
