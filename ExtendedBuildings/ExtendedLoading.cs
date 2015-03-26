using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtendedBuildings
{
    class ExtendedLoading : LoadingExtensionBase
    {
        static GameObject buildingWinCOMPANYGameObject;
        BuildingInfoWindow buildingWindow;
        private LoadMode _mode;

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;
            _mode = mode;
            buildingWinCOMPANYGameObject = new GameObject("BuildingWinCOMPANY");

            var buildingInfo = UIView.Find<UIPanel>("(Library) ZonedBuildingWorldInfoPanel");
            this.buildingWindow = buildingWinCOMPANYGameObject.AddComponent<BuildingInfoWindow>();
            this.buildingWindow.transform.parent = buildingInfo.transform;
            this.buildingWindow.size = new Vector3(buildingInfo.size.x, buildingInfo.size.y);
            //this.buildingWindow.ba = buildingInfo.gameObject.transform.GetComponentInChildren<ZonedBuildingWorldInfoPanel>();
            this.buildingWindow.position = new Vector3(0, -20);
            buildingInfo.eventVisibilityChanged += buildingInfo_eventVisibilityChanged;
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

            if (buildingWinCOMPANYGameObject != null)
            {
                GameObject.Destroy(buildingWinCOMPANYGameObject);
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
