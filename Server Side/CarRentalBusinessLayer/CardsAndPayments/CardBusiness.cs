using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using System.Diagnostics;
using KeePassNetStandard;
using KeePassLib;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using KeePassLib.Security;


namespace CarRentalBusinessLayer.CardsAndPayments
{
    public class CardBusiness : ICardBusiness
    {
        private readonly ICardData _CardData;
        private PwDatabase database = new PwDatabase();

        private string _databasePath = "";
        private readonly string _MasterPassword = "";
        public enum enMode { AddNewMode = 1, UpdateMode = 2 }

        public enMode Mode; 
        public DTO.CardDTO _CardInfo { get; set; }

        public CardBusiness(ICardData carddata,IConfiguration configuration)
        {
            _CardData = carddata;
            _CardInfo = new DTO.CardDTO();
            _databasePath = configuration["KeePassConfig:DatabasePath"];
            _MasterPassword = Environment.GetEnvironmentVariable("MASTER_PASSWORD");
        }

        public void Initialize(DTO.CardDTO cardinfo, enMode mode = enMode.AddNewMode)
        {
            _CardInfo = cardinfo;
            Mode = mode;
        }

        private bool _AddNewCard()
        {
            string _GeneratedKey = GenerateRandomKey(16);

            _CardInfo.CardExpirationDate = Encrypt(_CardInfo.CardExpirationDate, _GeneratedKey);
            _CardInfo.CardNumber = Encrypt(_CardInfo.CardNumber, _GeneratedKey);
            _CardInfo.CardHolderName = Encrypt(_CardInfo.CardHolderName, _GeneratedKey);

            int? result = _CardData.AddNewCard(_CardInfo);
            if (result.HasValue)
            {
                _CardInfo.CardId = result.Value;
                _StoringKeyProcess(_GeneratedKey);
                return true;

            }
            else 
                return false;
        }
    
        //  Storing Keys Operations
        private void _StoringKeyProcess(string generatedKey)
        {
          
     
            LoadDatabase(_databasePath, _MasterPassword);

            StoreKey(_CardInfo.CardId.ToString(), generatedKey);

            CloseDatabase();

        }

        // Retrieve Key Operations
        private string _RetrievingKeyProcess(int cardid)
        {
            LoadDatabase(_databasePath, _MasterPassword);

           string key =  RetrieveKey(cardid.ToString());

            CloseDatabase();
            return key;
        }


        private bool _UpdateCard()
        {
            string key = _RetrievingKeyProcess(_CardInfo.CardId);

            if (!string.IsNullOrEmpty(_CardInfo.CardNumber))
            {
                _CardInfo.CardNumber = Encrypt(_CardInfo.CardNumber,key);
            }
            if (!string.IsNullOrEmpty(_CardInfo.CardExpirationDate))
            {
                _CardInfo.CardExpirationDate = Encrypt(_CardInfo.CardExpirationDate, key);
            }
            if (!string.IsNullOrEmpty(_CardInfo.CardHolderName))
            {
                _CardInfo.CardHolderName = Encrypt(_CardInfo.CardHolderName, key);
            }

            return _CardData.UpdateCard(_CardInfo);
        }

        public bool DeleteCardInfoById(int cardid)
        {
            LoadDatabase(_databasePath,_MasterPassword);
            return _CardData.DeleteCard(cardid) ? DeleteKey(cardid.ToString()) ? true : false : false;
        }

        public DTO.CardDTO GetCardInfoById(int CardId)
        {                  
            var carinfo = _CardData.GetCardById(CardId);
            if (carinfo != null)
            {
                string key = _RetrievingKeyProcess(CardId);

                carinfo.CardExpirationDate = Decrypt(carinfo.CardExpirationDate, key);
                carinfo.CardNumber = Decrypt(carinfo.CardNumber, key);
                carinfo.CardHolderName = Decrypt(carinfo.CardHolderName, key);
            }
            return carinfo;
        }
        public List<DTO.CardDTO> GetAllCardsForAcustomer(int customerid)
        {
            var result = _CardData.GetAllCardsForACustomer(customerid);

            foreach (var card in result)
            {
                string key = _RetrievingKeyProcess(card.CardId);

                card.CardExpirationDate = Decrypt(card.CardExpirationDate, key);
                card.CardNumber = Decrypt(card.CardNumber, key);
                card.CardHolderName = Decrypt(card.CardHolderName, key);
            }
            return result;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewCard())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateCard())
                    {
                        return true;
                    }
                    else { return false; }
                default:
                    return false;
            }
        }


        private static string GenerateRandomKey(int length)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                // Convert random bytes to a Base64 string (or any other encoding of your choice)
                return Convert.ToBase64String(randomBytes).Substring(0, length); // Ensure it's exactly 'length' characters
            }
        }
        private string Encrypt(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Set the key and IV for AES encryption
                aesAlg.Key = Encoding.UTF8.GetBytes(key);

                /*
                Here, you are setting the IV of the AES algorithm to a block of bytes 
                with a size equal to the block size of the algorithm divided by 8. 
                The block size of AES is typically 128 bits (16 bytes), 
                so the IV size is 128 bits / 8 = 16 bytes.
                 */
                aesAlg.IV = new byte[aesAlg.BlockSize / 8];


                // Create an encryptor
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


                // Encrypt the data
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    // Return the encrypted data as a Base64-encoded string
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        private string Decrypt(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Set the key and IV for AES decryption
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[aesAlg.BlockSize / 8];


                // Create a decryptor
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);


                // Decrypt the data
                using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(cipherText)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                {
                    // Read the decrypted data from the StreamReader
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Loads the KeePass database.
        /// </summary>
        /// <param name = "path" > Path to the KeePass database file.</param>
        /// <param name = "password" > Master password for the KeePass database.</param>
        private void LoadDatabase(string path, string password)
        {
            database = KeePassUtilities.OpenPasswordDatabase(path, password);


        }

        /// <summary>
        /// Closes the KeePass database.
        /// </summary>
        private void CloseDatabase()
        {
            if (database != null)
            {
                database.Close();
                database = null;
            }
        }

        /// <summary>
        /// Stores an encryption key in the KeePass database with the given CardID.
        /// </summary>
        /// <param name="cardId">The CardID associated with the key.</param>
        /// <param name="key">The encryption key to store.</param>
        private void StoreKey(string cardId, string key)
        {
            if (database == null) throw new InvalidOperationException("Database is not opened.");

            // Check if an entry with the same title already exists and update it.
            PwEntry existingEntry = FindEntry(cardId);
            if (existingEntry != null)
            {
                existingEntry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, key));
            }
            else
            {
                // Create a new entry.
                PwEntry newEntry = new PwEntry(true, true);
                newEntry.Strings.Set(PwDefs.TitleField, new ProtectedString(false, cardId));
                newEntry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, key));

                // Add to the root group.
                database.RootGroup.AddEntry(newEntry, true);
            }
            SaveDatabase();
        }

        /// <summary>
        /// Saves the KeePass database to the specified path.
        /// </summary>
        private void SaveDatabase()
        {
            if (database != null)
            {
                database.Save(null); // Save to the original location
            }
        }

        /// <summary>
        /// Retrieves an encryption key from the KeePass database using the CardID.
        /// </summary>
        /// <param name="cardId">The CardID to search for.</param>
        /// <returns>The encryption key if found, otherwise null.</returns>
        private string RetrieveKey(string cardId)
        {
            if (database == null) throw new InvalidOperationException("Database is not opened.");

            PwEntry entry = FindEntry(cardId);
            return entry != null ? entry.Strings.ReadSafe(PwDefs.PasswordField) : null;
        }

        /// <summary>
        /// Deletes an encryption key from the KeePass database using the CardID.
        /// </summary>
        /// <param name="cardId">The CardID to search for.</param>
        /// <returns>True if the encryption key Deleted, otherwise False.</returns>
        private bool DeleteKey(string cardId)
        {
            if (database == null)
                throw new InvalidOperationException("Database is not opened.");

            // Get all entries (recursively) with a Title that equals the provided title.
            var entriesToDelete = database.RootGroup.GetEntries(true)
                .Where(e => e.Strings.ReadSafe(PwDefs.TitleField) == cardId)
                .ToList();

            if (entriesToDelete.Count == 0)
            {
                return false;
            }

            // Remove each matching entry from its parent group.
            foreach (var entry in entriesToDelete)
            {
                // Use the ParentGroup property to remove the entry from its actual group.
                if (entry.ParentGroup != null)
                {
                    bool removed = entry.ParentGroup.Entries.Remove(entry);
                    if (!removed)
                    {
                        return false;
                    }
                }
                else
                {
                    // If for some reason the entry doesn't have a parent, try removing it from the root group.
                    bool removed = database.RootGroup.Entries.Remove(entry);
                    if (!removed)
                    {
                        return false;
                    }
                }
            }

            // Save the database (using null to indicate saving to the original storage).
            SaveDatabase();
            CloseDatabase();

            return true;
        }
         
        /// <summary>
        /// Finds an entry by title in the root group.
        /// </summary>
        private PwEntry FindEntry(string title)
        {
            foreach (PwEntry entry in database.RootGroup.Entries)
            {
                if (entry.Strings.ReadSafe(PwDefs.TitleField) == title)
                {
                    return entry;
                }
            }
            return null;
        }
    }
}
