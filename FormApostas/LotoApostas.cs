using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace FormApostas
{
    public partial class frmLotoApostas : Form
    {
        private enum Volante
        {
            [Description("Mega Sena")]
            Mega = 0,
            [Description("Loto Facil")]
            LotoFacil = 1,
            [Description("Loto Mania")]
            LotoMania = 2,
            [Description("Quina")]
            Quina = 3
        }

        private ChromiumWebBrowser browser;
        private static int qtdDezenas = 0;
        private static int maxRandom = 0;
        private static Volante volante;
        private static List<int> listAposta;
        private static List<string> temListAposta;

        public frmLotoApostas()
        {
            InitializeComponent();

            CustomizeDesingMenus();

            // Create a browser component
            browser = new ChromiumWebBrowser(@"https://www.loteriasonline.caixa.gov.br/silce-web/?utm_source=site_loterias&utm_medium=cross&utm_campaign=loteriasonline&utm_term=mega#/home");
            // Add it to the form and fill it to the form window.
            pnlPaginaLoteria.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            temListAposta = new List<string>();
        }

        private void frmLotoApostas_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
        }

        /*
         * Implementar consultar aposta
        private async void ExecuteJavaScriptBtn_Click(object sender, RoutedEventArgs e)
        {
            //Check if the browser can execute JavaScript and the ScriptTextBox is filled
            if (browser.CanExecuteJavascriptInMainFrame && !string.IsNullOrWhiteSpace(ScriptTextBox.Text))
            {
                //Evaluate javascript and remember the evaluation result
                JavascriptResponse response = await browser.EvaluateScriptAsync(ScriptTextBox.Text);

                if (response.Result != null)
                {
                    //Display the evaluation result if it is not empty
                    MessageBox.Show(response.Result.ToString(), "JavaScript Result");
                }
            }
        }
        */

        #region Metodos Formulario

        private void CustomizeDesingMenus()
        {
            pnlMegaSena.Visible = false;
            pnlQuina.Visible = false;
            pnlLotoFacil.Visible = false;
            pnlLotoMania.Visible = false;
        }

        private void HideSubMenu()
        {
            if (pnlMegaSena.Visible == true)
                pnlMegaSena.Visible = false;

            if (pnlQuina.Visible == true)
                pnlQuina.Visible = false;

            if (pnlLotoFacil.Visible == true)
                pnlLotoFacil.Visible = false;

            if (pnlLotoMania.Visible == true)
                pnlLotoMania.Visible = false;
        }

        private void ShowSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                HideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private Form activeForm = null;
        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlPaginaLoteria.Controls.Clear();
            pnlPaginaLoteria.Controls.Add(childForm);
            pnlPaginaLoteria.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                browser.Load(url);
            }
        }

        private void Sortear(Volante volante)
        {
            switch (volante)
            {
                case Volante.Mega:
                    maxRandom = 60;
                    qtdDezenas = 6;
                    break;

                case Volante.LotoFacil:
                    maxRandom = 25;
                    qtdDezenas = 15;
                    break;

                case Volante.LotoMania:
                    maxRandom = 100;
                    qtdDezenas = 50;
                    break;

                case Volante.Quina:
                    maxRandom = 80;
                    qtdDezenas = 5;
                    break;
            }

            var rand = new Random();
            listAposta = new List<int>();
            int number;
            for (int i = 0; i < qtdDezenas; i++)
            {
                do
                {
                    number = rand.Next(1, maxRandom);
                } while (listAposta.Contains(number));
                listAposta.Add(number);
            }

            listAposta.Sort();

            var result = temListAposta.Contains(string.Join(",", listAposta));
            if (result)
                Sortear(volante);
            else
                temListAposta.Add(string.Join(",", listAposta));
        }

        #endregion

        #region Mega-Sena

        private void btnMegaSena_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlMegaSena);

            LoadUrl(@"https://www.loteriasonline.caixa.gov.br/silce-web/?utm_source=site_loterias&utm_medium=cross&utm_campaign=loteriasonline&utm_term=mega#/mega-sena");
        }

        private async void btnMegaSenaSortear_Click(object sender, EventArgs e)
        {
            bool adicionarCarrinho = true;

            Sortear(Volante.Mega);

            if (listAposta != null && listAposta.Count > 0)
            {
                foreach (var item in listAposta)
                {
                    JavascriptResponse result = await browser.EvaluateScriptAsync($"$(\"#n{item.ToString().PadLeft(2, '0')}\").trigger(\"click\")");

                    Thread.Sleep(200);

                    if (result == null)
                        adicionarCarrinho = false;
                }
            }

            if (adicionarCarrinho)
            {
                if (MessageBox.Show("Deseja adicionar aposta no carrinho", "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    btnMegaSenaApostar.PerformClick();
            }
        }

        private void btnMegaSenaApostar_Click(object sender, EventArgs e)
        {
            browser.ExecuteScriptAsync("$(\"#colocarnocarrinho\").click()");
        }

        private void btnMegaSenaHistorico_Click(object sender, EventArgs e)
        {

        }


        #endregion

        #region Quina

        private void btnQuina_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlQuina);

            LoadUrl(@"https://www.loteriasonline.caixa.gov.br/silce-web/?utm_source=site_loterias&utm_medium=cross&utm_campaign=loteriasonline&utm_term=mega#/quina");
        }

        private void btnQuinaSortear_Click(object sender, EventArgs e)
        {
            Sortear(Volante.Quina);

            if (listAposta != null && listAposta.Count > 0)
            {
                foreach (var item in listAposta)
                {
                    browser.ExecuteScriptAsync($"$(\"#n{item.ToString().PadLeft(2, '0')}\").trigger(\"click\")");

                    Thread.Sleep(200);
                }
            }
        }

        private void btnQuinaApostar_Click(object sender, EventArgs e)
        {
            browser.ExecuteScriptAsync("$(\"#colocarnocarrinho\").click()");
        }

        private void btnQuinaHistorico_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Loto-Facil

        private void btnLotoFacil_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlLotoFacil);

            LoadUrl(@"https://www.loteriasonline.caixa.gov.br/silce-web/?utm_source=site_loterias&utm_medium=cross&utm_campaign=loteriasonline&utm_term=mega#/lotofacil");
        }

        private void btnLotoFacilSortear_Click(object sender, EventArgs e)
        {
            Sortear(Volante.LotoFacil);

            if (listAposta != null && listAposta.Count > 0)
            {
                foreach (var item in listAposta)
                {
                    browser.ExecuteScriptAsync($"$(\"#n{item.ToString().PadLeft(2, '0')}\").trigger(\"click\")");

                    Thread.Sleep(200);
                }
            }
        }

        private void btnLotoFacilApostar_Click(object sender, EventArgs e)
        {
            browser.ExecuteScriptAsync("$(\"#colocarnocarrinho\").click()");
        }

        private void btnLotoFacilHistorico_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Loto-Mania

        private void btnLotoMania_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlLotoMania);

            LoadUrl(@"https://www.loteriasonline.caixa.gov.br/silce-web/?utm_source=site_loterias&utm_medium=cross&utm_campaign=loteriasonline&utm_term=mega#/lotomania");
        }

        private void btnLotoManiaSortear_Click(object sender, EventArgs e)
        {
            Sortear(Volante.LotoMania);

            if (listAposta != null && listAposta.Count > 0)
            {
                foreach (var item in listAposta)
                {
                    browser.ExecuteScriptAsync($"$(\"#n{item.ToString().PadLeft(2, '0')}\").trigger(\"click\")");

                    Thread.Sleep(200);
                }
            }
        }

        private void btnLotoManiaApostar_Click(object sender, EventArgs e)
        {
            browser.ExecuteScriptAsync("$(\"#colocarnocarrinho\").click()");
        }

        private void btnLotoManiaHistorico_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void btnCarrinho_Click(object sender, EventArgs e)
        {
            HideSubMenu();

            browser.ExecuteScriptAsync("$(\"#carrinho\").click()");
        }
    }
}
