using MySql.Data.MySqlClient;
using PskinAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PskinAPI.ADO
{
    public class ADO_Usuario
    {
        private static MySqlConnection con = Conexion.GetInstance().GetConnection();

        #region CONSULTAS
        private static readonly string LOGIN_Email = "SELECT * FROM usuarios WHERE Email = @Email and Pass = @Pass";
        private static readonly string UPDATE_Usuario = "UPDATE usuarios SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email WHERE idUsuarios = @idUsuarios";
        private static readonly string LOGIN_Username = "SELECT * FROM usuarios WHERE Username = @Username and Pass = @Pass";
        private static readonly string REGISTER = "INSERT INTO USUARIOS (Nombre, Apellido, Email, Pass, Username) VALUES (@Nombre, @Apellido, @Email, @Pass, @Username)";
        private static readonly string UPDATE_FotoUrl = "UPDATE usuarios set FotoUrl = @FotoUrl WHERE idUsuarios = @idUsuarios;";
        private static readonly string GETUSER_Id = "SELECT * FROM usuarios WHERE idUsuarios = @idUsuarios;";
        #endregion

        public static Usuario GetUsuarioEmail(string Email , string Pass)
        {
            Usuario usu = null;

            MySqlCommand cmd = new MySqlCommand(LOGIN_Email , con);
            cmd.Parameters.AddWithValue("@Email" , Email);
            cmd.Parameters.AddWithValue("@Pass" , Pass);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    reader.Read();
                   
                    usu = new Usuario()
                    {
                        Id = reader["idUsuarios"] != DBNull.Value ? reader.GetInt32("idUsuarios") : 0 ,
                        Nombre = reader["Nombre"] != DBNull.Value ? reader.GetString("Nombre") : string.Empty ,
                        Apellido = reader["Apellido"] != DBNull.Value ? reader.GetString("Apellido") : string.Empty ,
                        Email = reader["Email"] != DBNull.Value ? reader.GetString("Email") : string.Empty ,
                        Pass = reader["Pass"] != DBNull.Value ? reader.GetString("Pass") : string.Empty ,
                        FotoUrl = reader["FotoUrl"] != DBNull.Value ? reader.GetString("FotoUrl") : string.Empty ,
                        Username = reader["Username"] != DBNull.Value ? reader.GetString("Username") : string.Empty 
                    };
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

            try
            {
                List<Analisis> a = ADO_Analisis.GetAllAnalisisByUserId(usu.Id);
                List<Historia> h = ADOHistorias.GetAllHistoriasByUserId(usu.Id);

                usu.Galeria = new Galeria
                {
                    Analisis = a ,
                    Historias = h
                };
                usu.Fotos = GetFotosOrdenadas(a , h);
            }
            catch (Exception e)
            {

            }
            return usu;
        }

        private static List<Foto> GetFotosOrdenadas(List<Analisis> a , List<Historia> h)
        {
            List<Foto> fotos = new List<Foto>();

            int i = 0, j = 0;

            for (; i < a.Count; i++)
                fotos.Add(new Foto { Id = a[i].Id , ImagenUrl = a[i].ImagenUrl , EsAnalisis = true, Fecha = a[i].Fecha });

            for (; j < h.Count; j++)
                fotos.Add(new Foto { Id = h[j].Id , ImagenUrl = h[j].ImagenUrl , EsAnalisis = false, Fecha = h[j].Fecha });

            fotos = fotos.OrderByDescending(x => x.Fecha)
           .ToList();

            return fotos;
        }

        public static Usuario GetUsuarioUsername(string Username , string Pass)
        {
            Usuario usu = null;

            MySqlCommand cmd = new MySqlCommand(LOGIN_Username , con);
            cmd.Parameters.AddWithValue("@Username" , Username);
            cmd.Parameters.AddWithValue("@Pass" , Pass);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    usu = new Usuario()
                    {
                        Id = reader["idUsuarios"] != DBNull.Value ? reader.GetInt32("idUsuarios") : 0 ,
                        Nombre = reader["Nombre"] != DBNull.Value ? reader.GetString("Nombre") : string.Empty ,
                        Apellido = reader["Apellido"] != DBNull.Value ? reader.GetString("Apellido") : string.Empty ,
                        Email = reader["Email"] != DBNull.Value ? reader.GetString("Email") : string.Empty ,
                        Pass = reader["Pass"] != DBNull.Value ? reader.GetString("Pass") : string.Empty ,
                        FotoUrl = reader["FotoUrl"] != DBNull.Value ? reader.GetString("FotoUrl") : string.Empty ,
                        Username = reader["Username"] != DBNull.Value ? reader.GetString("Username") : string.Empty
                    };
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

            return usu;
        }

        public static int Register(Usuario usu)
        {

            MySqlCommand cmd = new MySqlCommand(REGISTER , con);
            cmd.Parameters.AddWithValue("@Nombre" , usu.Nombre);
            cmd.Parameters.AddWithValue("@Apellido" , usu.Apellido);
            cmd.Parameters.AddWithValue("@Email" , usu.Email);
            cmd.Parameters.AddWithValue("@Pass" , usu.Pass);
            cmd.Parameters.AddWithValue("@Username" , usu.Username);
            con.Open();

            try
            {
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
            catch (Exception e)
            {

                return -1;
                con.Close();
            }
            finally
            {
                con.Close();
            }

        }

        public static Usuario Actualizar(Usuario usu)
        {

            MySqlCommand cmd = new MySqlCommand(UPDATE_Usuario , con);
            cmd.Parameters.AddWithValue("@Nombre" , usu.Nombre);
            cmd.Parameters.AddWithValue("@Apellido" , usu.Apellido);
            cmd.Parameters.AddWithValue("@Email" , usu.Email);
            cmd.Parameters.AddWithValue("@idUsuarios" , usu.Id);
            //cmd.Parameters.AddWithValue("@Username" , usu.Username);
            con.Open();

            try
            {
                cmd.ExecuteNonQuery();
                return usu;
            }
            catch (Exception e)
            {
                con.Close();
                return null;
            }
            finally
            {
                con.Close();
            }

        }

        public static void UploadImage(string FotoUrl, int idUsuarios)
        {
            MySqlCommand cmd = new MySqlCommand(UPDATE_FotoUrl , con);
            cmd.Parameters.AddWithValue("@FotoUrl" , FotoUrl);
            cmd.Parameters.AddWithValue("@idUsuarios" , idUsuarios);
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

        public static Usuario GetUserById(int id)
        {
            Usuario usu = null;

            MySqlCommand cmd = new MySqlCommand(GETUSER_Id , con);
            cmd.Parameters.AddWithValue("@idUsuarios" , id);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    usu = new Usuario()
                    {
                        Id = reader["idUsuarios"] != DBNull.Value ? reader.GetInt32("idUsuarios") : 0 ,
                        Nombre = reader["Nombre"] != DBNull.Value ? reader.GetString("Nombre") : string.Empty ,
                        Apellido = reader["Apellido"] != DBNull.Value ? reader.GetString("Apellido") : string.Empty ,
                        Email = reader["Email"] != DBNull.Value ? reader.GetString("Email") : string.Empty ,
                        Pass = reader["Pass"] != DBNull.Value ? reader.GetString("Pass") : string.Empty ,
                        FotoUrl = reader["FotoUrl"] != DBNull.Value ? reader.GetString("FotoUrl") : string.Empty ,
                        Username = reader["Username"] != DBNull.Value ? reader.GetString("Username") : string.Empty
                    };
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

            List<Analisis> a = ADO_Analisis.GetAllAnalisisByUserId(usu.Id);
            List<Historia> h = ADOHistorias.GetAllHistoriasByUserId(usu.Id);

            usu.Galeria = new Galeria
            {
                Analisis = a ,
                Historias = h
            };
            usu.Fotos = GetFotosOrdenadas(a , h);

            return usu;
        }
    }
}