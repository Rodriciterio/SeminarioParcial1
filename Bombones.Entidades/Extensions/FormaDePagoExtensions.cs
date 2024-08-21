using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;

namespace Bombones.Entidades.Extensions
{
    public static class FormaDePagoExtensions
    {
        public static FormaDePagoListDto ToFormaDePagoListDto(FormaDePago formaDePago)
        {
            return new FormaDePagoListDto
            {
                FormaDePagoId = formaDePago.FormaDePagoId,
                Descripcion = formaDePago.Descripcion
            };
        }

    }
}
