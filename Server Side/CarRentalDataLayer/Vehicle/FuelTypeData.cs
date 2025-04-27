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
    public class FuelTypeData : IFuelTypeData
    {
        public int AddNewFuelType(DTO.FuelTypeDTO FuelType)
        {
            int NewId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddnewFuelType", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@fuelTypeName", FuelType.FuelTypeInfor.FuelType);

                    SqlParameter outputIdParam = new SqlParameter("@fueltypeId", SqlDbType.Int)
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

        public bool UpdateFuelType(DTO.FuelTypeDTO FuelType)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateFuelType", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@fueltypeId", FuelType.FuelTypeId);
                    command.Parameters.AddWithValue("@fuelTypeName", FuelType.FuelTypeInfor.FuelType);


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
        public bool DeleteFuelType(int FuelTypeId)
        {
            bool IsDeleted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DeleteFuelType", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@fueltypeId", FuelTypeId);


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
        public DTO.FuelTypeDTO GetFuelTypeBy(int? FuelTypeId, string FuelTypeName)
        {
            DTO.FuelTypeDTO FuelType = new DTO.FuelTypeDTO();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetFuelTypeInfoByIdOrName", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (FuelTypeId.HasValue)
                        command.Parameters.AddWithValue("@fueltypeId", FuelTypeId);
                    else if (FuelTypeName != "")
                        command.Parameters.AddWithValue("@fuelTypeName", FuelTypeName);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FuelType.FuelTypeId = Convert.ToInt32(reader["FuelTypeId"]);
                                FuelType.FuelTypeInfor.FuelType = reader["FuelTypeName"].ToString();
                            }
                            else { FuelType = null; }
                        }
                    }
                    catch (Exception)
                    {
                        FuelType = null;
                    }
                }

            }

            return FuelType;

        }
        public List<DTO.FuelTypeDTO> GetAllFuelType()
        {

            List<DTO.FuelTypeDTO> FuelTypes = new List<DTO.FuelTypeDTO>();

            string query = "SELECT * FROM FuelType";
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
                                FuelTypes.Add(new DTO.FuelTypeDTO(Convert.ToInt32(reader["FuelTypeId"]), reader["FuelTypeName"].ToString()));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        FuelTypes = null;
                    }
                }

            }

            return FuelTypes;
        }



    }
}
