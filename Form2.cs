using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geoprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesRaster;

namespace AE_practice
{
    public partial class Form2 : Form
    {
        private Form1 form1;
        private IGeoDataset inputDataset;   //输入数据集
        private IGeoDataset outputDataset;   //输出数据集
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
        /****************************事件响应函数********************************/
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
                basinShadow(layer, path);
                Close();
            }
            else
            {
                MessageBox.Show("请完成参数选择！");
            }
        }
        /****************************数据处理函数********************************/
        private void SaveAndShowFeatureDataset(string filePath)
        {
            string directoryPath = System.IO.Path.GetDirectoryName(filePath);
            string[] fileName = System.IO.Path.GetFileName(filePath).Split('.');

            IWorkspaceFactory wf = new ShapefileWorkspaceFactoryClass();		//要注意一下这里，因为是要保存矢量文件，所以应该创建矢量工作空间工厂对象
            IWorkspace ws = wf.OpenFromFile(directoryPath, 0) as IWorkspace;
            IConversionOp converop = new RasterConversionOpClass();
            converop.ToFeatureData(outputDataset, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline, ws, fileName[0]);
            IFeatureClass featureClass = outputDataset as IFeatureClass;
            IFeatureLayer featLayer = new FeatureLayerClass();
            featLayer.FeatureClass = featureClass;
            featLayer.Name = fileName[0];          //设置图层名字
            form1.axMapControl1.AddLayer(featLayer);
            form1.axMapControl1.Refresh();
        }
        private void SaveAndShowRasterDataset(string filePath)
        {
            string directoryPath = System.IO.Path.GetDirectoryName(filePath);
            string fileName = System.IO.Path.GetFileName(filePath);

            IWorkspaceFactory wf = new RasterWorkspaceFactoryClass();
            IWorkspace ws = wf.OpenFromFile(directoryPath, 0) as IWorkspace;
            IConversionOp converop = new RasterConversionOpClass();
            converop.ToRasterDataset(outputDataset, "TIFF", ws, fileName);
            IRasterLayer rlayer = new RasterLayerClass();
            IRaster raster = new Raster();
            raster = outputDataset as IRaster;
            rlayer.CreateFromRaster(raster);       //使用raster对象创建一个rasterLayer对象
            rlayer.Name = fileName;    //设置图层名字
            form1.axMapControl1.AddLayer(rlayer);
            form1.axMapControl1.Refresh();
        }
        private static string getLayerPath(ILayer pLayer)
        {
            IDataLayer2 dataLayer = pLayer as IDataLayer2;
            IDatasetName dataset = dataLayer.DataSourceName as IDatasetName;
            IWorkspaceName workspace = dataset.WorkspaceName;
            //string s = workspace.PathName + pLayer.Name;
            string s = workspace.PathName;
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
        private void basinShadow(ILayer rasterLayer, string path)
        {
            IGeoDataset inGeoDataset = rasterLayer as IGeoDataset;
            IGeoDataset outGeoDataset;
            ISurfaceOp surfaceOp;
            IWorkspaceFactory2 pWorkspaceFactoryShp = new ShapefileWorkspaceFactoryClass();
            IWorkspace pWorkspace = pWorkspaceFactoryShp.OpenFromFile(path, 0);
            IHydrologyOp hydrologyOp;
            IConversionOp conversionOp;
            IGeoProcessor geoProcessor = new GeoProcessorClass();
            geoProcessor.OverwriteOutput = true;
            surfaceOp = new RasterSurfaceOpClass();
            IGeoDataset hillShade = surfaceOp.HillShade(inGeoDataset,0,90,true);
            /*
            outGeoDataset = hillShade;
            IRasterLayer hillShadeLyr = new RasterLayerClass();
            IRaster hillShadeRaster;
            hillShadeRaster = (IRaster)outGeoDataset;
            hillShadeLyr.CreateFromRaster(hillShadeRaster);
            hillShadeLyr.Name = "hillshade";
            form1.axMapControl1.AddLayer((ILayer)hillShadeLyr,0);
            form1.axMapControl1.ActiveView.Refresh();
            */
            hydrologyOp = new RasterHydrologyOpClass();
            conversionOp = new RasterConversionOpClass();
            IGeoDataset flowDir = hydrologyOp.FlowDirection(hillShade, true, false);
            IGeoDataset basin = hydrologyOp.Basin(flowDir);
            string name = "basin2feature.shp";
            //IGeoDataset basin2feature = conversionOp.ToFeatureData(basin,ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon,path)
            IVariantArray parameters = new VarArrayClass();
            string saveName = "slopUnit.shp";
            parameters.Add(basin2feature);
            parameters.Add(path + "slopUnit.shp");
            IGeoProcessorResult geoProcessorResult = geoProcessor.Execute("MultiPartToSinglePart", parameters, null);
            form1.axMapControl1.AddShapeFile(path, saveName);
            form1.axMapControl1.ActiveView.Refresh();

        }
    }
}
