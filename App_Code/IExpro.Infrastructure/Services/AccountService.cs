
using IExpro.Core.Interfaces.Service;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Models.Account;
using IExpro.Infrastructure.Repository;

namespace IExpro.Infrastructure.Services
{
    public class AccountService : IAccountService
    {


        private IUnitOfWork IU { get; set; }
        private IAccountRepository AccountRepo { get; set; }


        public AccountService(IUnitOfWork _IU)
        {
            this.IU = _IU;
            this.AccountRepo = IU.AccontRepo;

        }
        public AccountService()
        {
            this.IU = new UnitOfWork();
            this.AccountRepo = IU.AccontRepo;

        }
        public AccountUser LoginUser(string email, string password)
        {
            AccountUser model = this.AccountRepo.AuthenticateUser(email, password);
            return model;
        }






    }
}
