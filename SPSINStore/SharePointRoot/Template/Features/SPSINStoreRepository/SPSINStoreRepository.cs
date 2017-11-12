using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Xml;
using Microsoft.SharePoint;
using System.Text;

namespace SPSIN.Store
{
    public class SPSINStoreRepository : PackageRepository
    {
        private static readonly string repositoryURL = "http://spsin.com/store/packages.xml";
        private static readonly string repositoryPackageURL_PropertyKey = "spsin_store_repositorypackageURL";

        public override string RepositoryHomePageURL
        {
            get
            {
                return base.RepositoryHomePageURL;
            }
        }

        public override Dictionary<Guid, StorePackage> GetPackages(StoreContext context)
        {
            Dictionary<Guid, StorePackage> packages = new Dictionary<Guid, StorePackage>();
            WebClient wc = new WebClient();
            string repositoryURLResolved = GetResolvedRepositoryURL(context.Web);

            wc.UseDefaultCredentials = true;
            wc.Encoding = Encoding.UTF8;
            
            byte[] content = wc.DownloadData(repositoryURLResolved);

            string utfString = Encoding.UTF8.GetString(content);

            string repositoryXML = wc.DownloadString(repositoryURLResolved);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(repositoryXML);

            foreach (XmlNode packageNode in doc.GetElementsByTagName("Package"))
            {
                StorePackage package = new StorePackage();

                string title = packageNode["Title"].InnerText;
                string description = packageNode["Description"].InnerText;
                string id = packageNode.Attributes["ID"].InnerText;
                string authorname = packageNode["AuthorName"].InnerText;
                string authorURL = packageNode["AuthorURL"].InnerText;
                string readmeURL = packageNode["ReadMeURL"].InnerText;
                string packageURL = packageNode["PackageURL"].InnerText;
                string packageFileName = packageNode["SolutionFileName"].InnerText;
                string setupFeatureID = packageNode["SetupFeatureID"].InnerText;
                SolutionType type = (SolutionType)Enum.Parse(typeof(SolutionType), packageNode["SolutionType"].InnerText);


                package.Title = title;
                package.ID = id;
                package.Description = description;
                package.AuthorName = authorname;
                package.AuthorURL = authorURL;
                package.ReadMeURL = readmeURL;
                package.PackageURL = packageURL;
                package.SetupFeatureID = setupFeatureID;
                package.SolutionFileName = packageFileName;
                package.SolutionType = type;

                packages.Add(new Guid(id), package);

            }

            return packages;
        }

        public static string GetResolvedRepositoryURL(SPWeb web)
        {
            try
            {
                if (!web.Properties.ContainsKey(repositoryPackageURL_PropertyKey))
                {
                    web.Properties.Add(repositoryPackageURL_PropertyKey, repositoryURL);
                    bool existing = web.AllowUnsafeUpdates;
                    web.AllowUnsafeUpdates = true;
                    web.Properties.Update();
                    web.AllowUnsafeUpdates = existing;
                }

                string returnValue = web.Properties[repositoryPackageURL_PropertyKey].ToString();
                return returnValue;

            }
            catch
            {
                return string.Empty;
            }
        }

        public static void SetRepositoryPackageURL(SPWeb web, string url)
        {
            try
            {
                bool existing = web.AllowUnsafeUpdates;

                if (!web.Properties.ContainsKey(repositoryPackageURL_PropertyKey))
                {
                    web.Properties.Add(repositoryPackageURL_PropertyKey, repositoryURL);
                    existing = web.AllowUnsafeUpdates;
                    web.AllowUnsafeUpdates = true;
                    web.Properties.Update();
                    web.AllowUnsafeUpdates = existing;
                }
                // TODO: Try verifying that this is a valid repository?
                web.Properties[repositoryPackageURL_PropertyKey] = url;
                existing = web.AllowUnsafeUpdates;
                web.AllowUnsafeUpdates = true;
                web.Properties.Update();
                web.AllowUnsafeUpdates = existing;
            }
            catch (Exception e)
            {
                throw new SPException("An error occurred trying to set packageURL", e);
            }
        }

        public static void ResetRepositoryPackageURL(SPWeb web)
        {
            web.Properties[repositoryPackageURL_PropertyKey] = repositoryURL;
            bool existing = web.AllowUnsafeUpdates;
            web.AllowUnsafeUpdates = true;
            web.Properties.Update();
            web.AllowUnsafeUpdates = existing;

        }
    }
}
