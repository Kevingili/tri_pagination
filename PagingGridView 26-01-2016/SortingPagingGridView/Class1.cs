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
        public static List<OtData> GetAllOT(int pageIndex, int pageSize, out int totalRows)
        {
            List<OtData> lst = new List<OtData>();
            string cs = ConfigurationManager.ConnectionStrings["connexionInfo"].ConnectionString;
            SqlConnection con = new SqlConnection(cs);

            SqlCommand cmd = new SqlCommand("spGetOt_by_PageIndex_and_PageSize", con);
            cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter paramStartIndex = new SqlParameter();
            paramStartIndex.ParameterName = "@PageIndex";
            paramStartIndex.Value = pageIndex;
            cmd.Parameters.Add(paramStartIndex);

            SqlParameter paramMaximumRows = new SqlParameter();
            paramMaximumRows.ParameterName = "@PageSize";
            paramMaximumRows.Value = pageSize;
            cmd.Parameters.Add(paramMaximumRows);

            
            SqlParameter paramOutputTotalRows = new SqlParameter();
            paramOutputTotalRows.ParameterName = "@TotalRows";
            paramOutputTotalRows.Direction = ParameterDirection.Output ;
            paramOutputTotalRows.SqlDbType = SqlDbType.Int;
            cmd.Parameters.Add(paramOutputTotalRows);

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
            rdr.Close();
                totalRows = (int)cmd.Parameters["@TotalRows"].Value;
            return lst;
        }    
    }

}