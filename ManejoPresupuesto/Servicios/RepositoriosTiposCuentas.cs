using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositoriosTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);

        Task Borrar(int id);
                
        Task Crear(TipoCuenta tipoCuenta);

        Task<bool> Existe(string nombre, int usuarioId);

        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);

        Task<TipoCuenta> obetnerPorId(int id, int usuarioId);

        Task ObtenerPorID();

        Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados);
    }



    public class RepositoriosTiposCuentas : IRepositoriosTiposCuentas
    {
        private readonly string connectionString;

        public RepositoriosTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MyConnection");
        }



        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                                                   ("TiposCuentas_Insertar",
                                                   new { usuarioId = tipoCuenta.UsuarioId,
                                                       nombre = tipoCuenta.Nombre },
                                                   commandType: System.Data.CommandType.StoredProcedure);
                                                   
            tipoCuenta.Id = id;
        }



        public async Task<bool> Existe(string nombre, int usuarioId)
        {

        
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"Select 1
                                                                        from TiposCuentas
                                                                        where Nombre = @Nombre and UsuarioId = @UsuarioId",
                                                                        new {nombre, usuarioId});
            return existe == 1;

        }



        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"Select Id, Nombre, Orden
                                                            From TiposCuentas
                                                            Where UsuarioId = @UsuarioId
                                                            Order By Orden",
                                                            new {usuarioId});
        }



        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas
                                            Set Nombre = @Nombre
                                            where Id = @Id", tipoCuenta);
        }


        public async Task<TipoCuenta> obetnerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"Select Id, Nombre, Orden
                                                                          From TiposCuentas
                                                                          Where Id = @Id AND UsuarioId = @UsuarioId",
                                                                          new { id, usuarioId });
        }



        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE TiposCuentas WHERE Id = @Id", new { id });
        }



        public async Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados)
        {
            var query = "update TiposCuentas Set Orden = @Orden Where Id = @Id;";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, tipoCuentasOrdenados);
        }



        public Task ObtenerPorID()
        {
            throw new NotImplementedException();
        }
    }
}
