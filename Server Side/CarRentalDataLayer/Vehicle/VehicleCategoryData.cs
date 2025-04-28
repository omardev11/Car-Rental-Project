using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDataLayer.Vehicle
{
    public class VehicleCategoryData : DataLayerInterfaces.IVehicleCategoryData
    {
        public int AddNewVehicleCategory(DTO.VehicleCategoryDTO NewVehicleCategory)
        {
            int NewId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewVehicleCategory", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CategoryName", NewVehicleCategory.CategoryInfo.Category);

                    SqlParameter outputIdParam = new SqlParameter("@CategoryId", SqlDbType.Int)
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
        public bool DeleteVehicleCategory(int VehicleCategoryId)
        {
            bool IsDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteVehicleCategory", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CategoryId", VehicleCategoryId);


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
        public DTO.VehicleCategoryDTO GetVehicleCategoryBy(int? VehicleCategoryId,string CategoryName)
        {
            DTO.VehicleCategoryDTO VehicleCategory = new DTO.VehicleCategoryDTO();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetVehicleCategoryInfoByIdOrName", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (VehicleCategoryId.HasValue)
                        command.Parameters.AddWithValue("@vehicleCategoryId", VehicleCategoryId);
                    else if (CategoryName != "")
                        command.Parameters.AddWithValue("@vehicleCategoryName", CategoryName);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                VehicleCategory.VehicleCategoryId = Convert.ToInt32(reader["VehicleCategoryId"]);
                                VehicleCategory.CategoryInfo.Category = reader["CategoryName"].ToString();
                            }
                            else { VehicleCategory = null; }
                        }
                    }
                    catch (Exception)
                    {
                        VehicleCategory = null;
                    }
                }

            }

            return VehicleCategory;
        }
        public bool UpdateVehicleCategory(DTO.VehicleCategoryDTO NewVehicleCategory)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateVehicleCategory", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CategoryId", NewVehicleCategory.VehicleCategoryId);
                    command.Parameters.AddWithValue("@CategoryName", NewVehicleCategory.CategoryInfo.Category);


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
        public List<DTO.VehicleCategoryDTO> GetAllVehicleCategory()
        {

            List<DTO.VehicleCategoryDTO> Categoryies = new List<DTO.VehicleCategoryDTO>();

            string query = "SELECT * FROM VehicleCategory";
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
                                Categoryies.Add(new DTO.VehicleCategoryDTO(Convert.ToInt32(reader["VehicleCategoryId"]), reader["CategoryName"].ToString()));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Categoryies = null;
                    }
                }

            }

            return Categoryies;
        }

    }
}

