using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedBuildings
{
    using ColossalFramework;
    using ColossalFramework.Math;
    using ColossalFramework.UI;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Timers;
    using UnityEngine;
    public class BuildingInfoWindow7 : UIPanel
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

        Markov buildingNames;
        Markov buildingDescriptions;

        UILabel descriptionLabel;

        ushort setName;
        bool showDescription = true;

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
                label.text = GetName(res);
                label.textScale = 0.5f;
                label.size = new Vector2(100, 20);
                resourceLabels.Add(res, label);
            }

            pollutionBar = AddUIComponent<UIProgressBar>();
            pollutionBar.backgroundSprite = "LevelBarBackground";
            pollutionBar.progressSprite = "LevelBarForeground";
            pollutionBar.progressColor = Color.red;
            pollutionLabel = AddUIComponent<UILabel>();
            pollutionLabel.text = "Pollution";
            pollutionLabel.textScale = 0.5f;
            pollutionLabel.size = new Vector2(100, 20);

            serviceLabel = AddUIComponent<UILabel>();
            serviceBar = AddUIComponent<UIProgressBar>();

            educationLabel = AddUIComponent<UILabel>();
            educationBar = AddUIComponent<UIProgressBar>();

            happyLabel = AddUIComponent<UILabel>();
            happyBar = AddUIComponent<UIProgressBar>();

            buildingNames = new Markov(Properties.Resources.nameCommercial, false, 4);
            buildingDescriptions = new Markov(Properties.Resources.descriptionsCommercial, false, 7);

            descriptionLabel = AddUIComponent<UILabel>();

            base.Awake();

        }

        private string GetName(ImmaterialResourceManager.Resource res)
        {
            switch (res)
            {
                case ImmaterialResourceManager.Resource.FireDepartment:
                    return "Fire";
                case ImmaterialResourceManager.Resource.PoliceDepartment:
                    return "Police";
                case ImmaterialResourceManager.Resource.PublicTransport:
                    return "Transp.";
                case ImmaterialResourceManager.Resource.Abandonment:
                    return "Abandonment";
                case ImmaterialResourceManager.Resource.Entertainment:
                    return "Parks";
                case ImmaterialResourceManager.Resource.NoisePollution:
                    return "Noise";
                case ImmaterialResourceManager.Resource.CargoTransport:
                    return "Cargo";
                case ImmaterialResourceManager.Resource.EducationElementary:
                    return "Elem.";
                case ImmaterialResourceManager.Resource.EducationHighSchool:
                    return "High Sch.";
                case ImmaterialResourceManager.Resource.EducationUniversity:
                    return "Uni.";
                case ImmaterialResourceManager.Resource.HealthCare:
                    return "Health";
                case ImmaterialResourceManager.Resource.DeathCare:
                    return "Death";
            }
            return res.ToString();
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

            SetLabel(serviceLabel, "Service Progress");
            SetBar(serviceBar);
            serviceBar.tooltip = "Progress until next level, combined score of factors shown above.";
            serviceLabel.tooltip = "Progress until next level, combined score of factors shown above.";
            y += vertPadding;

            SetLabel(educationLabel, "Education Progress");
            SetBar(educationBar);
            educationBar.tooltip = "Progress until next level.";
            educationLabel.tooltip = "Progress until next level.";
            y += vertPadding;

            SetLabel(happyLabel, "Happiness");
            SetBar(happyBar);
            happyBar.tooltip = "Average happiness.";
            happyLabel.tooltip = "Average happiness.";

            SetLabel(descriptionLabel, "Happiness");
            descriptionLabel.textScale = 0.65f;
            descriptionLabel.wordWrap = true;
            //descriptionLabel.size = new Vector2(barWidth - 20, 140);
            descriptionLabel.autoSize = false;
            descriptionLabel.width = barWidth;
            descriptionLabel.wordWrap = true;
            descriptionLabel.autoHeight = true;
            descriptionLabel.anchor = (UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right);

            y += vertPadding;
            height = y;
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
                if (this.baseBuildingWindow != null && this.enabled && isVisible && Singleton<BuildingManager>.exists && ((Singleton<SimulationManager>.instance.m_currentFrameIndex & 15u) == 15u || setName != building))
                    {
                    BuildingManager instance = Singleton<BuildingManager>.instance;
                    this.UpdateBuildingInfo(building, instance.m_buildings.m_buffer[(int)building]);
                    setName = building;
                }
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
                    var value = levelUpHelper.GetServiceScore(resBar.Key, zone, array, num);

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
                    resBar.Value.value = (float)(value / 100f);
                }
            }

            if (levelUpHelper.GetPollutionFactor(zone) < 0)
            {
                var value = levelUpHelper.GetPollutionScore(data, zone);
                var factor = levelUpHelper.GetPollutionFactor(zone);

                pollutionBar.size = new Vector2((float)(barWidth * -factor / totalNegativeFactor), 16);
                pollutionLabel.relativePosition = new Vector3(negativeX, 56);
                pollutionBar.value = (float)value / 100f;
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
                serviceLabel.text = "Land Value Progress";
            }
            else
            {
                serviceLabel.text = "Service Progress";
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
                educationLabel.text = "Wealth Progress";
            }
            else
            {
                educationLabel.text = "Education Progress";
            }

            y += 10;

            SetProgress(happyBar, happy, 0, 100);
            SetPos(happyLabel, happyBar, x, y, true);
            y += vertPadding;

            if (buildingName == null)
            {
                this.buildingName = this.baseBuildingWindow.Find<UITextField>("BuildingName");
            }

            var bName = this.buildingName.text;
            if (buildingName != null && (data.m_flags & Building.Flags.CustomName) == Building.Flags.None && !this.buildingName.hasFocus)
            {
                bName = GetName(buildingId, zone);
                this.buildingName.text = bName;
            }

            if (showDescription && (zone == ItemClass.Zone.Industrial || zone == ItemClass.Zone.CommercialLow || zone == ItemClass.Zone.CommercialHigh || zone == ItemClass.Zone.Office))
            {
                var desc = GetDescription(bName, buildingId, zone);
                descriptionLabel.text = desc;
                descriptionLabel.Show();
                descriptionLabel.relativePosition = new Vector3(x, y);
                y += descriptionLabel.height + 10;
            }
            else
            {
                descriptionLabel.Hide();
            }
            height = y;

        }

        private string GetDescription(string bName, ushort buildingId, ItemClass.Zone zone)
        {
            Randomizer randomizer = new Randomizer(buildingId);
            var year = 2015 - buildingId % 200;
            var text = this.buildingDescriptions.GetText(ref randomizer, 120, 220, true);
            text = text.Replace("COMPANY", bName).Replace("DATE", year.ToString());
            return text;
        }

        private string GetName(ushort buildingId,ItemClass.Zone zone)
        {
            Randomizer randomizer = new Randomizer(buildingId);
            if (buildingId % 4 != 0 && zone == ItemClass.Zone.Industrial || zone == ItemClass.Zone.Office)
            {
                return this.buildingNames.GetText(ref randomizer, 14, 80, true);                
            }
            else
            {
                return this.buildingName.text;
            }
            
        }

        private void SetProgress(UIProgressBar serviceBar, float val, float start, float target)
        {
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
