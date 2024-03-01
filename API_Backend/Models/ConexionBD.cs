using MySql.Data.MySqlClient;
using System;

namespace API_Backend.Modelos
{
    public class ConexionBD
    {
        public static MySqlConnection Conectar()
        {
            string servidor = "localhost";
            string bd = "red_social";
            string usuario = "root";
            string contrasena = "M@rio1002960089";
            string cadena_conexion = "database = " + bd + "; Data Source = " + servidor + "; User Id = " + usuario + "; Password = " + contrasena + "";
            try
            {
                MySqlConnection conexionBD = new(cadena_conexion);
                return conexionBD;
            }
            catch (MySqlException)
            {
                return null;
            }
        }

        public static bool EjecutarSentenciasNonQuery(string SQL)
        {
            bool bandera = false;
            MySqlConnection conexion = Conectar();
            conexion.Open();
            try
            {
                MySqlCommand comando = new(SQL, conexion);
                comando.ExecuteNonQuery();
                bandera = true;
            }
            catch (MySqlException) { }
            finally
            {
                conexion.Close();
            }
            return bandera;
        }
    }
}
