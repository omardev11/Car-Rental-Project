using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;

namespace CarRentalDataLayer.Vehicle
{
    public class VehicleReturnLogData : IVehicleReturnLogData
    {
        public int? AddNewVehicleReturnLog(DTO.VehicleReturnLog vehicleReturnLog)
        {
            int? newId = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewVehicleReturnLog", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Set parameters based on your DTO properties.
                    command.Parameters.AddWithValue("@BookingId", vehicleReturnLog.BookingId);
                    command.Parameters.AddWithValue("@ActualReturnDate", vehicleReturnLog.ActualReturnDate);
                    command.Parameters.AddWithValue("@FinalCheckNotes",vehicleReturnLog.FinalCheckNotes);

                    // Output parameter for new ReturnId.
                    SqlParameter outputIdParam = new SqlParameter("@returnid", SqlDbType.Int)
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

        public bool UpdateVehicleReturnLog(DTO.VehicleReturnLog vehicleReturnLog)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateVehicleReturnLog", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@ReturnId", SqlDbType.Int).Value = vehicleReturnLog.ReturnId;
                   
                    command.Parameters.Add("@BookingId", SqlDbType.Int).Value = vehicleReturnLog.BookingId ?? (object)DBNull.Value;
                    command.Parameters.Add("@ActualReturnDate", SqlDbType.Date).Value = vehicleReturnLog.ActualReturnDate ?? (object)DBNull.Value;
                    command.Parameters.Add("@FinalCheckNotes", SqlDbType.NVarChar ,50).Value = vehicleReturnLog.FinalCheckNotes ?? (object)DBNull.Value;


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


        public List<DTO.VehicleReturnLog> GetAllVehicleReturnLogsBy(DTO.VehicleReturnLog vehiclereturnLog)
        {
            List<DTO.VehicleReturnLog> logs = new List<DTO.VehicleReturnLog>();

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetVehicleReturnLogInfoBy", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (vehiclereturnLog.ReturnId.HasValue)
                    {
                        command.Parameters.Add("@returnid", SqlDbType.Int).Value = vehiclereturnLog.ReturnId;
                    }
                    else
                    {
                        command.Parameters.Add("@bookingid", SqlDbType.Int).Value = vehiclereturnLog.BookingId ?? (object)DBNull.Value;
                        command.Parameters.Add("@ActualReturnDate", SqlDbType.Date).Value = vehiclereturnLog.ActualReturnDate ?? (object)DBNull.Value;
                        command.Parameters.Add("@ActualRentalDays", SqlDbType.Int).Value = vehiclereturnLog.ActualRentalDays ?? (object)DBNull.Value;
                        command.Parameters.Add("@ActualTotalAmount", SqlDbType.Int).Value = vehiclereturnLog.ActualTotalAmount ?? (object)DBNull.Value;

                    }


                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new DTO.VehicleReturnLog
                                {
                                    ReturnId = Convert.ToInt32(reader["ReturnId"]),
                                    BookingId = Convert.ToInt32(reader["BookingId"]),
                                    ActualReturnDate = reader["ActualReturnDate"] as DateTime?,
                                    ActualRentalDays = reader["ActualRentalDays"] as int?,
                                    FinalCheckNotes = reader["FinalVehicleCheckNotes"].ToString(),
                                    ActualTotalAmount = reader["ActualTotalAmount"] as decimal?
                                });
                            }
                        }
                    }
                    catch (Exception)
                    {
                        logs = null;
                    }
                }
            }
            return logs;
        }
    }
}
