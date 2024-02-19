using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo_geo.Class.FORMS {
    public partial class ucMain : UserControl {
        public ucMain() {
            InitializeComponent();

            // formatowanie pliu txt
            cboFormat.Items.Insert(0, "nxyh");
            cboFormat.Items.Insert(1, "nyxh");
            cboFormat.Items.Insert(2, "xyh");
            cboFormat.Items.Insert(3, "yxh");
            cboFormat.SelectedIndex = 1;
            //

            // Rodzaje obiektów
            cboTyp.Items.Insert(0, "Tekst");
            cboTyp.Items.Insert(1, "MTekst");
            cboTyp.Items.Insert(2, "Sfera");
            cboTyp.Items.Insert(3, "AcPoint");
            cboTyp.Items.Insert(4, "Slup");
            cboTyp.Items.Insert(5, "Okrag");
            cboTyp.Items.Insert(6, "Blok");
            cboTyp.SelectedIndex = 0;
            //

            lblSkala.Text = "Wys.:";

            // Separatory
            cboSep.Items.Insert(0, "Spacja");
            cboSep.Items.Insert(1, "Tabulacja");
            cboSep.Items.Insert(2, "Przecinek");
            cboSep.Items.Insert(3, "Średnik");
            cboSep.SelectedIndex = 1;
            //

            // Typy punktów
            cboAcPoint.Items.Insert(0, "0");
            cboAcPoint.Items.Insert(1, "1");
            cboAcPoint.Items.Insert(2, "2");
            cboAcPoint.Items.Insert(3, "3");
            cboAcPoint.Items.Insert(4, "4");

            cboAcPoint.Items.Insert(5, "32");
            cboAcPoint.Items.Insert(6, "33");
            cboAcPoint.Items.Insert(7, "34");
            cboAcPoint.Items.Insert(8, "35");
            cboAcPoint.Items.Insert(9, "36");

            cboAcPoint.Items.Insert(10, "64");
            cboAcPoint.Items.Insert(11, "65");
            cboAcPoint.Items.Insert(12, "66");
            cboAcPoint.Items.Insert(13, "67");
            cboAcPoint.Items.Insert(14, "68");

            cboAcPoint.Items.Insert(15, "96");
            cboAcPoint.Items.Insert(16, "97");
            cboAcPoint.Items.Insert(17, "98");
            cboAcPoint.Items.Insert(18, "99");
            cboAcPoint.Items.Insert(19, "100");
            
            cboAcPoint.SelectedIndex = 7;
            //

            bool bloks_imported = false;

            int lp = 0;

            cboBlok.Items.Insert(0, "none");

            if (!bloks_imported) {

                cImportPik imp = new cImportPik();
                imp.ImportBlocks();
                bloks_imported = true;

                List<string>  blocks = imp.ListBlockReferences();

                foreach (string bl in blocks) {

                    cboBlok.Items.Insert(lp, bl); lp++;
                }

                if (lp > 0) {
                    cboBlok.SelectedIndex = 0;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void btnImport_Click(object sender, EventArgs e) {

            cImportPik imp = new Geo_geo.Class.cImportPik();


            string type = this.cboTyp.GetItemText(this.cboTyp.SelectedItem);
            string format = this.cboFormat.GetItemText(this.cboFormat.SelectedItem);
            double skala = double.Parse(this.tbSkala.Text);
            bool NrH = this.cbZamianaH.Checked;
            bool h0 = this.cbH0.Checked;
            string separator = this.cboSep.GetItemText(this.cboSep.SelectedItem);
            string typAcPoint = this.cboAcPoint.GetItemText(this.cboAcPoint.SelectedItem);

            double ox = double.Parse(tbOX.Text);
            double oy = double.Parse(tbOY.Text);

            string blokname = cboBlok.Text;

            int skip = int.Parse(nupRow.Value.ToString());

            imp.LoadPointsFromTxtForm(type, format, skala, NrH, h0, separator, typAcPoint, ox, oy, blokname, skip);

        }

        private void cbH0_CheckedChanged(object sender, EventArgs e) {

        }

        private void ucMain_Load(object sender, EventArgs e) {
        }

        private void cboTyp_SelectedIndexChanged(object sender, EventArgs e) {

            string chosen = this.cboTyp.GetItemText(this.cboTyp.SelectedItem);
            this.cboBlok.Visible = false;
            tbSkala.Visible = true;
            cboAtr.Visible = false;

            if (chosen == "AcPoint") {

                this.cboAcPoint.Visible = true;
                this.lblAcPoint.Visible = true;
                this.cbZamianaH.Visible = false;
                lblSkala.Text = "Skala:";

            } else if (chosen == "Blok") {
                this.cboAcPoint.Visible = false;
                this.lblAcPoint.Visible = false;
                this.cbZamianaH.Visible = false;
                this.cboBlok.Visible = true;

                lblSkala.Text = "Typ bloku:";
                tbSkala.Visible = false;
                //cboAtr.Visible = true;


            } else {

                this.cboAcPoint.Visible = false;
                this.lblAcPoint.Visible = false;
                this.cbZamianaH.Visible = true;

                if ((chosen == "Tekst" ) || (chosen == "MTekst")) {

                    lblSkala.Text = "Wys.:";

                }
                else {
                    lblSkala.Text = "R:";
                    this.cbZamianaH.Visible = false;
                }

            }

        }

        private void lbRow_Click(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void tbOX_TextChanged(object sender, EventArgs e) {


        }

        private void cboBlok_SelectedIndexChanged(object sender, EventArgs e) {

            //changeListObj();




        }

        private void changeListObj() {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            try {
                if (cboBlok.Visible) {
                    cImportPik imp = new cImportPik();

                    string name = cboBlok.Text.ToString();
                    ed.WriteMessage($"\nCase: {name}");

                    List<string> blocks = imp.ListBlockAtr(name);

                    int lp = 0;

                    cboAtr.Items.Clear();

                    if (blocks.Count > 0) {

                        foreach (string bl in blocks) {

                            cboAtr.Items.Insert(lp, bl); lp++;
                        }

                        if (lp > 0) {
                            cboAtr.SelectedIndex = 0;
                        }
                    }
                }
            } catch (Exception ex) {
                //ed.WriteMessage($"\n{ex.ToString()}");
            }

        }
    }
}
