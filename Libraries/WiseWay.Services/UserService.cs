using Microsoft.Extensions.Options;
using WiseWay.Core;
using WiseWay.Facade;

namespace WiseWay.Services
{
    public interface IUserService
    {
        User Authenticate(User objuser);
        //public void SaveRecord(User objModel);
        //public void UpdateRecord(User objModel);
        //public User GetUserDetailById(int Id);
        //public void DeleteRecord(int Id);
        //public List<User> GetUsers();
        //public void ResetPassword(User objModel);
        //public User AddUserProfile(User objModel);
        //public string GetSuggestedFriends(bool AllRecord);
        //public UserProfile GetUserProfileDetails(int UserId);
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
    }
}
