using DevExpress.XtraReports.UI;
using System.ComponentModel;

namespace Artı.Reports
{
    public partial class Etiket : DevExpress.XtraReports.UI.XtraReport
    {
        public Etiket()
        {
            InitializeComponent();
        } 

        private void Adisyon_BeforePrint(object sender, CancelEventArgs e)
        { 
           
        }
    }
}