using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Data.SqlClient;
using DTOs;

namespace API.Controllers
{
    public class PasportController : ApiController
    {
        // GET: Pasport
        public IEnumerable<object> Get(string ico = null, string id = null)
        {
            if ((ico == null && id == null) || (ico != null && id != null))
                return null;
            if (ico != null)
                return getBuildingIDs(ico);
            return getBuilding(id); // new string[] { "ico=" + Convert.ToString(ico) + '\n' + "id=" + Convert.ToString(id) };
        }

        private IEnumerable<string> getBuildingIDs(string ico)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            string query = "SELECT * FROM SVERENE_BUDOVY WHERE s_ico=@ICO;";

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand sCmd = new SqlCommand(query, sqlConnection);
            sCmd.Parameters.AddWithValue("@ICO", ico);

            sqlConnection.Open();
   
            SqlDataReader reader = sCmd.ExecuteReader();

            List<BuildingDTO> buildings = new List<BuildingDTO>();
            while (reader.Read())
            {
                BuildingDTO buildingDTO = new BuildingDTO(reader, connectionString);
                buildings.Add(buildingDTO);
            }

            sqlConnection.Close();
            reader.Close();

            List<string> ids = new List<string>();
            buildings.ForEach(buildingDTO => { ids.Add(Convert.ToString(buildingDTO.ID)); });
            return ids;
        }

        private IEnumerable<object> getBuilding(string id)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            string query = "SELECT * FROM SVERENE_BUDOVY WHERE id=@ID;";

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand sCmd = new SqlCommand(query, sqlConnection);
            sCmd.Parameters.AddWithValue("@ID", id);

            sqlConnection.Open();

            SqlDataReader reader = sCmd.ExecuteReader();

            List<BuildingDTO> buildings = new List<BuildingDTO>(); // list of one element
            while (reader.Read())
            {
                BuildingDTO buildingDTO = new BuildingDTO(reader, connectionString);
                buildings.Add(buildingDTO);
            }

            sqlConnection.Close();
            reader.Close();

            return buildings;
        }
    }
}