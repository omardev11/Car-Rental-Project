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
    public class PaymentMethodData : IPaymentMethodData
    {
        public int? AddNewPaymentMethod(DTO.PaymentMethodDTO paymentMethod)
        {
            int? newId = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewPaymentMethod", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@paymentmethod", paymentMethod.paymentMethodInfo.PaymentMethod);

                    SqlParameter outputIdParam = new SqlParameter("@PaymentMethodId", SqlDbType.Int)
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
                        // Handle exception
                    }
                }
            }
            return newId;
        }

        public bool UpdatePaymentMethod(DTO.PaymentMethodDTO paymentMethod)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdatePaymentMethod", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@PaymentMethodId", SqlDbType.Int).Value = 
                                                                paymentMethod.PaymentMethodId;
                    command.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar , 50).Value =
                                                     paymentMethod.paymentMethodInfo.PaymentMethod;

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

        public bool DeletePaymentMethod(int paymentMethodId)
        {
            bool isDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeletePaymentMethod", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PaymentMethodId", paymentMethodId);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int DeletedStatus))
                        {
                            isDeleted = DeletedStatus == 1;
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

        public DTO.PaymentMethodDTO GetPaymentMethodByIdOrName(int? VehicleCategoryId, string? CategoryName)
        {
            DTO.PaymentMethodDTO PaymentMethod = new DTO.PaymentMethodDTO();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetPaymentMethodsInfoByIdOrName", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (VehicleCategoryId.HasValue)
                        command.Parameters.AddWithValue("@paymentmethodId", VehicleCategoryId);
                    else if (CategoryName != "")
                        command.Parameters.AddWithValue("@paymentmethodName", CategoryName);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PaymentMethod.PaymentMethodId = Convert.ToInt32(reader["PaymentMethodId"]);
                                PaymentMethod.paymentMethodInfo.PaymentMethod = reader["PaymentMethodName"].ToString();
                            }
                            else { PaymentMethod = null; }
                        }
                    }
                    catch (Exception)
                    {
                        PaymentMethod = null;
                    }
                }

            }

            return PaymentMethod;
        }

        public List<DTO.PaymentMethodDTO> GetAllPaymentMethods()
        {
            List<DTO.PaymentMethodDTO> paymentMethods = new List<DTO.PaymentMethodDTO>();

            string query = "SELECT * FROM PaymentMethods";
            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                paymentMethods.Add(new DTO.PaymentMethodDTO(
                                    Convert.ToInt32(reader["PaymentMethodId"]),
                                    new DTO.PaymentMethodDTO.PaymentMethodInfo(reader["PaymentMethodName"].ToString())
                                ));
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return paymentMethods;
        }
    }
}
