#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2017
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;

using System;
using System.Linq;
using System.Text;
using DotNetNuke.Entities.Modules.Definitions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Services.Upgrade;

namespace DotNetNuke.Providers.RadEditorProvider
{

	public class UpgradeController : IUpgradeable
	{
		private const string ModuleFolder = "~/DesktopModules/Admin/RadEditorProvider";
		private const string ResourceFile = ModuleFolder + "/App_LocalResources/ProviderConfig.ascx.resx";

        private static readonly Regex HostNameRegex = new Regex("\\.host", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex AdminNameRegex = new Regex("\\.admin", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RegisteredNameRegex = new Regex("\\.registered", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
		/// 
		/// </summary>
		/// <param name="Version"></param>
		/// <returns></returns>
		/// <remarks>This is not localizing Page Name or description.</remarks>
		public string UpgradeModule(string Version)
		{
			try
			{
				var pageName = Localization.GetString("HTMLEditorPageName", ResourceFile);
				var pageDescription = Localization.GetString("HTMLEditorPageDescription", ResourceFile);

                switch (Version)
				{
					case "06.00.00":

						//Create Rad Editor Config Page (or get existing one)
						TabInfo newPage = Upgrade.AddHostPage(pageName, pageDescription, ModuleFolder + "/images/radeditor_config_small.png",ModuleFolder + "/images/radeditor_config_large.png", true);

						//Add Module To Page
						int moduleDefId = GetModuleDefinitionID();
						Upgrade.AddModuleToPage(newPage, moduleDefId, pageName, ModuleFolder + "/images/radeditor_config_large.png", true);

						foreach (var item in DesktopModuleController.GetDesktopModules(Null.NullInteger))
						{
							DesktopModuleInfo moduleInfo = item.Value;

							if (moduleInfo.ModuleName == "DotNetNuke.RadEditorProvider")
							{
								moduleInfo.Category = "Host";
								DesktopModuleController.SaveDesktopModule(moduleInfo, false, false);
							}
						}
						break;
					case "07.00.06":
						UpdateConfigOfLinksType();
						break;
                    case "07.03.00":
				        UpdateConfigFilesName();
                        UpdateToolsFilesName();
                        break;
                    case "07.04.00":
                        // Find the RadEditor page.  It should already exist and this will return it's reference.
                        var editorPage = Upgrade.AddHostPage(pageName, pageDescription, ModuleFolder + "/images/HtmlEditorManager_Standard_16x16.png", ModuleFolder + "/images/HtmlEditorManager_Standard_32x32.png", true);

                        // If the Html Editor Manager is installed, then remove the old RadEditor Manager
                        var htmlEditorManager = DesktopModuleController.GetDesktopModuleByModuleName("DotNetNuke.HtmlEditorManager", Null.NullInteger);
                        if (htmlEditorManager != null)
                        {
                            Upgrade.RemoveModule("RadEditor Manager", editorPage.TabName, editorPage.ParentId, false);
                        }
                        break;
                    case "09.01.01":
                        UpdateRadCfgFiles();
				        UpdateWebConfigFile();
                        break;
				}
			}
			catch (Exception ex)
			{
				ExceptionLogController xlc = new ExceptionLogController();
				xlc.AddLog(ex);

				return "Failed";
			}

			return "Success";
		}

		private int GetModuleDefinitionID()
		{
			// get desktop module
			DesktopModuleInfo desktopModule = DesktopModuleController.GetDesktopModuleByModuleName("DotNetNuke.RadEditorProvider", Null.NullInteger);
			if (desktopModule == null)
			{
				return -1;
			}

			//get module definition
			ModuleDefinitionInfo moduleDefinition = ModuleDefinitionController.GetModuleDefinitionByFriendlyName("RadEditor Manager", desktopModule.DesktopModuleID);
			if (moduleDefinition == null)
			{
				return -1;
			}

			return moduleDefinition.ModuleDefID;
		}

		private void UpdateConfigOfLinksType()
		{
			foreach (string file in Directory.GetFiles(HttpContext.Current.Server.MapPath(ModuleFolder + "/ConfigFile")))
			{
				var filename = Path.GetFileName(file).ToLowerInvariant();
				if (filename.StartsWith("configfile") && filename.EndsWith(".xml"))
				{
					UpdateConfigOfLinksTypeInFile(file);
				}
			}
		}

		private void UpdateConfigOfLinksTypeInFile(string file)
		{
			try
			{
				var config = new XmlDocument();
				config.Load(file);
				var node = config.SelectSingleNode("/configuration/property[@name='LinksUseTabNames']");
				if (node != null)
				{
					var value = bool.Parse(node.InnerText);
					config.DocumentElement.RemoveChild(node);
					var newNode = config.CreateElement("property");
					newNode.SetAttribute("name", "LinksType");
					newNode.InnerText = value ? "UseTabName" : "Normal";
					config.DocumentElement.AppendChild(newNode);
					config.Save(file);
				}
			}
			catch
			{
				//ignore error here.
			}
		}

	    private void UpdateConfigFilesName()
	    {
            foreach (string file in Directory.GetFiles(HttpContext.Current.Server.MapPath(ModuleFolder + "/ConfigFile")))
            {
                var filename = Path.GetFileName(file).ToLowerInvariant();
                if (filename.StartsWith("configfile") && filename.EndsWith(".xml"))
                {
                    UpdateFileNameWithRoleId(file);
                }
            }
	    }

        private void UpdateToolsFilesName()
        {
            foreach (string file in Directory.GetFiles(HttpContext.Current.Server.MapPath(ModuleFolder + "/ToolsFile")))
            {
                var filename = Path.GetFileName(file).ToLowerInvariant();
                if (filename.StartsWith("toolsfile") && filename.EndsWith(".xml"))
                {
                    UpdateFileNameWithRoleId(file);
                }
            }
        }

        private void UpdateFileNameWithRoleId(string file)
	    {
	        var newPath = file;
            if(file.ToLowerInvariant().Contains(".host"))
            {
                var rolePart = ".RoleId." + Globals.glbRoleSuperUser;
                newPath = HostNameRegex.Replace(file, rolePart);
            }
            else if (file.ToLowerInvariant().Contains(".admin"))
            {
                var portalSettings = new PortalSettings(Host.HostPortalID);
                var rolePart = ".RoleId." + portalSettings.AdministratorRoleId;
                newPath = AdminNameRegex.Replace(file, rolePart);
            }
            else if (file.ToLowerInvariant().Contains(".registered"))
            {
                var portalSettings = new PortalSettings(Host.HostPortalID);
                var rolePart = ".RoleId." + portalSettings.RegisteredRoleId;
                newPath = RegisteredNameRegex.Replace(file, rolePart);
            }

	        if (newPath != file)
	        {
	            File.Move(file, newPath);
	        }
	    }

        private static void UpdateRadCfgFiles()
        {
            var folder = Path.Combine(Globals.ApplicationMapPath, @"DesktopModules\Admin\RadEditorProvider\ConfigFile");
            UpdateRadCfgFiles(folder, "*ConfigFile*.xml");
        }

        private static void UpdateRadCfgFiles(string folder, string mask)
        {
            var allowedDocExtensions = "doc|docx|xls|xlsx|ppt|pptx|pdf|txt".Split('|');

            var files = Directory.GetFiles(folder, mask);
            foreach (var fname in files)
            {
                if (fname.Contains(".Original.xml")) continue;

                try
                {
                    var doc = new XmlDocument();
                    doc.Load(fname);
                    var root = doc.DocumentElement;
                    var docFilters = root?.SelectNodes("/configuration/property[@name='DocumentsFilters']");
                    if (docFilters != null)
                    {
                        var changed = false;
                        foreach (XmlNode filter in docFilters)
                        {
                            if (filter.InnerText == "*.*")
                            {
                                changed = true;
                                filter.InnerXml = "";
                                foreach (var extension in allowedDocExtensions)
                                {
                                    var node = doc.CreateElement("item");
                                    node.InnerText = "*." + extension;
                                    filter.AppendChild(node);
                                }
                            }
                        }

                        if (changed)
                        {
                            File.Copy(fname, fname + ".bak.resources", true);
                            doc.Save(fname);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var xlc = new ExceptionLogController();
                    xlc.AddLog(ex);
                }
            }
        }

        private static string UpdateWebConfigFile()
        {
            const string keyName = "Telerik.AsyncUpload.ConfigurationEncryptionKey";
            const string defaultValue = "MDEyMzQ1Njc4OUFCQ0RFRjAxMjM0NTY3ODlBQkNERUYwMTIzNDU2Nzg5QUJDREVG";

            var strError = "";
            var currentKey = Config.GetSetting(keyName);
            if (string.IsNullOrEmpty(currentKey) || defaultValue.Equals(currentKey) || currentKey.Length < 40)
            {
                try
                {
                    //open the web.config
                    var xmlConfig = Config.Load();

                    //save the current config file
                    Config.BackupConfig();

                    //create a random Telerik encryption key and add it under <appSettings>
                    var newKey = new PortalSecurity().CreateKey(32);
                    newKey = Convert.ToBase64String(Encoding.ASCII.GetBytes(newKey));
                    Config.AddAppSetting(xmlConfig, keyName, newKey);

                    //save a copy of the exitsing web.config
                    var backupFolder = string.Concat(Globals.glbConfigFolder, "Backup_", DateTime.Now.ToString("yyyyMMddHHmm"), "\\");
                    strError += Config.Save(xmlConfig, backupFolder + "web_.config") + Environment.NewLine;

                    //save the web.config
                    strError += Config.Save(xmlConfig) + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    strError += ex.Message;
                }
            }
            return strError;
        }
    }

}