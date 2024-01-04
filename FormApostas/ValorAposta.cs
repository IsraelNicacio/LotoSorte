using System;
using System.Windows.Forms;

namespace FormApostas
{
    public partial class ValorAposta : Form
    {
        public ValorAposta()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtValorApostado.Text))
                    throw new Exception("Informe o valor");

                OnClose(string.Format("{0:c}", txtValorApostado.Text));

                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public delegate void OnCloseFormHandle(string valor);
        public event OnCloseFormHandle OnCloseForm;
        protected virtual void OnClose(string valor)
        {
            if (OnCloseForm != null)
                OnCloseForm(valor);
        }
    }
}
