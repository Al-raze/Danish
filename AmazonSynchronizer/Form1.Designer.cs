namespace AmazonSynchronizer
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSynchronize = new System.Windows.Forms.Button();
            this.btnSynchronizeImages = new System.Windows.Forms.Button();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.btnReportSync = new System.Windows.Forms.Button();
            this.txtOrderNumber = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtOrderNum = new System.Windows.Forms.TextBox();
            this.btnSyncOrder = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dateTimeFrom
            // 
            this.dateTimeFrom.Location = new System.Drawing.Point(48, 55);
            this.dateTimeFrom.Name = "dateTimeFrom";
            this.dateTimeFrom.Size = new System.Drawing.Size(200, 20);
            this.dateTimeFrom.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "From Date";
            // 
            // dateTimeTo
            // 
            this.dateTimeTo.Location = new System.Drawing.Point(48, 114);
            this.dateTimeTo.Name = "dateTimeTo";
            this.dateTimeTo.Size = new System.Drawing.Size(200, 20);
            this.dateTimeTo.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "To Date";
            // 
            // btnSynchronize
            // 
            this.btnSynchronize.Location = new System.Drawing.Point(296, 56);
            this.btnSynchronize.Name = "btnSynchronize";
            this.btnSynchronize.Size = new System.Drawing.Size(200, 23);
            this.btnSynchronize.TabIndex = 4;
            this.btnSynchronize.Text = "Synchronize Orders";
            this.btnSynchronize.UseVisualStyleBackColor = true;
            this.btnSynchronize.Click += new System.EventHandler(this.btnSynchronize_Click);
            // 
            // btnSynchronizeImages
            // 
            this.btnSynchronizeImages.Location = new System.Drawing.Point(296, 114);
            this.btnSynchronizeImages.Name = "btnSynchronizeImages";
            this.btnSynchronizeImages.Size = new System.Drawing.Size(197, 23);
            this.btnSynchronizeImages.TabIndex = 5;
            this.btnSynchronizeImages.Text = "Synchronize Images";
            this.btnSynchronizeImages.UseVisualStyleBackColor = true;
            this.btnSynchronizeImages.Click += new System.EventHandler(this.btnSynchronizeImages_Click);
            // 
            // txtMessages
            // 
            this.txtMessages.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtMessages.ForeColor = System.Drawing.Color.GreenYellow;
            this.txtMessages.Location = new System.Drawing.Point(27, 152);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.Size = new System.Drawing.Size(688, 283);
            this.txtMessages.TabIndex = 14;
            this.txtMessages.Text = "Amazon Synchronizer Console";
            this.txtMessages.TextChanged += new System.EventHandler(this.txtMessages_TextChanged);
            // 
            // btnReportSync
            // 
            this.btnReportSync.Enabled = false;
            this.btnReportSync.Location = new System.Drawing.Point(296, 86);
            this.btnReportSync.Name = "btnReportSync";
            this.btnReportSync.Size = new System.Drawing.Size(197, 23);
            this.btnReportSync.TabIndex = 15;
            this.btnReportSync.Text = "Sync Report";
            this.btnReportSync.UseVisualStyleBackColor = true;
            this.btnReportSync.Click += new System.EventHandler(this.btnReportSync_Click);
            // 
            // txtOrderNumber
            // 
            this.txtOrderNumber.ForeColor = System.Drawing.Color.Tomato;
            this.txtOrderNumber.Location = new System.Drawing.Point(525, 59);
            this.txtOrderNumber.Name = "txtOrderNumber";
            this.txtOrderNumber.ReadOnly = true;
            this.txtOrderNumber.Size = new System.Drawing.Size(365, 20);
            this.txtOrderNumber.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(525, 85);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Sync Order File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtOrderNum
            // 
            this.txtOrderNum.Location = new System.Drawing.Point(525, 116);
            this.txtOrderNum.Name = "txtOrderNum";
            this.txtOrderNum.Size = new System.Drawing.Size(227, 20);
            this.txtOrderNum.TabIndex = 18;
            // 
            // btnSyncOrder
            // 
            this.btnSyncOrder.Location = new System.Drawing.Point(759, 113);
            this.btnSyncOrder.Name = "btnSyncOrder";
            this.btnSyncOrder.Size = new System.Drawing.Size(99, 23);
            this.btnSyncOrder.TabIndex = 19;
            this.btnSyncOrder.Text = "Sync Order";
            this.btnSyncOrder.UseVisualStyleBackColor = true;
            this.btnSyncOrder.Click += new System.EventHandler(this.btnSyncOrder_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(656, 86);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(124, 23);
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Text = "Refresh Order Data";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 455);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSyncOrder);
            this.Controls.Add(this.txtOrderNum);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtOrderNumber);
            this.Controls.Add(this.btnReportSync);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.btnSynchronizeImages);
            this.Controls.Add(this.btnSynchronize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimeTo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimeFrom);
            this.Name = "Form1";
            this.Text = "Amazon Synchronization Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimeFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimeTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSynchronize;
        private System.Windows.Forms.Button btnSynchronizeImages;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.Button btnReportSync;
        private System.Windows.Forms.TextBox txtOrderNumber;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtOrderNum;
        private System.Windows.Forms.Button btnSyncOrder;
        private System.Windows.Forms.Button btnRefresh;
    }
}

