using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI;
using System.Web.Script;


namespace nStuff.AtlasControls
{

    public sealed class HistoryEventArgs : EventArgs
    {

        private string _identifier;

        internal HistoryEventArgs(string identifier)
        {
            _identifier = identifier;
        }

        public string Identifier
        {
            get
            {
                return _identifier;
            }
        }
    }

    public delegate void HistoryEventHandler(object sender, HistoryEventArgs e);

    [
    DefaultEvent("Navigate"),
    NonVisualControl
    ]
    public class HistoryControl : Control, IPostBackEventHandler
    {

        private static readonly object NavigateEventKey = new object();

        public HistoryControl()
        {
        }

        [
        Category("Behavior")
        ]
        public event HistoryEventHandler Navigate
        {
            add
            {
                Events.AddHandler(NavigateEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(NavigateEventKey, value);
            }
        }

        public void AddEntry(string identifier)
        {
            if (String.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException("identifier");
            }

            string script = "window.historyTracker.addEntry('" + identifier + "');";
            Page.ClientScript.RegisterClientScriptBlock(typeof(HistoryControl), String.Empty, script, /* addScriptTags */ true);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Get a postback event reference to ensure that the postback script is generated
            // and to make sure this is an expected name/value pair from an event validation
            // perspective.

            Page.ClientScript.GetPostBackEventReference(this, String.Empty);
            if (!Page.IsPostBack)
            {
                string script = "window.historyTarget = '" + UniqueID + "';";
                Page.ClientScript.RegisterStartupScript(typeof(HistoryControl), String.Empty, script, /* addScriptTags */ true);

                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                if (scriptManager == null)
                {
                    throw new InvalidOperationException("You must add a ScriptManager to this page to use the History control");
                }

               
                //scriptManager.RegisterScriptReference(ResolveClientUrl("~/js/History.js"));
            }
        }

        protected virtual void OnNavigate(HistoryEventArgs e)
        {
            HistoryEventHandler handler = (HistoryEventHandler)Events[NavigateEventKey];
            if (handler != null)
            {
                handler(this, e);
            }
        }
      

        protected override void Render(HtmlTextWriter writer)
        {
            string emptyHtmlPage = ResolveClientUrl("~/Empty.htm");
            writer.Write("<iframe id=\"__historyFrame\" src=\"" + emptyHtmlPage + "\" style=\"display: none;\"></iframe>");
        }

        #region IPostBackEventHandler Members
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            OnNavigate(new HistoryEventArgs(eventArgument));
        }
        #endregion
    }

     
}


