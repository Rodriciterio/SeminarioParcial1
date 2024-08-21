using Bombones.Entidades.Entidades;

namespace Bombones.Windows
{
    public partial class frmFormaDePagoAE : Form
    {
        private readonly IServiceProvider? _servicios;
        private FormaDePago? formaDePago;

        public frmFormaDePagoAE(IServiceProvider? servicios)
        {
            InitializeComponent();
            _servicios = servicios;
        }

        private void frmFormaDePagoAE_Load(object sender, EventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (formaDePago is not null)
            {
                txtForma.Text = formaDePago.Descripcion;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (formaDePago is null)
                {
                    formaDePago = new FormaDePago();
                }
                formaDePago.Descripcion = txtForma.Text;
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtForma.Text.Trim()))
            {
                valido = false;
                errorProvider1.SetError(txtForma, "Descripcion requerida!!");
            }
            return valido;
        }

        public FormaDePago? GetFormaDePago()
        {
            return formaDePago;
        }

        internal void SetFormaDePago(FormaDePago formaDePago)
        {
            this.formaDePago = formaDePago;
        }



    }
}
