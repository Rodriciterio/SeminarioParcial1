using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using System.Data.SqlClient;

namespace Bombones.Datos.Interfaces
{
    public interface IRepositorioFormasDePago
    {
        List<FormaDePagoListDto> GetLista(SqlConnection conn, SqlTransaction? tran = null);
        bool Existe(FormaDePago formaDePago, SqlConnection conn, SqlTransaction? tran = null);
        void Agregar(FormaDePago formaDePago, SqlConnection conn, SqlTransaction tran);
        void Editar(FormaDePago formaDePago, SqlConnection conn, SqlTransaction tran);
        void Borrar(int formaDePagoId, SqlConnection conn, SqlTransaction tran);
        bool EstaRelacionado(int formaDePagoId, SqlConnection conn, SqlTransaction? tran = null);
        FormaDePago? GetFormaDePagoPorId(int formaDePagoId, SqlConnection conn);
    }
}
