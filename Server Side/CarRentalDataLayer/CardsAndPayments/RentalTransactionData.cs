using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;

namespace CarRentalDataLayer.CardsAndPayments
{
    public class RentalTransactionData : IRentalTransactionData
    {

        public int? AddNewRentalTransaction(DTO.RentalTransactionDTO rentalTransaction)
        {
            int? newId = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewRentalTransaction", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Set parameters based on your DTO properties
                    command.Parameters.AddWithValue("@TransactionTypeId", rentalTransaction.TransactionTypeId);
                    command.Parameters.AddWithValue("@BookingId", rentalTransaction.BookingId);
                    command.Parameters.AddWithValue("@Amount", rentalTransaction.Amount);
                    command.Parameters.AddWithValue("@PaymentMethodId", rentalTransaction.PaymentMethodId);
                    command.Parameters.AddWithValue("@PaymentStatusId", rentalTransaction.PaymentStatusId);

                    // Output parameter for the new TransactionId
                    SqlParameter outputIdParam = new SqlParameter("@TransactonId", SqlDbType.Int)
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

        public bool UpdateRentalTransaction(DTO.RentalTransactionDTO rentalTransaction)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateRentalTransaction", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@TransactionId", SqlDbType.Int).Value = rentalTransaction.TransactionId;
                    command.Parameters.Add("@TransactionTypeId", SqlDbType.Int).Value = rentalTransaction.TransactionTypeId ?? (object)DBNull.Value;
                    command.Parameters.Add("@BookingId", SqlDbType.Int).Value = rentalTransaction.BookingId ?? (object)DBNull.Value;
                    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = rentalTransaction.Amount ?? (object)DBNull.Value;
                    command.Parameters.Add("@PaymentMethodId", SqlDbType.Int).Value = rentalTransaction.PaymentMethodId ?? (object)DBNull.Value;
                    command.Parameters.Add("@PaymentStatusId", SqlDbType.Int).Value = rentalTransaction.PaymentStatusId ?? (object)DBNull.Value;



                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;

                        
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return isUpdated;
        }

        public List<DTO.RentalTransactionDTO> GetAllRentalTransactionsBy(DTO.RentalTransactionDTO rentaltransaction)
        {
            List<DTO.RentalTransactionDTO> transactions = new List<DTO.RentalTransactionDTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetRentalTransactionInfoBy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (rentaltransaction.TransactionId.HasValue)
                    {
                        command.Parameters.Add("@transactionid", SqlDbType.Int).Value = rentaltransaction.TransactionId;
                    }
                    else
                    {
                        command.Parameters.Add("@bookingid", SqlDbType.Int).Value = rentaltransaction.BookingId ?? (object)DBNull.Value;
                        command.Parameters.Add("@TransactionTypeId", SqlDbType.Int).Value = rentaltransaction.TransactionTypeId ?? (object)DBNull.Value;
                        command.Parameters.Add("@TransactioDate", SqlDbType.Date).Value = rentaltransaction.TransactionDate ?? (object)DBNull.Value;
                        command.Parameters.Add("@PaymentMethodId", SqlDbType.Int).Value = rentaltransaction.PaymentMethodId ?? (object)DBNull.Value;
                        command.Parameters.Add("@PaymentStatusId", SqlDbType.Int).Value = rentaltransaction.PaymentStatusId ?? (object)DBNull.Value;
                        command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = rentaltransaction.Amount ?? (object)DBNull.Value;


                    }

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new DTO.RentalTransactionDTO
                                {
                                    TransactionId = Convert.ToInt32(reader["TransactionId"]),
                                    TransactionTypeId = Convert.ToInt32(reader["TransactionTypeId"]),
                                    BookingId = Convert.ToInt32(reader["BookingId"]),
                                    Amount = Convert.ToDecimal(reader["Amount"]),
                                    TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                                    PaymentMethodId = Convert.ToInt32(reader["PaymentMethodId"]),
                                    PaymentStatusId = Convert.ToInt32(reader["PaymentStatusId"])
                                });
                            }
                        }
                    }
                    catch (Exception)
                    {
                        transactions = null;
                    }
                }
            }

            return transactions;
        }
    }


}
