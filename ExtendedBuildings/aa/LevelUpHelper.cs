using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtendedBuildings
{
    class LevelUpHelper3
    {
        private static LevelUpHelper3 m_instance;
        public static LevelUpHelper3 instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new LevelUpHelper3();
                }
                return m_instance;
            }
        }

        public double GetPollutionFactor(ItemClass.Zone zone)
        {
            if (zone == ItemClass.Zone.ResidentialHigh || zone == ItemClass.Zone.ResidentialLow)
            {
                return -0.2;
            }
            else if (zone == ItemClass.Zone.Office)
            {
                return -0.25;
            }
            else
            {
                return -0.1667;
            }            
        }

        public double GetFactor(ItemClass.Zone zone, ImmaterialResourceManager.Resource resource)
        {

            if (zone == ItemClass.Zone.Industrial)
            {
                switch (resource)
                {
                    case ImmaterialResourceManager.Resource.PublicTransport:
                        return 0.3333;
                    case ImmaterialResourceManager.Resource.PoliceDepartment:
                    case ImmaterialResourceManager.Resource.HealthCare:
                    case ImmaterialResourceManager.Resource.DeathCare:
                        return 0.2;
                    case ImmaterialResourceManager.Resource.FireDepartment:
                        return 0.5;
                    case ImmaterialResourceManager.Resource.Entertainment:
                    case ImmaterialResourceManager.Resource.EducationElementary:
                    case ImmaterialResourceManager.Resource.EducationHighSchool:
                    case ImmaterialResourceManager.Resource.EducationUniversity:
                        return 0.125;
                    case ImmaterialResourceManager.Resource.CargoTransport:
                        return 1;
                    case ImmaterialResourceManager.Resource.NoisePollution:
                    case ImmaterialResourceManager.Resource.Abandonment:
                        return -0.1429;
                    
                }
            }
            else if (zone == ItemClass.Zone.Office)
            {
                switch (resource)
                {
                    case ImmaterialResourceManager.Resource.PublicTransport:
                        return 0.3333;
                    case ImmaterialResourceManager.Resource.PoliceDepartment:
                    case ImmaterialResourceManager.Resource.HealthCare:
                    case ImmaterialResourceManager.Resource.DeathCare:
                    case ImmaterialResourceManager.Resource.FireDepartment:
                        return 0.2;
                    case ImmaterialResourceManager.Resource.Entertainment:
                        return 0.1667;
                    case ImmaterialResourceManager.Resource.EducationElementary:
                    case ImmaterialResourceManager.Resource.EducationHighSchool:
                    case ImmaterialResourceManager.Resource.EducationUniversity:
                        return 0.1429;
                    case ImmaterialResourceManager.Resource.NoisePollution:
                        return -0.25;
                    case ImmaterialResourceManager.Resource.Abandonment:
                        return -0.3333;
                }
            }
            else
            {
                switch (resource)
                {
                    case ImmaterialResourceManager.Resource.EducationElementary:
                    case ImmaterialResourceManager.Resource.EducationHighSchool:
                    case ImmaterialResourceManager.Resource.EducationUniversity:
                    case ImmaterialResourceManager.Resource.HealthCare:
                    case ImmaterialResourceManager.Resource.FireDepartment:
                    case ImmaterialResourceManager.Resource.PoliceDepartment:
                    case ImmaterialResourceManager.Resource.PublicTransport:
                    case ImmaterialResourceManager.Resource.DeathCare:
                    case ImmaterialResourceManager.Resource.Entertainment:
                    case ImmaterialResourceManager.Resource.CargoTransport:
                        return 1;
                    case ImmaterialResourceManager.Resource.NoisePollution:
                    case ImmaterialResourceManager.Resource.CrimeRate:
                    case ImmaterialResourceManager.Resource.FireHazard:
                    case ImmaterialResourceManager.Resource.Abandonment:
                        return -1;
                }
            }
            return 0;
        }

        public double GetPollutionScore(Building data, ItemClass.Zone zone)
        {
            byte resourceRate13;
            Singleton<NaturalResourceManager>.instance.CheckPollution(data.m_position, out resourceRate13);
            return ImmaterialResourceManager.CalculateResourceEffect((int)resourceRate13, 50, 255, 50, 100);
        }

        public double GetServiceScore(int resourceRate, ImmaterialResourceManager.Resource resource, ItemClass.Zone zone,ref int maxLimit)
        {
            if (zone == ItemClass.Zone.ResidentialHigh || zone == ItemClass.Zone.ResidentialLow || zone == ItemClass.Zone.CommercialHigh || zone == ItemClass.Zone.CommercialLow){
                switch (resource)
                {
                    case ImmaterialResourceManager.Resource.NoisePollution:
                    case ImmaterialResourceManager.Resource.CrimeRate:
                        maxLimit = 100;
                        return ImmaterialResourceManager.CalculateResourceEffect(resourceRate, 10, 100, 0, 100);
                    
                    case ImmaterialResourceManager.Resource.FireHazard:
                        maxLimit = 100;
                        return ImmaterialResourceManager.CalculateResourceEffect(resourceRate, 50,100,10,50);
                    case ImmaterialResourceManager.Resource.Abandonment:
                        maxLimit = 50;
                        return ImmaterialResourceManager.CalculateResourceEffect(resourceRate, 15, 50, 100, 200);
                }
            }
            maxLimit = 500;
            return ImmaterialResourceManager.CalculateResourceEffect(resourceRate, 100, 500, 50, 100);
        }

        public double GetServiceScore(ImmaterialResourceManager.Resource resource, ItemClass.Zone zone, ushort[] array, int num,ref int rawValue, ref int maxLimit)
        {
            rawValue = array[num + (int)resource];
            return GetServiceScore(rawValue, resource, zone, ref maxLimit);
        }

        public int GetProperServiceScore(ushort buildingID)
        {
            Building data = Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)buildingID];
            ushort[] array;
            int num;
            Singleton<ImmaterialResourceManager>.instance.CheckLocalResources(data.m_position, out array, out num);
            double num2 = 0;
            var zone = data.Info.m_class.GetZone();
            for (var i = 0; i < 20; i += 1)
            {
                int max = 0;
                int raw = 0;
                var imr = (ImmaterialResourceManager.Resource)i;
                num2 += GetServiceScore(imr, zone, array, num,ref raw,  ref max) * GetFactor(zone, imr);
            }

            num2 -= GetPollutionScore(data, zone) * GetPollutionFactor(zone);

            return Math.Max(0, (int)num2);
        }

        public void GetEducationHappyScore(ushort buildingID, out float education, out float happy, out float commute)
        {
            Citizen.BehaviourData behaviour = default(Citizen.BehaviourData);
            Building data = Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)buildingID];
            ItemClass.Zone zone = data.Info.m_class.GetZone();

            commute = 0;

            int alive = 0;
            int total = 0;
            int COMPANYCount = 0;
            int aliveCOMPANYCount = 0;
            int emptyCOMPANY = 0;

            if (zone == ItemClass.Zone.ResidentialLow || zone == ItemClass.Zone.ResidentialHigh)
            {
                CitizenHelper.instance.GetHomeBehaviour(buildingID, data, ref behaviour, ref alive, ref total, ref COMPANYCount, ref aliveCOMPANYCount, ref emptyCOMPANY);
                if (alive > 0)
                {
                    int num = behaviour.m_educated1Count + behaviour.m_educated2Count * 2 + behaviour.m_educated3Count * 3;
                    int num2 = behaviour.m_teenCount + behaviour.m_youngCount * 2 + behaviour.m_adultCount * 3 + behaviour.m_seniorCount * 3;                    
                    if (num2 != 0)
                    {
                        education = (num * 72 + (num2 >> 1)) / num2;
                        happy =  behaviour.m_wellbeingAccumulation / (float)alive;
                        return;
                    }
                }
            }
            else if (zone == ItemClass.Zone.CommercialHigh || zone == ItemClass.Zone.CommercialLow)
            {
                CitizenHelper.instance.GetVisitBehaviour(buildingID, data, ref behaviour, ref alive, ref total);
                if (alive > 0)
                {
                    int num = num = behaviour.m_wealth1Count + behaviour.m_wealth2Count * 2 + behaviour.m_wealth3Count * 3;
                    education = (num * 18 + (alive >> 1)) / alive;
                    happy =  behaviour.m_wellbeingAccumulation / (float)alive;
                    commute = 0;
                    return;
                }
            }
            else if (zone == ItemClass.Zone.Office)
            {
                CitizenHelper.instance.GetWorkBehaviour(buildingID, data, ref behaviour, ref alive, ref total);
                int num = behaviour.m_educated1Count + behaviour.m_educated2Count * 2 + behaviour.m_educated3Count * 3;
                if (alive > 0)
                {
                    education = (num * 12 + (alive >> 1)) / alive;
                    happy =  behaviour.m_wellbeingAccumulation / (float)alive;
                    return;
                }
            }
            else
            {
                CitizenHelper.instance.GetWorkBehaviour(buildingID, data, ref behaviour, ref alive, ref total);
                int num = behaviour.m_educated1Count + behaviour.m_educated2Count * 2 + behaviour.m_educated3Count * 3;
                if (alive > 0)
                {
                    education = num = (num * 20 + (alive >> 1)) / alive;
                    happy =  behaviour.m_wellbeingAccumulation / (float)alive;
                    return;
                }
            }

            education = 0;
            happy = 0;
            commute = 0;
        }
        
        public int GetServiceThreshhold(ItemClass.Level level, ItemClass.Zone zone)
        {
            switch (zone)
            {
                case ItemClass.Zone.Office:
                    if (level == ItemClass.Level.None)
                    {
                        return 0;
                    }
                    else if (level == ItemClass.Level.Level1)
                    {
                            return 45;
                    }
                    else if (level == ItemClass.Level.Level2)
                    {
                            return 90;
                        
                    }
                    else
                    {
                        return int.MaxValue;
                    }
                case ItemClass.Zone.Industrial:
                    if (level == ItemClass.Level.None)
                    {
                        return 0;
                    }
                    else if (level == ItemClass.Level.Level1)
                    {                        
                            return 30;                       
                    }
                    else if (level == ItemClass.Level.Level2)
                    {
                            return 60;
                    }
                    else
                    {
                        return int.MaxValue;
                    }
                case ItemClass.Zone.ResidentialLow:
                case ItemClass.Zone.ResidentialHigh:
                    if (level == ItemClass.Level.None)
                    {
                        return 0;
                    }
                    else if (level == ItemClass.Level.Level1)
                    {
                            return 6;
                    }
                    else if (level == ItemClass.Level.Level2)
                    {
                        return 21;
                    }

                    else if (level == ItemClass.Level.Level3)
                    {
                            return 41;
                    }

                    else if (level == ItemClass.Level.Level4)
                    {
                        return 61;
                    }
                    else
                    {
                        return int.MaxValue;
                    }
                case ItemClass.Zone.CommercialLow:
                case ItemClass.Zone.CommercialHigh:
                    if (level == ItemClass.Level.None)
                    {
                        return 0;
                    }
                    else if (level == ItemClass.Level.Level1)
                    {
                            return 21;
                    }
                    else if (level == ItemClass.Level.Level2)
                    {
                            return 41;
                    }
                    else
                    {
                        return int.MaxValue;
                    }                    
            }
            return int.MaxValue;
        }

        public int GetEducationThreshhold(ItemClass.Level level, ItemClass.Zone zone)
        {
            if (level == ItemClass.Level.None)
            {
                return 0;
            }
            if (zone == ItemClass.Zone.ResidentialHigh || zone == ItemClass.Zone.ResidentialLow)
            {
                if (level == ItemClass.Level.Level1)
                {

                    return 15;
                }
                else if (level == ItemClass.Level.Level2)
                {
                    return 30;
                }
                else if (level == ItemClass.Level.Level3)
                {
                    return 45;
                }
                else if (level == ItemClass.Level.Level4)
                {
                    return 60;
                }
                else
                {
                    return int.MaxValue;
                }
            }
            else if (zone == ItemClass.Zone.Industrial)
            {
                if (level == ItemClass.Level.Level1)
                {
                    return 15;
                }
                else if (level == ItemClass.Level.Level2)
                {
                    return 30;
                }
                else
                {
                    return int.MaxValue;
                }
            }
            else if (zone == ItemClass.Zone.CommercialLow || zone == ItemClass.Zone.CommercialHigh)
            {
                if (level == ItemClass.Level.Level1)
                {
                    return 30;
                }
                else if (level == ItemClass.Level.Level2)
                {
                    return 45;
                }
                else
                {
                    return int.MaxValue;
                }
            }
            else
            {

                if (level == ItemClass.Level.Level1)
                {
                    return 15;
                }
                else if (level == ItemClass.Level.Level2)
                {
                    return 30;
                }
                else
                {
                    return int.MaxValue;
                }
            }
        }
    }
}
