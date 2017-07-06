using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace DTOs
{
    public class BuildingDTO
    {
        // in this case there is no need to store anything as 'int'
        // converting only for possible use in the future

        public int ID { get; set; }
        public string Type { get; set; }
        public string Use { get; set; }
        public string Municipality { get; set; }
        public int HouseNumber { get; set; }
        public string Plat { get; set; }
        public bool Temporary { get; set; }
        public int FractionDenominator { get; set; }
        public int FractionNominator { get; set; }
        public int SubjectICO { get; set; }
        public string SubjectName { get; set; }
        public string SubjectStreet { get; set; }
        public int SubjectHouseNumber { get; set; }
        public int SubjectOrientationNumber { get; set; }
        public string SubjectMunicipality { get; set; }
        public string SubjectPostCode { get; set; }
        public string SubjectDistrict { get; set; }
        public string TelephoneNumber { get; set; }

        public BuildingDTO(SqlDataReader pasportReader, string connectionString)
        {
            if (pasportReader["id"] != DBNull.Value)
                ID = Convert.ToInt32(pasportReader["id"]);

            string query = "SELECT * FROM SVERENE_BUDOVY WHERE id=@ID;"; 

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand sCmd = new SqlCommand(query, sqlConnection);
            sCmd.Parameters.AddWithValue("@ID", ID);

            sqlConnection.Open();

            SqlDataReader reader = sCmd.ExecuteReader();

            while (reader.Read()) // there is only one row
            {
                Type = reader["typ_budovy"].ToString();
                Use = reader["zpusob_vyuziti"].ToString();
                Municipality = reader["obec"].ToString();
                if (reader["cislo_domovni"] != DBNull.Value)
                    HouseNumber = Convert.ToInt32(reader["cislo_domovni"]);
                Plat = reader["na_parcele"].ToString();
                string temporaryStr = reader["docasna_stavba"].ToString();
                if (temporaryStr.Equals("n"))
                    Temporary = false;
                else
                    Temporary = true;
                if (reader["podil_citatel"] != DBNull.Value)
                    FractionDenominator = Convert.ToInt32(reader["podil_citatel"]);
                if (reader["podil_jmenovatel"] != DBNull.Value)
                    FractionNominator = Convert.ToInt32(reader["podil_jmenovatel"]);
                if (reader["s_ico"] != DBNull.Value)
                    SubjectICO = Convert.ToInt32(reader["s_ico"]);
                SubjectName = reader["s_nazev"].ToString();
                SubjectStreet = reader["s_ulice"].ToString();
                if (reader["s_cislo_domovni"] != DBNull.Value)
                    SubjectHouseNumber = Convert.ToInt32(reader["s_cislo_domovni"]);
                if (reader["s_cislo_orientacni"] != DBNull.Value)
                    SubjectOrientationNumber = Convert.ToInt32(reader["s_cislo_orientacni"]);
                SubjectMunicipality = reader["s_obec"].ToString();
                SubjectPostCode = reader["s_psc"].ToString();
                SubjectDistrict = reader["s_okres"].ToString();
                TelephoneNumber = reader["tel_id"].ToString();
            }

            sqlConnection.Close();
            reader.Close();
        }

        private static string generateHeader(bool id, bool type, bool use, bool municipality, bool house_number, bool plat)
        {
            string header = "<tr>";
            
            if (id)
                header += "<th> id </th>";
            if (type)
                header += "<th> type </th>";
            if (use)
                header += "<th> use </th>";
            if (municipality)
                header += "<th> municipality </th>";
            if (house_number)
                header += "<th> house_number </th>";
            if (plat)
                header += "<th> plat </th>";
            header += "<th></th></tr>";

            return header;
        }

        public static string generateForm(List<BuildingDTO> buildings, bool id = true, bool type = true, bool use = true, bool municipality = true, bool house_number = true, bool plat = false)
        {
            string form = "<table border=\"1\">"; //<form method=\"post\" action=\"/Home/Form\">

            form += generateHeader(id, type, use, municipality, house_number, plat);
           
            foreach (BuildingDTO building in buildings)
            {
                form += "<tr>";

                if (id)
                    form += "<td>" + building.ID.ToString() + "</td>";
                if (type)
                    form += "<td>" + building.Type + "</td>";
                if (use)
                    form += "<td>" + building.Use + "</td>";
                if (municipality)
                    form += "<td>" + building.Municipality + "</td>";
                if (house_number)
                    form += "<td>" + building.HouseNumber + "</td>";
                if (plat)
                    form += "<td>" + building.Plat + "</td>";

                //  form += "<td><input type=\"submit\" value=\"Submit Form\" class=\"btn btn-default\" name=\"" + building.ID.ToString() + "\" />";
                form += "<td><a href=\"/Home/Form/" + building.ID.ToString() + "\"><button class=\"btn btn-default\">Submit Form</button></a></td>";
                form += "</tr>";
            }

            form += "</table>"; // </ form >
            return form;
        }
    }
}