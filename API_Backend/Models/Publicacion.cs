using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Backend.Modelos
{
    public class Publicacion
    {
        public string Usuario { get; set; }

        public string Mensaje { get; set; }

        public DateTime Hora { get; set; }

        public Publicacion(){}

        public Publicacion(string Usuario, string Mensaje, DateTime Hora) 
        {
            this.Usuario = Usuario;
            this.Mensaje = Mensaje;
            this.Hora = Hora;
        }

        public string HacerPublicacion(Usuario Usuario, string Mensaje)
        {
            string Hora = DateTime.Now.ToString("HH:mm");
            string SQL = "INSERT INTO red_social.publicacion " +
                "(Mensaje_Publicacion, Hora_Publicacion, Id_Usuario_Publicacion) " +
                "VALUES ('" + Mensaje + "', '" + Hora + "', " + Usuario.Id + ")";
            ConexionBD.EjecutarSentenciasNonQuery(SQL);
            return Usuario.Nombre + " POSTED -> " + '"' + Mensaje + '"' + " @" + Hora + "\n";
        }

        public string TraerPublicaciones(int IdUsuario)
        {
            List<Publicacion> PublicacionesDesordenadas = new();
            string PublicacionesOrdenadas = "";
            string SQL = "SELECT publicacion.Mensaje_Publicacion, usuario.Nombre_Usuario, publicacion.Hora_Publicacion " +
                "FROM red_social.publicacion INNER JOIN red_social.usuario " +
                "ON red_social.publicacion.Id_Usuario_Publicacion = red_social.usuario.Id_Usuario " +
                "AND red_social.usuario.Id_Usuario = " + IdUsuario + " " +
                "ORDER BY publicacion.Hora_Publicacion ASC";
            MySqlConnection Conexion = ConexionBD.Conectar();
            Conexion.Open();
            try
            {
                MySqlCommand Comando = new(SQL, Conexion);
                MySqlDataReader Reader = Comando.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        Usuario Usuario = new();
                        PublicacionesDesordenadas.Add(new Publicacion(Reader.GetString(1), Reader.GetString(0), DateTime.ParseExact(Reader.GetString(2), "H:mm", null) ));
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                Conexion.Close();
            }
            List<Publicacion> ListaOrdenada = PublicacionesDesordenadas.OrderBy(Elm => Elm.Hora).ToList();
            
            foreach (var Publicacion in ListaOrdenada)
            {
                PublicacionesOrdenadas += '"' + Publicacion.Mensaje + '"' + " @" + Publicacion.Usuario + " @" + Publicacion.Hora.ToString("HH:mm") + "\n";
            }
            return PublicacionesOrdenadas;
        }

    }
}
