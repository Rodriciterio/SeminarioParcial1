using Bombones.Datos.Interfaces;
using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using Dapper;
using System.Data.SqlClient;

namespace Bombones.Datos.Repositorios
{
    public class RepositorioFormasDePago : IRepositorioFormasDePago
    {
        public RepositorioFormasDePago()
        {
            
        }

        public void Agregar(FormaDePago formaDePago, SqlConnection conn, SqlTransaction tran)
        {
            string insertQuery = @"INSERT INTO FormasDePago 
         (Descripcion) 
         VALUES (@Descripcion); 
         SELECT CAST(SCOPE_IDENTITY() as int)";

            int primaryKey = conn.QuerySingle<int>(insertQuery, formaDePago, tran);
            if (primaryKey > 0)
            {
                formaDePago.FormaDePagoId = primaryKey; // Asignar el ID de la fábrica
                return;
            }
            throw new Exception("No se pudo agregar la forma de pago");
        }

        public void Borrar(int formaDePagoId, SqlConnection conn, SqlTransaction tran)
        {
            var deleteQuery = @"DELETE FROM FormasDePago 
                WHERE FormaDePagoId=@FormaDePago";
            int registrosAfectados = conn
                .Execute(deleteQuery, new { formaDePagoId }, tran);
            if (registrosAfectados == 0)
            {
                throw new Exception("No se pudo borrar la forma de pago");
            }
        }

        public void Editar(FormaDePago formaDePago, SqlConnection conn, SqlTransaction tran)
        {
            var updateQuery = @"UPDATE FormasDePago
                SET Descripcion = @Descripcion,
                WHERE FormaDePagoId = @FormaDePagoId";

            int registrosAfectados = conn.Execute(updateQuery, formaDePago, tran);
            if (registrosAfectados == 0)
            {
                throw new Exception("No se pudo editar la forma de pago");
            }
        }

        public bool EstaRelacionado(int formaDePagoId, SqlConnection conn, SqlTransaction? tran = null)
        {
            var selectQuery = @"SELECT COUNT(*) FROM [Bombones]
                 WHERE FormaDePagoId = @FormaDePagoId";

            Console.WriteLine($"Verificando relación para FormaDePagoId: {formaDePagoId}");

            int count = conn.QuerySingle<int>(selectQuery, new { formaDePagoId = formaDePagoId }, tran);

            Console.WriteLine($"Registros encontrados: {count}");

            return count > 0;
        }

        public bool Existe(FormaDePago formaDePago, SqlConnection conn, SqlTransaction? tran = null)
        {
            string selectQuery = @"SELECT COUNT(*) FROM FormasDePago ";
            string condicionalQuery = string.Empty;
            string finalQuery = string.Empty;
            condicionalQuery = formaDePago.FormaDePagoId == 0 ?
                " WHERE Descripcion=@Descripcion " :
                " WHERE Descripcion=@Descripcion " +
                "AND FormaDePagoId<>@FormaDePagoId";
            finalQuery = string.Concat(selectQuery, condicionalQuery);
            return conn.QuerySingle<int>(finalQuery, formaDePago) > 0;
        }

        public FormaDePago? GetFormaDePagoPorId(int formaDePagoId, SqlConnection conn)
        {
            string selectQuery = @"SELECT FormaDePagoId, Descripcion
                FROM FormasDePago 
                WHERE FormaDePagoId=@FormaDePagoId";
            return conn.QuerySingleOrDefault<FormaDePago>(
                selectQuery, new { @FormaDePagoId = formaDePagoId });
        }

        public List<FormaDePagoListDto> GetLista(SqlConnection conn, SqlTransaction? tran = null)
        {
            try
            {
                string selectQuery = @"SELECT 
                f.FormaDePagoId,
                f.Descripcion
            FROM FormasDePago f
            INNER JOIN Ventas v ON f.FormaDePagoId = v.FormaDePagoId";

                return conn.Query<FormaDePagoListDto>(selectQuery, transaction: tran).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

      
    }
}
