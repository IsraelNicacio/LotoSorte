using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FormApostas.Controles
{
    public class NumericTextBox : TextBox
    {
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.SelectAll();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            //troca o '.' por ','.
            if (e.KeyChar == '.' || e.KeyChar == ',')
            {
                e.KeyChar = ',';

                //Verifica se já existe alguma vírgula digitada no TextBox
                if (this.Text.Contains(","))
                    e.Handled = true;  //Caso exista, aborte
            }
            //Permite apenas a digitação de números e da tecla backspace   
            else if (!char.IsNumber(e.KeyChar) && !(e.KeyChar == (char) Keys.Back))
            {
                e.Handled = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if(e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete)
            {
                this.Text = string.Empty;
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.TextAlign = HorizontalAlignment.Right;
        }
    }
}
