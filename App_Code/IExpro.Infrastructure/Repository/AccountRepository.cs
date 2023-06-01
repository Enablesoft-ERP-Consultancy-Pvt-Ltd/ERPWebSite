

using IExpro.Core.Entity;
using IExpro.Core.Interfaces.Common;
using IExpro.Core.Interfaces.Repository;
using IExpro.Core.Models.Account;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;


namespace IExpro.Infrastructure.Repository
{
    public class AccountRepository : GenericRepository<NewUserDetail>, IAccountRepository
    {



        public AccountRepository(IDataContext context)
          : base(context)
        {
        }
        public IExproContext Context
        {
            get { return base.entities as IExproContext; }
        }


        public AccountUser AuthenticateUser(string _email, string _password)
        {

            try
            {

                SqlParameter category = new SqlParameter("@CategoryName", "Test");
                var dd = Context.Database.ExecuteSqlCommand("NewCategory @CategoryName", category);



                var salesPeople = Context.Database.SqlQuery<AccountUser>("AllSalesPeople").ToListAsync();



                var totalOrders = Context.Database.SqlQuery<int>("CountOfOrders @ProductID", category).SingleAsync();



                SqlParameter Id = new SqlParameter("@EmployeeId", 1);

                SqlParameter fname = new SqlParameter("@FirstName", 23);

                SqlParameter lname = new SqlParameter("@LastName", 22);

                int result = Context.Database.ExecuteSqlCommand("UpdateEmployee @EmployeeId, @FirstName, @LastName", Id, fname, lname);





                var user = (from usr in Context.NewUserDetails.Where(u => (u.LoginName.ToUpper() == _email.ToUpper() && u.PassWord == _password))
                            select new AccountUser
                            {
                                UserId = usr.UserId,
                                FirstName = usr.UserName,
                                LastName = usr.UserName,
                                Mobile = usr.Designation,
                                Email = usr.LoginName
                            }).FirstOrDefault();

                if (user != null)
                {
                    return user;
                }
                else
                {
                    return new AccountUser();
                }
            }
            catch (Exception ex)
            {
                return new AccountUser();

            }

        }











        //public AccountUser AuthenticateUser(string _email, string _password)
        //{
        //    var result = from usr in Context.tblOrders.Where(u => (u.usrLoginId.ToUpper() == _email.ToUpper() && u.usrStatus == (int)Status.Active)) select usr;
        //    if (result != null && result.FirstOrDefault().usrPassword == _password)
        //    {
        //        var resultSet = from usr in Context.tblUsers.Where(u => (u.usrLoginId.ToUpper() == _email.ToUpper() && u.usrPassword == _password && u.usrStatus == (int)Status.Active))
        //                        join usrL in Context.tblUserDetail_Locale on usr.usrId equals usrL.usrId
        //                        join usrDt in Context.tblUserDetailSpecifics on usr.usrScopeLevel equals usrDt.usrId
        //                        select new AccountUser
        //                        {
        //                            UserId = usr.usrId,
        //                            UserTypeId = usr.usrtypId,
        //                            ScopeId = usr.usrScopeLevel,
        //                            ParentId = usr.usrParentId.HasValue ? usr.usrParentId.Value : 0,
        //                            CultureId = usrDt.usrDtCulture,
        //                            TimeZone = usrDt.usrDtTimeZone,
        //                            FirstName = usrL.usrDtLclFirstName,
        //                            MiddleName = usrL.usrDtLclMiddleName,
        //                            LastName = usrL.usrDtLclLastName,
        //                            RoleId = usr.usrRoleId,
        //                            lstRight = (from usrRgt in Context.tblUsrRel_Rgt.Where(x => x.usrId == usr.usrId && x.rgtStatus == true)
        //                                        join rgt in Context.tblRightMasters.Where(x => x.rgtStatus == (int)Status.Active)
        //                                        on usrRgt.rgtId equals rgt.rgtId
        //                                        select new AccessRight
        //                                        {
        //                                            RightId = usrRgt.rgtId,
        //                                            RightCode = rgt.rgtCode,
        //                                            ControllerName = rgt.rgtController,
        //                                            ActionName = rgt.rgtAction,
        //                                            Parameter = string.IsNullOrEmpty(rgt.rgtParameter) ? "N/A" : rgt.rgtParameter
        //                                        }).Distinct().ToList(),
        //                        };
        //        return resultSet.FirstOrDefault();

        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public AccountUser AuthenticateUser(string _email, string _password)
        //{
        //    var flag = new SqlParameter();
        //    flag.ParameterName = "@flag";
        //    flag.Direction = ParameterDirection.Input;
        //    flag.SqlDbType = SqlDbType.Int;
        //    flag.Value = 1;

        //    var email = new SqlParameter();
        //    email.ParameterName = "@Email";
        //    email.Direction = ParameterDirection.Input;
        //    email.SqlDbType = SqlDbType.VarChar;
        //    email.Value = _email;

        //    var password = new SqlParameter();
        //    password.ParameterName = "@Password";
        //    password.Direction = ParameterDirection.Input;
        //    password.SqlDbType = SqlDbType.VarChar;
        //    password.Value = _password;

        //    var culture = new SqlParameter();
        //    culture.ParameterName = "@Culture";
        //    culture.Direction = ParameterDirection.Input;
        //    culture.SqlDbType = SqlDbType.Int;
        //    culture.Value = 1;

        //    var command = Context.Database.Connection.CreateCommand();
        //    command.CommandText = "LoginProc";
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.Parameters.Add(flag);
        //    command.Parameters.Add(email);
        //    command.Parameters.Add(password);
        //    command.Parameters.Add(culture);
        //    Context.Database.Connection.Open();
        //    var result = command.ExecuteReader();


        //    AccountUser dbresult = null;
        //    while (result.Read())
        //    {
        //        AccountUser model = new AccountUser();
        //        model.UserId = result["usrId"] == DBNull.Value ? 0 : (long)result["usrId"];
        //        model.UserTypeId = result["usrtypId"] == DBNull.Value ? 0 : (long)result["usrtypId"];
        //        model.ScopeId = result["usrScopeLevel"] == DBNull.Value ? 0 : (long)result["usrScopeLevel"];
        //        model.ParentId = result["usrParentId"] == DBNull.Value ? 0 : (long)result["usrParentId"];
        //        model.CultureId = result["usrDtCulture"] == DBNull.Value ? 0 : (byte)result["usrDtCulture"];
        //        model.TimeZone = result["usrDtTimeZone"] == DBNull.Value ? "" : (string)result["usrDtTimeZone"];
        //        model.FirstName = result["usrDtLclFirstName"] == DBNull.Value ? "" : (string)result["usrDtLclFirstName"];
        //        model.MiddleName = result["usrDtLclMiddleName"] == DBNull.Value ? "" : (string)result["usrDtLclMiddleName"];
        //        model.LastName = result["usrDtLclLastName"] == DBNull.Value ? "" : (string)result["usrDtLclLastName"];
        //        model.RoleId = result["usrRoleId"] == DBNull.Value ? 0 : (long)result["usrRoleId"];
        //        model.ProfilePath = result["usrImage"] == DBNull.Value ? "profile-img.png" : (string)result["usrImage"];
        //        dbresult = model;
        //    }
        //    Context.Database.Connection.Close();
        //    return dbresult;
        //}























    }
}
