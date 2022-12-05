namespace Loupedeck.OpenHABPlugin.Actions
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class ToggleOffice : PluginDynamicCommand
    {
        public ToggleOffice() : base("Control Item", "Send a command to an Item", "Item")
        {
            this.MakeProfileAction("tree");
        }

        protected override PluginProfileActionData GetProfileActionData()
        {
            var tree = new PluginProfileActionTree("Select Service and item");
            tree.AddLevel("Service");
            tree.AddLevel("Item");
            if (OpenHABPlugin.Config != null)
            {
                if (OpenHABPlugin.Config.Entries.Length > 0)
                {
                    foreach (OpenHABPlugin.OHServiceEntry entry in OpenHABPlugin.Config.Entries)
                    {
                        PluginProfileActionTreeNode pluginProfileActionTreeNode = tree.Root.AddNode(entry.Service);
                        foreach (String item in entry.Items)
                        {
                            pluginProfileActionTreeNode.SetPropertyValue("Service", entry.Service);
                            pluginProfileActionTreeNode.AddItem(entry.Service + "|" + item, item, entry.Service);
                        }
                    }
                }
            }
            return tree;
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize) => String.IsNullOrEmpty(actionParameter) ? "OpenHAB" : actionParameter;


        protected override void RunCommand(String actionParameter)
        {
            
                var item = actionParameter.Split("|")[1];
                var _client = new HttpClient();
                var url = "http://" + OpenHABPlugin.Config.User + ":" + OpenHABPlugin.Config.Password + "@" + OpenHABPlugin.Config.Url + item;
             
                var body = "TOGGLE";
                var content = new StringContent(body, System.Text.Encoding.UTF8, "text/plain"); 
                _client.PostAsync(url, content);
            
        }
    }
}
