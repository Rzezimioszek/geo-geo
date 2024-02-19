using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Geo_geo.Class.FORMS {
    public partial class ucCzolowki : UserControl {
        public ucCzolowki() {
            InitializeComponent();

            cboPrec.Items.Insert(0, "0");
            cboPrec.Items.Insert(1, "1");
            cboPrec.Items.Insert(2, "2");
            cboPrec.Items.Insert(3, "3");
            cboPrec.SelectedIndex = 2;


        }

        private void lblFormat_Click(object sender, EventArgs e) {

        }

        private void ucCzolowki_Load(object sender, EventArgs e) {

        }

        private void btnCzolowki_Click(object sender, EventArgs e) {

            cCzolowki runner = new cCzolowki();

            string prec = this.cboPrec.GetItemText(this.cboPrec.SelectedItem);
            prec = "F" + prec; 
            string prefix = tbPrefix.Text;
            string sufix = tbSufix.Text;
            double height = Convert.ToDouble(tbHeight.Text);
            bool duplicate = this.cbDuplicate.Checked;
            double widthfactor = Convert.ToDouble(tbWF.Text);

            runner.PlaceDistOnLine(prec, prefix, sufix, height, duplicate, widthfactor);
        }

        private void lblSufix_Click(object sender, EventArgs e) {

        }

        private void lblPrefix_Click(object sender, EventArgs e) {

        }

        private void cbDuplicate_CheckedChanged(object sender, EventArgs e) {

        }

        private void lblHeight_Click(object sender, EventArgs e) {

        }
    }
}
