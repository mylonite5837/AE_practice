using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SpatialAnalyst;
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
    public partial class Form1 : Form
    {
        public static Form1 form1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axTOCControl1.SetBuddyControl(axMapControl1);
        }

        private void axMapControl1_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            toolStripStatusLabel1.Text = string.Format("{0},{1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
        }

        private void 盆域阴影法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form2 = new Form2(this);
            form2.ShowDialog();
        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
