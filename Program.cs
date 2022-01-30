using ESRI.ArcGIS.esriSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS;

namespace AE_practice
{
    static class Program
    {
        private static LicenseInitializer m_AOLicenseInitializer = new AE_practice.LicenseInitializer();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ESRI License Initializer generated code.
            m_AOLicenseInitializer.InitializeApplication(new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeEngine },
            new esriLicenseExtensionCode[] { esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst,
                                             esriLicenseExtensionCode.esriLicenseExtensionCode3DAnalyst,
                                             esriLicenseExtensionCode.esriLicenseExtensionCodeDataInteroperability});
            Application.EnableVisualStyles();
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //ESRI License Initializer generated code.
            //Do not make any call to ArcObjects after ShutDownApplication()
            m_AOLicenseInitializer.ShutdownApplication();
        }
    }
}
