using MySql.Data.MySqlClient;
using PskinAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PskinAPI.ADO
{
    public class ADO_Analisis
    {
        private static MySqlConnection con = Conexion.GetInstance().GetConnection();

        #region CONSULTAS
        private static readonly string GET_ANALISIS_ID = "SELECT * FROM analisis WHERE idUsuario = @idUsuario;";
        private static readonly string INSERT_ANALISIS = "INSERT INTO analisis (Tag, Probabilidad, ImagenUrl, idUsuario) VALUES (@Tag, @Probabilidad, @ImagenUrl, @idUsuario);";
        private static readonly string DELETE_ANALISIS = "DELETE FROM analisis WHERE idAnalisis = @idAnalisis;";
        #endregion

        public static void Insert_Analisis(string Tag, double Probabilidad, string ImagenUrl, int idUsuario)
        {

            MySqlCommand cmd = new MySqlCommand(INSERT_ANALISIS , con);
            cmd.Parameters.AddWithValue("@Tag" , Tag);
            cmd.Parameters.AddWithValue("@Probabilidad" , Probabilidad);
            cmd.Parameters.AddWithValue("@ImagenUrl" , ImagenUrl);
            cmd.Parameters.AddWithValue("@idUsuario" , idUsuario);
            con.Open();

            try
            {
                cmd.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {
                con.Close();
                
            }
            finally
            {
                con.Close();
            }

        }

        public static List<Analisis> GetAllAnalisisByUserId(int idUsuario)
        {
            List<Analisis> analisis = new List<Analisis>();

            MySqlCommand cmd = new MySqlCommand(GET_ANALISIS_ID , con);
            cmd.Parameters.AddWithValue("@idUsuario" , idUsuario);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        analisis.Add(new Analisis()
                        {
                            Id = reader["idAnalisis"] != DBNull.Value ? reader.GetInt32("idAnalisis") : 0 ,
                            ImagenUrl = reader["ImagenUrl"] != DBNull.Value ? reader.GetString("ImagenUrl") : string.Empty ,
                            Probabilidad = reader["Probabilidad"] != DBNull.Value ? reader.GetDouble("Probabilidad") : 0.0 ,
                            Fecha = reader["Fecha"] != DBNull.Value ? reader.GetDateTime("Fecha") : DateTime.Now ,
                            Tag = reader["Tag"] != DBNull.Value ? reader.GetString("Tag") : string.Empty
                        });
                    } 
                }
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                con.Close();
            }

            return analisis;
        }

        public static int Eliminar_Analisis(int idAnalisis)
        {
            MySqlCommand cmd = new MySqlCommand(DELETE_ANALISIS , con);
            cmd.Parameters.AddWithValue("@idAnalisis" , idAnalisis);
            con.Open();

            try
            {
                if(cmd.ExecuteNonQuery() == 1)
                    return 1;
                else return 0;
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                con.Close();
            }
        }

    }
}