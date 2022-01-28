using ESRI.ArcGIS.Carto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AE_practice
{
    public partial class Form2 : Form
    {
        private Form1 form1;
        public Form2(Form1 form)
        {
            InitializeComponent();
            form1 = form;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < form1.axMapControl1.Map.LayerCount; i++)
            {
                string layerName = form1.axMapControl1.get_Layer(i).Name;
                comboBox1.Items.Add(layerName);
        }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string foldPath = null;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foldPath = dialog.SelectedPath;
            }
            textBox1.Text = foldPath;
        }
    }
}
