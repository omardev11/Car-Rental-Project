using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CarRentalDataLayer.Booking
{
    public class BookingVehicleData : DataLayerInterfaces.IBookingVehicleData
    {
        public int? AddNewBookingVehicle(DTO.AddingBookingInfoDTO bookinginfo)
        {
            int? NewId = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewBookingVehicle", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomerId", bookinginfo.UserViewBookingInfo.CustomerId);
                    command.Parameters.AddWithValue("@VehicleId", bookinginfo.UserViewBookingInfo.VehicleId);
                    command.Parameters.AddWithValue("@RentalStartDate", bookinginfo.BookingInfoFromCustomer.RentalStartDate);
                    command.Parameters.AddWithValue("@RentalEndDate", bookinginfo.BookingInfoFromCustomer.RentalEndDate);
                    command.Parameters.AddWithValue("@PickupLocation", bookinginfo.BookingInfoFromCustomer.PickupLocation);
                    command.Parameters.AddWithValue("@DrpOffLocation", bookinginfo.BookingInfoFromCustomer.DropOfLocation);


                    SqlParameter outputIdParam = new SqlParameter("@bookingid", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output  // Set direction to Output
                    };
                    command.Parameters.Add(outputIdParam);

                    try
                    {
                        connection.Open();

                        object Result = command.ExecuteNonQuery();

                        if (outputIdParam.Value != DBNull.Value)
                        {
                            NewId = (int)outputIdParam.Value;
                        }
                        else { NewId = null; }
                    }
                    catch (Exception)
                    { }
                }




            }

            return NewId;

        }
        public bool UpdateBookingVehicleInfoFromCustomer(DTO.BookingInfoFromCustomer bookinginfo,int bookingId)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateBookingVehicle", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    command.Parameters.Add("@RentalStartDate", SqlDbType.DateTime).Value = bookinginfo.RentalStartDate ?? (object)DBNull.Value;
                    command.Parameters.Add("@RentalEndDate", SqlDbType.DateTime).Value = bookinginfo.RentalEndDate ?? (object)DBNull.Value;
                    command.Parameters.Add("@PickupLocation", SqlDbType.NVarChar, 90).Value = bookinginfo.PickupLocation ?? (object)DBNull.Value;
                    command.Parameters.Add("@DropOffLocation", SqlDbType.NVarChar, 90).Value = bookinginfo.DropOfLocation ?? (object)DBNull.Value;
                  


                    command.Parameters.Add("@BookingId", SqlDbType.Int).Value = bookingId;



                    try
                    {
                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Result != null && int.TryParse(Result.ToString(), out int updatedStatus))
                        {
                            if (updatedStatus == 1)
                            {
                                Isupdated = true;
                            }
                            if (updatedStatus == 0)
                            {
                                Isupdated = false;
                            }
                        }
                        else { Isupdated = false; }
                    }
                    catch (Exception)
                    { }
                }

            }

            return Isupdated;

        }
        public bool UpdateBookingVehicleInfoFromUserOrAddingInitialCheckNotes(DTO.UserViewBookingInfo bookinginfo)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateBookingVehicle", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    command.Parameters.Add("@InitialCheckNotes", SqlDbType.NVarChar).Value = bookinginfo.InitialCheckNotes ?? (object)DBNull.Value;
                    command.Parameters.Add("@BookingStatusId", SqlDbType.Int).Value = bookinginfo.BookingStatusId ?? (object)DBNull.Value;
                   


                    command.Parameters.Add("@BookingId", SqlDbType.Int).Value = bookinginfo.BookingId;



                    try
                    {
                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Result != null && int.TryParse(Result.ToString(), out int updatedStatus))
                        {
                            if (updatedStatus == 1)
                            {
                                Isupdated = true;
                            }
                            if (updatedStatus == 0)
                            {
                                Isupdated = false;
                            }
                        }
                        else { Isupdated = false; }
                    }
                    catch (Exception)
                    { }
                }

            }

            return Isupdated;

        }
        public List<DTO.MiniInfoOfBookingDTO> GetAllBookingVehiclesForACustomer(int CustomerId)
        {
            List<DTO.MiniInfoOfBookingDTO> AllBooking = new List<DTO.MiniInfoOfBookingDTO>();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetBookingVehicleInfoByBookingIdOrCustomerId", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@customerid", SqlDbType.Int).Value = CustomerId;


                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                           
                            while (reader.Read())
                            {
                              
                                DTO.MiniInfoOfBookingDTO UserSideInfo = new DTO.MiniInfoOfBookingDTO();
                                UserSideInfo.SendingMiniBookingInfo.BookingId = Convert.ToInt32(reader["BookingId"]);
                                UserSideInfo.VehicleId = Convert.ToInt32(reader["VehicleId"]);
                                UserSideInfo.BookingStatusId = Convert.ToInt32(reader["BookingStatusId"]);

                                AllBooking.Add(UserSideInfo);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        AllBooking = new List<DTO.MiniInfoOfBookingDTO>();
                    }
                }

            }

            return AllBooking;

        }
        public List<DTO.MiniInfoOfBookingDTO> GetAllBookingWaitingApproval()
        {
            List<DTO.MiniInfoOfBookingDTO> AllBooking = new List<DTO.MiniInfoOfBookingDTO>();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetAllBookingWaitingApproval ", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;



                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                           
                            while (reader.Read())
                            {
                                DTO.MiniInfoOfBookingDTO UserSideInfo = new DTO.MiniInfoOfBookingDTO();
                                UserSideInfo.SendingMiniBookingInfo.BookingId = Convert.ToInt32(reader["BookingId"]);
                                UserSideInfo.VehicleId = Convert.ToInt32(reader["VehicleId"]);
                                UserSideInfo.BookingStatusId = Convert.ToInt32(reader["BookingStatusId"]);

                                AllBooking.Add(UserSideInfo);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        AllBooking = new List<DTO.MiniInfoOfBookingDTO>();
                    }
                }

            }

            return AllBooking;

        }

        public DTO.GettingBookingInfoDTO GetBookingVehicleByBookingId(int bookingid)
        {
            DTO.GettingBookingInfoDTO bookinginfo = new DTO.GettingBookingInfoDTO();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetBookingVehicleInfoByBookingIdOrCustomerId", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("@bookingid", SqlDbType.Int).Value = bookingid;


                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                throw new Exception("There is no Rows ");
                            }
                            while (reader.Read())
                            {
                                DTO.CustomerViewBookingInfo customerSideInfo = new DTO.CustomerViewBookingInfo();
                                customerSideInfo.InitialRentalDays = Convert.ToInt32(reader["InitialRentalDays"]);
                                customerSideInfo.InitialTotalAmount = Convert.ToInt32(reader["InitialTotalAmount"]);

                                customerSideInfo.BookingInfoFromCustomer.RentalStartDate = Convert.ToDateTime(reader["RentalStartDate"]);
                                customerSideInfo.BookingInfoFromCustomer.RentalEndDate = Convert.ToDateTime(reader["RentalEndDate"]);
                                customerSideInfo.BookingInfoFromCustomer.PickupLocation = reader["PickUpLocation"].ToString();
                                customerSideInfo.BookingInfoFromCustomer.DropOfLocation = reader["DropOffLocation"].ToString();

                                DTO.UserViewBookingInfo UserSideInfo = new DTO.UserViewBookingInfo();
                                UserSideInfo.BookingId = Convert.ToInt32(reader["BookingId"]);
                                UserSideInfo.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                                UserSideInfo.VehicleId = Convert.ToInt32(reader["VehicleId"]);
                                UserSideInfo.BookingStatusId = Convert.ToInt32(reader["BookingStatusId"]);
                                UserSideInfo.InitialCheckNotes = reader["InitialCheckNotes"].ToString();

                                bookinginfo = new DTO.GettingBookingInfoDTO(customerSideInfo, UserSideInfo);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        bookinginfo = new DTO.GettingBookingInfoDTO();
                    }
                }

            }

            return bookinginfo;

        }
    }
}
