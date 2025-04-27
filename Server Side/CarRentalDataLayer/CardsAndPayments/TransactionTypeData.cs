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
    public class TransactionTypeData : ITransactionTypeData
    {
        public int? AddNewTransactionType(DTO.TransactionTypeDTO transactionType)
        {
            int? newId = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewTransactionType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TransactionType", transactionType.transactionTypeInfo.TransactionType);

                    SqlParameter outputIdParam = new SqlParameter("@TransactionTypeId", SqlDbType.Int)
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

        public bool UpdateTransactionType(DTO.TransactionTypeDTO transactionType)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateTransactionType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TransactionType", transactionType.transactionTypeInfo.TransactionType);
                    command.Parameters.AddWithValue("@TransactionTypeId", transactionType.TransactionTypeId);

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
                    }
                }
            }
            return isUpdated;
        }

        public bool DeleteTransactionType(int transactionTypeId)
        {
            bool isDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteTransactionType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TransactionTypeId", transactionTypeId);

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
                    }
                }
            }
            return isDeleted;
        }

        public DTO.TransactionTypeDTO GetTransactionTypeBy(int? transactionTypeId, string typeName)
        {
            DTO.TransactionTypeDTO transactionType = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetTransactionTypeInfoByIdOrName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (transactionTypeId.HasValue)
                    {
                        command.Parameters.AddWithValue("@transactionTypeid", transactionTypeId);
                    }
                    else if (!string.IsNullOrEmpty(typeName))
                    {
                        command.Parameters.AddWithValue("@transactionTypeName", typeName);
                    }

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                transactionType = new DTO.TransactionTypeDTO(
                                    Convert.ToInt32(reader["TransactionTypeId"]),
                                    new DTO.TransactionTypeDTO.TransactionTypeInfo(reader["TransactionTypeName"].ToString())
                                );
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return transactionType;
        }

        public List<DTO.TransactionTypeDTO> GetAllTransactionTypes()
        {
            List<DTO.TransactionTypeDTO> transactionTypes = new List<DTO.TransactionTypeDTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM TransactionType", connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactionTypes.Add(new DTO.TransactionTypeDTO(
                                    Convert.ToInt32(reader["TransactionTypeId"]),
                                    new DTO.TransactionTypeDTO.TransactionTypeInfo(reader["TransactionTypeName"].ToString())
                                ));
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return transactionTypes;
        }
    }
}
