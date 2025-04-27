using static CarRentalDataLayer.Settings.DataLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CarRentalDataLayer
{
    public class UserData :IUserData
    {
        public int? AddNewUser(DTO.UserDTO User)
        {
            int NewId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserName", User.UserName);
                    command.Parameters.AddWithValue("@Password", User.Password);
                    command.Parameters.AddWithValue("@FirstName", User.FirstName);
                    command.Parameters.AddWithValue("@LastName", User.LastName);
                    command.Parameters.AddWithValue("@Address", User.Adress);
                    command.Parameters.AddWithValue("@BirthDate", User.Birthdate);
                    command.Parameters.AddWithValue("@CanManageUsers", User.CanManageUsers);



                    SqlParameter outputIdParam = new SqlParameter("@UserId", SqlDbType.Int)
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

        public bool UpdateUser(DTO.UserDTO User)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = User.FirstName ?? (object)DBNull.Value;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = User.LastName ?? (object)DBNull.Value;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 50).Value = User.Adress ?? (object)DBNull.Value;
                    command.Parameters.Add("@BirthDate", SqlDbType.Date).Value = (object?)User.Birthdate ?? DBNull.Value;
                    command.Parameters.Add("@userName", SqlDbType.NVarChar, 50).Value = User.UserName ?? (object)DBNull.Value;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = User.Password ?? (object)DBNull.Value;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = User.IsActive.HasValue ? (object)User.IsActive.Value : DBNull.Value;
                    command.Parameters.Add("@CanManageUsers", SqlDbType.Bit).Value = User.CanManageUsers.HasValue ? (object)User.CanManageUsers.Value : DBNull.Value;


                    command.Parameters.AddWithValue("@UserId", User.UserId);


                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int updatedStatus))
                        {
                            Isupdated = updatedStatus == 1;
                        }
                    }
                    catch (Exception)
                    { }
                }

            }

            return Isupdated;

        }
        public bool IsThisUserCanManageUsers(int userId)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_IsThisUserCanManageUsers", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userId);


                    try
                    {
                        connection.Open();

                        SqlDataReader result = command.ExecuteReader();
                        if (result.HasRows)
                            Isupdated = true;
                        else
                            Isupdated = false;

                    }
                    catch (Exception)
                    { }
                }

            }

            return Isupdated;

        }
        public DTO.UserDTO GetUserInfoByUserName(string UserName)
        {
            DTO.UserDTO User = new DTO.UserDTO();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetUserInfoBy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@userName", UserName);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                User.UserId = Convert.ToInt32(reader["UserId"]);
                                User.UserName = UserName;
                                User.Password = reader["Password"].ToString();
                            }
                            else { User = null; }
                        }
                    }
                    catch (Exception)
                    {
                        User = null;
                    }
                }

            }

            return User;

        }
        public List<DTO.UserDTO> GetAllUsersBy(DTO.UserDTO user)
        {
            List<DTO.UserDTO> Users = new List<DTO.UserDTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetUserInfoBy", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (user.UserId.HasValue)
                    {
                        command.Parameters.Add("@userId", SqlDbType.Int).Value = user.UserId.HasValue ? (object)user.UserId.Value :
                                                                                                                  (object)DBNull.Value;                                                                                                       
                    }
                    else
                    {

                        command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = user.FirstName ?? (object)DBNull.Value;
                        command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = user.LastName ?? (object)DBNull.Value;
                        command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = user.Adress ?? (object)DBNull.Value;
                        command.Parameters.Add("@BirthDate", SqlDbType.Date).Value = user.Birthdate ?? (object)DBNull.Value;
                        command.Parameters.Add("@isActive", SqlDbType.Bit).Value = user.IsActive.HasValue ? (object)user.IsActive : (object)DBNull.Value;
                        command.Parameters.Add("@CanManageUsers", SqlDbType.Bit).Value = user.CanManageUsers.HasValue ? (object)user.CanManageUsers : (object)DBNull.Value;
                        command.Parameters.Add("@Age", SqlDbType.Int).Value = user.Age.HasValue ? (object)user.Age.Value : (object)DBNull.Value;

                    }


                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Users.Add(new DTO.UserDTO(
                                    Convert.ToInt32(reader["UserId"]), reader["UserName"].ToString(), reader["Password"].ToString(),
                                    Convert.ToBoolean(reader["IsActive"]), Convert.ToBoolean(reader["CanManageUsers"]),
                                    reader["FirstName"].ToString(), reader["LastName"].ToString(),
                                    Convert.ToInt32(reader["Age"]), Convert.ToDateTime(reader["BirthDate"]), reader["Address"].ToString()
                                    ));                          
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Users = null;
                    }
                }
            }

            return Users;
        }
    }
}
