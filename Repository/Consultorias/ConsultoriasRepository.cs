using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Repository.Consultorias
{
    public class ConsultoriasRepository
    {
        public static bool VerificaExistenciaConsultorias(string cnpj, string email)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM Consultoria_Interno WHERE Email = @email AND Cnpj = @cnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@cnpj", SqlDbType.NVarChar).Value = cnpj;
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;

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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ConsultoriasRepository.VerificaExistenciaConsultorias()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public static bool ApagarConsultorias(string cnpj, string email)
        {
            var apagado = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "DELETE FROM Consultoria_Interno WHERE Email = @email AND Cnpj = @cnpj";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@email", SqlDbType.NVarChar).Value = email;
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
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "ConsultoriasRepository.ApagarConsultorias()", "Automações Jessica", e.StackTrace);
            }

            return apagado;
        }



    }
}
