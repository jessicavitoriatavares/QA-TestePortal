using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.FundoTransferencia
{
    public class FundoTransferenciaRepository
    {
        public static bool VerificaExistenciaFundoTransferencia(string cnpj, string nomeFundo)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM FundosTransferencia WHERE CNPJ = @cnpj AND NomeFundo = @nomeFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@nomeFundo", SqlDbType.NVarChar).Value = nomeFundo;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "FundosTransferenciaRepository.VerificaExistenciaFundoTransferencia()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarFundoTransferencia(string cnpj, string nomeFundo)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM FundosTransferencia WHERE CNPJ = @cnpj AND NomeFundo = @nomeFundo";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@nomeFundo", SqlDbType.NVarChar).Value = nomeFundo;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "FundosTransferenciaRepository.ApagarFundoTransferencia()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }

    }
}
