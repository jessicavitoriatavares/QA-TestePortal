using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.Ativos
{
    public class AtivosRepository
    {

        public static bool VerificaExistenciaAtivos(string fundo, string observacoes)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM contratos WHERE Fundo = @fundo AND OBservacoes = @observacoes";
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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "AtivosRepository.VerificaExistenciaAtivos()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarAtivos(string fundo, string observacoes)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM contratos WHERE Fundo = @fundo AND OBservacoes = @observacoes";
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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "AtivosRepository.ApagarAtivos()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }
    }
}
