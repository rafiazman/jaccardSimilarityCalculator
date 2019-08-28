namespace SimilarityCalculator.DataPlotter
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_loadedBruteForceTxt = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_loadedMinHashTxt = new System.Windows.Forms.Label();
            this.btn_drawGraph = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 45);
            this.button1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(240, 55);
            this.button1.TabIndex = 0;
            this.button1.Text = "Bruteforce File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(32, 29);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Size = new System.Drawing.Size(328, 210);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Open";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 114);
            this.button2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(240, 55);
            this.button2.TabIndex = 1;
            this.button2.Text = "MinHash File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(507, 29);
            this.chart1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1003, 787);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 300);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 32);
            this.label1.TabIndex = 3;
            this.label1.Text = "Loaded bruteforce file:";
            // 
            // lbl_loadedBruteForceTxt
            // 
            this.lbl_loadedBruteForceTxt.AutoSize = true;
            this.lbl_loadedBruteForceTxt.Location = new System.Drawing.Point(56, 341);
            this.lbl_loadedBruteForceTxt.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lbl_loadedBruteForceTxt.Name = "lbl_loadedBruteForceTxt";
            this.lbl_loadedBruteForceTxt.Size = new System.Drawing.Size(111, 32);
            this.lbl_loadedBruteForceTxt.TabIndex = 4;
            this.lbl_loadedBruteForceTxt.Text = "<none>";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 429);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(279, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "Loaded minhash file:";
            // 
            // lbl_loadedMinHashTxt
            // 
            this.lbl_loadedMinHashTxt.AutoSize = true;
            this.lbl_loadedMinHashTxt.Location = new System.Drawing.Point(56, 465);
            this.lbl_loadedMinHashTxt.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.lbl_loadedMinHashTxt.Name = "lbl_loadedMinHashTxt";
            this.lbl_loadedMinHashTxt.Size = new System.Drawing.Size(111, 32);
            this.lbl_loadedMinHashTxt.TabIndex = 6;
            this.lbl_loadedMinHashTxt.Text = "<none>";
            // 
            // btn_drawGraph
            // 
            this.btn_drawGraph.Location = new System.Drawing.Point(48, 641);
            this.btn_drawGraph.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.btn_drawGraph.Name = "btn_drawGraph";
            this.btn_drawGraph.Size = new System.Drawing.Size(200, 55);
            this.btn_drawGraph.TabIndex = 7;
            this.btn_drawGraph.Text = "Plot";
            this.btn_drawGraph.UseVisualStyleBackColor = true;
            this.btn_drawGraph.Click += new System.EventHandler(this.Btn_drawGraph_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1541, 844);
            this.Controls.Add(this.btn_drawGraph);
            this.Controls.Add(this.lbl_loadedMinHashTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_loadedBruteForceTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Form1";
            this.Text = "SimilarityCalculator Data Plotter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_loadedBruteForceTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_loadedMinHashTxt;
        private System.Windows.Forms.Button btn_drawGraph;
    }
}

