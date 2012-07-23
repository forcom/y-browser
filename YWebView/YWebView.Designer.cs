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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
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
            this.Page.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Page_MouseClick);
            this.Page.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Page_MouseMove);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(538, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // YWebView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Page);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "YWebView";
            this.Size = new System.Drawing.Size(800, 600);
            this.Resize += new System.EventHandler(this.YWebView_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Page)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrNavigate;
        private System.Windows.Forms.Timer tmrShowPage;
        private System.Windows.Forms.PictureBox Page;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip;


    }
}
