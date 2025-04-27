namespace Artı.Reports
{
    partial class Etiket
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraPrinting.BarCode.EAN13Generator eaN13Generator1 = new DevExpress.XtraPrinting.BarCode.EAN13Generator();
            DevExpress.XtraReports.UI.XRWatermark xrWatermark1 = new DevExpress.XtraReports.UI.XRWatermark();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbUrunAdi = new DevExpress.XtraReports.UI.XRLabel();
            this.lbFiyat = new DevExpress.XtraReports.UI.XRLabel();
            this.lbKdvDahil = new DevExpress.XtraReports.UI.XRLabel();
            this.lbBarkod = new DevExpress.XtraReports.UI.XRBarCode();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.lbUrunAdi,
            this.lbFiyat,
            this.lbKdvDahil,
            this.lbBarkod});
            this.Detail.HeightF = 80F;
            this.Detail.Name = "Detail";
            // 
            // xrLabel2
            // 
            this.xrLabel2.CanGrow = false;
            this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Verdana Pro Cond Black", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(247.31F, 35F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(32.69F, 35F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UsePadding = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "TL";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel2.WordWrap = false;
            // 
            // lbUrunAdi
            // 
            this.lbUrunAdi.CanGrow = false;
            this.lbUrunAdi.Font = new DevExpress.Drawing.DXFont("Verdana Pro Cond Black", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lbUrunAdi.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lbUrunAdi.Multiline = true;
            this.lbUrunAdi.Name = "lbUrunAdi";
            this.lbUrunAdi.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbUrunAdi.SizeF = new System.Drawing.SizeF(280F, 34.97005F);
            this.lbUrunAdi.StylePriority.UseFont = false;
            this.lbUrunAdi.StylePriority.UseTextAlignment = false;
            this.lbUrunAdi.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lbFiyat
            // 
            this.lbFiyat.CanGrow = false;
            this.lbFiyat.Font = new DevExpress.Drawing.DXFont("Verdana Pro Cond Black", 30F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lbFiyat.LocationFloat = new DevExpress.Utils.PointFloat(152.7405F, 34.97005F);
            this.lbFiyat.Name = "lbFiyat";
            this.lbFiyat.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbFiyat.SizeF = new System.Drawing.SizeF(94.56949F, 35.02993F);
            this.lbFiyat.StylePriority.UseFont = false;
            this.lbFiyat.StylePriority.UseTextAlignment = false;
            this.lbFiyat.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lbFiyat.TextTrimming = DevExpress.Drawing.DXStringTrimming.None;
            this.lbFiyat.WordWrap = false;
            // 
            // lbKdvDahil
            // 
            this.lbKdvDahil.CanGrow = false;
            this.lbKdvDahil.Font = new DevExpress.Drawing.DXFont("Verdana Pro Cond Black", 8.25F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(162)))});
            this.lbKdvDahil.LocationFloat = new DevExpress.Utils.PointFloat(152.7361F, 69.99998F);
            this.lbKdvDahil.Name = "lbKdvDahil";
            this.lbKdvDahil.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbKdvDahil.SizeF = new System.Drawing.SizeF(94.5739F, 10.00002F);
            this.lbKdvDahil.StylePriority.UseFont = false;
            this.lbKdvDahil.StylePriority.UseTextAlignment = false;
            this.lbKdvDahil.Text = "KDV DAHILDIR";
            this.lbKdvDahil.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lbKdvDahil.WordWrap = false;
            // 
            // lbBarkod
            // 
            this.lbBarkod.Alignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            this.lbBarkod.AutoModule = true;
            this.lbBarkod.Font = new DevExpress.Drawing.DXFont("Verdana Pro Cond Black", 6F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lbBarkod.LocationFloat = new DevExpress.Utils.PointFloat(0F, 35F);
            this.lbBarkod.Name = "lbBarkod";
            this.lbBarkod.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 5, 0, 100F);
            this.lbBarkod.SizeF = new System.Drawing.SizeF(152.7361F, 44.99999F);
            this.lbBarkod.StylePriority.UseFont = false;
            this.lbBarkod.StylePriority.UsePadding = false;
            this.lbBarkod.Symbology = eaN13Generator1;
            this.lbBarkod.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // Etiket
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail});
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(0F, 0F, 0F, 0F);
            this.PageHeight = 100;
            this.PageWidth = 280;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.PaperName = "Custom";
            this.Version = "23.2";
            xrWatermark1.Id = "Watermark1";
            this.Watermarks.Add(xrWatermark1);
            this.BeforePrint += new DevExpress.XtraReports.UI.BeforePrintEventHandler(this.Adisyon_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        public DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        public DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        public DevExpress.XtraReports.UI.XRLabel xrLabel2;
        public DevExpress.XtraReports.UI.XRLabel lbUrunAdi;
        public DevExpress.XtraReports.UI.XRLabel lbFiyat;
        public DevExpress.XtraReports.UI.XRLabel lbKdvDahil;
        public DevExpress.XtraReports.UI.XRBarCode lbBarkod;
    }
}
