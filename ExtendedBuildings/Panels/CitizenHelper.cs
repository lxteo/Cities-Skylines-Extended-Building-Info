using ColossalFramework;
using ColossalFramework.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtendedBuildings
{
    class CitizenHelper
    {
        private static CitizenHelper m_instance;
        public static CitizenHelper instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CitizenHelper();
                }
                return m_instance;
            }
        }

        public void GetCitizenIncome(CitizenUnit.Flags flags, Building buildingData, ref int income, ref int tourists)
        {
            int level = 0;
            UnlockManager um = Singleton<UnlockManager>.instance;
            if (!um.Unlocked(ItemClass.SubService.PublicTransportMetro))
            {
                level += 1;
                if (!um.Unlocked(ItemClass.Service.PublicTransport))
                {
                    level += 1;
                    if (!um.Unlocked(ItemClass.Service.HealthCare))
                    {
                        level += 1;
                    }
                }
            }

            CitizenManager instance = Singleton<CitizenManager>.instance;
            uint num = buildingData.m_citizenUnits;
            int num2 = 0;
            while (num != 0u)
            {
                if ((ushort)(instance.m_units.m_buffer[(int)((UIntPtr)num)].m_flags & flags) != 0)
                {
                    GetCitizenIncome(instance.m_units.m_buffer[(int)((UIntPtr)num)], level, ref income, ref tourists);
                }
                num = instance.m_units.m_buffer[(int)((UIntPtr)num)].m_nextUnit;
                if (++num2 > 524288)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                    break;
                }
            }
        }

        public void GetCitizenIncome(CitizenUnit citizenUnit, int level, ref int income, ref int tourists)
        {
            CitizenManager instance = Singleton<CitizenManager>.instance;
            if (citizenUnit.m_citizen0 != 0u)
            {
                GetCitizenIncome(instance.m_citizens.m_buffer[(int)((UIntPtr)citizenUnit.m_citizen0)], level, ref income, ref tourists);
            }
            if (citizenUnit.m_citizen1 != 0u)
            {
                GetCitizenIncome(instance.m_citizens.m_buffer[(int)((UIntPtr)citizenUnit.m_citizen1)], level, ref income, ref tourists);
            }
            if (citizenUnit.m_citizen2 != 0u)
            {
                GetCitizenIncome(instance.m_citizens.m_buffer[(int)((UIntPtr)citizenUnit.m_citizen2)], level, ref income, ref tourists);
            }
            if (citizenUnit.m_citizen3 != 0u)
            {
                GetCitizenIncome(instance.m_citizens.m_buffer[(int)((UIntPtr)citizenUnit.m_citizen3)], level, ref income, ref tourists);
            }
            if (citizenUnit.m_citizen4 != 0u)
            {
                GetCitizenIncome(instance.m_citizens.m_buffer[(int)((UIntPtr)citizenUnit.m_citizen4)], level, ref income, ref tourists);
            }
        }

        public void GetCitizenIncome(Citizen citizen, int level, ref int income, ref int tourists)
        {
            if ((citizen.m_flags & Citizen.Flags.MovingIn) == Citizen.Flags.None && !citizen.Dead)
            {
                bool tourist = ((citizen.m_flags & Citizen.Flags.Tourist) != Citizen.Flags.None);
                int age = citizen.Age;
                Citizen.Education educationLevel = citizen.EducationLevel;
                Citizen.AgePhase agePhase = Citizen.GetAgePhase(educationLevel, age);
                int unemployed = citizen.Unemployed;
                var result = 0;

                if (citizen.Sick && level < 3 && !tourist)
                {
                    result -= 50;
                }
                if (unemployed == 0 || tourist)
                {
                    switch (agePhase)
                    {
                        case Citizen.AgePhase.Child:
                            result += 20;
                            break;
                        case Citizen.AgePhase.Teen0:
                            result += 30;
                            break;
                        case Citizen.AgePhase.Teen1:
                            result += 40;
                            break;
                        case Citizen.AgePhase.Young0:
                            result += 50;
                            break;
                        case Citizen.AgePhase.Young1:
                            result += 60;
                            break;
                        case Citizen.AgePhase.Young2:
                            result += 70;
                            break;
                        case Citizen.AgePhase.Adult0:
                            result += 50;
                            break;
                        case Citizen.AgePhase.Adult1:
                            result += 60;
                            break;
                        case Citizen.AgePhase.Adult2:
                            result += 70;
                            break;
                        case Citizen.AgePhase.Adult3:
                            result += 80;
                            break;
                        case Citizen.AgePhase.Senior0:
                            result += 50;
                            break;
                        case Citizen.AgePhase.Senior1:
                            result += 60;
                            break;
                        case Citizen.AgePhase.Senior2:
                            result += 70;
                            break;
                        case Citizen.AgePhase.Senior3:
                            result += 80;
                            break;
                    }
                }

                if (tourist || level > 2)
                {
                }
                else if (level == 2)
                {
                    result += Math.Max(-10, citizen.m_health + citizen.m_wellbeing - 140);
                }
                else if (level == 1)
                {
                    result += Math.Max(-20, citizen.m_health + citizen.m_wellbeing - 145);
                }
                else
                {
                    result += Math.Max(-44, citizen.m_health + citizen.m_wellbeing - 155);
                }

                if (tourist)
                {
                    tourists += result;
                }
                else
                {
                    income += result;
                }
            }
        }


        public void GetHomeBehaviour(ushort buildingID, Building buildingData, ref Citizen.BehaviourData behaviour, ref int aliveCount, ref int totalCount, ref int homeCount, ref int aliveHomeCount, ref int emptyHomeCount)
        {
            CitizenManager instance = Singleton<CitizenManager>.instance;
            uint num = buildingData.m_citizenUnits;
            int num2 = 0;
            while (num != 0u)
            {
                if ((ushort)(instance.m_units.m_buffer[(int)((UIntPtr)num)].m_flags & CitizenUnit.Flags.Home) != 0)
                {
                    int num3 = 0;
                    int num4 = 0;
                    instance.m_units.m_buffer[(int)((UIntPtr)num)].GetCitizenHomeBehaviour(ref behaviour, ref num3, ref num4);
                    if (num3 != 0)
                    {
                        aliveHomeCount++;
                        aliveCount += num3;
                    }
                    if (num4 != 0)
                    {
                        totalCount += num4;
                    }
                    else
                    {
                        emptyHomeCount++;
                    }
                    homeCount++;
                }
                num = instance.m_units.m_buffer[(int)((UIntPtr)num)].m_nextUnit;
                if (++num2 > 524288)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                    break;
                }
            }
        }

        public void GetWorkBehaviour(ushort buildingID, Building buildingData, ref Citizen.BehaviourData behaviour, ref int aliveCount, ref int totalCount)
        {
            CitizenManager instance = Singleton<CitizenManager>.instance;
            uint num = buildingData.m_citizenUnits;
            int num2 = 0;
            while (num != 0u)
            {
                if ((ushort)(instance.m_units.m_buffer[(int)((UIntPtr)num)].m_flags & CitizenUnit.Flags.Work) != 0)
                {
                    instance.m_units.m_buffer[(int)((UIntPtr)num)].GetCitizenWorkBehaviour(ref behaviour, ref aliveCount, ref totalCount);
                }
                num = instance.m_units.m_buffer[(int)((UIntPtr)num)].m_nextUnit;
                if (++num2 > 524288)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                    break;
                }
            }
        }

        public void GetVisitBehaviour(ushort buildingID, Building buildingData, ref Citizen.BehaviourData behaviour, ref int aliveCount, ref int totalCount)
        {
            CitizenManager instance = Singleton<CitizenManager>.instance;
            uint num = buildingData.m_citizenUnits;
            int num2 = 0;
            while (num != 0u)
            {
                if ((ushort)(instance.m_units.m_buffer[(int)((UIntPtr)num)].m_flags & CitizenUnit.Flags.Visit) != 0)
                {
                    instance.m_units.m_buffer[(int)((UIntPtr)num)].GetCitizenVisitBehaviour(ref behaviour, ref aliveCount, ref totalCount);
                }
                num = instance.m_units.m_buffer[(int)((UIntPtr)num)].m_nextUnit;
                if (++num2 > 524288)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
                    break;
                }
            }
        }
        
        internal void GetIncome(ushort buildingID, Building buildingData, ref int income, ref int tourists)
        {
            DistrictManager instance = Singleton<DistrictManager>.instance;
            byte district = instance.GetDistrict(buildingData.m_position);
            DistrictPolicies.Services servicePolicies = instance.m_districts.m_buffer[(int)district].m_servicePolicies;
            ItemClass.Zone zone = buildingData.Info.m_class.GetZone();

            int baseIncome = 0;// GetBaseIncome(buildingData.Info.m_class.m_level, zone);
            if ((servicePolicies & DistrictPolicies.Services.Recycling) != DistrictPolicies.Services.None)
            {
                baseIncome = baseIncome * 95 / 100;
            }

            int percentage = 100;

            if (zone == ItemClass.Zone.ResidentialHigh || zone == ItemClass.Zone.ResidentialLow)
            {
                GetCitizenIncome(CitizenUnit.Flags.Home, buildingData, ref income, ref tourists);
            }
            else if (zone == ItemClass.Zone.CommercialHigh || zone == ItemClass.Zone.CommercialLow)
            {
                GetCitizenIncome(CitizenUnit.Flags.Visit, buildingData, ref income, ref tourists);

                DistrictPolicies.CityPlanning cityPlanningPolicies = instance.m_districts.m_buffer[(int)district].m_cityPlanningPolicies;
                if (buildingData.Info.m_class.m_subService == ItemClass.SubService.CommercialLow)
                {
                    if ((cityPlanningPolicies & DistrictPolicies.CityPlanning.SmallBusiness) != DistrictPolicies.CityPlanning.None)
                    {
                        baseIncome += 20;
                    }
                }
                else if ((cityPlanningPolicies & DistrictPolicies.CityPlanning.BigBusiness) != DistrictPolicies.CityPlanning.None)
                {
                    baseIncome += 20;
                }
                if ((servicePolicies & DistrictPolicies.Services.RecreationalUse) != DistrictPolicies.Services.None)
                {
                    baseIncome = (baseIncome * 105 + 99) / 100;
                }
                if (buildingData.m_outgoingProblemTimer >= 128 || buildingData.m_customBuffer1 == 0)
                {
                    percentage = 0;
                }
            }
            else if (zone == ItemClass.Zone.Industrial)
            {
                GetCitizenIncome(CitizenUnit.Flags.Work, buildingData, ref income, ref tourists);

                if (buildingData.m_outgoingProblemTimer >= 128 || buildingData.m_customBuffer1 == 0)
                {
                    percentage = 0;
                }
            }
            else if (zone == ItemClass.Zone.Office)
            {
                GetCitizenIncome(CitizenUnit.Flags.Work, buildingData, ref income, ref tourists);
            }


            income = (income * baseIncome + 99) / 100;
            tourists = (tourists * baseIncome + 99) / 100;
            if (buildingData.m_electricityProblemTimer >= 1 || buildingData.m_waterProblemTimer >= 1 || buildingData.m_waterProblemTimer >= 1 || buildingData.m_garbageBuffer > 60000)
            {
                percentage = 0;
            }
            float num18 = Singleton<TerrainManager>.instance.WaterLevel(VectorUtils.XZ(buildingData.m_position));
            if (num18 > buildingData.m_position.y)
            {
                percentage = 0;
            }
            income = (income * percentage + 99) / 100;
            tourists = (tourists * percentage * 2 + 99) / 100;
        }
    }
}
