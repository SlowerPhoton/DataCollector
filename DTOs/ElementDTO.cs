using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;


namespace DTOs
{
    public class ElementDTO
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string NeedOfInvestment { get; set; }
        public string AmountOfInvestment { get; set; }
        public string Notes { get; set; }

        public static List<ElementDTO> GetElementsDTOByID (string id, string connectionString)
        {
            string query = "SELECT * FROM ELEMENT WHERE pasport=@PASPORT;";

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand sCmd = new SqlCommand(query, sqlConnection);
            sCmd.Parameters.AddWithValue("@PASPORT", id);

            sqlConnection.Open();

            SqlDataReader reader = sCmd.ExecuteReader();

            List<ElementDTO> elements = new List<ElementDTO>();
            while (reader.Read())
            {
                ElementDTO elementDTO = new ElementDTO(reader);
                elements.Add(elementDTO);
            }

            sqlConnection.Close();
            reader.Close();

            return elements;
        }

        public ElementDTO(SqlDataReader reader)
        {
            Name = Convert.ToString(reader["name"]);
            State = Convert.ToString(reader["state"]);
            NeedOfInvestment = Convert.ToString(reader["need_of_investment"]);
            AmountOfInvestment = Convert.ToString(reader["amount_of_investment"]);
            Notes = Convert.ToString(reader["notes"]);
        }

        public ElementDTO(string name, string state, string need_of_investment, string amount_of_investment, string notes)
        {
            Name = name;
            State = state;
            NeedOfInvestment = need_of_investment;
            AmountOfInvestment = amount_of_investment;
            Notes = notes;
        }

        public int InsertIntoDatabase(string pasport, string connectionString)
        {
            // if one of the attributes (aside from Name) is null or empty, this is an empty element and do nothing
            if (String.IsNullOrEmpty(State))
                return 1;

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string query = "INSERT INTO [student3].[dbo].[ELEMENT]([name], [state], [need_of_investment], [amount_of_investment], [notes], [pasport]) " +
                "VALUES (@name, @state, @need_of_investment, @amount_of_investment, @notes, @pasport);";
            
            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Name;
            cmd.Parameters.Add("@state", SqlDbType.SmallInt).Value = Convert.ToInt32(State);
            cmd.Parameters.Add("@need_of_investment", SqlDbType.VarChar).Value = Convert.ToInt32(NeedOfInvestment);
            cmd.Parameters.Add("@amount_of_investment", SqlDbType.VarChar).Value = Convert.ToInt32(AmountOfInvestment);
            cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = Notes;
            cmd.Parameters.Add("@pasport", SqlDbType.VarChar).Value = pasport;

            int ret = 0;
            try
            {
                ret = cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException) { } // ignore them for now 

            sqlConnection.Close();
            return ret;
        }
    }
}