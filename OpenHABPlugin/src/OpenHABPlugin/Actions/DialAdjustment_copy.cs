namespace Loupedeck.OpenHABPlugin.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json.Nodes;
    using System.Timers;

    // This class implements an example adjustment that counts the rotation ticks of a dial.

    public class DialAdjustment : PluginDynamicAdjustment
    {
        // This variable holds the current value of the counter.
        protected Timer timer;
        protected IDictionary<String, StateData> stateData = new Dictionary<String, StateData>();
        protected HttpClient httpClient = new HttpClient();
        protected class StateData
        {
            public Int32 state;
            public String service;
            public Boolean IsValid = false;
            public Boolean IsLoading = false;
        }

        // Initializes the adjustment class.
        // When `hasReset` is set to true, a reset command is automatically created for this adjustment.
        public DialAdjustment()
            : base(displayName: "Dial", description: "Counts rotation ticks", groupName: "Dial", hasReset: true)
        {
            this.MakeProfileAction("tree");
            this.timer = new Timer(5 * 1 * 1000);
            this.timer.Elapsed += (Object, ElapsedEventArgs) =>
            {
                foreach (var item in new List<String>(this.stateData.Keys))
                {
                    this.LoadData(item);
                }
                    
                
            };
            this.timer.AutoReset = true;
            this.timer.Enabled = true; 
        }

        protected StateData GetStateData(String item)
        {
            StateData stateData;
            
            if (this.stateData.TryGetValue(item, out stateData))
            {
                return stateData;
            }

            stateData = new StateData();
            this.stateData[item] = stateData;

            this.LoadData(item);

            return stateData;
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

        // This method is called when the dial associated to the plugin is rotated.
        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            StateData stateData = this.GetStateData(actionParameter.Split("|")[1]);
            stateData.state = stateData.state + diff;
            var item = actionParameter.Split("|")[1];
            var _client = new HttpClient();
            var url = "http://" + OpenHABPlugin.Config.User + ":" + OpenHABPlugin.Config.Password + "@" + OpenHABPlugin.Config.Url + item;
            var body = stateData.state.ToString();
            var content = new StringContent(body, System.Text.Encoding.UTF8, "text/plain");
             _client.PostAsync(url, content);
            this.AdjustmentValueChanged(actionParameter); // Notify the Loupedeck service that the adjustment value has changed.
        }

        // This method is called when the reset command related to the adjustment is executed.
        protected override void RunCommand(String actionParameter)
        {
            
            var item = actionParameter.Split("|")[1];
            this.LoadData(item);
            
        }

        protected async void LoadData(String item)
        {

            if (item == null)
            {
                return;
            }

            StateData stateData = this.GetStateData(item);
            
            if (stateData.IsLoading)
            {
                stateData.IsValid = false;
                return;
            }

            stateData.IsLoading = true;

             try
            {
                var _client = new HttpClient();

                var url = "http://" + OpenHABPlugin.Config.User + ":" + OpenHABPlugin.Config.Password + "@" + OpenHABPlugin.Config.Url + item;
                
                var resp = await _client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {
                    try
                    {
                        var body = await resp.Content.ReadAsStringAsync();
                        var json = JsonNode.Parse(body);
                        var state = json["state"].GetValue<String>();
                        stateData.state = Int32.Parse(state);
                    } 
                    catch (HttpRequestException e)
                    {
                        stateData.IsValid = true; 
                    }
                }
                else {}
               

            }
            catch (Exception e)
            {
                
            }
            finally
            {
                stateData.IsLoading = false;
                this.AdjustmentValueChanged(item);   
            }
        }

        // Returns the adjustment value that is shown next to the dial.
        protected override String GetAdjustmentValue(String actionParameter) =>  this.GetStateData(actionParameter.Split("|")[1]).state.ToString();
        

           /*  StateData stateData = this.GetStateData(actionParameter.Split("|")[1]);
            var service = actionParameter.Split("|")[0];
            var unit = "";
            switch(service) 
            {
            case "termostat":
                unit = "°C";
                break;
            case "LED":
                unit = "%";
                break;
            default:
                unit = "";
                break;
            } */
        
    }
}
