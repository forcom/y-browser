namespace YWebView
{
    partial class YWebView
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrNavigate = new System.Windows.Forms.Timer(this.components);
            this.tmrShowPage = new System.Windows.Forms.Timer(this.components);
            this.Page = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lblLocation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Page)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrShowPage
            // 
            this.tmrShowPage.Tick += new System.EventHandler(this.tmrShowPage_Tick);
            // 
            // Page
            // 
            this.Page.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Page.Dock = System.Windows.Forms.DockStyle.Top;
            this.Page.Location = new System.Drawing.Point(0, 0);
            this.Page.Name = "Page";
            this.Page.Size = new System.Drawing.Size(800, 50);
            this.Page.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Page.TabIndex = 0;
            this.Page.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(538, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(70, 431);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(35, 12);
            this.lblLocation.TabIndex = 2;
            this.lblLocation.Text = "label1";
            // 
            // YWebView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Page);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "YWebView";
            this.Size = new System.Drawing.Size(800, 600);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.YWebView_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.Page)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrNavigate;
        private System.Windows.Forms.Timer tmrShowPage;
        private System.Windows.Forms.PictureBox Page;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblLocation;


    }
}
