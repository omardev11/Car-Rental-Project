using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.SqlClient;
using Konscious.Security.Cryptography;
using static CarRentalDataLayer.Settings.DTO.CustomerDTO;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;
using CarRentalDataLayer.Settings;


namespace CarRentalDataLayer
{
    public class CustomerData : ICustomerData
    {
        private readonly IPeopleData _PeopleData;

        public CustomerData(IPeopleData peopleData)
        {
            this._PeopleData = peopleData;
        }

        public int AddNewCustomer(DTO.CustomerDTO.CustomerInfo Customer)
        {
            int NewId = -1;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_AddNewCustomer", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FirstName", Customer.PersonInfo.FirstName);
                    command.Parameters.AddWithValue("@LastName", Customer.PersonInfo.LastName);
                    command.Parameters.AddWithValue("@Address", Customer.PersonInfo.Adress);
                    command.Parameters.AddWithValue("@BirthDate", Customer.PersonInfo.Birthdate);
                    command.Parameters.AddWithValue("@Email", Customer.Email);
                    command.Parameters.AddWithValue("@Password", Customer.PassWord);
       


                    SqlParameter outputIdParam = new SqlParameter("@CustomerId", SqlDbType.Int)
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

        public  bool UpdateCustomer(DTO.CustomerDTO Customer)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateCustomer", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                   
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = Customer.customerInfo.PersonInfo.FirstName ?? (object)DBNull.Value;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = Customer.customerInfo.PersonInfo.LastName ?? (object)DBNull.Value;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 50).Value = Customer.customerInfo.PersonInfo.Adress ?? (object)DBNull.Value;
                    command.Parameters.Add("@BirthDate", SqlDbType.Date).Value = (object?)Customer.customerInfo.PersonInfo.Birthdate ?? DBNull.Value;
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = Customer.customerInfo.Email ?? (object)DBNull.Value;


                    command.Parameters.AddWithValue("@CustomerId", Customer.CustomerId);


                    try
                    {
                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Result != null && int.TryParse(Result.ToString() , out int updatedStatus))
                        {
                            if (updatedStatus == 1)
                            {
                                Isupdated = true;
                            }
                            if (updatedStatus == 0)
                            {
                                Isupdated = false ;
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

        public  bool UpdateCustomerPasswordByid(string Password,int customerid)
        {
            bool Isupdated = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_UpdateCustomer", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                   
                    command.Parameters.AddWithValue("@Password", Password);

                    command.Parameters.AddWithValue("@CustomerId", customerid);

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

        public DTO.CustomerDTO GetCustomerInfoById(int customerid)
        {
            DTO.CustomerDTO CustomerInfo = new DTO.CustomerDTO();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetCustomerInfoByEmailOrCustomerId", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomerId", customerid);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CustomerInfo.personId = Convert.ToInt32(reader["PersonId"]);
                                CustomerInfo.customerInfo.Email = reader["Email"].ToString();
                                CustomerInfo.customerInfo.PassWord = reader["Password"].ToString();
                                CustomerInfo.CustomerId = customerid;
                                CustomerInfo.customerInfo.PersonInfo = _PeopleData.GetPersonInfoById(CustomerInfo.personId);

                            }
                            else { CustomerInfo = null; }
                        }
                    }
                    catch (Exception)
                    {
                        CustomerInfo = null;
                    }
                }

            }

            return CustomerInfo;
        
    }

        public DTO.CustomerDTO GetCustomerInfoByEmail(string Email)
        {
            DTO.CustomerDTO CustomerInfo = new DTO.CustomerDTO();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetCustomerInfoByEmailOrCustomerId", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Email", Email);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CustomerInfo.personId = Convert.ToInt32(reader["PersonId"]);
                                CustomerInfo.customerInfo.Email = Email;
                                CustomerInfo.customerInfo.PassWord = reader["Password"].ToString();
                                CustomerInfo.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                                CustomerInfo.customerInfo.PersonInfo = _PeopleData.GetPersonInfoById(CustomerInfo.personId);

                            }
                            else { CustomerInfo = null; }
                        }
                    }
                    catch (Exception)
                    {
                        CustomerInfo = null;
                    }
                }

            }

            return CustomerInfo;

        }
        public bool CheckEmailExists(string Email)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetCustomerInfoByEmailOrCustomerId", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Email", Email);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                               result = true;

                            }
                            else { result = false; }
                        }
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                }

            }

            return result;

        }


    }
}
