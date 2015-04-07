using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ExtendedBuildings
{

    public class ServiceInfoWindow2 : MonoBehaviour
    {
        UILabel info;
        UILabel label1;
        FieldInfo baseSub;

        CityServiceWorldInfoPanel m_servicePanel;
        public CityServiceWorldInfoPanel servicePanel
        {
            get { return m_servicePanel; }
            set
            {
                var stats = value.Find<UIPanel>("StatsPanel");
                info = stats.Find<UILabel>("Info");
                label1 = stats.AddUIComponent<UILabel>();
                label1.color = info.color;
                label1.textColor = info.textColor;
                label1.textScale = info.textScale;
                label1.relativePosition = new Vector3(0, info.height + info.relativePosition.y - 40);
                label1.size = new Vector2(230, 84);
                label1.font = info.font;

                m_servicePanel = value;
            }
        }

        int lastSelected;

        public void Update()
        {
            if (servicePanel == null)
            {
                return;
            }

            var buildingId = GetParentInstanceId().Building;
            if (this.enabled && info.isVisible && BuildingManager.instance != null && ((SimulationManager.instance.m_currentFrameIndex & 15u) == 15u || lastSelected != buildingId))
            {
                Debug.Log("running");
                lastSelected = buildingId;
                Building data = BuildingManager.instance.m_buildings.m_buffer[buildingId];
                var service = data.Info.m_class.m_service;
                var productionRate = PlayerBuildingAI.GetProductionRate(data.m_productionRate, EconomyManager.instance.GetBudget(data.Info.m_class));
                if (data.m_fireIntensity != 0)
                {
                    productionRate = 0;
                }

                
                var sb = new StringBuilder();
                var ii = 0;
                var ai = data.Info.m_buildingAI;

                if (ai is FireStationAI)
                {
                    var strength = (int)(((FireStationAI)data.Info.m_buildingAI).m_fireDepartmentAccumulation * productionRate / 100);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "FireFighting") + ": " + strength.ToString());
                    var radius = (int)(((FireStationAI)data.Info.m_buildingAI).m_fireDepartmentRadius / 8);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Radius") + ": " + radius.ToString());
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "KittensSaved") + ": " + GetLlamaSightings(1.4));
                    ii +=3;
                }
                else if (ai is MonumentAI)
                {
                    var strength = (int)(((MonumentAI)data.Info.m_buildingAI).m_entertainmentAccumulation * productionRate / 100);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Entertainment") + ": " + strength.ToString());
                    var radius = (int)(((MonumentAI)data.Info.m_buildingAI).m_entertainmentRadius  / 8);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Radius") + ": " + radius.ToString());
                    var tourism = (int)(((MonumentAI)data.Info.m_buildingAI).m_attractivenessAccumulation * productionRate / 100);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Attractiveness") + ": " + tourism);
                    ii += 3;
                }
                else if (ai is HospitalAI){
                    
                    var strength = (int)(((HospitalAI)data.Info.m_buildingAI).m_healthCareAccumulation * productionRate / 100);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Radius") + ": " + strength.ToString());
                    var radius = (int)(((HospitalAI)data.Info.m_buildingAI).m_healthCareRadius  / 8);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Healthcare") + ": " + radius.ToString());
                    ii += 2;
                }
                 else if (ai is CemeteryAI)
                {
                    var strength = (int)(((CemeteryAI)data.Info.m_buildingAI).m_deathCareAccumulation * productionRate / 100);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Deathcare") + ": " + strength.ToString());
                    var radius = (int)(((CemeteryAI)data.Info.m_buildingAI).m_deathCareRadius / 8);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Radius") + ": " + radius.ToString());
                    ii += 3;
                }
                 else if (ai is ParkAI)
                {
                    var strength = (int)(((ParkAI)data.Info.m_buildingAI).m_entertainmentAccumulation * productionRate / 100);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Entertainment") + ": " + strength.ToString());
                    var radius = (int)(((ParkAI)data.Info.m_buildingAI).m_entertainmentRadius / 8);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Radius") + ": " + radius.ToString());
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Llamas") + ": " + GetLlamaSightings(2));
                    ii += 3;
                }
                 else if (ai is SchoolAI)
                {
                    var strength = (int)(((SchoolAI)data.Info.m_buildingAI).m_educationAccumulation * productionRate / 100);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Education") + ": " + strength.ToString());
                    var radius = (int)(((SchoolAI)data.Info.m_buildingAI).m_educationRadius / 8);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Radius") + ": " + radius.ToString());
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "ClassesSkipped") + ": " + GetLlamaSightings(2));
                    ii += 3;

                }
                 else if (ai is PoliceStationAI)
                {
                    var strength = (int)(((PoliceStationAI)data.Info.m_buildingAI).m_policeDepartmentAccumulation * productionRate / 100);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Police") + ": " + strength.ToString());
                    var radius = (int)(((PoliceStationAI)data.Info.m_buildingAI).m_policeDepartmentRadius  / 8);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Radius") + ": " + radius.ToString());
                    ii += 2;
                }
                else if (ai is CargoStationAI)
                {
                    var strength = (int)(((CargoStationAI)data.Info.m_buildingAI).m_cargoTransportAccumulation * productionRate / 100);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Cargo") + ": " + strength.ToString());
                    var radius = (int)(((CargoStationAI)data.Info.m_buildingAI).m_cargoTransportRadius / 8);
                    sb.AppendLine(Localization.Get(LocalizationCategory.ServiceInfo, "Radius") + ": " + radius.ToString());
                    ii += 2;
                }
                label1.relativePosition = new Vector3(0, info.height + info.relativePosition.y - 14 * ii);
                label1.text = sb.ToString();
            }
            
        }

        private string GetLlamaSightings(double scale)
        {
            return ((int)((SimulationManager.instance.m_currentGameTime.DayOfYear * scale + GetParentInstanceId().Building) / 1000)).ToString();
        }

        private InstanceID GetParentInstanceId()
        {
            if (baseSub == null)
            {
                baseSub = this.m_servicePanel.GetType().GetField("m_InstanceID", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return (InstanceID)baseSub.GetValue(this.m_servicePanel);
        }
    }
}
