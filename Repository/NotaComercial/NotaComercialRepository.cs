using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.NotaComercial
{
    public class NotaComercialRepository
    {
        public static bool VerificaExistenciaNotaComercial(string fundo, string observacoes)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM NC_Operacoes WHERE Fundo = @fundo AND Observacoes = @observacoes";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", SqlDbType.NVarChar).Value = fundo;
                        oCmd.Parameters.AddWithValue("@observacoes", SqlDbType.NVarChar).Value = observacoes;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "NotaComercialRepository.VerificaExistenciaNotaComercial()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarNotaComercial(string fundo, string observacoes)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM NC_Operacoes WHERE Fundo = @fundo AND Observacoes = @observacoes";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@fundo", SqlDbType.NVarChar).Value = fundo;
                        oCmd.Parameters.AddWithValue("@observacoes", SqlDbType.NVarChar).Value = observacoes;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "NotaComercialRepository.ApagarNotaComercial()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }


    }
}
