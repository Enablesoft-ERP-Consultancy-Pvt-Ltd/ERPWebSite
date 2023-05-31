using IExpro.Core.Models.Account;


namespace IExpro.Core.Interfaces.Service
{
    public interface IAccountService
    {
        AccountUser LoginUser(string _email, string _password);
      
    }
}


 