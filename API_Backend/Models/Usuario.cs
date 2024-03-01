using MySql.Data.MySqlClient;

namespace API_Backend.Modelos
{
    public class Usuario
    {

        public int Id { get; set; }
        public string Nombre { get; set; }

        public Usuario() { }

        public Usuario(int Id, String Nombre)
        {
            this.Id = Id;
            this.Nombre = Nombre;
        }

        public Usuario TraerUsuario(string Nombre)
        {
            string SQL = "SELECT usuario.Id_Usuario, usuario.Nombre_Usuario " +
                "FROM red_social.usuario " +
                "WHERE usuario.Nombre_Usuario = '" + Nombre.Trim() + "' ";

            Usuario Usuario = null;
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
                        Usuario = new(Reader.GetInt32(0), Reader.GetString(1));
                    }
                }
            }
            catch (Exception) {}
            finally
            {
                Conexion.Close();
            }
            return Usuario;
        }

        public string CrearUsuario(string Nombre)
        {
            if (TraerUsuario(Nombre) == null)
            {
                string SQL = "INSERT INTO red_social.usuario (Nombre_Usuario) VALUES ('" + Nombre.Trim() + "')";
                ConexionBD.EjecutarSentenciasNonQuery(SQL);
                return "SE HA CREADO EL USUARIO " + '"' + Nombre + '"' + "\n";
            }
            else
            {
                return "NO SE HA PODIDO CREAR EL USUARIO. EL NOMBRE " + '"' + Nombre + '"' + " YA ESTÁ EN USO" + "\n";
            }
        }

        public List<int> TraerSeguidos(int IdUsuario)
        {
            List<int> Seguidos = new();
            string SQL = "SELECT seguidores.Id_Usuario_Seguido_Seguidores " +
                "FROM red_social.seguidores " +
                "WHERE seguidores.Id_Usuario_Seguidor_Seguidores = " + IdUsuario;
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
                        Seguidos.Add(Reader.GetInt32(0));

                    }
                }
            }
            catch (Exception) { }
            finally
            {
                Conexion.Close();
            }
            return Seguidos;
        }

        public string SeguirUsuario(string NombreSeguidor, string NombreSeguido)
        {
            Usuario UsuarioSeguidor = TraerUsuario(NombreSeguidor);
            Usuario UsuarioSeguido = TraerUsuario(NombreSeguido);

            if (UsuarioSeguidor == null)
            {
                return "EL USUARIO " + NombreSeguidor + " NO EXISTE" + "\n";
            }else if (UsuarioSeguido == null)
            {
                return "EL USUARIO " + NombreSeguido + " NO EXISTE" + "\n";
            }
            else
            {
                if (!TraerSeguidos(UsuarioSeguidor.Id).Contains(UsuarioSeguido.Id))
                {
                    string SQL = "INSERT INTO red_social.seguidores (Id_Usuario_Seguidor_Seguidores, Id_Usuario_Seguido_Seguidores) " +
                        "VALUES (" + UsuarioSeguidor .Id + ", " + UsuarioSeguido.Id + ")";
                    ConexionBD.EjecutarSentenciasNonQuery(SQL);
                    return NombreSeguidor + " EMPEZÓ A SEGUIR A " + NombreSeguido + "\n";
                }
                else
                {
                    return "@" + NombreSeguidor + " YA ESTÁ SIGUIENDO A @" + NombreSeguido + "\n";
                }
            }
        }
    }
}
