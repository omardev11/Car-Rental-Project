using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDataLayer.CardsAndPayments
{
    public class CardData : DataLayerInterfaces.ICardData
    {
        public int? AddNewCard(DTO.CardDTO newCard)
        {
            int? newId = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewCard", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@bankName", newCard.BankName);
                    command.Parameters.AddWithValue("@cardType", newCard.CardType);
                    command.Parameters.AddWithValue("@CardHolderName", newCard.CardHolderName);
                    command.Parameters.AddWithValue("@CardNumber", newCard.CardNumber);
                    command.Parameters.AddWithValue("@cardExpirationDate", newCard.CardExpirationDate);
                    command.Parameters.AddWithValue("@customerid", newCard.CustomerId);

                    SqlParameter outputIdParam = new SqlParameter("@CardId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIdParam);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        if (outputIdParam.Value != DBNull.Value)
                        {
                            newId = (int)outputIdParam.Value;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return newId;
        }
        public bool DeleteCard(int cardId)
        {
            bool isDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteCard", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@cardid", cardId);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int updatedStatus))
                        {
                            isDeleted = updatedStatus == 1;
                        }
                    }
                    catch (Exception)
                    {
                        // Handle exception
                    }
                }
            }

            return isDeleted;
        }
        public bool UpdateCard(DTO.CardDTO updatedCard)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateCard", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@BankName", SqlDbType.NVarChar, 50).Value = updatedCard.BankName ?? (object)DBNull.Value;
                    command.Parameters.Add("@CardType", SqlDbType.NVarChar, 50).Value = updatedCard.CardType ?? (object)DBNull.Value;
                    command.Parameters.Add("@CardHolderName", SqlDbType.NVarChar, 1000).Value = updatedCard.CardHolderName ?? (object)DBNull.Value;
                    command.Parameters.Add("@CardNumber", SqlDbType.NVarChar, 100).Value = updatedCard.CardNumber ?? (object)DBNull.Value;
                    command.Parameters.Add("@CardExpirationDate", SqlDbType.NVarChar, 100).Value = updatedCard.CardExpirationDate ?? (object)DBNull.Value;
                   
                    command.Parameters.Add("@CardId", SqlDbType.Int).Value = updatedCard.CardId;

                
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int updatedStatus))
                        {
                            isUpdated = updatedStatus == 1;
                        }
                    }
                    catch (Exception)
                    {
                        // Handle exception
                    }
                }
            }

            return isUpdated;
        }
        public DTO.CardDTO GetCardById(int cardId)
        {
            DTO.CardDTO card = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetCardInfoBy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CardId", cardId);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                card = new DTO.CardDTO
                                {
                                    CardId = cardId,
                                    BankName = reader["BankName"].ToString(),
                                    CardType = reader["CardType"].ToString(),
                                    CardHolderName = reader["CardHolderName"].ToString(),
                                    CardNumber = reader["CardNumber"].ToString(),
                                    CardExpirationDate = reader["CardExpirationDate"].ToString(),
                                    CustomerId = Convert.ToInt32(reader["CustomerId"])
                                };
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return card;
        }
        public List<DTO.CardDTO> GetAllCardsForACustomer(int customerId)
        {
            List<DTO.CardDTO> cards = new List<DTO.CardDTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetCardInfoBy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@customerid", customerId);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cards.Add(new DTO.CardDTO
                                {
                                    CardId = Convert.ToInt32(reader["CardId"]),
                                    BankName = reader["BankName"].ToString(),
                                    CardType = reader["CardType"].ToString(),
                                    CardHolderName = reader["CardHolderName"].ToString(),
                                    CardNumber = reader["CardNumber"].ToString(),
                                    CardExpirationDate = reader["CardExpirationDate"].ToString(),
                                    CustomerId = Convert.ToInt32(reader["CustomerId"])
                                });
                            }
                        }
                    }
                    catch (Exception)
                    {
                        cards = null;
                    }
                }
            }

            return cards;
        }
    }
}
