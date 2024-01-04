using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            browser = new ChromiumWebBrowser(@"https://www.loteriasonline.caixa.gov.br/silce-web/#/home");
            // Add it to the form and fill it to the form window.
            browser.FrameLoadEnd += Browser_FrameLoadEnd;

            browser.Dock = DockStyle.Fill;
            pnlPaginaLoteria.Controls.Add(browser);

            temListAposta = new List<string>();
        }

        private void frmLotoApostas_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                browser.GetSourceAsync().ContinueWith(taskHtml =>
                {
                    var html = taskHtml.Result;
                    if (html.Contains("<h5>Você tem mais de 18 anos?</h5>"))
                    {
                        if (html.Contains("botaosim"))
                            browser.ExecuteScriptAsync("document.getElementById('botaosim').click();");
                    }
                });
            }
        }

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
                browser.Load(url);
        }

        private async void Apostar(Volante volante)
        {
            try
            {
                using (ValorAposta frmvalorAposta = new ValorAposta())
                {
                    frmvalorAposta.OnCloseForm += FrmvalorAposta_OnCloseForm;
                    if (frmvalorAposta.ShowDialog() != DialogResult.OK)
                        throw new Exception("Informe o valor à apostar");
                }

                var quant = QuantidadeApostas();

                for (int i = 0; i < quant; i++)
                {
                    Sortear(volante);

                    if (listAposta != null && listAposta.Count > 0)
                    {
                        foreach (var item in listAposta)
                        {
                            JavascriptResponse result = await browser.EvaluateScriptAsync($"$(\"#n{item.ToString().PadLeft(2, '0')}\").trigger(\"click\")");

                            Thread.Sleep(200);

                            if (result == null)
                                continue;
                        }
                    }

                    switch(volante)
                    {
                        case Volante.Mega:
                            btnMegaSenaApostar.PerformClick();
                            break;
                        case Volante.LotoFacil:
                            btnLotoFacilApostar.PerformClick();
                            break;
                        case Volante.LotoMania:
                            btnLotoManiaApostar.PerformClick();
                            break;
                        case Volante.Quina:
                            btnQuinaApostar.PerformClick();
                            break;
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                btnHome.PerformClick();

                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private decimal QuantidadeApostas()
        {
            try
            {
                decimal valorApostado = 1;
                decimal valorAposta = 1;

                decimal.TryParse(txtValorApostado.Text.Replace("R$ ", "").Trim(), out valorApostado);
                decimal.TryParse(txtValorAposta.Text.Replace("R$ ", "").Trim(), out valorAposta);

                decimal quatidadeApostas = valorApostado / valorAposta;

                return Decimal.Round(quatidadeApostas, 0);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task ClickButton(string id)
        {
            JavascriptResponse javascriptResponse = await browser.GetMainFrame()
                .EvaluateScriptAsync(string.Format("document.getElementById(\"{0}\").click()", id));

            if (!javascriptResponse.Success)
                throw new Exception(javascriptResponse.Message);
        }

        private async void RecuperarValorAposta()
        {
            await browser.EvaluateScriptAsync("document.getElementById(\"valoraposta\").innerText;")
            .ContinueWith(x =>
            {
                var response = x.Result;

                if (response.Success && response.Result != null)
                {
                    txtValorAposta.Invoke((MethodInvoker) delegate
                    {
                        txtValorAposta.Text = response.Result.ToString();
                    });
                }
            });
        }

        private async void RecuperarValorTotalAposta()
        {
            await browser.EvaluateScriptAsync("document.getElementById(\"valortotalapostas\").innerText;")
           .ContinueWith(x =>
           {
               var response = x.Result;

               if (response.Success && response.Result != null)
               {
                   txtValorTotalAposta.Invoke((MethodInvoker) delegate
                   {
                       txtValorTotalAposta.Text = response.Result.ToString();
                   });
               }
           });
        }

        #endregion

        #region Eventos - Botoes

        private void btnHome_Click(object sender, EventArgs e)
        {
            LoadUrl(@"https://www.loteriasonline.caixa.gov.br/silce-web/#/home");
            HideSubMenu();
        }

        private async void btnCarrinho_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            await ClickButton("carrinho");
        }

        #region Mega-Sena

        private async void btnMegaSena_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlMegaSena);
            await ClickButton("data-jogo-mega-sena-especial");
            RecuperarValorAposta();

            txtValorApostado.Text = "R$ 0,00";
        }

        private async void btnMegaSenaSortear_Click(object sender, EventArgs e)
            => Apostar(Volante.Mega);

        private void btnMegaSenaApostar_Click(object sender, EventArgs e)
        {
            ClickButton("colocarnocarrinho");
            RecuperarValorTotalAposta();
        }

        private void FrmvalorAposta_OnCloseForm(string valor)
            => txtValorApostado.Text = string.Format("{0:C}", valor);

        #endregion

        #region Quina

        private async void btnQuina_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlQuina);
            await ClickButton("Quina");
            RecuperarValorAposta();

            txtValorApostado.Text = "R$ 0,00";
        }

        private void btnQuinaSortear_Click(object sender, EventArgs e)
            => Apostar(Volante.Quina);

        private void btnQuinaApostar_Click(object sender, EventArgs e)
        {
            ClickButton("colocarnocarrinho");
            RecuperarValorTotalAposta();
        }

        private void btnQuinaHistorico_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Loto-Facil

        private async void btnLotoFacil_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlLotoFacil);
            await ClickButton("Lotofácil");
            RecuperarValorAposta();

            txtValorApostado.Text = "R$ 0,00";
        }

        private void btnLotoFacilSortear_Click(object sender, EventArgs e)
        => Apostar(Volante.LotoFacil);

        private void btnLotoFacilApostar_Click(object sender, EventArgs e)
        {
            ClickButton("colocarnocarrinho");
            RecuperarValorTotalAposta();
        }

        private void btnLotoFacilHistorico_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Loto-Mania

        private async void btnLotoMania_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlLotoMania);
            await ClickButton("Lotomania");
            RecuperarValorAposta();

            txtValorApostado.Text = "R$ 0,00";
        }

        private void btnLotoManiaSortear_Click(object sender, EventArgs e)
        => Apostar(Volante.LotoMania);

        private void btnLotoManiaApostar_Click(object sender, EventArgs e)
        {
            ClickButton("colocarnocarrinho");
            RecuperarValorTotalAposta();
        }

        private void btnLotoManiaHistorico_Click(object sender, EventArgs e)
        {

        }

        #endregion 

        #endregion Eventos - Botoes
    }
}
