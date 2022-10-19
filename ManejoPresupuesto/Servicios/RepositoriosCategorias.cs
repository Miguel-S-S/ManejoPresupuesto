using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositoriosCategorias: IRepositorioCategorias
    {
        private readonly string connectionString;

        public RepositoriosCategorias(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MyConnection");
        }

        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId)
                                                                                                                Values(@Nombre, @TipoOperacionId, @UsuarioId);
                                                                                                              SELECT SCOPE_IDENTITY();", categoria);
            categoria.Id = id;
        }
        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(
                "SELECT * FROM Categorias WHERE UsuarioId = @UsuarioId", new { usuarioId });
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(
                "SELECT * FROM Categorias " +
                "WHERE UsuarioId = @UsuarioId AND TipoOperacionId = @TipoOperacionId", new { usuarioId, tipoOperacionId });
        }


        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(
                                 @"Select * FROM Categorias WHERE Id = @Id AND UsuarioId = @UsuarioId",
                                 new { id, usuarioId });
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Categorias
                           SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
                            Where Id = @Id", categoria);
        }



        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE Categorias WHERE Id = @Id", new { id });
        }
    }    
}
