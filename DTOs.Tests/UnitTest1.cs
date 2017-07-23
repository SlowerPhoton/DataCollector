using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTOs;
using System.Data;


namespace DTOs.Tests
{
    [TestClass]
    public class BuildingDTOTest
    {
        public string ConnectionString = "Data Source=q.benefitcz.cz,50698;" + 
         "Initial Catalog = student3;" +
         "User id = STUDENT3;" +
         "Password=St3Ben07mk;";

        /*[TestMethod]
        public void TestConstructor1()
        {
            string query = "SELECT * FROM SVERENE_BUDOVY WHERE id=659369713;";
            SQLTests.TestService

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand sCmd = new SqlCommand(query, sqlConnection);
            sCmd.Parameters.AddWithValue("@ICO", ico);

            sqlConnection.Open();

            SqlDataReader reader = sCmd.ExecuteReader();

            List<string> columnNames = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

            List<BuildingDTO> buildings = new List<BuildingDTO>();
            while (reader.Read())
            {
                BuildingDTO buildingDTO = new BuildingDTO(reader, connectionString);
                buildings.Add(buildingDTO);
            }

            sqlConnection.Close();
            reader.Close();

            BuildingDTO buildingDTO = new BuildingDTO();
        }*/
    }
}
