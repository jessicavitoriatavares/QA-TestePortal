using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.ContasEscrows
{
    public class ContasEscrows
    {
        public static bool VerificaExistenciaContasEscrow (string solicitanteBanco, string titularBanco)
        {
            var existe = false; 

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM bancoEscrow WHERE solicitanteBanco = @solicitanteBanco AND titularBanco = @titularBanco";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@titularBanco", SqlDbType.NVarChar).Value = titularBanco;
                        oCmd.Parameters.AddWithValue("@solicitanteBanco", SqlDbType.NVarChar).Value = solicitanteBanco;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ContasEscrowRepository.VerificaExistenciaContasEscrow()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarContasEscrow(string solicitanteBanco, string titularBanco)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM bancoEscrow WHERE solicitanteBanco = @solicitanteBanco AND titularBanco = @titularBanco";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@titularBanco", SqlDbType.NVarChar).Value = titularBanco;
                        oCmd.Parameters.AddWithValue("@solicitanteBanco", SqlDbType.NVarChar).Value = solicitanteBanco;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ContasEscrowRepository.ApagarContasEscrow()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }


    }
}
