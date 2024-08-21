using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;

namespace Bombones.Servicios.Intefaces
{
    public interface IServiciosFormasDePago
    {
        void Borrar(int formaDePagoId);
        bool EstaRelacionado(int formaDePagoId);
        bool Existe(FormaDePago formaDePago);
        FormaDePago? GetFormaDePagoPorId(int formaDePagoId);
        List<FormaDePagoListDto> GetLista();
        void Guardar(FormaDePago formaDePago);
    }
}
