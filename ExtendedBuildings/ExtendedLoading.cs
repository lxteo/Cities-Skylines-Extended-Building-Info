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
<<<<<<< HEAD
        BuildingInfoWindow4 buildingWindow;
=======
        BuildingInfoWindow5 buildingWindow;
>>>>>>> lxteo/master
        ServiceInfoWindow2 serviceWindow;
        private LoadMode _mode;

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;
            _mode = mode;

            buildingWindowGameObject = new GameObject("buildingWindowObject");

            var buildingInfo = UIView.Find<UIPanel>("(Library) ZonedBuildingWorldInfoPanel");
<<<<<<< HEAD
            this.buildingWindow = buildingWindowGameObject.AddComponent<BuildingInfoWindow4>();
=======
            this.buildingWindow = buildingWindowGameObject.AddComponent<BuildingInfoWindow5>();
>>>>>>> lxteo/master
            this.buildingWindow.transform.parent = buildingInfo.transform;
            this.buildingWindow.size = new Vector3(buildingInfo.size.x, buildingInfo.size.y);
            this.buildingWindow.baseBuildingWindow = buildingInfo.gameObject.transform.GetComponentInChildren<ZonedBuildingWorldInfoPanel>();
            this.buildingWindow.position = new Vector3(0, 12);
            buildingInfo.eventVisibilityChanged += buildingInfo_eventVisibilityChanged;

            var serviceBuildingInfo = UIView.Find<UIPanel>("(Library) CityServiceWorldInfoPanel");
            serviceWindow = buildingWindowGameObject.AddComponent<ServiceInfoWindow2>(); 
            serviceWindow.servicePanel = serviceBuildingInfo.gameObject.transform.GetComponentInChildren<CityServiceWorldInfoPanel>();
            
            serviceBuildingInfo.eventVisibilityChanged += serviceBuildingInfo_eventVisibilityChanged;
        }

        private void serviceBuildingInfo_eventVisibilityChanged(UIComponent component, bool value)
        {
            serviceWindow.Update();
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
