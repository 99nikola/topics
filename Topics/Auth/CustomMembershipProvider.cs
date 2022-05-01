using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using Topics.Repository.Models.Account;
using Topics.Repository.Models.DB;
using Topics.Services.Implementations;
using Topics.Services.Interfaces;

namespace Topics.Authentication
{
    public class CustomMembershipProvider : MembershipProvider
    {
        private IUserService userService;

        public CustomMembershipProvider() 
        {
            this.userService = new UserService();
        }

        public override bool ValidateUser(string username, string password)
        {
            Debug.WriteLine("Validating user " + username);
            return userService.ValidateUser(username, password);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            UserModel user = userService.GetUser(username);
            if (user == null)
                return null;
            return new CustomMembershipUser(user);
        }

        public override string GetUserNameByEmail(string email)
        {
            return userService.GetUsernameByEmail(email);
        }


        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            bool created = userService.CreateUser(new SignUpViewModel() 
            { 
                Username = username,
                Password = password,
                Email = email
            });

            if (created)
                status = MembershipCreateStatus.Success;
            else
                status = MembershipCreateStatus.UserRejected;

            MembershipUser user = new CustomMembershipUser(new UserModel() 
            { 
                Username = username,
                HashedPassword = password,
                Email = email
            });
            status = MembershipCreateStatus.Success;
            return user;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => throw new NotImplementedException();

        public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
    }
}