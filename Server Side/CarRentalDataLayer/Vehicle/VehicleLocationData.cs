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
    public class VehicleLocationData : ILocationData
    {
        public int AddNewLocation(DTO.LocationDTO NewLocation)
        {
            int NewId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewLocation", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LocationName", NewLocation.LocationInfor.LocationName);
                    command.Parameters.AddWithValue("@Phone", NewLocation.LocationInfor.Phone);
                    command.Parameters.AddWithValue("@Adress", NewLocation.LocationInfor.Adress);


                    SqlParameter outputIdParam = new SqlParameter("@LocationId", SqlDbType.Int)
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
                        else { NewId = -1; }
                    }
                    catch (Exception)
                    { }
                }




            }

            return NewId;
        }
        public bool DeleteLocation(int LocationId)
        {
            bool IsDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteLocation", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LocationId", LocationId);


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
        public DTO.LocationDTO GetLocationById(int LocationId)
        {
            DTO.LocationDTO Location = new DTO.LocationDTO();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetLocationINfoBy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LocationId", LocationId);

                    //if (VehicleCategoryId != -1)
                    //    command.Parameters.AddWithValue("@vehicleCategoryId", VehicleCategoryId);
                    //else if (CategoryName != "")
                    //    command.Parameters.AddWithValue("@vehicleCategoryName", CategoryName);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Location.LocationInfor.LocationName = reader["LocationName"].ToString();
                                Location.LocationInfor.Adress = reader["Address"].ToString();
                                Location.LocationInfor.Phone = reader["Phone"].ToString();
                                Location.LocationId = LocationId;
                            }
                            else { Location = null; }
                        }
                    }
                    catch (Exception)
                    {
                        Location = null;
                    }
                }

            }

            return Location;
        }
        public bool UpdateLocation(DTO.LocationDTO NewLocation)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateLocation", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LocationName", NewLocation.LocationInfor.LocationName);
                    command.Parameters.AddWithValue("@Phone", NewLocation.LocationInfor.Phone);
                    command.Parameters.AddWithValue("@Address", NewLocation.LocationInfor.Adress);
                    command.Parameters.AddWithValue("@LocationId", NewLocation.LocationId);


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

        public List<DTO.LocationDTO> GetAllLocationsByLocationNameOrAdress(string LocationName, string Adress)
        {
            List<DTO.LocationDTO> Locations = new List<DTO.LocationDTO>();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetLocationINfoBy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    if (!string.IsNullOrWhiteSpace(LocationName))
                        command.Parameters.AddWithValue("@LocationName", LocationName);
                    else if (!string.IsNullOrWhiteSpace(Adress))
                        command.Parameters.AddWithValue("@Address", Adress);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Locations.Add(new DTO.LocationDTO(Convert.ToInt32(reader["LocationId"]),
                                   new DTO.LocationDTO.LocationInfo(reader["LocationName"].ToString() , reader["Address"].ToString(),
                                    reader["Phone"].ToString())));

                            }
                        }
                    }
                    catch (Exception)
                    {
                        Locations = null;
                    }
                }

            }

            return Locations;
        }
        public List<DTO.LocationDTO> GetAllLocations()
        {

            List<DTO.LocationDTO> Locations = new List<DTO.LocationDTO>();

            string query = "SELECT * FROM Location";
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
                                Locations.Add(new DTO.LocationDTO(Convert.ToInt32(reader["LocationId"]),
                                 new DTO.LocationDTO.LocationInfo(reader["LocationName"].ToString(), reader["Address"].ToString(),
                                  reader["Phone"].ToString())));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Locations = null;
                    }
                }

            }

            return Locations;
        }

    }


}

