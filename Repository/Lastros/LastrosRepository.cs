using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.Lastros
{
    public class LastrosRepository
    {

        public static bool VerificaExistenciaLastros(string cnpjFundo, string observacao)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM lastros WHERE CnpjFundo = @cnpjFundo AND Observacao = @observacao";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.AddWithValue("@observacao", SqlDbType.NVarChar).Value = observacao;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "LastrosRepository.VerificaExistenciaLastros()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarLastros (string cnpjFundo, string observacao)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM lastros WHERE CnpjFundo = @cnpjFundo AND Observacao = @observacao";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpjFundo", SqlDbType.NVarChar).Value = cnpjFundo;
                        oCmd.Parameters.AddWithValue("@observacao", SqlDbType.NVarChar).Value = observacao;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "LastrosRepository.ApagarLastros()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }

    }
}
