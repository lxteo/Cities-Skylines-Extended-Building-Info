using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedBuildings
{
    using ColossalFramework;
    using ColossalFramework.Globalization;
    using ColossalFramework.Math;
    using ColossalFramework.UI;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Timers;
    using UnityEngine;

<<<<<<< HEAD:ExtendedBuildings/Panels/BuildingInfoWindow.cs
    public class BuildingInfoWindow4 : UIPanel
=======
    public class BuildingInfoWindow5 : UIPanel
>>>>>>> lxteo/master:ExtendedBuildings/Panels/BuildingInfoWindow.cs
    {
        const float vertPadding = 26;
        float barWidth;
        Dictionary<ImmaterialResourceManager.Resource, UIProgressBar> resourceBars;
        Dictionary<ImmaterialResourceManager.Resource, UILabel> resourceLabels;

        UIProgressBar pollutionBar;
        UILabel pollutionLabel;

        UILabel serviceLabel;
        UIProgressBar serviceBar;

        UILabel educationLabel;
        UIProgressBar educationBar;


        UILabel happyLabel;
        UIProgressBar happyBar;

        UITextField buildingName;

        public ZonedBuildingWorldInfoPanel baseBuildingWindow;
        FieldInfo baseSub;

        Dictionary<string, Markov> buildingNames = new Dictionary<string, Markov>();
        Dictionary<string, Markov> buildingDescriptions = new Dictionary<string, Markov>();

        UIButton descriptionButton;
        UILabel descriptionLabel;

        ushort selectedBuilding;
        bool showDescription = true;
        bool showName = true;

        public override void Awake()
        {
            resourceBars = new Dictionary<ImmaterialResourceManager.Resource, UIProgressBar>();
            resourceLabels = new Dictionary<ImmaterialResourceManager.Resource, UILabel>();

            for (var i = 0; i < 20; i += 1)
            {
                var res = (ImmaterialResourceManager.Resource)i;
                var bar = AddUIComponent<UIProgressBar>();
                bar.backgroundSprite = "LevelBarBackground";
                bar.progressSprite = "LevelBarForeground";
                bar.progressColor = Color.green;
                resourceBars.Add(res, bar);
                var label = AddUIComponent<UILabel>();
                label.text = Localization.Get(LocalizationCategory.BuildingInfo, res.ToString());
                label.textScale = 0.5f;
                label.size = new Vector2(100, 20);
                resourceLabels.Add(res, label);
            }

            pollutionBar = AddUIComponent<UIProgressBar>();
            pollutionBar.backgroundSprite = "LevelBarBackground";
            pollutionBar.progressSprite = "LevelBarForeground";
            pollutionBar.progressColor = Color.red;
            pollutionLabel = AddUIComponent<UILabel>();
            pollutionLabel.text = Localization.Get(LocalizationCategory.BuildingInfo, "Pollution");
            pollutionLabel.textScale = 0.5f;
            pollutionLabel.size = new Vector2(100, 20);

            serviceLabel = AddUIComponent<UILabel>();
            serviceBar = AddUIComponent<UIProgressBar>();

            educationLabel = AddUIComponent<UILabel>();
            educationBar = AddUIComponent<UIProgressBar>();

            happyLabel = AddUIComponent<UILabel>();
            happyBar = AddUIComponent<UIProgressBar>();

            buildingNames.Clear();
            LoadTextFiles();
            
            descriptionLabel = AddUIComponent<UILabel>();
            descriptionButton = AddUIComponent<UIButton>();

            base.Awake();

        }


        private void LoadTextFiles()
        {

            var commercialName = new Markov("nameCommercial", false, 4);
            buildingNames.Add(ItemClass.Zone.CommercialHigh.ToString(), commercialName);
            buildingNames.Add(ItemClass.Zone.CommercialLow.ToString(), commercialName);
            var resName = new Markov("nameResidential", false, 4);
            buildingNames.Add(ItemClass.Zone.ResidentialHigh.ToString(), resName);
            buildingNames.Add(ItemClass.Zone.ResidentialLow.ToString(), resName);
            var indyName = new Markov("nameIndustrial", false, 4);
            buildingNames.Add(ItemClass.Zone.Industrial.ToString(), indyName);
            var officeName = new Markov("nameOffice", false, 4);
            buildingNames.Add(ItemClass.Zone.Office.ToString(), officeName);

            buildingNames.Add(ItemClass.SubService.IndustrialFarming.ToString(), new Markov("nameFarm", false, 4));
            buildingNames.Add(ItemClass.SubService.IndustrialForestry.ToString(), new Markov("nameForest", false, 4));
            buildingNames.Add(ItemClass.SubService.IndustrialOre.ToString(), new Markov("nameMine", false, 4));
            buildingNames.Add(ItemClass.SubService.IndustrialOil.ToString(), new Markov("nameOil", false, 4));

            buildingDescriptions.Clear();
            var commercialDescription = new Markov("descriptionsCommercial", false, 9);
            buildingDescriptions.Add(ItemClass.Zone.CommercialHigh.ToString(), commercialDescription);
            buildingDescriptions.Add(ItemClass.Zone.CommercialLow.ToString(), commercialDescription);
            var resDescription = new Markov("descriptionsResidential", false, 9);
            buildingDescriptions.Add(ItemClass.Zone.ResidentialHigh.ToString(), resDescription);
            buildingDescriptions.Add(ItemClass.Zone.ResidentialLow.ToString(), resDescription);
            var indyDescription = new Markov("descriptionsIndustrial", false, 9);
            buildingDescriptions.Add(ItemClass.Zone.Industrial.ToString(), indyDescription);
            var officeDescription = new Markov("descriptionsOffice", false, 9);
            buildingDescriptions.Add(ItemClass.Zone.Office.ToString(), officeDescription);


            buildingDescriptions.Add(ItemClass.SubService.IndustrialFarming.ToString(), new Markov("descriptionsFarm", false, 4));
            buildingDescriptions.Add(ItemClass.SubService.IndustrialForestry.ToString(), new Markov("descriptionsForest", false, 4));
            buildingDescriptions.Add(ItemClass.SubService.IndustrialOre.ToString(), new Markov("descriptionsMine", false, 4));
            buildingDescriptions.Add(ItemClass.SubService.IndustrialOil.ToString(), new Markov("descriptionsOil", false, 4));
        }


        public override void Start()
        {
            base.Start();

            backgroundSprite = "MenuPanel2";
            opacity = 0.8f;
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            SetupControls();
        }

        public void SetupControls()
        {
            base.Start();

            barWidth = this.size.x - 28;
            float y = 70;

            SetLabel(serviceLabel, Localization.Get(LocalizationCategory.BuildingInfo, "Service"));
            SetBar(serviceBar);
            serviceBar.tooltip = Localization.Get(LocalizationCategory.BuildingInfo, "ServiceDescription") + " 0/0";
            serviceLabel.tooltip = Localization.Get(LocalizationCategory.BuildingInfo, "ServiceDescription");
            y += vertPadding;

            SetLabel(educationLabel, Localization.Get(LocalizationCategory.BuildingInfo, "Education"));
            SetBar(educationBar);
            educationBar.tooltip = Localization.Get(LocalizationCategory.BuildingInfo, "EducationDescription") + " 0/0";
            educationLabel.tooltip = Localization.Get(LocalizationCategory.BuildingInfo, "EducationDescription");
            y += vertPadding;

            SetLabel(happyLabel, Localization.Get(LocalizationCategory.BuildingInfo, "Happiness"));
            SetBar(happyBar);
            happyBar.tooltip = Localization.Get(LocalizationCategory.BuildingInfo, "HappinessDescription") + " 0/0";
            happyLabel.tooltip = Localization.Get(LocalizationCategory.BuildingInfo, "HappinessDescription");

            SetLabel(descriptionLabel, "Happiness");
            descriptionLabel.textScale = 0.65f;
            descriptionLabel.wordWrap = true;
            //descriptionLabel.size = new Vector2(barWidth - 20, 140);
            descriptionLabel.autoSize = false;
            descriptionLabel.width = barWidth;
            descriptionLabel.wordWrap = true;
            descriptionLabel.autoHeight = true;
            descriptionLabel.anchor = (UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right);
            descriptionButton.normalBgSprite = "IconDownArrow";
            descriptionButton.hoveredBgSprite = "IconDownArrowHovered";
            descriptionButton.focusedBgSprite = "IconDownArrowFocused";
            descriptionButton.pressedBgSprite = "IconDownArrow";
            descriptionButton.size = new Vector3(80, 20);
            descriptionButton.color = Color.white;
            descriptionButton.colorizeSprites = true;

            descriptionButton.eventClick += descriptionButton_eventClick;

            y += vertPadding;
            height = y;
        }

        private void descriptionButton_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            showDescription = !showDescription;
        }

        private void SetBar(UIProgressBar bar)
        {
            bar.backgroundSprite = "LevelBarBackground";
            bar.progressSprite = "LevelBarForeground";
            bar.progressColor = Color.green;
            bar.size = new Vector2(barWidth - 120, 16);
            bar.minValue = 0f;
            bar.maxValue = 1f;
        }

        private void SetLabel(UILabel title, string p)
        {
            title.text = p;
            title.textScale = 0.7f;
            title.size = new Vector2(120, 30);
        }

        private void SetPos(UILabel title, UIProgressBar bar, float x, float y, bool visible)
        {
            bar.relativePosition = new Vector3(x + 120, y - 3);
            title.relativePosition = new Vector3(x, y);
            if (visible)
            {
                bar.Show();
                title.Show();
            }
            else
            {
                bar.Hide();
                title.Hide();
            }
        }

        public override void Update()
        {
            var instanceId = GetParentInstanceId();
            if (instanceId.Type == InstanceType.Building && instanceId.Building != 0)
            {
                ushort building = instanceId.Building;
                if (this.baseBuildingWindow != null && this.enabled && isVisible && Singleton<BuildingManager>.exists && ((Singleton<SimulationManager>.instance.m_currentFrameIndex & 15u) == 15u || selectedBuilding != building))
                {
                    BuildingManager instance = Singleton<BuildingManager>.instance;
                    this.UpdateBuildingInfo(building, instance.m_buildings.m_buffer[(int)building]);
                    selectedBuilding = building;
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                showDescription = !showName;
                showName = showDescription;
            }

            base.Update();
        }

        private void UpdateBuildingInfo(ushort buildingId, Building building)
        {
            var levelUpHelper = LevelUpHelper3.instance;
            var info = building.Info;
            var zone = info.m_class.GetZone();
            Building data = Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)buildingId];
            ushort[] array;
            int num;
            Singleton<ImmaterialResourceManager>.instance.CheckLocalResources(data.m_position, out array, out num);
            double totalFactor = 0;
            double totalNegativeFactor = 0;
            foreach (var resBar in this.resourceBars)
            {
                if (levelUpHelper.GetFactor(zone, resBar.Key) > 0)
                {
                    totalFactor += levelUpHelper.GetFactor(zone, resBar.Key);
                }
                else
                {
                    totalNegativeFactor -= levelUpHelper.GetFactor(zone, resBar.Key);
                }
            }
            totalNegativeFactor -= levelUpHelper.GetPollutionFactor(zone);

            var x = 14f;
            var negativeX = 14f;
            foreach (var resBar in this.resourceBars)
            {
                var label = this.resourceLabels[resBar.Key];
                var factor = levelUpHelper.GetFactor(zone, resBar.Key);
                if (factor == 0)
                {
                    label.Hide();
                    resBar.Value.Hide();
                }
                else
                {
                    label.Show();
                    resBar.Value.Show();
                    int max = 0;
                    int raw = 0;
                    var value = levelUpHelper.GetServiceScore(resBar.Key, zone, array, num,ref raw, ref max);

                    if (factor > 0)
                    {
                        resBar.Value.size = new Vector2((float)(barWidth * factor / totalFactor), 16);
                        label.relativePosition = new Vector3(x, 10);
                        resBar.Value.relativePosition = new Vector3(x, 20);
                        x += resBar.Value.size.x;
                    }
                    else
                    {
                        resBar.Value.size = new Vector2((float)(barWidth * -factor / totalNegativeFactor), 16);
                        label.relativePosition = new Vector3(negativeX, 56);
                        resBar.Value.relativePosition = new Vector3(negativeX, 36);
                        negativeX += resBar.Value.size.x;
                        resBar.Value.progressColor = Color.red;                        
                    }
                    SetProgress(resBar.Value, (float)value, 0, 100, raw, max);                
                }
            }

            if (levelUpHelper.GetPollutionFactor(zone) < 0)
            {
                var value = levelUpHelper.GetPollutionScore(data, zone);
                var factor = levelUpHelper.GetPollutionFactor(zone);

                pollutionBar.size = new Vector2((float)(barWidth * -factor / totalNegativeFactor), 16);
                pollutionLabel.relativePosition = new Vector3(negativeX, 56);
                SetProgress(pollutionBar, (float)value, 0, 100);
                pollutionBar.relativePosition = new Vector3(negativeX, 36);
                negativeX += pollutionBar.size.x;

                pollutionBar.Show();
                pollutionLabel.Show();
            }
            else
            {
                pollutionBar.Hide();
                pollutionLabel.Hide();
            }

            x = 14f;
            float y = 70f;
            SetProgress(serviceBar, levelUpHelper.GetProperServiceScore(buildingId), levelUpHelper.GetServiceThreshhold((ItemClass.Level)(Math.Max(-1, (int)data.Info.m_class.m_level - 1)), zone), levelUpHelper.GetServiceThreshhold(data.Info.m_class.m_level, zone));
            SetPos(serviceLabel, serviceBar, x, y, true);

            if (zone == ItemClass.Zone.ResidentialHigh || zone == ItemClass.Zone.ResidentialLow || zone == ItemClass.Zone.CommercialHigh || zone == ItemClass.Zone.CommercialLow)
            {
                serviceLabel.text = Localization.Get(LocalizationCategory.BuildingInfo, "LandValueProgress");
            }
            else
            {
                serviceLabel.text = Localization.Get(LocalizationCategory.BuildingInfo, "Service");
            }
            y += vertPadding;

            float education;
            float happy;
            float commute;
            levelUpHelper.GetEducationHappyScore(buildingId, out education, out happy, out commute);

            SetProgress(educationBar, education, levelUpHelper.GetEducationThreshhold((ItemClass.Level)(Math.Max(-1, (int)data.Info.m_class.m_level - 1)), zone), levelUpHelper.GetEducationThreshhold(data.Info.m_class.m_level, zone));
            SetPos(educationLabel, educationBar, x, y, true);
            y += vertPadding;


            if (zone == ItemClass.Zone.CommercialHigh || zone == ItemClass.Zone.CommercialLow)
            {
                educationLabel.text = Localization.Get(LocalizationCategory.BuildingInfo, "Wealth");
            }
            else
            {
                educationLabel.text = Localization.Get(LocalizationCategory.BuildingInfo, "Education");
            }

            SetProgress(happyBar, happy, 0, 100);
            SetPos(happyLabel, happyBar, x, y, true);
            y += vertPadding;

            descriptionButton.relativePosition = new Vector3(this.width / 2 - 40, y - 10);
            y += 12;

            if (this.baseBuildingWindow != null)
            {
                if (buildingName == null)
                {
                    this.buildingName = this.baseBuildingWindow.Find<UITextField>("BuildingName");
                    this.buildingName.maxLength = 50;
                    this.buildingName.textScale = 0.87f;
                }
                if (buildingName != null)
                {
                    var bName = this.buildingName.text;
                    if (showName)
                    {
                        if ((data.m_flags & Building.Flags.CustomName) == Building.Flags.None && !this.buildingName.hasFocus)
                        {
                            bName = GetName(buildingId, zone, data.Info.m_class.m_subService);
                            this.buildingName.text = bName;
                        }
                    }

                    if (showDescription)
                    {
                        var desc = GetDescription(bName, buildingId, zone, data.Info.m_class.m_subService);
                        descriptionLabel.text = desc;
                        descriptionLabel.Show();
                        descriptionLabel.relativePosition = new Vector3(x, y);
                        y += descriptionLabel.height + 10;
                    }
                    else
                    {
                        descriptionLabel.Hide();
                    }
                }
            }
            height = y;

        }

        private string GetDescription(string bName, ushort buildingId, ItemClass.Zone zone, ItemClass.SubService ss)
        {

            Randomizer randomizer = new Randomizer(Singleton<SimulationManager>.instance.m_metaData.m_gameInstanceIdentifier.GetHashCode() - buildingId);
            var year = 2015 - buildingId % 200;
            Markov markov = null;
            if (!this.buildingDescriptions.TryGetValue(ss.ToString(), out markov))
            {
                this.buildingDescriptions.TryGetValue(zone.ToString(), out markov);
            }
            if (markov != null)
            {
                var text = markov.GetText(ref randomizer, 100, 200, true);
                var cityName = Singleton<SimulationManager>.instance.m_metaData.m_CityName.Trim();
                text = text.Replace("COMPANY", bName).Replace("DATE", year.ToString()).Replace("SITY", cityName);
                return text;
            }
            return "";
        }

        private string GetName(ushort buildingId, ItemClass.Zone zone, ItemClass.SubService ss)
        {
            Randomizer randomizer = new Randomizer(Singleton<SimulationManager>.instance.m_metaData.m_gameInstanceIdentifier.GetHashCode() - buildingId);
            if (buildingId % 6 != 0)
            {
                Markov markov = null;
                if (!this.buildingNames.TryGetValue(ss.ToString(), out markov))
                {
                    this.buildingNames.TryGetValue(zone.ToString(), out markov);
                }
                if (markov != null)
                {
                    return markov.GetText(ref randomizer, 6, 16, true, true);
                }
            }
            return this.buildingName.text;
        }

        private void SetProgress(UIProgressBar serviceBar, float val, float start, float target, int raw = -1, int max = -1)
<<<<<<< HEAD:ExtendedBuildings/Panels/BuildingInfoWindow.cs
        {            
            if (target == int.MaxValue)
            {
                serviceBar.tooltip = "Max!";
            }
            else if (raw != -1)
            {
                serviceBar.tooltip = raw.ToString("F0") + " / " + max.ToString("F0");
            }
            else if (start == 0)
            {
                serviceBar.tooltip = val.ToString("F0") + " / " + target.ToString("F0");
=======
        {
            var extraTip = "";
            if (target == int.MaxValue)
            {
                extraTip = Localization.Get(LocalizationCategory.BuildingInfo, "Max");
            }
            else if (raw != -1)
            {
                extraTip = raw.ToString("F0") + "/" + max.ToString("F0");
            }
            else if (start == 0)
            {
                extraTip = val.ToString("F0") + "/" + target.ToString("F0");
>>>>>>> lxteo/master:ExtendedBuildings/Panels/BuildingInfoWindow.cs

            }
            else
            {
<<<<<<< HEAD:ExtendedBuildings/Panels/BuildingInfoWindow.cs
                serviceBar.tooltip = start.ToString("F0") + " / " + val.ToString("F0") + " / " + target.ToString("F0");
            }

=======
                extraTip = start.ToString("F0") + "/" + val.ToString("F0") + "/" + target.ToString("F0");
            }

            var lastIndex = serviceBar.tooltip.LastIndexOf(' ');
            if (lastIndex > 0)
            {
                serviceBar.tooltip = serviceBar.tooltip.Substring(0, lastIndex) + " " + extraTip;
            }
            else
            {
                serviceBar.tooltip = extraTip;
            }
>>>>>>> lxteo/master:ExtendedBuildings/Panels/BuildingInfoWindow.cs
            if (target == int.MaxValue)
            {
                target = start;
                start -= 20;
            }
            serviceBar.value = Mathf.Clamp((val - start) / (float)(target - start), 0f, 1f);
        }

        private InstanceID GetParentInstanceId()
        {
            if (baseSub == null)
            {
                baseSub = this.baseBuildingWindow.GetType().GetField("m_InstanceID", BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return (InstanceID)baseSub.GetValue(this.baseBuildingWindow);
        }

    }
}
