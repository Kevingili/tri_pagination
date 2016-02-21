using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SortingPagingGridView
{
    public class OtData
    {
        public int ordre { get; set; }
        public DateTime date_saisie { get; set; }
        public string designation_ot { get; set; }
    }
    public class AccesOT
    {
        public static List<OtData> GetAllOT(string sortColumn)
        {
            List<OtData> lst = new List<OtData>();
            string cs = ConfigurationManager.ConnectionStrings["connexionInfo"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            string sqlQuery = "select ordre, date_saisie, designation_ot from ot";
            if (!string.IsNullOrEmpty(sortColumn))
            {
                sqlQuery += " order by " + sortColumn;
            }
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                OtData otdata = new OtData();
                otdata.ordre = Convert.ToInt32(rdr["ordre"]);
                otdata.date_saisie = Convert.ToDateTime(rdr["date_saisie"]);
                otdata.designation_ot = rdr["designation_ot"].ToString();
                lst.Add(otdata);
            }

            return lst;
        }    
    }

}