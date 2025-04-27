using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;

namespace CarRentalDataLayer
{
    public class LicenseData : ILicenseData
    {

        public int AddNewLicense(DTO.LicenseDTO NewLicense)
        {
            int NewId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewLicense", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IssueDate", NewLicense.IssuDate);
                    command.Parameters.AddWithValue("@ExpirationDate", NewLicense.ExpirationDate);
                    command.Parameters.AddWithValue("@LicenseNumber", NewLicense.LicenseNumber);
                    command.Parameters.AddWithValue("@customerid", NewLicense.CustomerId);



                    SqlParameter outputIdParam = new SqlParameter("@LicenseId", SqlDbType.Int)
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

        public bool UpdateLicense(DTO.LicenseDTO NewLicense)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateLicenseInfo", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IssueDate", NewLicense.IssuDate);
                    command.Parameters.AddWithValue("@ExpirationDate", NewLicense.ExpirationDate);
                    command.Parameters.AddWithValue("@LicenseNumber", NewLicense.LicenseNumber);


                    command.Parameters.AddWithValue("@Licenseid", NewLicense.LicenseId);


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

        public bool DeleteLicense(int LicenseId)
        {
            bool IsDelted = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_DelteLicense", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@licenseid", LicenseId);


                    try
                    {
                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Result != null && int.TryParse(Result.ToString(), out int DeletedStatus))
                        {
                            if (DeletedStatus == 1)
                            {
                                IsDelted = true;
                            }
                            if (DeletedStatus == 0)
                            {
                                IsDelted = false;
                            }
                        }
                        else { IsDelted = false; }
                    }
                    catch (Exception)
                    { }
                }

            }

            return IsDelted;

        }

        public List<DTO.LicenseDTO> GetAllLicenseForThiCustomerId(int customerid)
        {
            List<DTO.LicenseDTO> Licenses = new List<DTO.LicenseDTO>();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetLicenseInfoByCustomerId", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomerId", customerid);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Licenses.Add(new DTO.LicenseDTO(Convert.ToInt32(reader["LicenseId"]), reader["LicenseNumber"].ToString(),
                                    Convert.ToDateTime(reader["IssueDate"]), Convert.ToDateTime(reader["ExpirationDate"]),
                                                      Convert.ToBoolean(reader["IsActive"]), Convert.ToInt32(reader["CustomerId"])));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Licenses = null;
                    }
                }

            }

            return Licenses;
        }

    }
}
