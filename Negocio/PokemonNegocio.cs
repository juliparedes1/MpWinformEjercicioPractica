using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class PokemonNegocio
    {

        public List<Pokemon> listar()
        {
            List<Pokemon> lista = new List<Pokemon>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server =.\\SQLEXPRESS01; database =POKEDEX_DB; integrated security= true ";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Numero, Nombre, P.Id , P.Descripcion , UrlImagen, E.Descripcion as Tipo, E.Id as IdTipo , D.Descripcion as Debilidad, D.Id as IdDebilidad from POKEMONS P, ELEMENTOS E, ELEMENTOS D where E.Id = P.IdTipo and P.IdDebilidad = D.Id and activo != 0";
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    
                    aux.Id = (int)lector["Id"];
                    aux.Numero = lector.GetInt32(0);
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    if (!(lector["UrlImagen"] is DBNull))
                    {
                        aux.UrlImagen = (string)lector["UrlImagen"];
                    }
                    aux.Tipo = new Elemento();
                    aux.Tipo.Descripcion = (string)lector["Tipo"];
                    aux.Tipo.Id = (int)lector["IdTipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];
                    aux.Debilidad.Id = (int)lector["IdDebilidad"];
                    

                    lista.Add(aux);
                }

                
                return lista;
            }
            catch (Exception ex)
            {

                throw ex ;
            }
            finally
            {
                conexion.Close();
            }




        }


        public void agregar(Pokemon nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearParametros("@idTipo", nuevo.Tipo.Id);
                datos.setearParametros("@idDebilidad", nuevo.Debilidad.Id);
                datos.setearParametros("@UrlImagen", nuevo.UrlImagen);
                datos.setearConsulta($"insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen) values ({nuevo.Numero} ,'{nuevo.Nombre}','{nuevo.Descripcion}',1, @idTipo, @idDebilidad, @UrlImagen)");
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Pokemon modificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta($"update POKEMONS set Numero = {modificar.Numero}, Nombre = '{modificar.Nombre}', Descripcion = '{modificar.Descripcion}', UrlImagen = '{modificar.UrlImagen}', IdTipo = {modificar.Tipo.Id}, IdDebilidad = {modificar.Debilidad.Id} where Id = {modificar.Id}");
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarFisico(Pokemon eliminado)
        {
                AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta($"delete from POKEMONS where id = {eliminado.Id}");
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarLogico(Pokemon eliminado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta($"update POKEMONS set Activo = 0 where id = {eliminado.Id}");
                datos.ejecutarAccion();
            }
            catch (Exception ex )
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> listaPokemons = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select Numero, Nombre, P.Id , P.Descripcion , UrlImagen, E.Descripcion as Tipo, E.Id as IdTipo , D.Descripcion as Debilidad, D.Id as IdDebilidad from POKEMONS P, ELEMENTOS E, ELEMENTOS D where E.Id = P.IdTipo and P.IdDebilidad = D.Id and activo != 0 and ";

                if (campo == "Número")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Igual a":
                            consulta += "Numero = " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                            break;
                    }
                }
                else if(campo == "Nombre"){
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += $"Nombre like  '{filtro}%'";
                            break;
                        case "Termina con":
                            consulta += $"Nombre like '%{filtro}'";
                            break;      
                        case "Contiene":        
                            consulta += $"Nombre like '%{filtro}%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += $"P.Descripcion like  '{filtro}%'";
                            break;
                        case "Termina Con":
                            consulta += $"P.Descripcion like '%{filtro}'";
                            break;
                        case "Contiene":
                            consulta += $"P.Descripcion like '%{filtro}%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["UrlImagen"] is DBNull))
                    {
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];
                    }
                    aux.Tipo = new Elemento();
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];


                    listaPokemons.Add(aux);
                }


                return listaPokemons;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }



}
