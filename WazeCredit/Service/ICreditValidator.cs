using System.Collections.Generic;
using System.Threading.Tasks;
using WazeCredit.Models;

namespace WazeCredit.Service
{
    public interface ICreditValidator
    {
        Task<(bool, IEnumerable<string>)> PassAllValidator(CreditApplication creditApplication);
    }
}
