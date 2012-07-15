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
            this.Page = new System.Windows.Forms.RichTextBox();
            this.tmrNavigate = new System.Windows.Forms.Timer(this.components);
            this.tmrShowPage = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Page
            // 
            this.Page.BackColor = System.Drawing.Color.White;
            this.Page.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Page.DetectUrls = false;
            this.Page.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Page.ForeColor = System.Drawing.Color.Black;
            this.Page.Location = new System.Drawing.Point(0, 0);
            this.Page.Name = "Page";
            this.Page.ReadOnly = true;
            this.Page.Size = new System.Drawing.Size(800, 600);
            this.Page.TabIndex = 0;
            this.Page.Text = "";
            // 
            // tmrShowPage
            // 
            this.tmrShowPage.Tick += new System.EventHandler(this.tmrShowPage_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(443, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // YWebView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Page);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "YWebView";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox Page;
        private System.Windows.Forms.Timer tmrNavigate;
        private System.Windows.Forms.Timer tmrShowPage;
        private System.Windows.Forms.Button button1;


    }
}
