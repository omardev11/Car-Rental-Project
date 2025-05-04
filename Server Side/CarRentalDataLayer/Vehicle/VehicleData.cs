using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDataLayer.Vehicle
{
    public class VehicleData : DataLayerInterfaces.IVehicleData
    {
        public int? AddNewVehicle(DTO.UserViewVehicleDTO Vehicle)
        {
            int? NewId = null;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddnewVehicle", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@VehicleName", Vehicle.VehicleInfo.VehicleName);
                    command.Parameters.AddWithValue("@PricePerDay", Vehicle.VehicleInfo.PricePerday);
                    command.Parameters.AddWithValue("@Make", Vehicle.VehicleInfo.Make);
                    command.Parameters.AddWithValue("@Model", Vehicle.VehicleInfo.Model);
                    command.Parameters.AddWithValue("@Year", Vehicle.VehicleInfo.Year);
                    command.Parameters.AddWithValue("@StatusId", Vehicle.VehiclePropertyIdies.VehicleStatusId);
                    command.Parameters.AddWithValue("@LicensePlate", Vehicle.VehicleInfo.LicensePlate);
                    command.Parameters.AddWithValue("@LocationId", Vehicle.VehiclePropertyIdies.LocationId);
                    command.Parameters.AddWithValue("@FuelTypeId", Vehicle.VehiclePropertyIdies.FuelTypeId);
                    command.Parameters.AddWithValue("@VehicleCategoryId", Vehicle.VehiclePropertyIdies.VehicleCategoryId);


                    SqlParameter outputIdParam = new SqlParameter("@VehicleId", SqlDbType.Int)
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

        public bool UpdateVehicle(DTO.UserViewVehicleDTO Vehicle)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateVehicle", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    command.Parameters.Add("@VehicleName", SqlDbType.NVarChar, 50).Value = Vehicle.VehicleInfo.VehicleName ?? (object)DBNull.Value;
                    command.Parameters.Add("@PricePerDay", SqlDbType.Decimal).Value = (object?)Vehicle.VehicleInfo.PricePerday ?? DBNull.Value;
                    command.Parameters.Add("@Make", SqlDbType.VarChar, 50).Value = Vehicle.VehicleInfo.Make ?? (object)DBNull.Value;
                    command.Parameters.Add("@Model", SqlDbType.VarChar, 50).Value = Vehicle.VehicleInfo.Model ?? (object)DBNull.Value;
                    command.Parameters.Add("@Year", SqlDbType.VarChar, 4).Value = Vehicle.VehicleInfo.Year.ToString() ?? (object)DBNull.Value;
                    command.Parameters.Add("@LicensePlate", SqlDbType.VarChar, 50).Value = Vehicle.VehicleInfo.LicensePlate ?? (object)DBNull.Value;
                    command.Parameters.Add("@StatusId", SqlDbType.Int).Value = (object?)Vehicle.VehiclePropertyIdies.VehicleStatusId ?? DBNull.Value;
                    command.Parameters.Add("@LocationId", SqlDbType.Int).Value = (object?)Vehicle.VehiclePropertyIdies.LocationId ?? DBNull.Value;
                    command.Parameters.Add("@FuelTypeId", SqlDbType.Int).Value = (object?)Vehicle.VehiclePropertyIdies.FuelTypeId ?? DBNull.Value;
                    command.Parameters.Add("@VehicleCategoryId", SqlDbType.Int).Value = (object?)Vehicle.VehiclePropertyIdies.VehicleCategoryId ?? DBNull.Value;

                    

                    command.Parameters.Add("@VehicleId", SqlDbType.Int).Value = Vehicle.VehiclePropertyIdies.VehicleId;



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
        public bool DeleteVehicle(int VehicleId)
        {
            bool IsDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteVehicle", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@VehicleId", VehicleId);


                    try
                    {
                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Result != null && int.TryParse(Result.ToString(), out int updatedStatus))
                        {
                            if (updatedStatus == 1)
                            {
                                IsDeleted = true;
                            }
                            if (updatedStatus == 0)
                            {
                                IsDeleted = false;
                            }
                        }
                        else { IsDeleted = false; }
                    }
                    catch (Exception)
                    { }
                }

            }

            return IsDeleted;

        }
        public List<DTO.UserViewVehicleDTO> GetVehicleby(DTO.UserViewVehicleDTO Vehicle,decimal? StartPrice,decimal? EndPrice)
        {
            List<DTO.UserViewVehicleDTO> vehicles = new List<DTO.UserViewVehicleDTO>();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetVehicleInfo", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (Vehicle.VehiclePropertyIdies.VehicleId.HasValue)
                    {
                        command.Parameters.Add("@VehicleId", SqlDbType.Int).Value = Vehicle.VehiclePropertyIdies.VehicleId ?? (object)DBNull.Value;
                    }
                    else
                    {

                        command.Parameters.Add("@VehicleName", SqlDbType.NVarChar, 50).Value = Vehicle.VehicleInfo.VehicleName ?? (object)DBNull.Value;
                        command.Parameters.Add("@Make", SqlDbType.VarChar, 50).Value = Vehicle.VehicleInfo.Make ?? (object)DBNull.Value;
                        command.Parameters.Add("@Model", SqlDbType.VarChar, 50).Value = Vehicle.VehicleInfo.Model ?? (object)DBNull.Value;
                        command.Parameters.Add("@Year", SqlDbType.VarChar, 4).Value = Vehicle.VehicleInfo.Year?.ToString() ?? (object)DBNull.Value;
                        command.Parameters.Add("@LicensePlate", SqlDbType.VarChar, 50).Value = Vehicle.VehicleInfo.LicensePlate ?? (object)DBNull.Value;
                        command.Parameters.Add("@StatusId", SqlDbType.Int).Value = Vehicle.VehiclePropertyIdies.VehicleStatusId ?? (object)DBNull.Value;
                        command.Parameters.Add("@LocationId", SqlDbType.Int).Value = Vehicle.VehiclePropertyIdies.LocationId ?? (object)DBNull.Value;
                        command.Parameters.Add("@FuelTypeId", SqlDbType.Int).Value = Vehicle.VehiclePropertyIdies.FuelTypeId ?? (object)DBNull.Value;
                        command.Parameters.Add("@VehicleCategoryId", SqlDbType.Int).Value = Vehicle.VehiclePropertyIdies.VehicleCategoryId ?? (object)DBNull.Value;

                        if (StartPrice.HasValue && EndPrice.HasValue)
                        {
                            command.Parameters.Add("@StartPricePerDay", SqlDbType.Decimal).Value = StartPrice.Value;
                            command.Parameters.Add("@EndPricePerDay", SqlDbType.Decimal).Value = EndPrice.Value;
                        }
                       
                    }





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
                                DTO.VehicleNamesDTO vehicleNamesDTO = new DTO.VehicleNamesDTO();
                                vehicleNamesDTO.VehicleName = reader["VehicleName"].ToString();
                                vehicleNamesDTO.PricePerday = Convert.ToDecimal(reader["PricePerDay"]);
                                vehicleNamesDTO.Make = reader["Make"].ToString();
                                vehicleNamesDTO.Model = reader["Model"].ToString();
                                vehicleNamesDTO.Year = Convert.ToInt32(reader["Year"]);
                                vehicleNamesDTO.LicensePlate = reader["LicensePlate"].ToString();

                                DTO.VehicleIdiesDTO vehicleIdiesDTO = new DTO.VehicleIdiesDTO();
                                vehicleIdiesDTO.VehicleId = Convert.ToInt32(reader["VehicleId"]);
                                vehicleIdiesDTO.LocationId = Convert.ToInt32(reader["LocationId"]);
                                vehicleIdiesDTO.FuelTypeId = Convert.ToInt32(reader["FuelTypeId"]);
                                vehicleIdiesDTO.VehicleCategoryId = Convert.ToInt32(reader["VehicleCategoryId"]);
                                vehicleIdiesDTO.VehicleStatusId = Convert.ToInt32(reader["StatusId"]);

                                vehicles.Add(new DTO.UserViewVehicleDTO(vehicleNamesDTO,vehicleIdiesDTO));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        vehicles = new List<DTO.UserViewVehicleDTO>();
                    }
                }

            }

            return vehicles;

        }
        //public List<DTO.UserViewVehicleDTO> GetVehiclebyPriceBetween(decimal StartPrice,decimal EndPrice)
        //{
        //    List<DTO.UserViewVehicleDTO> vehicles = new List<DTO.UserViewVehicleDTO>();


        //    using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand("usp_GetVehicleInfo", connection))
        //        {
        //            command.CommandType = System.Data.CommandType.StoredProcedure;

                  

        //            command.Parameters.AddWithValue("@StartPricePerDay", StartPrice);
        //            command.Parameters.AddWithValue("@EndPricePerDay",EndPrice);





        //            try
        //            {
        //                connection.Open();

        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        DTO.VehicleNamesDTO vehicleNamesDTO = new DTO.VehicleNamesDTO();
        //                        vehicleNamesDTO.VehicleName = reader["VehicleName"].ToString();
        //                        vehicleNamesDTO.PricePerday = Convert.ToDecimal(reader["PricePerDay"]);
        //                        vehicleNamesDTO.Make = reader["Make"].ToString();
        //                        vehicleNamesDTO.Model = reader["Model"].ToString();
        //                        vehicleNamesDTO.Year = Convert.ToInt32(reader["Year"]);
        //                        vehicleNamesDTO.LicensePlate = reader["LicensePlate"].ToString();

        //                        DTO.VehicleIdiesDTO vehicleIdiesDTO = new DTO.VehicleIdiesDTO();
        //                        vehicleIdiesDTO.VehicleId = Convert.ToInt32(reader["VehicleId"]);
        //                        vehicleIdiesDTO.LocationId = Convert.ToInt32(reader["LocationId"]);
        //                        vehicleIdiesDTO.FuelTypeId = Convert.ToInt32(reader["FuelTypeId"]);
        //                        vehicleIdiesDTO.VehicleCategoryId = Convert.ToInt32(reader["VehicleCategoryId"]);
        //                        vehicleIdiesDTO.VehicleStatusId = Convert.ToInt32(reader["StatusId"]);

        //                        vehicles.Add(new DTO.UserViewVehicleDTO(vehicleNamesDTO, vehicleIdiesDTO));
        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                vehicles = new List<DTO.UserViewVehicleDTO>();
        //            }
        //        }

        //    }

        //    return vehicles;

        //}

      
    }
}
