using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedBuildings
{
    using ColossalFramework;
    using ColossalFramework.UI;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Timers;
    using UnityEngine;
    public class BuildingInfoWindow : UIPanel
    {
        //const float vertPadding = 26;
        //float barWidth;
        //Dictionary<ImmaterialResourceManager.Resource, UIProgressBar> resourceBars;
        //Dictionary<ImmaterialResourceManager.Resource, UILabel> resourceLabels;

        //UIProgressBar pollutionBar;
        //UILabel pollutionLabel;

        //UILabel serviceLabel;
        //UIProgressBar serviceBar;

        //UILabel wealthLabel;
        //UIProgressBar wealthBar;

        //UILabel educationLabel;
        //UIProgressBar educationBar;

        //UILabel waitLabel;
        //UIProgressBar commuteWaitTimeBar;

        //UILabel happyLabel;
        //UIProgressBar happyBar;

        //UILabel incomeLabel;

        //public ZonedBuildingWorldInfoPanel baseBuildingWinCOMPANY;
        //FieldInfo baseSub;

        //public override void Awake()
        //{
        //    resourceBars = new Dictionary<ImmaterialResourceManager.Resource, UIProgressBar>();
        //    resourceLabels = new Dictionary<ImmaterialResourceManager.Resource, UILabel>();

        //    for (var i = 0; i < 20; i += 1)
        //    {
        //        var res = (ImmaterialResourceManager.Resource)i;
        //        var bar = AddUIComponent<UIProgressBar>();
        //        bar.backgroundSprite = "LevelBarBackground";
        //        bar.progressSprite = "LevelBarForeground";
        //        bar.progressColor = Color.green;
        //        resourceBars.Add(res, bar);
        //        var label = AddUIComponent<UILabel>();
        //        label.text = GetName(res);
        //        label.textScale = 0.5f;
        //        label.size = new Vector2(100, 20);
        //        resourceLabels.Add(res, label);
        //    }

        //    pollutionBar = AddUIComponent<UIProgressBar>();
        //    pollutionBar.backgroundSprite = "LevelBarBackground";
        //    pollutionBar.progressSprite = "LevelBarForeground";
        //    pollutionBar.progressColor = Color.red;
        //    pollutionLabel = AddUIComponent<UILabel>();
        //    pollutionLabel.text = "Pollution";
        //    pollutionLabel.textScale = 0.5f;
        //    pollutionLabel.size = new Vector2(100, 20);

        //    serviceLabel = AddUIComponent<UILabel>();
        //    serviceBar = AddUIComponent<UIProgressBar>();

        //    wealthLabel = AddUIComponent<UILabel>();
        //    wealthBar = AddUIComponent<UIProgressBar>();

        //    educationLabel = AddUIComponent<UILabel>();
        //    educationBar = AddUIComponent<UIProgressBar>();

        //    waitLabel = AddUIComponent<UILabel>();
        //    commuteWaitTimeBar = AddUIComponent<UIProgressBar>();
            
        //    happyLabel = AddUIComponent<UILabel>();
        //    happyBar = AddUIComponent<UIProgressBar>();

        //    incomeLabel = AddUIComponent<UILabel>();
        //    base.Awake();

        //}

        //private string GetName(ImmaterialResourceManager.Resource res)
        //{
        //    switch (res)
        //    {
        //        case ImmaterialResourceManager.Resource.FireDepartment:
        //            return "Fire";
        //        case ImmaterialResourceManager.Resource.PoliceDepartment:
        //            return "Police";
        //        case ImmaterialResourceManager.Resource.PublicTransport:
        //            return "Transport";
        //        case ImmaterialResourceManager.Resource.Abandonment:
        //            return "Abandonment";
        //        case ImmaterialResourceManager.Resource.Entertainment:
        //            return "Entertainment";
        //        case ImmaterialResourceManager.Resource.NoisePollution:
        //            return "Noise";
        //        case ImmaterialResourceManager.Resource.CargoTransport:
        //            return "Cargo";            
        //        case ImmaterialResourceManager.Resource.EducationElementary:
        //            return "Elem.";
        //        case ImmaterialResourceManager.Resource.EducationHighSchool:
        //            return "High Sch.";
        //        case ImmaterialResourceManager.Resource.EducationUniversity:
        //            return "Uni.";
        //        case ImmaterialResourceManager.Resource.HealthCare:
        //            return "Health";
        //        case ImmaterialResourceManager.Resource.DeathCare:
        //            return "Death";
        //    }
        //    return res.ToString();        
        //}

        //public override void Start()
        //{
        //    base.Start();

        //    //relativePosition = new Vector3(396, 58);
        //    backgroundSprite = "MenuPanel2";
        //    opacity = 0.75f;
        //    isVisible = true;
        //    canFocus = true;
        //    isInteractive = true;
        //    SetupControls();
        //}

        //public void SetupControls()
        //{
        //    base.Start();
            
        //    barWidth = this.size.x - 28;
        //    float y = 70;

        //    SetLabel(serviceLabel, "Service Progress");
        //    SetBar(serviceBar);
        //    serviceBar.tooltip = "Progress until next level, combined score of factors shown above.";
        //    serviceLabel.tooltip = "Progress until next level, combined score of factors shown above.";
        //    y += vertPadding;

        //    SetLabel(wealthLabel, "Wealth Progress");
        //    SetBar(wealthBar);
        //    y += vertPadding;

        //    SetLabel(educationLabel, "Education Progress");
        //    SetBar(educationBar);
        //    educationBar.tooltip = "Progress until next level, educate more cims to increase.";
        //    educationLabel.tooltip = "Progress until next level, educate more cims to increase.";
        //    y += vertPadding;

        //    SetLabel(waitLabel, "Idle Commute Time");
        //    SetBar(commuteWaitTimeBar);
        //    commuteWaitTimeBar.tooltip = "Average time cims spend waiting (for public transport or stopped in traffic), affects their happiness.";
        //    waitLabel.tooltip = "Average time cims spend waiting (for public transport or stopped in traffic), affects their happiness.";
        //    commuteWaitTimeBar.progressColor = Color.red;

        //    y += vertPadding;

        //    SetLabel(happyLabel, "Happiness");
        //    SetBar(happyBar);
        //    happyBar.tooltip = "Average happiness, affects amount of tax paid.";
        //    happyLabel.tooltip = "Average happiness, affects amount of tax paid.";
        //    happyBar.size = new Vector2(barWidth - 260, 16);


        //    SetLabel(incomeLabel, "Tax Income:");
        //    incomeLabel.tooltip = "Total building tax income.";
        //    y += vertPadding;
        //    height = y;
        //}

        //private void SetBar(UIProgressBar bar)
        //{
        //    bar.backgroundSprite = "LevelBarBackground";
        //    bar.progressSprite = "LevelBarForeground";
        //    bar.progressColor = Color.green;
        //    bar.size = new Vector2(barWidth - 120, 16);
        //    bar.minValue = 0f;
        //    bar.maxValue = 1f;
        //}

        //private void SetLabel(UILabel title, string p)
        //{
        //    title.text = p;
        //    title.textScale = 0.7f;
        //    title.size = new Vector2(120, 30);
        //}

        //private void SetPos(UILabel title,UIProgressBar bar, float x, float y,bool visible)
        //{
        //    bar.relativePosition = new Vector3(x + 120, y - 3);
        //    title.relativePosition = new Vector3(x, y);
        //    if (visible)
        //    {
        //        bar.Show();
        //        title.Show();
        //    }
        //    else
        //    {
        //        bar.Hide();
        //        title.Hide();
        //    }
        //}

        //public override void Update()
        //{
        //    if (this.baseBuildingWinCOMPANY != null && this.enabled && isVisible && Singleton<BuildingManager>.exists && (Singleton<SimulationManager>.instance.m_currentFrameIndex & 7u) == 7u)
        //    {
        //        var instanceId = GetParentInstanceId();
        //        if (instanceId.Type == InstanceType.Building && instanceId.Building != 0)
        //        {
        //            ushort building = instanceId.Building;
        //            BuildingManager instance = Singleton<BuildingManager>.instance;

        //            this.UpdateBuildingInfo(building, instance.m_buildings.m_buffer[(int)building]);
        //        }
        //    }

        //    base.Update();
        //}

        //private void UpdateBuildingInfo(ushort buildingId, Building building)
        //{
        //    var levelUpHelper = LevelUpHelper3.instance;
        //    var info = building.Info;
        //    var zone = info.m_class.GetZone();
        //    Building data = Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)buildingId];
        //    ushort[] array;
        //    int num;
        //    Singleton<ImmaterialResourceManager>.instance.CheckLocalResources(data.m_position, out array, out num);
        //    double totalFactor = 0;
        //    double totalNegativeFactor = 0;
        //    foreach (var resBar in this.resourceBars)
        //    {
        //        if (levelUpHelper.GetFactor(zone, resBar.Key) > 0)
        //        {
        //            totalFactor += levelUpHelper.GetFactor(zone, resBar.Key);
        //        }
        //        else
        //        {
        //            totalNegativeFactor -= levelUpHelper.GetFactor(zone, resBar.Key);
        //        }
        //    }
        //    totalNegativeFactor -= levelUpHelper.GetPollutionFactor(zone);

        //    var x = 14f;
        //    var negativeX = 14f;
        //    foreach (var resBar in this.resourceBars)
        //    {
        //        var label = this.resourceLabels[resBar.Key];
        //        var factor = levelUpHelper.GetFactor(zone, resBar.Key);
        //        if (factor == 0)
        //        {
        //            label.Hide();
        //            resBar.Value.Hide();
        //        }
        //        else
        //        {
        //            label.Show();
        //            resBar.Value.Show();
        //            var value = levelUpHelper.GetServiceScore(resBar.Key, zone, array, num);

        //            if (factor > 0)
        //            {
        //                resBar.Value.size = new Vector2((float)(barWidth * factor / totalFactor), 16);
        //                label.relativePosition = new Vector3(x, 10);
        //                resBar.Value.relativePosition = new Vector3(x, 20);
        //                x += resBar.Value.size.x;
        //            }
        //            else
        //            {
        //                resBar.Value.size = new Vector2((float)(barWidth * -factor / totalNegativeFactor), 16);
        //                label.relativePosition = new Vector3(negativeX, 56);
        //                resBar.Value.relativePosition = new Vector3(negativeX, 36);
        //                negativeX += resBar.Value.size.x;
        //                resBar.Value.progressColor = Color.red;
        //            }
        //            resBar.Value.value = (float)(value / 100f);
        //            //label.text = value.ToString();
        //        }
        //    }

        //    if (levelUpHelper.GetPollutionFactor(zone) < 0)
        //    {
        //        var value = levelUpHelper.GetPollutionScore(data, zone);
        //        var factor = levelUpHelper.GetPollutionFactor(zone);

        //        pollutionBar.size = new Vector2((float)(barWidth * -factor / totalNegativeFactor), 16);
        //        pollutionLabel.relativePosition = new Vector3(negativeX, 56);
        //        pollutionBar.value = (float)value / 100f;
        //        pollutionBar.relativePosition = new Vector3(negativeX, 36);
        //        negativeX += pollutionBar.size.x;

        //        pollutionBar.Show();
        //        pollutionLabel.Show();
        //    }
        //    else
        //    {
        //        pollutionBar.Hide();
        //        pollutionLabel.Hide();
        //    }

        //    x = 14f;
        //    float y = 70f;
        //    SetProgress(serviceBar, levelUpHelper.GetProperServiceScore(buildingId), levelUpHelper.GetServiceThreshhold((ItemClass.Level)(Math.Max(-1, (int)data.Info.m_class.m_level - 1)), zone), levelUpHelper.GetServiceThreshhold(data.Info.m_class.m_level, zone));
        //    SetPos(serviceLabel, serviceBar, x, y,true);
        //    y += vertPadding;

        //    float education;
        //    float happy;
        //    float commute;
        //    levelUpHelper.GetEducationHappyScore(buildingId, out education, out happy, out commute);

        //    if (zone == ItemClass.Zone.CommercialHigh || zone == ItemClass.Zone.CommercialLow)
        //    {
        //        SetPos(educationLabel, educationBar, x, y, false);
        //    }
        //    else
        //    {
        //        SetProgress(educationBar, education, levelUpHelper.GetEducationThreshhold((ItemClass.Level)(Math.Max(-1, (int)data.Info.m_class.m_level - 1)), zone), levelUpHelper.GetEducationThreshhold(data.Info.m_class.m_level, zone));
        //        SetPos(educationLabel, educationBar, x, y, true);
        //        y += vertPadding;
        //    }
            

        //    if (zone == ItemClass.Zone.ResidentialHigh || zone == ItemClass.Zone.ResidentialLow)
        //    {
        //        SetProgress(wealthBar, data.m_customBuffer1, levelUpHelper.GetWealthThreshhold((ItemClass.Level)(Math.Max(-1, (int)data.Info.m_class.m_level - 1)), zone), levelUpHelper.GetWealthThreshhold(data.Info.m_class.m_level, zone));
        //        SetPos(wealthLabel, wealthBar, x, y, true);
        //        wealthBar.tooltip = "Progress until next level, increases when cims reach work or shops.";
        //        wealthLabel.tooltip = "Progress until next level, increases when cims reach work or shops.";
        //        y += vertPadding;
        //    }
        //    else if (zone == ItemClass.Zone.CommercialHigh || zone == ItemClass.Zone.CommercialLow)
        //    {

        //        SetProgress(wealthBar, education, levelUpHelper.GetWealthThreshhold((ItemClass.Level)(Math.Max(-1, (int)data.Info.m_class.m_level - 1)), zone), levelUpHelper.GetWealthThreshhold(data.Info.m_class.m_level, zone));
        //        SetPos(wealthLabel, wealthBar, x, y, true);
        //        wealthBar.tooltip = "Progress until next level, increases when wealthier cims shop here.";
        //        wealthLabel.tooltip = "Progress until next level, increases when wealthier cims shop here.";
        //        y += vertPadding;
        //    }
        //    else
        //    {
        //        SetPos(wealthLabel, wealthBar, x, y, false);
        //    }
        //    y += 10;

        //    SetProgress(happyBar, happy, 0, 100);
        //    SetPos(happyLabel, happyBar, x, y, true);
        //    incomeLabel.relativePosition = new Vector3(barWidth - 90, y);

        //    y += vertPadding;

        //    SetProgress(commuteWaitTimeBar, commute, 0, 100);            
        //    SetPos(waitLabel, commuteWaitTimeBar, x, y, true);
        //    y += vertPadding;

        //    int income = 0;
        //    int tourists = 0;
        //    CitizenHelper.instance.GetIncome(buildingId, data,ref income,ref tourists);

        //    incomeLabel.text = "Tax Income: " + ((income + tourists) / 100.0).ToString("0.00");
        //    height = y;
        //}

        //private void SetProgress(UIProgressBar serviceBar, float val, float start, float target)
        //{
        //    if (target == int.MaxValue)
        //    {
        //        target = start;
        //        start -= 20;
        //    }
        //    serviceBar.value = Mathf.Clamp((val - start) / (float)(target - start),0f,1f);
        //}

        //private InstanceID GetParentInstanceId()
        //{
        //    if (baseSub == null)
        //    {
        //        baseSub = this.baseBuildingWinCOMPANY.GetType().GetField("m_InstanceID", BindingFlags.NonPublic | BindingFlags.Instance);
        //    }
        //    return (InstanceID)baseSub.GetValue(this.baseBuildingWinCOMPANY);
        //}

    }
}
