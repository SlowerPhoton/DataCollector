using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DTOs
{
    public class Database
    {

        public static List<ElementDTO> GetPasportElementsByID(string id, SqlConnection connection)
        {
            List<ElementDTO> elements = new List<ElementDTO>();
            if (String.IsNullOrEmpty(id))
                return elements;

            string query = "SELECT * FROM ELEMENT WHERE pasport=@PASPORT;";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PASPORT", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ElementDTO elementDTO = new ElementDTO()
                        {
                            Name = Convert.ToString(reader["name"]),
                            State = Convert.ToString(reader["state"]),
                            NeedOfInvestment = Convert.ToString(reader["need_of_investment"]),
                            AmountOfInvestment = Convert.ToString(reader["amount_of_investment"]),
                            Notes = Convert.ToString(reader["notes"])
                        };
                        elements.Add(elementDTO);
                    }
                }
            }
            return elements;
        }

        public static List<PasportDTO> GetByID(string id, SqlConnection connection)
        {
            List<PasportDTO> pasports = new List<PasportDTO>();

            string query = "SELECT * FROM SVERENE_BUDOVY " +
                "LEFT JOIN PASPORT ON SVERENE_BUDOVY.id = PASPORT.building_id " +
                "WHERE id = @ID;";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) // there is only one row
                    {
                        PasportDTO pasport = new PasportDTO()
                        {
                            FinalInspection = reader["final_inspection"] == null ? null : Convert.ToString(reader["final_inspection"]),
                            ID = reader["building_id"] == null ? null : Convert.ToString(reader["building_id"])
                        };

                        BuildingDTO building = new BuildingDTO();
                        building.ID = id;
                        building.Type = Convert.ToString(reader["typ_budovy"]);
                        building.Use = Convert.ToString(reader["zpusob_vyuziti"]);
                        building.Municipality = Convert.ToString(reader["obec"]);
                        if (reader["cislo_domovni"] != DBNull.Value)
                            building.HouseNumber = Convert.ToInt32(reader["cislo_domovni"]);
                        building.Plat = reader["na_parcele"].ToString();
                        string temporaryStr = Convert.ToString(reader["docasna_stavba"]);
                        building.Temporary = temporaryStr.Equals("n") ? false : true;
                        if (reader["podil_citatel"] != DBNull.Value)
                            building.FractionDenominator = Convert.ToInt32(reader["podil_citatel"]);
                        if (reader["podil_jmenovatel"] != DBNull.Value)
                            building.FractionNominator = Convert.ToInt32(reader["podil_jmenovatel"]);
                        building.SubjectICO = Convert.ToString(reader["s_ico"]);
                        building.SubjectName = Convert.ToString(reader["s_nazev"]);
                        building.SubjectStreet = Convert.ToString(reader["s_ulice"]);
                        if (reader["s_cislo_domovni"] != DBNull.Value)
                            building.SubjectHouseNumber = Convert.ToInt32(reader["s_cislo_domovni"]);
                        if (reader["s_cislo_orientacni"] != DBNull.Value)
                            building.SubjectOrientationNumber = Convert.ToInt32(reader["s_cislo_orientacni"]);
                        building.SubjectMunicipality = Convert.ToString(reader["s_obec"]);
                        building.SubjectPostCode = Convert.ToString(reader["s_psc"]);
                        building.SubjectDistrict = Convert.ToString(reader["s_okres"]);
                        building.TelephoneNumber = Convert.ToString(reader["tel_id"]);

                        pasport.Building = building;

                        pasports.Add(pasport);
                    }
                }
            }
            pasports.ForEach(pasport => pasport.Elements = GetPasportElementsByID(pasport.ID, connection));

            return pasports;
        }

        public static List<PasportDTO> GetByICO(string ico, SqlConnection connection)
        {
            List<PasportDTO> pasports = new List<PasportDTO>();

            List<string> ids = new List<string>();
            string query = "SELECT * FROM SVERENE_BUDOVY " +
                "LEFT JOIN PASPORT ON SVERENE_BUDOVY.id = PASPORT.building_id " +
                "WHERE s_ico=@ICO;";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ICO", ico);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add(Convert.ToString(reader["id"]));
                    }
                }
            }

            ids.ForEach(id => pasports.AddRange(GetByID(id, connection)));

            return pasports;
        }

        public static List<SpecifiedElementDTO> GetElements(SqlConnection connection)
        {
            List<SpecifiedElementDTO> specifiedElements = new List<SpecifiedElementDTO>();

            string query = "SELECT * FROM SPECIFIED_ELEMENTS;";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SpecifiedElementDTO specifiedElement = new SpecifiedElementDTO
                        {
                            Name = Convert.ToString(reader["element_name"]),
                            DisplayName = reader["display_name"] == System.DBNull.Value ? (string)reader["element_name"] : (string)reader["display_name"],
                            Required = Convert.ToBoolean(reader["required"])
                        };
                        specifiedElements.Add(specifiedElement);
                    }
                }
            }

            return specifiedElements;
        }

        public static int InsertElement(ElementDTO element, string pasport, SqlConnection connection)
        {
            // if one of the attributes (aside from Name) is null or empty, this is an empty element and do nothing
            if (String.IsNullOrEmpty(element.State))
                return 1;

            if (String.IsNullOrEmpty(element.Name) || String.IsNullOrEmpty(element.NeedOfInvestment) || String.IsNullOrEmpty(element.AmountOfInvestment))
            {
                throw new DataException("Element can't be only partially empty.");
            }

            string query = "INSERT INTO [student3].[dbo].[ELEMENT]([name], [state], [need_of_investment], [amount_of_investment], [notes], [pasport]) " +
                "VALUES (@name, @state, @need_of_investment, @amount_of_investment, @notes, @pasport);";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = element.Name;
            cmd.Parameters.Add("@state", SqlDbType.SmallInt).Value = Convert.ToInt32(element.State);
            cmd.Parameters.Add("@need_of_investment", SqlDbType.VarChar).Value = Convert.ToInt32(element.NeedOfInvestment);
            cmd.Parameters.Add("@amount_of_investment", SqlDbType.VarChar).Value = Convert.ToInt32(element.AmountOfInvestment);
            cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = element.Notes;
            cmd.Parameters.Add("@pasport", SqlDbType.VarChar).Value = pasport;

            int ret = 0;
            try
            {
                ret = cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException) { } // ignore them for now 

            return ret;
        }

        public static int InsertPasport(PasportDTO pasport, SqlConnection connection)
        {
            int ret = 0;

            string query = "INSERT INTO [student3].[dbo].[PASPORT]([final_inspection], [building_id]) " +
                "VALUES (@final_inspection, @id);";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {

                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = pasport.ID;
                cmd.Parameters.Add("@final_inspection", SqlDbType.Date).Value = DateTime.Parse(pasport.FinalInspection, CultureInfo.CreateSpecificCulture("cz-CZ"));

                try
                {
                    ret = cmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException) { }
            }

            foreach (ElementDTO element in pasport.Elements)
            {
                InsertElement(element, pasport.ID, connection);
            }

            return ret;
        }

    }
}
