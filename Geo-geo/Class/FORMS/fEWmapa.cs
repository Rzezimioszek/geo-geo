using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Autodesk.AutoCAD.DatabaseServices.GripData;

namespace Geo_geo.Class.FORMS {
    public partial class fEWmapa : Form {

        private int desiredStartLocationX;
        private int desiredStartLocationY;

        public fEWmapa(int x, int y)
               : this() {
            // here store the value for x & y into instance variables
            this.desiredStartLocationX = x;
            this.desiredStartLocationY = y;

            Load += new EventHandler(fEWmapa_Load);
        }

        private void fEWmapa_Load(object sender, System.EventArgs e) {
            this.SetDesktopLocation(desiredStartLocationX, desiredStartLocationY);
        }


        public string ReturnValue { get; set; }
        public fEWmapa() {
            InitializeComponent();
        }

        private void btnOp1_Click(object sender, EventArgs e) {

            ReturnValue = "OTRN";

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnOp2_Click(object sender, EventArgs e) {

            ReturnValue = "OTRS";

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void btnOp3_Click(object sender, EventArgs e) {

            ReturnValue = "RTPW01";

            this.DialogResult = DialogResult.OK;
            this.Close();

        }


        private void btnOptOwn_Click(object sender, EventArgs e) {

            ReturnValue = this.txtMyCode.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void btnOp4_Click_1(object sender, EventArgs e) {
            ReturnValue = "RTPW02";

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void fEWmapa_Load_1(object sender, EventArgs e) {

        }
    }
}
