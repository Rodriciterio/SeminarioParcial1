using Bombones.Entidades.Dtos;
using Bombones.Entidades.Entidades;
using Bombones.Entidades.Extensions;
using Bombones.Servicios.Intefaces;
using Bombones.Windows.Helpers;

namespace Bombones.Windows
{
    public partial class frmFormasDePago : Form
    {

        private readonly IServiceProvider? _serviceProvider;
        private readonly IServiciosFormasDePago? _servicios;
        private List<FormaDePagoListDto>? lista;
        private IServiciosFormasDePago? serviciosFormasDePago;

        public frmFormasDePago(IServiciosFormasDePago? servicios, IServiceProvider? serviceProvider)
        {
            InitializeComponent();
            _servicios = servicios;
            _serviceProvider = serviceProvider;
        }

        private void frmFormasDePago_Load(object sender, EventArgs e)
        {
            try
            {
                lista = _servicios?.GetLista();
                MostrarDatosEnGrilla();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MostrarDatosEnGrilla()
        {
            GridHelper.LimpiarGrilla(dgvDatos);
            if (lista is not null)
            {
                foreach (var fp in lista)
                {
                    var r = GridHelper.ConstruirFila(dgvDatos);
                    GridHelper.SetearFila(r, fp);
                    GridHelper.AgregarFila(r, dgvDatos);
                }
            }
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            frmFormaDePagoAE frm = new frmFormaDePagoAE(_serviceProvider);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            try
            {
                FormaDePago? formaDePago = frm.GetFormaDePago();
                if (formaDePago is null) return;
                if (!_servicios?.Existe(formaDePago) ?? false)
                {
                    _servicios?.Guardar(formaDePago);
                    var r = GridHelper.ConstruirFila(dgvDatos);
                    FormaDePagoListDto formaDePagoDto = FormaDePagoExtensions
                        .ToFormaDePagoListDto(formaDePago);
                    GridHelper.SetearFila(r, formaDePagoDto);
                    GridHelper.AgregarFila(r, dgvDatos);
                    MessageBox.Show("Registro agregado",
                                    "Mensaje",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Registro existente\nAlta denegada",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            }
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            if (r.Tag == null) return;
            var formaDePagoDto = (FormaDePagoListDto)r.Tag;
            DialogResult dr = MessageBox.Show($"¿Desea dar de baja la forma de pago {formaDePagoDto.Descripcion}?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No) return;
            try
            {
                if (!_servicios.EstaRelacionado(formaDePagoDto.FormaDePagoId))
                {
                    _servicios.Borrar(formaDePagoDto.FormaDePagoId);
                    // Actualiza la lista de datos después de la eliminación
                    lista = _servicios.GetLista(); // O cualquier método que recargue la lista
                    MostrarDatosEnGrilla();
                    MessageBox.Show("Registro eliminado!!", "Mensaje",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Registro relacionado!!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar registro: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 0)
            {
                return;
            }
            var r = dgvDatos.SelectedRows[0];
            if (r.Tag == null) return;
            FormaDePagoListDto formaDePagoDto = (FormaDePagoListDto)r.Tag;
            FormaDePago? formaDePago = _servicios?.GetFormaDePagoPorId(formaDePagoDto.FormaDePagoId);
            if (formaDePago is null) return;
            frmFormaDePagoAE frm = new frmFormaDePagoAE(_serviceProvider) { Text = "Editar Forma de pago" };
            frm.SetFormaDePago(formaDePago);
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.Cancel) return;
            formaDePago = frm.GetFormaDePago();

            if (formaDePago == null) return;
            try
            {
                if (!_servicios?.Existe(formaDePago) ?? false)
                {
                    _servicios?.Guardar(formaDePago);

                    formaDePagoDto = FormaDePagoExtensions.ToFormaDePagoListDto(formaDePago);

                    GridHelper.SetearFila(r, formaDePagoDto);
                    MessageBox.Show("Registro editado",
                                    "Mensaje",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Registro existente\nEdición denegada",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
