using CarRentalDataLayer.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;

namespace CarRentalDataLayer
{
    public class PeopleData : IPeopleData
    {
        public DTO.PersonDTO.personInfo GetPersonInfoById(int personid)
        {
            DTO.PersonDTO.personInfo PerssonInfo = new DTO.PersonDTO.personInfo();


            using (SqlConnection connection = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand("usp_GetPersonInfoById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonId", personid);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PerssonInfo.FirstName = reader["FirstName"].ToString();
                                PerssonInfo.LastName = reader["LastName"].ToString();
                                PerssonInfo.Birthdate = Convert.ToDateTime(reader["BirthDate"]);
                                PerssonInfo.Age = Convert.ToInt32(reader["Age"]);
                                PerssonInfo.Adress = reader["Address"].ToString();

                            }
                            else { PerssonInfo = null; }
                        }
                    }
                    catch (Exception)
                    {
                        PerssonInfo = null;
                    }
                }

            }

            return PerssonInfo;

        }

    }
}
