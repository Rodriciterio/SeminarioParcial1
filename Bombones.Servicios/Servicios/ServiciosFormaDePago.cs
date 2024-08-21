using Bombones.Datos.Interfaces;
using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using Bombones.Servicios.Intefaces;
using System.Data.SqlClient;

namespace Bombones.Servicios.Servicios
{
    public class ServiciosFormaDePago : IServiciosFormasDePago
    {

        private readonly IRepositorioFormasDePago? _repositorio;
        private readonly string? _cadena;

        public ServiciosFormaDePago(IRepositorioFormasDePago? repositorio, String? cadena)
        {
            _repositorio = repositorio;
            _cadena = cadena;
        }

        public void Borrar(int formaDePagoId)
        {
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        _repositorio?.Borrar(formaDePagoId, conn, tran);
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool EstaRelacionado(int formaDePagoId)
        {
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                return _repositorio?.EstaRelacionado(formaDePagoId, conn) ?? true;
            }
        }

        public bool Existe(FormaDePago formaDePago)
        {
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                return _repositorio?.Existe(formaDePago, conn) ?? true;
            }
        }

        public FormaDePago? GetFormaDePagoPorId(int formaDePagoId)
        {
            using (var conn = new SqlConnection(_cadena))
            {
                return _repositorio?.GetFormaDePagoPorId(formaDePagoId, conn);
            }
        }

        public List<FormaDePagoListDto> GetLista()
        {
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                return _repositorio?.GetLista(conn) ?? new List<FormaDePagoListDto>();
            }
        }

        public void Guardar(FormaDePago formaDePago)
        {
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (formaDePago.FormaDePagoId == 0)
                        {
                            _repositorio?.Agregar(formaDePago, conn, tran);
                        }
                        else
                        {
                            _repositorio?.Editar(formaDePago, conn, tran);
                        }

                        tran.Commit(); // Guarda efectivamente
                    }
                    catch (Exception)
                    {
                        tran.Rollback(); // Revierte los cambios
                        throw;
                    }
                }
            }
        }

        List<FormaDePagoListDto> IServiciosFormasDePago.GetLista()
        {
            using (var conn = new SqlConnection(_cadena))
            {
                conn.Open();
                return _repositorio?.GetLista(conn) ?? new List<FormaDePagoListDto>();
            }
        }
    }
}
