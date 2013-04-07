namespace Zapoctak
{
    partial class Dama
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
            this.MStrip = new System.Windows.Forms.MenuStrip();
            this.TSCHra = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMINova = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMINastaveni = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIKonec = new System.Windows.Forms.ToolStripMenuItem();
            this.TSCTest = new System.Windows.Forms.ToolStripMenuItem();
            this.Test1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Test2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Test3 = new System.Windows.Forms.ToolStripMenuItem();
            this.Test4 = new System.Windows.Forms.ToolStripMenuItem();
            this.Test5 = new System.Windows.Forms.ToolStripMenuItem();
            this.Test6 = new System.Windows.Forms.ToolStripMenuItem();
            this.Test7 = new System.Windows.Forms.ToolStripMenuItem();
            this.Test8 = new System.Windows.Forms.ToolStripMenuItem();
            this.Test9 = new System.Windows.Forms.ToolStripMenuItem();
            this.Test10 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSCNapoveda = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIOhre = new System.Windows.Forms.ToolStripMenuItem();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.MStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MStrip
            // 
            this.MStrip.AutoSize = false;
            this.MStrip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSCHra,
            this.TSCTest,
            this.TSCNapoveda});
            this.MStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.MStrip.Location = new System.Drawing.Point(0, 0);
            this.MStrip.Name = "MStrip";
            this.MStrip.Size = new System.Drawing.Size(794, 30);
            this.MStrip.Stretch = false;
            this.MStrip.TabIndex = 0;
            this.MStrip.Text = "menuStrip1";
            this.MStrip.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Dama_Click);
            this.MStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Dama_Click);
            this.MStrip.Click += new System.EventHandler(this.Dama_Click);
            // 
            // TSCHra
            // 
            this.TSCHra.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMINova,
            this.TSMINastaveni,
            this.toolStripSeparator1,
            this.TSMIKonec});
            this.TSCHra.Name = "TSCHra";
            this.TSCHra.Size = new System.Drawing.Size(38, 26);
            this.TSCHra.Text = "Hra";
            // 
            // TSMINova
            // 
            this.TSMINova.Name = "TSMINova";
            this.TSMINova.Size = new System.Drawing.Size(126, 22);
            this.TSMINova.Text = "Nová Hra";
            this.TSMINova.Click += new System.EventHandler(this.TSMINova_Click);
            // 
            // TSMINastaveni
            // 
            this.TSMINastaveni.Name = "TSMINastaveni";
            this.TSMINastaveni.Size = new System.Drawing.Size(126, 22);
            this.TSMINastaveni.Text = "Nastavení";
            this.TSMINastaveni.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(123, 6);
            // 
            // TSMIKonec
            // 
            this.TSMIKonec.Name = "TSMIKonec";
            this.TSMIKonec.Size = new System.Drawing.Size(126, 22);
            this.TSMIKonec.Text = "Konec";
            this.TSMIKonec.Click += new System.EventHandler(this.TSMIKonec_Click);
            // 
            // TSCTest
            // 
            this.TSCTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Test1,
            this.Test2,
            this.Test3,
            this.Test4,
            this.Test5,
            this.Test6,
            this.Test7,
            this.Test8,
            this.Test9,
            this.Test10});
            this.TSCTest.Name = "TSCTest";
            this.TSCTest.Size = new System.Drawing.Size(96, 26);
            this.TSCTest.Text = "Testovaci sady";
            // 
            // Test1
            // 
            this.Test1.Name = "Test1";
            this.Test1.Size = new System.Drawing.Size(152, 22);
            this.Test1.Text = "Test1";
            this.Test1.ToolTipText = "Testovani zacykleni pri skakani";
            this.Test1.Click += new System.EventHandler(this.Test1_Click);
            // 
            // Test2
            // 
            this.Test2.Name = "Test2";
            this.Test2.Size = new System.Drawing.Size(152, 22);
            this.Test2.Text = "Test2";
            this.Test2.ToolTipText = "Testovani zmeny figurky na damu";
            this.Test2.Click += new System.EventHandler(this.Test2_Click);
            // 
            // Test3
            // 
            this.Test3.Name = "Test3";
            this.Test3.Size = new System.Drawing.Size(152, 22);
            this.Test3.Text = "Test3";
            this.Test3.ToolTipText = "Testovani multiple skoku";
            this.Test3.Click += new System.EventHandler(this.Test3_Click);
            // 
            // Test4
            // 
            this.Test4.Name = "Test4";
            this.Test4.Size = new System.Drawing.Size(152, 22);
            this.Test4.Text = "Test4";
            this.Test4.Click += new System.EventHandler(this.Test4_Click);
            // 
            // Test5
            // 
            this.Test5.Name = "Test5";
            this.Test5.Size = new System.Drawing.Size(152, 22);
            this.Test5.Text = "Test5";
            this.Test5.Click += new System.EventHandler(this.Test5_Click);
            // 
            // Test6
            // 
            this.Test6.Name = "Test6";
            this.Test6.Size = new System.Drawing.Size(152, 22);
            this.Test6.Text = "Test6";
            this.Test6.Click += new System.EventHandler(this.Test6_Click);
            // 
            // Test7
            // 
            this.Test7.Name = "Test7";
            this.Test7.Size = new System.Drawing.Size(152, 22);
            this.Test7.Text = "Test7";
            this.Test7.Click += new System.EventHandler(this.Test7_Click);
            // 
            // Test8
            // 
            this.Test8.Name = "Test8";
            this.Test8.Size = new System.Drawing.Size(152, 22);
            this.Test8.Text = "Test8";
            this.Test8.Click += new System.EventHandler(this.Test8_Click);
            // 
            // Test9
            // 
            this.Test9.Name = "Test9";
            this.Test9.Size = new System.Drawing.Size(152, 22);
            this.Test9.Text = "Test9";
            this.Test9.Click += new System.EventHandler(this.Test9_Click);
            // 
            // Test10
            // 
            this.Test10.Name = "Test10";
            this.Test10.Size = new System.Drawing.Size(152, 22);
            this.Test10.Text = "Test10";
            this.Test10.Click += new System.EventHandler(this.Test10_Click);
            // 
            // TSCNapoveda
            // 
            this.TSCNapoveda.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIOhre});
            this.TSCNapoveda.Name = "TSCNapoveda";
            this.TSCNapoveda.Size = new System.Drawing.Size(73, 26);
            this.TSCNapoveda.Text = "Nápověda";
            // 
            // TSMIOhre
            // 
            this.TSMIOhre.Name = "TSMIOhre";
            this.TSMIOhre.Size = new System.Drawing.Size(137, 22);
            this.TSMIOhre.Text = "O hře Dáma";
            // 
            // timer2
            // 
            this.timer2.Interval = 1;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 1;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(133, 531);
            this.panel1.TabIndex = 1;
            this.panel1.Click += new System.EventHandler(this.Dama_Click);
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Dama_Click);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Dama_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(0, 330);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 201);
            this.label7.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(0, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 23);
            this.label6.TabIndex = 5;
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(0, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 69);
            this.label5.TabIndex = 4;
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(0, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 23);
            this.label4.TabIndex = 3;
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(0, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 23);
            this.label3.TabIndex = 2;
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(0, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 25);
            this.label2.TabIndex = 1;
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(0, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Na tahu je:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Location = new System.Drawing.Point(136, 31);
            this.panel2.MinimumSize = new System.Drawing.Size(480, 320);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(646, 531);
            this.panel2.TabIndex = 2;
            this.panel2.Click += new System.EventHandler(this.Dama_Click);
            this.panel2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Dama_Click);
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Dama_Click);
            // 
            // Dama
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 574);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.MStrip;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "Dama";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Dáma (hra)";
            this.Load += new System.EventHandler(this.Dama_Load);
            this.ResizeBegin += new System.EventHandler(this.Dama_ResizeBegin);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Dama_Click);
            this.Activated += new System.EventHandler(this.Dama_Activated);
            this.Click += new System.EventHandler(this.Dama_Click);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Dama_Click);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dama_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.Dama_ResizeEnd);
            this.MStrip.ResumeLayout(false);
            this.MStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip MStrip;
        private System.Windows.Forms.ToolStripMenuItem TSCHra;
        private System.Windows.Forms.ToolStripMenuItem TSCNapoveda;
        private System.Windows.Forms.ToolStripMenuItem TSMIOhre;
        private System.Windows.Forms.ToolStripMenuItem TSMINova;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem TSMIKonec;
        private System.Windows.Forms.ToolStripMenuItem TSMINastaveni;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripMenuItem TSCTest;
        private System.Windows.Forms.ToolStripMenuItem Test2;
        private System.Windows.Forms.ToolStripMenuItem Test1;
        private System.Windows.Forms.ToolStripMenuItem Test3;
        private System.Windows.Forms.ToolStripMenuItem Test4;
        private System.Windows.Forms.ToolStripMenuItem Test5;
        private System.Windows.Forms.ToolStripMenuItem Test6;
        private System.Windows.Forms.ToolStripMenuItem Test7;
        private System.Windows.Forms.ToolStripMenuItem Test8;
        private System.Windows.Forms.ToolStripMenuItem Test9;
        private System.Windows.Forms.ToolStripMenuItem Test10;
    }
}

