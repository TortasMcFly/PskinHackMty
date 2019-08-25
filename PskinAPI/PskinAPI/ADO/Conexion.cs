using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PskinAPI.ADO
{
    public class Conexion
    {
        private static bool flag = false;
        private static Conexion conexion = null;
        private static readonly string cadenaConexion = ConfigurationManager.ConnectionStrings["PskinCS"].ConnectionString;
        private static MySqlConnection Connection;

        private Conexion()
        {
            Connection = new MySqlConnection(cadenaConexion);
        }

        public static Conexion GetInstance()
        {
            if (flag)
            {
                return conexion;
            }
            else
            {
                conexion = new Conexion();
                flag = true;
                return conexion;
            }
        }

        public MySqlConnection GetConnection()
        {
            return Connection;
        }

        public static void finalize()
        {
            flag = false;
        }
    }
}