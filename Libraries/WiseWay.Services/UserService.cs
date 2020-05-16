using Microsoft.Extensions.Options;
using WiseWay.Core;
using WiseWay.Facade;

namespace WiseWay.Services
{
    public interface IUserService
    {
        User Authenticate(User objuser);
        public User AddUpdateUser(User objModel);
        public string GetUserList();
        public string DeleteUser(int Id);
        public string ChangeUserStatus(int Id);
    }   
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public User Authenticate(User objuser)
        {
            return UserFacade.Authenticate(objuser, _appSettings.Secret, _appSettings.ExpiryDay);
        }
        public User AddUpdateUser(User objuser)
        {
            return UserFacade.AddUpdateUser(objuser);
        }
        public string GetUserList()
        {
            return UserFacade.GetUserList();
        }
        public string DeleteUser(int Id)
        {
            return UserFacade.DeleteUser(Id);
        }
        public string ChangeUserStatus(int Id)
        {
            return UserFacade.ChangeUserStatus(Id);
        }
    }
}
