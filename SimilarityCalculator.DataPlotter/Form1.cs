using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimilarityCalculator.DataPlotter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "CSV files|*.csv";

            InitializeComponent();
            chart1.Series[0].Name = "Absolute Error";

            MinhashResults = new List<(int dValue, List<decimal> results)>();
        }

        private List<decimal> BruteforceResults { get; set; }
        private List<(int dValue, List<decimal> results)> MinhashResults { get; set; }
        private OpenFileDialog OpenFileDialog { get; }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog.Multiselect = false;

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                BruteforceResults = new List<decimal>();

                using (var streamReader = new StreamReader(File.OpenRead(OpenFileDialog.FileName)))
                {
                    // Skip first 2 lines
                    streamReader.ReadLine();
                    streamReader.ReadLine();

                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var lineData = line.Split(',');
                        if (lineData.Length != 3)
                        {
                            continue;
                        }

                        var similarity = decimal.Parse(lineData[2]);

                        BruteforceResults.Add(similarity);
                    }
                }

                lbl_loadedBruteForceTxt.Text = OpenFileDialog.SafeFileName;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog.Multiselect = true;

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                var results = new List<decimal>();
                foreach (var openedFile in OpenFileDialog.FileNames)
                {
                    var d = int.Parse(Path.GetFileNameWithoutExtension(OpenFileDialog.SafeFileName?.Substring(16)) ?? throw new InvalidOperationException());
                    using (var streamReader = new StreamReader(File.OpenRead(openedFile)))
                    {
                        // Skip first 2 lines
                        streamReader.ReadLine();
                        streamReader.ReadLine();

                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            var lineData = line.Split(',');
                            if (lineData.Length != 3)
                            {
                                continue;
                            }

                            var similarity = decimal.Parse(lineData[2]);

                            results.Add(similarity);
                        }
                    }

                    MinhashResults.Add((d, results));
                }
                
                lbl_loadedMinHashTxt.Text = $@"Loaded {MinhashResults.Count} CSV files";
            }
        }

        private void Btn_drawGraph_Click(object sender, EventArgs e)
        {
            foreach (var minhashResult in MinhashResults)
            {
                var absErrorCount = 0;
                var sumOfErrors = (decimal) 0;

                var dValue = minhashResult.dValue;

                for (int i = 0; i < BruteforceResults.Count; i++)
                {
                    var absError = Math.Abs(minhashResult.results[i] - BruteforceResults[i]);
                    sumOfErrors += absError;
                    absErrorCount++;
                }
                var mae = sumOfErrors / absErrorCount;

                chart1.Series[0].Points.AddXY(dValue, mae);
            }
        }
    }
}
