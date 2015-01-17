namespace Landlady.WIN
{
    partial class Mainfrm
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
            this.components = new System.ComponentModel.Container();
            this.tvApp = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Main_Setting = new System.Windows.Forms.Button();
            this.btn_Main_Service = new System.Windows.Forms.Button();
            this.btn_Main_Sales = new System.Windows.Forms.Button();
            this.btn_Main_Rent = new System.Windows.Forms.Button();
            this.tabRent = new System.Windows.Forms.TabPage();
            this.dgv_Rent = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.buildname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value_init = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value_end = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.roomcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabRent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Rent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvApp
            // 
            this.tvApp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvApp.Location = new System.Drawing.Point(0, 0);
            this.tvApp.Name = "tvApp";
            this.tvApp.Size = new System.Drawing.Size(207, 474);
            this.tvApp.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabMain);
            this.tabControl1.Controls.Add(this.tabRent);
            this.tabControl1.Location = new System.Drawing.Point(213, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(543, 540);
            this.tabControl1.TabIndex = 1;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tableLayoutPanel1);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(535, 514);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "主菜单";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btn_Main_Setting, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_Main_Service, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_Main_Sales, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_Main_Rent, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(62, 86);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(404, 338);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btn_Main_Setting
            // 
            this.btn_Main_Setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Main_Setting.Location = new System.Drawing.Point(232, 199);
            this.btn_Main_Setting.Margin = new System.Windows.Forms.Padding(30);
            this.btn_Main_Setting.Name = "btn_Main_Setting";
            this.btn_Main_Setting.Size = new System.Drawing.Size(142, 109);
            this.btn_Main_Setting.TabIndex = 3;
            this.btn_Main_Setting.Text = "设置";
            this.btn_Main_Setting.UseVisualStyleBackColor = true;
            // 
            // btn_Main_Service
            // 
            this.btn_Main_Service.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Main_Service.Location = new System.Drawing.Point(30, 199);
            this.btn_Main_Service.Margin = new System.Windows.Forms.Padding(30);
            this.btn_Main_Service.Name = "btn_Main_Service";
            this.btn_Main_Service.Size = new System.Drawing.Size(142, 109);
            this.btn_Main_Service.TabIndex = 2;
            this.btn_Main_Service.Text = "服务";
            this.btn_Main_Service.UseVisualStyleBackColor = true;
            // 
            // btn_Main_Sales
            // 
            this.btn_Main_Sales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Main_Sales.Location = new System.Drawing.Point(232, 30);
            this.btn_Main_Sales.Margin = new System.Windows.Forms.Padding(30);
            this.btn_Main_Sales.Name = "btn_Main_Sales";
            this.btn_Main_Sales.Size = new System.Drawing.Size(142, 109);
            this.btn_Main_Sales.TabIndex = 1;
            this.btn_Main_Sales.Text = "招租";
            this.btn_Main_Sales.UseVisualStyleBackColor = true;
            // 
            // btn_Main_Rent
            // 
            this.btn_Main_Rent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Main_Rent.Location = new System.Drawing.Point(30, 30);
            this.btn_Main_Rent.Margin = new System.Windows.Forms.Padding(30);
            this.btn_Main_Rent.Name = "btn_Main_Rent";
            this.btn_Main_Rent.Size = new System.Drawing.Size(142, 109);
            this.btn_Main_Rent.TabIndex = 0;
            this.btn_Main_Rent.Text = "收租";
            this.btn_Main_Rent.UseVisualStyleBackColor = true;
            // 
            // tabRent
            // 
            this.tabRent.Controls.Add(this.dgv_Rent);
            this.tabRent.Location = new System.Drawing.Point(4, 22);
            this.tabRent.Name = "tabRent";
            this.tabRent.Padding = new System.Windows.Forms.Padding(3);
            this.tabRent.Size = new System.Drawing.Size(535, 514);
            this.tabRent.TabIndex = 1;
            this.tabRent.Text = "收租";
            this.tabRent.UseVisualStyleBackColor = true;
            // 
            // dgv_Rent
            // 
            this.dgv_Rent.AllowUserToAddRows = false;
            this.dgv_Rent.AllowUserToDeleteRows = false;
            this.dgv_Rent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_Rent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Rent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.buildname,
            this.value_init,
            this.value_end,
            this.roomcode});
            this.dgv_Rent.Location = new System.Drawing.Point(6, 6);
            this.dgv_Rent.MultiSelect = false;
            this.dgv_Rent.Name = "dgv_Rent";
            this.dgv_Rent.RowTemplate.Height = 23;
            this.dgv_Rent.Size = new System.Drawing.Size(522, 500);
            this.dgv_Rent.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tvApp);
            this.splitContainer1.Size = new System.Drawing.Size(207, 540);
            this.splitContainer1.SplitterDistance = 62;
            this.splitContainer1.TabIndex = 2;
            // 
            // ilTree
            // 
            this.ilTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilTree.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // buildname
            // 
            this.buildname.HeaderText = "楼房";
            this.buildname.Name = "buildname";
            // 
            // value_init
            // 
            this.value_init.HeaderText = "水费|期初";
            this.value_init.Name = "value_init";
            // 
            // value_end
            // 
            this.value_end.HeaderText = "水费|期末";
            this.value_end.Name = "value_end";
            // 
            // roomcode
            // 
            this.roomcode.HeaderText = "房号";
            this.roomcode.Name = "roomcode";
            // 
            // Mainfrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 540);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Mainfrm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Mainfrm_FormClosed);
            this.Load += new System.EventHandler(this.Mainfrm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabRent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Rent)).EndInit();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvApp;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.TabPage tabRent;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_Main_Setting;
        private System.Windows.Forms.Button btn_Main_Service;
        private System.Windows.Forms.Button btn_Main_Sales;
        private System.Windows.Forms.Button btn_Main_Rent;
        private System.Windows.Forms.DataGridView dgv_Rent;
        private System.Windows.Forms.DataGridViewTextBoxColumn buildname;
        private System.Windows.Forms.DataGridViewTextBoxColumn value_init;
        private System.Windows.Forms.DataGridViewTextBoxColumn value_end;
        private System.Windows.Forms.DataGridViewTextBoxColumn roomcode;
    }
}

