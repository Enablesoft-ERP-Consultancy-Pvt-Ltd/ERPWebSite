using IExpro.Core.Entity;
using IExpro.Core.Models.Account;


namespace IExpro.Core.Interfaces.Repository
{
    public interface IAccountRepository : IGenericRepository<NewUserDetail>
    {
        AccountUser AuthenticateUser(string _email, string _password);
     
    }
}
