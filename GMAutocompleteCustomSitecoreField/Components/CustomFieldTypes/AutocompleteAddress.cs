using System;
using System.Web;
using System.Web.UI.HtmlControls;
using Sitecore;
using Control = Sitecore.Web.UI.HtmlControls.Control;
using System.Collections.Specialized;

namespace GMAutocompleteCustomSitecoreField.Components.CustomFieldTypes
{
    public class AutocompleteAddress : Control
    {
        private enum ControlsField
        {
            Address,
            Latitude,
            Longitude
        }

        /// <summary>
        /// To get ID of latitude control
        /// </summary>
        public string TextLatitudeId => GetID("textlatitude");

        /// <summary>
        /// To get ID of auto complete address control
        /// </summary>
        public string TextAutocompleteId => GetID("textautocomplete");

        /// <summary>
        /// To get ID of longitude control
        /// </summary>
        public string TextLongitudeId => GetID("textlongitude");

        public const string Latitude = "Latitude";
        public const string Longitude = "Longitude";
        public const string TagnameDiv = "div";
        public const string TagnameInput = "input";
        public const string CssDiv = "scEditorFieldLabel";
        public const string CssInput = "scContentControl";
        public const string SpecialCharacter = "&amp;";
        public const string ReplaceWith = "&";

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Sitecore.Context.ClientPage.IsEvent)
            {
                CreateControls();
            }
            else
            {
                SetValues();
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// This method is used to create Control
        /// </summary>
        protected void CreateControls()
        {
            var divAutoCompleteAddress = GetHtmlGenericControl(TagnameDiv, string.Empty, "divAutoCompleteAddress", true);

            var textAutocomplete = GetHtmlGenericControl(TagnameInput, string.Empty, TextAutocompleteId);

            var divLatitude = GetHtmlGenericControl(TagnameDiv, Latitude, "divLatitude", true);

            var textLatitude = GetHtmlGenericControl(TagnameInput, string.Empty, TextLatitudeId);

            var divLongitude = GetHtmlGenericControl(TagnameDiv, Longitude, "divLongitude", true);

            var textLongitude = GetHtmlGenericControl(TagnameInput, string.Empty, TextLongitudeId);

            // Add attribute to invoke JS function
            textAutocomplete.Attributes.Add("onfocus", "javascript:autoCompleteAddress.initAutocomplete('" + TextAutocompleteId + "','" + TextLatitudeId + "','" + TextLongitudeId + "');");

            // Add CSS
            divLatitude.Attributes["class"] = divLongitude.Attributes["class"] = CssDiv;

            divLatitude.Attributes["style"] = divLongitude.Attributes["style"] = "padding-top: 15px;";

            textLongitude.Attributes["class"] = textLatitude.Attributes["class"] = textAutocomplete.Attributes["class"] = CssInput;

            // Add Value
            if (!string.IsNullOrEmpty(Value))
            {
                Value = Value.Replace(SpecialCharacter, ReplaceWith);

                var data = HttpUtility.ParseQueryString(Value);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (data != null)
                {
                    textAutocomplete.Attributes["value"] = data[ControlsField.Address.ToString()];
                    textLatitude.Attributes["value"] = data[ControlsField.Latitude.ToString()];
                    textLongitude.Attributes["value"] = data[ControlsField.Longitude.ToString()];
                }
            }

            // Add Controls
            divAutoCompleteAddress.Controls.Add(textAutocomplete);
            divAutoCompleteAddress.Controls.Add(divLatitude);
            divAutoCompleteAddress.Controls.Add(textLatitude);
            divAutoCompleteAddress.Controls.Add(divLongitude);
            divAutoCompleteAddress.Controls.Add(textLongitude);

            Controls.Add(divAutoCompleteAddress);
        }

        /// <summary>
        /// This method is used to set values to Value Property
        /// </summary>
        public void SetValues()
        {
            var autocompleteAddress = Context.Request.Form[TextAutocompleteId] ?? string.Empty;
            var latitude = Context.Request.Form[TextLatitudeId] ?? string.Empty;
            var longitude = Context.Request.Form[TextLongitudeId] ?? string.Empty;

            var allValues = new NameValueCollection
            {
                { ControlsField.Address.ToString(), autocompleteAddress },
                { ControlsField.Latitude.ToString(), latitude },
                { ControlsField.Longitude.ToString(), longitude }
            };

            var combinedValue = StringUtil.NameValuesToString(allValues, "&");

            Sitecore.Context.ClientPage.Modified = (Value != combinedValue);

            if (combinedValue != null && combinedValue != Value)
            {
                Value = combinedValue.Replace(SpecialCharacter, ReplaceWith);
            }
        }

        /// <summary>
        /// This method is used to get a new HtmlGenericControl
        /// </summary>
        /// <param name="tagName">It is a string type of parameter that holds Tag Name</param>
        /// <param name="innerText">It is a string type of parameter that holds Inner Text</param>
        /// <param name="controlId">It is a string type of parameter that holds Control ID</param>
        /// <param name="getControlId">It is a bool type of parameter that True when control ID is to be Get</param>
        /// <returns>It returns HtmlGenericControl</returns>
        public HtmlGenericControl GetHtmlGenericControl(string tagName, string innerText, string controlId, bool getControlId = false)
        {
            return new HtmlGenericControl
            {
                TagName = tagName,
                InnerText = innerText,
                ID = getControlId ? GetID(controlId) : controlId
            };
        }
    }
}