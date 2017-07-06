using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DTOs
{
    public class PasportDTO
    {
        public string FinalInspection { get; set; }
        public string ID { get; set; }
        public BuildingDTO Building { get; set; }
        public List<ElementDTO> Elements { get; set; }

        public PasportDTO(SqlDataReader reader, string connectionString)
        {
            // in this case there is no need to store anything as 'int'
            // converting only for possible use in the future

            ID = Convert.ToString(reader["id"]);
            FinalInspection = Convert.ToString(reader["final_inspection"]);
            Elements = ElementDTO.GetElementsDTOByID(ID, connectionString);
            Building = new BuildingDTO(reader, connectionString);
        }

        public PasportDTO(string final_inspection, string id, List<ElementDTO> elements, BuildingDTO building = null)
        {
            FinalInspection = final_inspection;
            ID = id;
            Elements = elements;
            Building = building;
        }

        public int InsertIntoDatabase(string connectionString)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string query = "INSERT INTO [student3].[dbo].[PASPORT]([final_inspection], [id]) " +
                "VALUES (@final_inspection, @id);";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = ID;
            cmd.Parameters.Add("@final_inspection", SqlDbType.Date).Value = DateTime.Parse(FinalInspection, CultureInfo.CreateSpecificCulture("cz-CZ"));

            int ret = 0;
            try
            {
                ret = cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (System.Data.SqlClient.SqlException) { } 
            foreach (ElementDTO element in Elements)
            {
                element.InsertIntoDatabase(ID, connectionString);
            }
            return ret;
        }
    }
}