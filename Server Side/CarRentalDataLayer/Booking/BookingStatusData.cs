using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;

namespace CarRentalDataLayer.Booking
{
    public class BookingStatusData : IBookingStatusData
    {
        public int? AddNewBookingStatus(DTO.BookingStatusDTO bookingStatus)
        {
            int? newId = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewBookingStatus", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Status", bookingStatus.BookingStatusInfor.Status);

                    SqlParameter outputIdParam = new SqlParameter("@StatusId", SqlDbType.Int)
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
                        newId=null;
                    }
                }
            }

            return newId;
        }

        public bool UpdateBookingStatus(DTO.BookingStatusDTO bookingStatus)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateBookingStatus", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Status", bookingStatus.BookingStatusInfor.Status);
                    command.Parameters.AddWithValue("@StatusId", bookingStatus.BookingStatusId);

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

        public bool DeleteBookingStatus(int bookingStatusId)
        {
            bool isDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteBookingStatus", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@StatusId", bookingStatusId);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int deletedStatus))
                        {
                            isDeleted = deletedStatus == 1;
                        }
                    }
                    catch (Exception)
                    {
                        // Handle exceptions
                    }
                }
            }

            return isDeleted;
        }

        public DTO.BookingStatusDTO GetBookingStatusBy(int? bookingStatusId, string statusName)
        {
            DTO.BookingStatusDTO bookingStatus = new DTO.BookingStatusDTO();

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetBookingStatusBy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (bookingStatusId.HasValue)
                        command.Parameters.AddWithValue("@StatusId", bookingStatusId);
                    if (!string.IsNullOrEmpty(statusName))
                        command.Parameters.AddWithValue("@StatusName", statusName);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bookingStatus.BookingStatusId = Convert.ToInt32(reader["BookingStatusId"]);
                                bookingStatus.BookingStatusInfor.Status = reader["StatusName"].ToString();
                            }
                            else
                            {
                                bookingStatus = null;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        bookingStatus = null;
                    }
                }
            }

            return bookingStatus;
        }

        public List<DTO.BookingStatusDTO> GetAllBookingStatuses()
        {
            List<DTO.BookingStatusDTO> bookingStatuses = new List<DTO.BookingStatusDTO>();

            string query = "SELECT * FROM BookingStatus";
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
                                bookingStatuses.Add(new DTO.BookingStatusDTO(
                                    Convert.ToInt32(reader["BookingStatusId"]),
                                    new DTO.BookingStatusDTO.BookingStatusInfo(reader["StatusName"].ToString())
                                ));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        bookingStatuses = null;
                    }
                }
            }

            return bookingStatuses;
        }
    }
}
