using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalDataLayer.Settings;

namespace CarRentalBusinessLayer
{
    public class CustomerBusiness : BusinessLayerInterfaces.ICustomerBusiness
    {
        private readonly DataLayerInterfaces.ICustomerData _CustomerData;
        public enum enMode { AddNewMode = 1, UpdateMode = 2 }

        public enMode Mode;

        public DTO.CustomerDTO Customer { get; set; }
        public CustomerBusiness(DataLayerInterfaces.ICustomerData CustomerData) 
        {
            _CustomerData = CustomerData;
            Customer = new DTO.CustomerDTO();
        
        }

        public void Initialize(DTO.CustomerDTO Customer, enMode Mode = enMode.AddNewMode)
        {
            this.Customer = Customer;
            this.Mode = Mode;
        }
        private bool _AddNewCusomer()
        {
            // Hsh the password
            Customer.customerInfo.PassWord = HashPassword(Customer.customerInfo.PassWord);

            // Add the customer
            Customer.CustomerId = _CustomerData.AddNewCustomer(this.Customer.customerInfo);

            if (Customer.CustomerId != -1)
            {
                return true;
            }
            else 
                return false;
           
        }

        private bool _UpdateCusomer()
        {
            if (_CustomerData.UpdateCustomer(this.Customer))
            {
                return true ;   
            }
            else
                return false ;
        }

        public  bool UpdateCustomerPasswordByid(string NewPassword,int customerid)
        {
            NewPassword = HashPassword(NewPassword);
            return _CustomerData.UpdateCustomerPasswordByid(NewPassword,customerid); 
        }

        public DTO.CustomerDTO GetCustomerInfoById(int customerid)
        {
            return _CustomerData.GetCustomerInfoById(customerid);
        }
        public bool CheckEmailExists(string Email)
        {
            return _CustomerData.CheckEmailExists(Email);
        }


        public DTO.CustomerDTO GetCustomerByEmailAndPassword(string Gamil,string password)
        {

            DTO.CustomerDTO Customer =  _CustomerData.GetCustomerInfoByEmail(Gamil);
            if (Customer != null)
            {
                if (VerifyPassword(password,Customer.customerInfo.PassWord))
                {
                    return Customer; 
                }
            
            }
            return null;

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewCusomer())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateCusomer())
                    {
                        return true;
                    }
                    else { return false ; }
                default:
                    return false;
            }
        }

        private  string HashPassword(string password)
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
