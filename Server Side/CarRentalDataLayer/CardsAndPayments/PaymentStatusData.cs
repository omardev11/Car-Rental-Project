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
    public class PaymentStatusData : IPaymentStatusData
    {
        public int? AddNewPaymentStatus(DTO.PaymentStatusDTO paymentStatus)
        {
            int? newId = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewPaymentStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Pass the PaymentStatus (from the inner class) as parameter
                    command.Parameters.AddWithValue("@PaymentStatus", paymentStatus.paymentStatusInfo.PaymentStatus);

                    // Output parameter for new PaymentStatusId
                    SqlParameter outputIdParam = new SqlParameter("@PaymentStatusId", SqlDbType.Int)
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

        public bool UpdatePaymentStatus(DTO.PaymentStatusDTO paymentStatus)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdatePaymentStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Status", paymentStatus.paymentStatusInfo.PaymentStatus);
                    command.Parameters.AddWithValue("@StatusId", paymentStatus.PaymentStatusId);

                    try
                    {
                        connection.Open();

                        // Assume the stored procedure returns 1 for success, 0 otherwise.
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

        public bool DeletePaymentStatus(int paymentStatusId)
        {
            bool isDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeletePaymentStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@paymentstatusId", paymentStatusId);

                    try
                    {
                        connection.Open();

                        // Assume the stored procedure returns 1 for success, 0 otherwise.
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int deletedStatus))
                        {
                            isDeleted = deletedStatus == 1;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return isDeleted;
        }

        public DTO.PaymentStatusDTO GetPaymentStatusBy(int? paymentStatusId, string statusName)
        {
            DTO.PaymentStatusDTO paymentStatus = new DTO.PaymentStatusDTO();

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetPaymentStatusInfoByIdOrName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (paymentStatusId.HasValue)
                    {
                        command.Parameters.AddWithValue("@PaymentStatusId", paymentStatusId);
                    }
                    else if (!string.IsNullOrEmpty(statusName))
                    {
                        command.Parameters.AddWithValue("@PaymentStatusName", statusName);
                    }

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                paymentStatus.PaymentStatusId = Convert.ToInt32(reader["PaymentStatusId"]);
                                paymentStatus.paymentStatusInfo.PaymentStatus = reader["PaymentStatusName"].ToString();
                            }
                            else
                            {
                                paymentStatus = null;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        paymentStatus = null;
                    }
                }
            }

            return paymentStatus;
        }

        public List<DTO.PaymentStatusDTO> GetAllPaymentStatuses()
        {
            List<DTO.PaymentStatusDTO> statuses = new List<DTO.PaymentStatusDTO>();

            string query = "SELECT * FROM PaymentStatus";
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
                                statuses.Add(new DTO.PaymentStatusDTO(
                                    Convert.ToInt32(reader["PaymentStatusId"]),
                                    new DTO.PaymentStatusDTO.PaymentStatusInfo(reader["PaymentStatusName"].ToString())
                                ));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        statuses = null;
                    }
                }
            }

            return statuses;
        }
    }
}
