using WazeCredit.Data.Repository.IRepository;
using WazeCredit.Models;

namespace WazeCredit.Data.Repository
{
    public class CreditApplicationRepository : Repository<CreditApplication>, ICreditApplicationRepository
    {
        private readonly ApplicationDbContext _db;

        public CreditApplicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CreditApplication creditApplicationRequest)
        {
            _db.CreditApplications.Update(creditApplicationRequest);
        }
    }
}
