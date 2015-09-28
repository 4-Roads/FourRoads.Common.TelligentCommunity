using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FourRoads.Common.TelligentCommunity.Components;
using FourRoads.Common.TelligentCommunity.Controls;
using Telligent.DynamicConfiguration.Components;
using Telligent.Evolution.Extensibility.Api.Version1;
using Telligent.Evolution.Extensibility.Version1;

namespace FourRoads.Common.TelligentCommunity.Plugins.Base
{

    public abstract class CfsFilestoreInstaller : IInstallablePlugin
    {
        protected abstract string ProjectName { get; }
        protected abstract string BaseResourcePath { get; }
        protected abstract EmbeddedResourcesBase EmbeddedResources { get; }

        #region IPlugin Members

        public string Name
        {
            get { return ProjectName + " - CFS Installer"; }
        }

        public string Description
        {
            get { return "Defines the CFS files to be installed for " + ProjectName + "."; }
        }

        public void Initialize()
        {
        }

        #endregion

        public void Install(Version lastInstalledVersion)
        {
            string basePath = BaseResourcePath + ".Filestore.";

            EmbeddedResources.EnumerateReosurces(basePath, "", resourceName =>
            {
                string cfsFolder = resourceName.Replace(basePath, "");

                //Assume all files are *.*
                int pos = cfsFolder.LastIndexOf('.');

                if (pos > 0)
                {
                    int previousDot = cfsFolder.LastIndexOf('.', pos - 1);

                    if (previousDot < 0)
                        previousDot = -1;

                    string folder = cfsFolder.Substring(0, previousDot);
                    string file = cfsFolder.Substring(previousDot+1);

                    var cfsStore = Telligent.Evolution.Extensibility.Storage.Version1.CentralizedFileStorage.GetFileStore(folder);

                    if (cfsStore != null)
                    {
                        cfsStore.AddUpdateFile("", file, EmbeddedResources.GetStream(resourceName));
                    }
                }
            });
        }

        public void Uninstall()
        {
            EmbeddedResources.EnumerateReosurces(BaseResourcePath + "Filestore.", "", resourceName =>
            {
     
            });
        }

 
        public Version Version { get { return GetType().Assembly.GetName().Version; } }

    }
}
