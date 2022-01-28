using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
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
        private static string getLayerPath(ILayer pLayer)
        {
            IDataLayer2 dataLayer = pLayer as IDataLayer2;
            IDatasetName dataset = dataLayer.DataSourceName as IDatasetName;
            IWorkspaceName workspace = dataset.WorkspaceName;
            string s = workspace.PathName + pLayer.Name;
            return s;
        }

        private ILayer GetLayer(IMapControlDefault mapcontrol, string LayerName)
        {
            ILayer layer = null;
            if (LayerName != null)
            {
                for (int i = 0; i < mapcontrol.LayerCount; i++)//遍历所有图层
                {
                    if (mapcontrol.get_Layer(i).Name == LayerName)
                    {
                        layer = mapcontrol.get_Layer(i);
                    }
                }
            }
            return layer;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string lyrName = comboBox1.Text;
            string savePath = textBox1.Text;
            IMapControlDefault Mapcontrol = form1.axMapControl1.Object as IMapControlDefault;           
            if (lyrName != "" && savePath != "")
            {
                ILayer layer = GetLayer(Mapcontrol, lyrName);
                string path = getLayerPath(layer);
                //MessageBox.Show(path);
            }
            else
            {
                MessageBox.Show("请完成参数选择！");
            }
        }
    }
}
