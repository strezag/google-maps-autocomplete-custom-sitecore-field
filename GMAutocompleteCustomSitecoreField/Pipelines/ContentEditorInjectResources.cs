using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Sitecore.Pipelines;
using Sitecore.StringExtensions;

namespace GMAutocompleteCustomSitecoreField.Pipelines
{
    public class ContentEditorInjectResources
    {
        private readonly IList<string> _listScripts = new List<string>();
        private readonly IList<string> _listStyles = new List<string>();
        private const string JavaScriptTag = "<script type='text/javascript' language='javascript' src='{0}'></script>";
        private const string StyleSheetLinkTag = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{ 0 }\"/>";

        /// <summary>
        /// This method is used to Add CSS Resource
        /// </summary>
        /// <param name="resource">It is a Path of CSS file that is added to the "<resource/>" tag under styles in the Rightpoint.ContentEditorInjectResources.config</param>
        public void AddStyleResource(string resource)
        {
            try
            {
                _listStyles.Add(resource);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Could not add script to Content Editor.", ex.StackTrace);
            }
        }

        /// <summary>
        /// This method is used to Add JS Resource
        /// </summary>
        /// <param name="resource">It is a Path of JS file that is added to the "<resource/>" tag under scripts in the Rightpoint.ContentEditorInjectResources.config</param>
        public void AddScriptResource(string resource)
        {
            try
            {
                if (string.IsNullOrEmpty(resource)) return;
                _listScripts.Add(resource);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Could not add script to Content Editor.", ex.StackTrace);
            }
        }

        public void Process(PipelineArgs args)
        {
            try
            {
                AddResourcesToPage();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Could not add script to Content Editor.", ex.StackTrace);
            }
        }

        /// <summary>
        /// This method is used to Add resources the Page
        /// </summary>
        private void AddResourcesToPage()
        {
            if (Sitecore.Context.ClientPage.IsEvent) return;
            var page = HttpContext.Current.Handler as Page;

            try
            {
                foreach (var script in _listScripts)
                {
                    page?.Header.Controls.Add(
                        new LiteralControl(
                            JavaScriptTag.FormatWith(script)));
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Could not add script to Content Editor.", ex.StackTrace);
            }

            try
            {
                foreach (var css in _listStyles)
                {
                    page?.Header.Controls.Add(
                        new LiteralControl(StyleSheetLinkTag.FormatWith(css)));
                }
            }
            catch (Exception ex2)
            {
                Sitecore.Diagnostics.Log.Error("Could not add script to Content Editor.", ex2.StackTrace);
            }
        }
    }
}