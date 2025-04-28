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
    public class VehicleStatusData : DataLayerInterfaces.IVehicleStatusData
    {
        public int AddNewVehicleStatus(DTO.VehicleStatusDTO VehicleStatus)
        {
            int NewId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddnewVehicleStatus", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Status", VehicleStatus.VehicleStatus.Status);

                    SqlParameter outputIdParam = new SqlParameter("@StatusId", SqlDbType.Int)
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

        public bool UpdateVehicleStatus(DTO.VehicleStatusDTO VehicleStatus)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateVehicleStatus", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Status", VehicleStatus.VehicleStatus.Status);
                    command.Parameters.AddWithValue("@StatusId", VehicleStatus.VehicleStatusId);


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
        public bool DeleteVehicleStatus(int VehicleStatusId)
        {
            bool IsDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteVehicleStatus", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@StatusId", VehicleStatusId);


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
        public DTO.VehicleStatusDTO GetVehicleStatusBy(int? VehicleStatusId,string StatusName)
        {
            DTO.VehicleStatusDTO vehicleStatus = new DTO.VehicleStatusDTO();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetVehicleStatusBy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (VehicleStatusId.HasValue)
                        command.Parameters.AddWithValue("@StatusId", VehicleStatusId);
                    else if (!StatusName.IsNullOrEmpty())
                        command.Parameters.AddWithValue("@StatusName", StatusName);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                vehicleStatus.VehicleStatusId = Convert.ToInt32(reader["StatusId"]);
                                vehicleStatus.VehicleStatus.Status = reader["StatusName"].ToString();
                            }
                            else { vehicleStatus = null; }
                        }
                    }
                    catch (Exception)
                    {
                        vehicleStatus = null;
                    }
                }

            }

            return vehicleStatus;

        }
        public List<DTO.VehicleStatusDTO> GetAllVehicleStatus()
        {

            List<DTO.VehicleStatusDTO> Vehicles = new List<DTO.VehicleStatusDTO>();

            string query = "SELECT * FROM VehicleStatus";
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
                                Vehicles.Add(new DTO.VehicleStatusDTO(Convert.ToInt32(reader["StatusId"]), reader["StatusName"].ToString()));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Vehicles = null;
                    }
                }

            }

            return Vehicles;
        }


    }
}
