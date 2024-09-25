using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.BoletagemAporte
{
    public class BoletagemAporteRepository
    {

        public static bool VerificaExistenciaBoletagemAporte(string nomeCotista, string tipoCota)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Boleta WHERE NomeCotista = @nomeCotista AND TipoCota = @tipoCota";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeCotista", SqlDbType.NVarChar).Value = nomeCotista;
                        oCmd.Parameters.AddWithValue("@tipoCota", SqlDbType.NVarChar).Value = tipoCota;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                existe = true;

                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "BoletagemAporteRepository.VerificaExistenciaBoletagemAporte()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarBoletagemAporte(string nomeCotista, string tipoCota)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Boleta WHERE NomeCotista = @nomeCotista AND TipoCota = @tipoCota";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeCotista", SqlDbType.NVarChar).Value = nomeCotista;
                        oCmd.Parameters.AddWithValue("@tipoCota", SqlDbType.NVarChar).Value = tipoCota;

                        int rowsAffected = oCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            apagado = true;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "BoletagemAporteRepository.ApagarBoletagemAporte()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }



    }
}
