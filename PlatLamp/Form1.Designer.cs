namespace PlatLamp
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblConnection = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.btn_getSpaceDetail = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtElectric = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(646, 16);
            this.lblConnection.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(0, 12);
            this.lblConnection.TabIndex = 6;
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(9, 86);
            this.txtMsg.Margin = new System.Windows.Forms.Padding(2);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(601, 359);
            this.txtMsg.TabIndex = 7;
            // 
            // btn_getSpaceDetail
            // 
            this.btn_getSpaceDetail.Location = new System.Drawing.Point(20, 52);
            this.btn_getSpaceDetail.Margin = new System.Windows.Forms.Padding(2);
            this.btn_getSpaceDetail.Name = "btn_getSpaceDetail";
            this.btn_getSpaceDetail.Size = new System.Drawing.Size(91, 28);
            this.btn_getSpaceDetail.TabIndex = 8;
            this.btn_getSpaceDetail.Text = "取得在席資訊";
            this.btn_getSpaceDetail.UseVisualStyleBackColor = true;
            this.btn_getSpaceDetail.Click += new System.EventHandler(this.btn_getSpaceDetail_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(646, 16);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "電動車:";
            // 
            // txtElectric
            // 
            this.txtElectric.Location = new System.Drawing.Point(627, 38);
            this.txtElectric.Margin = new System.Windows.Forms.Padding(2);
            this.txtElectric.Multiline = true;
            this.txtElectric.Name = "txtElectric";
            this.txtElectric.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtElectric.Size = new System.Drawing.Size(87, 407);
            this.txtElectric.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(134, 52);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 28);
            this.button1.TabIndex = 16;
            this.button1.Text = "分析狀態";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 459);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtElectric);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_getSpaceDetail);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.lblConnection);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button btn_getSpaceDetail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtElectric;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
    }
}

