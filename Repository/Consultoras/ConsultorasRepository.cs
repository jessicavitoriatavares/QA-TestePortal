using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.Consultoras
{
    public class ConsultorasRepository
    {

        public static bool VerificaExistenciaConsultoras(string cnpj, string nome)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Consultoria WHERE Nome = @nome AND Cnpj = @cnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@nome", SqlDbType.NVarChar).Value = nome;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ConsultorasRepository.VerificaExistenciaConsultoras()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarConsultoras(string cnpj, string nome)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Consultoria WHERE Nome = @nome AND Cnpj = @cnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@nome", SqlDbType.NVarChar).Value = nome;
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ConsultorasRepository.ApagarConsultoras()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }



    }
}
