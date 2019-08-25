using MySql.Data.MySqlClient;
using PskinAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PskinAPI.ADO
{
    public class ADOHistorias
    {
        private static MySqlConnection con = Conexion.GetInstance().GetConnection();

        #region CONSULTAS
        private static readonly string GET_ALL_HISTORIAS = "SELECT *, (select count(idLikes) from likes where idHistoria = historias.idHistoria) AS 'likes' FROM historias join usuarios on usuarios.idUsuarios = historias.idUsuario order by fecha desc;";
        private static readonly string GET_HISTORIAS_USER = "SELECT * FROM historias WHERE idUsuario = @idUsuario";
        private static readonly string INSERT_HISTORIA = "INSERT INTO historias (ImagenUrl, Descripcion, idUsuario) VALUES (@ImagenUrl, @Descripcion, @idUsuario)";
        private static readonly string INSERT_LIKE = "INSERT INTO likes (idHistoria, idUsuario) VALUES (@idHistoria, @idUsuario)";
        private static readonly string DELETE_LIKE = "DELETE FROM likes WHERE idHistoria = @idHistoria AND idUsuario = @idUsuario";
        private static readonly string Existe = "select count(idLikes) from likes where idHistoria = @idHistoria and idUsuario = @idUsuario;";

        #endregion

        public static void Insert_Historia(string ImagenUrl , string Descripcion , int idUsuario)
        {

            MySqlCommand cmd = new MySqlCommand(INSERT_HISTORIA , con);
            cmd.Parameters.AddWithValue("@ImagenUrl" , ImagenUrl);
            cmd.Parameters.AddWithValue("@Descripcion" , Descripcion);
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

        public static void Insert_Like(int idHistoria, int idUsuario)
        {
            MySqlCommand cmd = new MySqlCommand(INSERT_LIKE , con);
            cmd.Parameters.AddWithValue("@idHistoria" , idHistoria);
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

        public static void Delete_Like(int idHistoria , int idUsuario)
        {
            MySqlCommand cmd = new MySqlCommand(DELETE_LIKE , con);
            cmd.Parameters.AddWithValue("@idHistoria" , idHistoria);
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

        public static List<Historia> GetAllHistoriasByUserId(int idUsuario)
        {
            List<Historia> historias = new List<Historia>();

            MySqlCommand cmd = new MySqlCommand(GET_HISTORIAS_USER , con);
            cmd.Parameters.AddWithValue("@idUsuario" , idUsuario);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        historias.Add(new Historia()
                        {
                            Id = reader["idHistoria"] != DBNull.Value ? reader.GetInt32("idHistoria") : 0 ,
                            ImagenUrl = reader["ImagenUrl"] != DBNull.Value ? reader.GetString("ImagenUrl") : string.Empty ,
                            Descripcion = reader["Descripcion"] != DBNull.Value ? reader.GetString("Descripcion") : string.Empty ,
                            Fecha = reader["Fecha"] != DBNull.Value ? reader.GetDateTime("Fecha") : DateTime.Now ,
                            Likes = reader["Likes"] != DBNull.Value ? reader.GetInt32("Likes") : 0
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

            return historias;
        }

        public static List<Historia> GetAllHistoriasDesc()
        {
            List<Historia> historias = new List<Historia>();

            MySqlCommand cmd = new MySqlCommand(GET_ALL_HISTORIAS , con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        historias.Add(new Historia()
                        {
                            Id = reader["idHistoria"] != DBNull.Value ? reader.GetInt32("idHistoria") : 0 ,
                            ImagenUrl = reader["ImagenUrl"] != DBNull.Value ? reader.GetString("ImagenUrl") : string.Empty ,
                            Descripcion = reader["Descripcion"] != DBNull.Value ? reader.GetString("Descripcion") : string.Empty ,
                            Fecha = reader["Fecha"] != DBNull.Value ? reader.GetDateTime("Fecha") : DateTime.Now ,
                            Likes = reader["Likes"] != DBNull.Value ? reader.GetInt32("Likes") : 0,
                            Usuario = new Usuario
                            {
                                Id = reader["idUsuario"] != DBNull.Value ? reader.GetInt32("idUsuarios") : 0,
                                Nombre = reader["Nombre"] != DBNull.Value ? reader.GetString("Nombre") : string.Empty,
                                Apellido = reader["Apellido"] != DBNull.Value ? reader.GetString("Apellido") : string.Empty,
                                FotoUrl = reader["FotoUrl"] != DBNull.Value ? reader.GetString("FotoUrl") : string.Empty,
                            }
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

            return historias;
        }
    }
}