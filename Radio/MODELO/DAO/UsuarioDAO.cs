using Radio.MODELO.DB;
using Radio.MODELO.POCO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.MODELO.DAO
{
    class UsuarioDAO
    {
        public static List<Usuario> getUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader sqlDataReader;
                    String query = String.Format("SELECT U.U_ID, U.U_NOMBRE, U.U_USERNAME, U.U_PASSWORD, U.RAD_ID, U.ROL_ID FROM dbo.mus_usuarios U WHERE U.ROL_ID <> 1");
                    command = new SqlCommand(query, conn);
                    sqlDataReader = command.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Usuario user = new Usuario();
                        user.IdUsuario = (int)((!sqlDataReader.IsDBNull(0)) ? sqlDataReader.GetInt64(0) : 0);
                        user.Nombre = (!sqlDataReader.IsDBNull(1)) ? sqlDataReader.GetString(1) : "";
                        user.NombreUsuario = (!sqlDataReader.IsDBNull(2)) ? sqlDataReader.GetString(2) : "";
                        user.Contraseña = (!sqlDataReader.IsDBNull(3)) ? sqlDataReader.GetString(3) : "";
                        user.IdRadio = (!sqlDataReader.IsDBNull(4)) ? sqlDataReader.GetInt32(4) : 0;
                        user.IdRol = (int)((!sqlDataReader.IsDBNull(5)) ? sqlDataReader.GetInt64(5) : 0);
                        usuarios.Add(user);
                    }
                    sqlDataReader.Close();
                    command.Dispose();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return usuarios;
        }

        public static bool getUsuarioRepetido(String nombreUsuario)
        {
            bool esRegistrado = false;
            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader sqlDataReader;
                    String query = String.Format("SELECT * FROM dbo.mus_usuarios U WHERE U.U_USERNAME = '{0}'", nombreUsuario);
                    command = new SqlCommand(query, conn);
                    sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        esRegistrado = true;
                    }
                    else
                    {
                        esRegistrado = false;
                    }
                    sqlDataReader.Close();
                    command.Dispose();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return esRegistrado;
        }

        public static bool getUsuarioRepetidoModificar(String nombreUsuario, int idUsuario)
        {
            bool esModificado = false;
            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    SqlDataReader sqlDataReader;
                    String query = String.Format("SELECT * FROM dbo.mus_usuarios U WHERE U.U_USERNAME = '{0}' AND U.U_ID <> '{1}'", nombreUsuario, idUsuario);
                    command = new SqlCommand(query, conn);
                    sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        esModificado = true;
                    }
                    else
                    {
                        esModificado = false;
                    }
                    sqlDataReader.Close();
                    command.Dispose();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return esModificado;
        }

        public static bool registrarUsuario(Usuario usuario)
        {
            bool registrado = false;
            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (!getUsuarioRepetido(usuario.NombreUsuario))
                {
                    if (conn != null)
                    {
                        SqlCommand command;
                        String query = String.Format("INSERT INTO dbo.mus_usuarios (U_NOMBRE, U_USERNAME, U_PASSWORD, RAD_ID, ROL_ID) " +
                            "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');", usuario.Nombre, usuario.NombreUsuario, usuario.Contraseña, usuario.IdRadio, usuario.IdRol);
                        command = new SqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        registrado = true;
                    }
                }
                else
                {
                    registrado = false;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return registrado;
        }

        public static bool actualizarUsuario(Usuario usuario)
        {
            bool esActualizado = false;
            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (!getUsuarioRepetidoModificar(usuario.NombreUsuario, usuario.IdUsuario))
                {
                    if (conn != null)
                    {
                        SqlCommand command;
                        String query = String.Format("UPDATE dbo.mus_usuarios SET U_NOMBRE = '{0}' , U_USERNAME = '{1}', U_PASSWORD = '{2}', RAD_ID = '{3}', ROL_ID = '{4}' WHERE U_ID = '{5}';", usuario.Nombre, usuario.NombreUsuario, usuario.Contraseña, usuario.IdRadio, usuario.IdRol, usuario.IdUsuario);
                        command = new SqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        esActualizado = true;
                    }
                }
                else
                {
                    esActualizado = false;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return esActualizado;
        }

        public static void eliminarUsuario(int idUsuario)
        {
            SqlConnection conn = null;
            try
            {
                conn = ConexionBD.getConnection();
                if (conn != null)
                {
                    SqlCommand command;
                    String query = String.Format("DELETE FROM dbo.mus_usuarios " +
                        "WHERE U_ID = '{0}'; ", idUsuario);
                    command = new SqlCommand(query, conn);
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
