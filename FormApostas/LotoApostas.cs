using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace FormApostas
{
    public partial class frmLotoApostas : Form
    {
        private ChromiumWebBrowser browser;

        public frmLotoApostas()
        {
            InitializeComponent();

            CustomizeDesingMenus();

            // Create a browser component
            browser = new ChromiumWebBrowser();
            // Add it to the form and fill it to the form window.
            pnlPaginaLoteria.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        private void frmLotoApostas_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
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
            if(pnlMegaSena.Visible == true)
                pnlMegaSena.Visible = false;

            if(pnlQuina.Visible == true)
                pnlQuina.Visible = false;

            if(pnlLotoFacil.Visible == true)
                pnlLotoFacil.Visible = false;

            if(pnlLotoMania.Visible == true)
                pnlLotoMania.Visible = false;
        }

        private void ShowSubMenu(Panel subMenu)
        {
            if(subMenu.Visible == false)
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
            if(activeForm != null)
                activeForm.Close();

            activeForm=childForm;
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

        #endregion

        #region Mega-Sena

        private void btnMegaSena_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlMegaSena);

            LoadUrl(@"https://www.loteriasonline.caixa.gov.br/silce-web/?utm_source=site_loterias&utm_medium=cross&utm_campaign=loteriasonline&utm_term=mega#/mega-sena");
        }

        private void btnMegaSenaSortear_Click(object sender, EventArgs e)
        {
            
        }

        private void btnMegaSenaApostar_Click(object sender, EventArgs e)
        {
            
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
            HideSubMenu();
        }

        private void btnQuinaApostar_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        private void btnQuinaHistorico_Click(object sender, EventArgs e)
        {
            HideSubMenu();
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
            HideSubMenu();
        }

        private void btnLotoFacilApostar_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        private void btnLotoFacilHistorico_Click(object sender, EventArgs e)
        {
            HideSubMenu();
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
            HideSubMenu();
        }

        private void btnLotoManiaApostar_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        private void btnLotoManiaHistorico_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        #endregion
    }
}
