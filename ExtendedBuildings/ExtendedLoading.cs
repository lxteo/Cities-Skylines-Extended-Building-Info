using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtendedBuildings
{
    public class ExtendedLoading : LoadingExtensionBase
    {
        static GameObject buildingWindowGameObject;
        BuildingInfoWindow7 buildingWindow;
        private LoadMode _mode;

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;
            _mode = mode;

            buildingWindowGameObject = new GameObject("buildingWindowObject");

            var buildingInfo = UIView.Find<UIPanel>("(Library) ZonedBuildingWorldInfoPanel");
            this.buildingWindow = buildingWindowGameObject.AddComponent<BuildingInfoWindow7>();
            this.buildingWindow.transform.parent = buildingInfo.transform;
            this.buildingWindow.size = new Vector3(buildingInfo.size.x, buildingInfo.size.y);
            this.buildingWindow.baseBuildingWindow = buildingInfo.gameObject.transform.GetComponentInChildren<ZonedBuildingWorldInfoPanel>();
            this.buildingWindow.position = new Vector3(0, 12);
            buildingInfo.eventVisibilityChanged += buildingInfo_eventVisibilityChanged;

            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, "Loaded Extended Building Information Mod");
        }

        public override void OnLevelUnloading()
        {
            if (_mode != LoadMode.LoadGame && _mode != LoadMode.NewGame)
                return;

            if (buildingWindow != null)
            {
                if (this.buildingWindow.parent != null)
                {
                    this.buildingWindow.parent.eventVisibilityChanged -= buildingInfo_eventVisibilityChanged;
                }
            }

            if (buildingWindowGameObject != null)
            {
                GameObject.Destroy(buildingWindowGameObject);
            }
        }

        void buildingInfo_eventVisibilityChanged(UIComponent component, bool value)
        {
            this.buildingWindow.isEnabled = value;
            if (value)
            {
                this.buildingWindow.Show();
            }
            else
            {
                this.buildingWindow.Hide();
            }
        }

    }
}
