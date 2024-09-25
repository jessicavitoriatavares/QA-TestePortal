using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.Correntistas
{
    public class CorrentistaRepository
    {

        public static bool VerificaExistenciaCorrentista(string emailCorrentista, string CpfCnpj)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Correntista WHERE Email = @emailCorrentista AND CPFCNPJ = @CpfCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@emailCorrentista", SqlDbType.NVarChar).Value = emailCorrentista;
                        oCmd.Parameters.AddWithValue("@CPFCNPJ", SqlDbType.NVarChar).Value = CpfCnpj;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "CorrentistaRepository.VerificaExistenciaCorrentista()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarCorrentista(string emailCorrentista, string CpfCnpj)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Correntista WHERE Email = @emailCorrentista AND CPFCNPJ = @CpfCnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@emailCorrentista", SqlDbType.NVarChar).Value = emailCorrentista;
                        oCmd.Parameters.AddWithValue("@CPFCNPJ", SqlDbType.NVarChar).Value = CpfCnpj;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "UsuarioRepository.ApagarCorrentista()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }



    }
}
