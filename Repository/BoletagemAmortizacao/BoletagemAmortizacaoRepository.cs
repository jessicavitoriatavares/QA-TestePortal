using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.BoletagemAmortizacao
{
    public class BoletagemAmortizacaoRepository
    {

        public static bool VerificaExistenciaBoletagemAmortizacao(string nomeCotista, string cpfCotista)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Amortizacao WHERE NomeCotista = @nomeCotista AND CpfCnpj = @cpfCotista";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeCotista", SqlDbType.NVarChar).Value = nomeCotista;
                        oCmd.Parameters.AddWithValue("@cpfCotista", SqlDbType.NVarChar).Value = cpfCotista;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "BoletagemAmortizacaoRepository.VerificaExistenciaBoletagemAmortizacao()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarBoletagemAmortizacao(string nomeCotista, string cpfCotista)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM  Amortizacao WHERE NomeCotista = @nomeCotista AND CpfCnpj = @cpfCotista";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nomeCotista", SqlDbType.NVarChar).Value = nomeCotista;
                        oCmd.Parameters.AddWithValue("@cpfCotista", SqlDbType.NVarChar).Value = cpfCotista;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "BoletagemAmortizacaoRepository.ApagarBoletagemAmortizacao()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }
    }
}
