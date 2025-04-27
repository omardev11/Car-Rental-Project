using CarRentalDataLayer.Settings;
using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;

namespace CarRentalBusinessLayer
{
    public class UserBusiness : IUserBusiness
    { 
        private readonly IUserData _UserData;
        public enum enMode { AddNewMode = 1, UpdateMode = 2 }

        public enMode Mode;

        public DTO.UserDTO _UserInfo { get; set; }
        public UserBusiness(IUserData UserData)
        {
            _UserData = UserData;
            _UserInfo = new DTO.UserDTO();

        }

        public void Initialize(DTO.UserDTO _UserInfo, enMode Mode = enMode.AddNewMode)
        {
            this._UserInfo = _UserInfo;
            this.Mode = Mode;
        }
        private bool _AddNewUser()
        {

            // Hsh the password
            _UserInfo.Password = HashPassword(_UserInfo.Password);

            // Add the _UserInfo
            _UserInfo.UserId = _UserData.AddNewUser(this._UserInfo);

            return _UserInfo.UserId.HasValue;
          
        }

        private bool _UpdateUser()
        {
            return _UserData.UpdateUser(_UserInfo);        
        }

        public bool IsThisUserCanManageUsers(int userId)
        {
            return _UserData.IsThisUserCanManageUsers(userId);  
        }
        public DTO.UserDTO GetUserInfoByUserNameAndPassword(string UserName,string Password)
        {
            var user = _UserData.GetUserInfoByUserName(UserName);
           

            if (user != null)
            {
                return VerifyPassword(Password, user.Password) ? user : null;
            }
            return null;
        }

        public List<DTO.UserDTO> GetAllUsersBy(DTO.UserDTO User)
        {
            return _UserData.GetAllUsersBy(User);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewUser())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateUser())
                    {
                        return true;
                    }
                    else { return false; }
                default:
                    return false;
            }
        }

        private string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[16];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash the password using Argon2id
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = 8; // Number of threads
                argon2.MemorySize = 65536; // 64 MB memory usage
                argon2.Iterations = 4; // Number of iterations

                // Generate the hash
                byte[] hash = argon2.GetBytes(32); // Output hash size in bytes

                // Combine the salt and hash for storage
                string saltBase64 = Convert.ToBase64String(salt);
                string hashBase64 = Convert.ToBase64String(hash);

                return $"{saltBase64}:{hashBase64}"; // Format: "salt:hash"
            }
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            // Split the stored hash into salt and hash
            string[] parts = storedHash.Split(':');
            if (parts.Length != 2) return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            string originalHash = parts[1];

            // Hash the input password with the same salt
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = 8;
                argon2.MemorySize = 65536;
                argon2.Iterations = 4;

                byte[] hash = argon2.GetBytes(32);
                string hashBase64 = Convert.ToBase64String(hash);

                // Compare the hashes
                return hashBase64 == originalHash;
            }
        }
    }
}
