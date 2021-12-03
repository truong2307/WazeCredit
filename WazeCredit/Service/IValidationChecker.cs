using WazeCredit.Models;

namespace WazeCredit.Service
{
    public interface IValidationChecker
    {
        bool ValidatorLogic(CreditApplication creditApplication);
        string Message { get; }
    }
}
