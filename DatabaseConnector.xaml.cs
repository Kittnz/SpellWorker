using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace SpellWorker
{
    public class DatabaseConnector
    {
        private List<SpellDuration> spellDurations;
        private List<SpellCastTime> spellCastTimes;
        private List<SpellRange> spellRanges;
        private List<SpellRadius> spellRadiuses;

        // Property to access durations
        public List<SpellDuration> SpellDurations => spellDurations;
        public List<SpellCastTime> SpellCastTimes => spellCastTimes;
        public List<SpellRange> SpellRanges => spellRanges;
        public List<SpellRadius> SpellRadius => spellRadiuses;

        private readonly string connectionString;

        public DatabaseConnector(string server, string database, string username, string password, int port = 3310)
        {
            connectionString = $"Server={server};Database={database};Port={port};User ID={username};Password={password};";

            spellDurations = new List<SpellDuration> {
                new SpellDuration { Id = 0, Base = 0, PerLevel = 0, Max = 0 }
            };

            spellCastTimes = new List<SpellCastTime> {
                new SpellCastTime { Id = 0, Base = 0, PerLevel = 0, Min = 0 }
            };

            spellRanges = new List<SpellRange> {
                new SpellRange { Id = 0, RangeMin = 0, RangeMax = 0, Flags = 0, Name_enUS = "", ShortName_enUS = "" }
            };

            spellRadiuses = new List<SpellRadius> {
                new SpellRadius { Id = 0, Radius = 0, RadiusPerLevel = 0, RadiusMax = 0 }
            };
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    await LoadSpellDurationAsync();
                    await LoadSpellCastTimeAsync();
                    await LoadSpellRangeAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<SpellListItem>> GetSpellListAsync()
        {
            List<SpellListItem> spellList = new List<SpellListItem>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT entry, name, spellIconId FROM spell_template ORDER BY entry";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                spellList.Add(new SpellListItem
                                {
                                    Id = reader.GetUInt32("entry"),
                                    Name = reader.GetString("name"),
                                    IconId = reader.GetUInt32("spellIconId")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading spell list: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return spellList;
        }

        public async Task<SpellData> GetSpellByIdAsync(uint spellId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT * FROM spell_template WHERE entry = @spellId";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@spellId", spellId);

                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                SpellData spell = new SpellData();

                                // Load basic properties
                                spell.Id = reader.GetUInt32("entry");
                                spell.School = reader.GetUInt32("school");
                                spell.Category = reader.GetUInt32("category");
                                spell.Dispel = reader.GetUInt32("dispel");
                                spell.Mechanic = reader.GetUInt32("mechanic");
                                spell.Attributes = reader.GetUInt32("attributes");
                                spell.AttributesEx = reader.GetUInt32("attributesEx");
                                spell.AttributesEx2 = reader.GetUInt32("attributesEx2");
                                spell.AttributesEx3 = reader.GetUInt32("attributesEx3");
                                spell.AttributesEx4 = reader.GetUInt32("attributesEx4");

                                System.Diagnostics.Debug.WriteLine($"=== DATABASE VALUES FOR SPELL {spell.Id} ===");
                                System.Diagnostics.Debug.WriteLine($"attributes: {spell.Attributes}");
                                System.Diagnostics.Debug.WriteLine($"attributesEx: {spell.AttributesEx}");
                                System.Diagnostics.Debug.WriteLine($"attributesEx2: {spell.AttributesEx2}");
                                System.Diagnostics.Debug.WriteLine($"attributesEx3: {spell.AttributesEx3}");
                                System.Diagnostics.Debug.WriteLine($"attributesEx4: {spell.AttributesEx4}");

                                spell.Stances = reader.GetUInt32("stances");
                                spell.StancesNot = reader.GetUInt32("stancesNot");
                                spell.Targets = reader.GetUInt32("targets");
                                spell.TargetCreatureType = reader.GetUInt32("targetCreatureType");
                                spell.RequiresSpellFocus = reader.GetUInt32("requiresSpellFocus");
                                spell.CasterAuraState = reader.GetUInt32("casterAuraState");
                                spell.TargetAuraState = reader.GetUInt32("targetAuraState");
                                spell.CastingTimeIndex = reader.GetUInt32("castingTimeIndex");
                                spell.RecoveryTime = reader.GetUInt32("recoveryTime");
                                spell.CategoryRecoveryTime = reader.GetUInt32("categoryRecoveryTime");
                                spell.InterruptFlags = reader.GetUInt32("interruptFlags");
                                spell.AuraInterruptFlags = reader.GetUInt32("auraInterruptFlags");
                                spell.ChannelInterruptFlags = reader.GetUInt32("channelInterruptFlags");
                                spell.procFlags = reader.GetUInt32("procFlags");
                                spell.procChance = reader.GetUInt32("procChance");
                                spell.procCharges = reader.GetUInt32("procCharges");
                                spell.maxLevel = reader.GetUInt32("maxLevel");
                                spell.baseLevel = reader.GetUInt32("baseLevel");
                                spell.spellLevel = reader.GetUInt32("spellLevel");
                                spell.DurationIndex = reader.GetUInt32("durationIndex");
                                spell.powerType = reader.GetUInt32("powerType");
                                spell.manaCost = reader.GetUInt32("manaCost");
                                spell.manaCostPerlevel = reader.GetUInt32("manCostPerLevel");
                                spell.manaPerSecond = reader.GetUInt32("manaPerSecond");
                                spell.manaPerSecondPerLevel = reader.GetUInt32("manaPerSecondPerLevel");
                                spell.rangeIndex = reader.GetUInt32("rangeIndex");
                                spell.speed = reader.GetFloat("speed");
                                spell.StackAmount = reader.GetUInt32("stackAmount");

                                // Load totem data
                                spell.Totem[0] = reader.GetUInt32("totem1");
                                spell.Totem[1] = reader.GetUInt32("totem2");

                                // Load reagent data
                                spell.Reagent[0] = reader.GetInt32("reagent1");
                                spell.Reagent[1] = reader.GetInt32("reagent2");
                                spell.Reagent[2] = reader.GetInt32("reagent3");
                                spell.Reagent[3] = reader.GetInt32("reagent4");
                                spell.Reagent[4] = reader.GetInt32("reagent5");
                                spell.Reagent[5] = reader.GetInt32("reagent6");
                                spell.Reagent[6] = reader.GetInt32("reagent7");
                                spell.Reagent[7] = reader.GetInt32("reagent8");

                                spell.ReagentCount[0] = reader.GetUInt32("reagentCount1");
                                spell.ReagentCount[1] = reader.GetUInt32("reagentCount2");
                                spell.ReagentCount[2] = reader.GetUInt32("reagentCount3");
                                spell.ReagentCount[3] = reader.GetUInt32("reagentCount4");
                                spell.ReagentCount[4] = reader.GetUInt32("reagentCount5");
                                spell.ReagentCount[5] = reader.GetUInt32("reagentCount6");
                                spell.ReagentCount[6] = reader.GetUInt32("reagentCount7");
                                spell.ReagentCount[7] = reader.GetUInt32("reagentCount8");

                                // Load equipment requirements
                                spell.EquippedItemClass = reader.GetInt32("equippedItemClass");
                                spell.EquippedItemSubClassMask = reader.GetInt32("equippedItemSubClassMask");
                                spell.EquippedItemInventoryTypeMask = reader.GetInt32("equippedItemInventoryTypeMask");

                                // Load effect data
                                spell.Effect[0] = reader.GetUInt32("effect1");
                                spell.Effect[1] = reader.GetUInt32("effect2");
                                spell.Effect[2] = reader.GetUInt32("effect3");

                                spell.EffectDieSides[0] = reader.GetInt32("effectDieSides1");
                                spell.EffectDieSides[1] = reader.GetInt32("effectDieSides2");
                                spell.EffectDieSides[2] = reader.GetInt32("effectDieSides3");

                                spell.EffectBaseDice[0] = reader.GetUInt32("effectBaseDice1");
                                spell.EffectBaseDice[1] = reader.GetUInt32("effectBaseDice2");
                                spell.EffectBaseDice[2] = reader.GetUInt32("effectBaseDice3");

                                spell.EffectDicePerLevel[0] = reader.GetFloat("effectDicePerLevel1");
                                spell.EffectDicePerLevel[1] = reader.GetFloat("effectDicePerLevel2");
                                spell.EffectDicePerLevel[2] = reader.GetFloat("effectDicePerLevel3");

                                spell.EffectRealPointsPerLevel[0] = reader.GetFloat("effectRealPointsPerLevel1");
                                spell.EffectRealPointsPerLevel[1] = reader.GetFloat("effectRealPointsPerLevel2");
                                spell.EffectRealPointsPerLevel[2] = reader.GetFloat("effectRealPointsPerLevel3");

                                spell.EffectBasePoints[0] = reader.GetInt32("effectBasePoints1");
                                spell.EffectBasePoints[1] = reader.GetInt32("effectBasePoints2");
                                spell.EffectBasePoints[2] = reader.GetInt32("effectBasePoints3");

                                spell.EffectBonusCoefficient[0] = reader.GetFloat("effectBonusCoefficient1");
                                spell.EffectBonusCoefficient[1] = reader.GetFloat("effectBonusCoefficient2");
                                spell.EffectBonusCoefficient[2] = reader.GetFloat("effectBonusCoefficient3");

                                spell.EffectMechanic[0] = reader.GetUInt32("effectMechanic1");
                                spell.EffectMechanic[1] = reader.GetUInt32("effectMechanic2");
                                spell.EffectMechanic[2] = reader.GetUInt32("effectMechanic3");

                                spell.EffectImplicitTargetA[0] = reader.GetUInt32("effectImplicitTargetA1");
                                spell.EffectImplicitTargetA[1] = reader.GetUInt32("effectImplicitTargetA2");
                                spell.EffectImplicitTargetA[2] = reader.GetUInt32("effectImplicitTargetA3");

                                spell.EffectImplicitTargetB[0] = reader.GetUInt32("effectImplicitTargetB1");
                                spell.EffectImplicitTargetB[1] = reader.GetUInt32("effectImplicitTargetB2");
                                spell.EffectImplicitTargetB[2] = reader.GetUInt32("effectImplicitTargetB3");

                                spell.EffectRadiusIndex[0] = reader.GetUInt32("effectRadiusIndex1");
                                spell.EffectRadiusIndex[1] = reader.GetUInt32("effectRadiusIndex2");
                                spell.EffectRadiusIndex[2] = reader.GetUInt32("effectRadiusIndex3");

                                spell.EffectApplyAuraName[0] = reader.GetUInt32("effectApplyAuraName1");
                                spell.EffectApplyAuraName[1] = reader.GetUInt32("effectApplyAuraName2");
                                spell.EffectApplyAuraName[2] = reader.GetUInt32("effectApplyAuraName3");

                                spell.EffectAmplitude[0] = reader.GetUInt32("effectAmplitude1");
                                spell.EffectAmplitude[1] = reader.GetUInt32("effectAmplitude2");
                                spell.EffectAmplitude[2] = reader.GetUInt32("effectAmplitude3");

                                spell.EffectMultipleValue[0] = reader.GetFloat("effectMultipleValue1");
                                spell.EffectMultipleValue[1] = reader.GetFloat("effectMultipleValue2");
                                spell.EffectMultipleValue[2] = reader.GetFloat("effectMultipleValue3");

                                spell.EffectChainTarget[0] = reader.GetUInt32("effectChainTarget1");
                                spell.EffectChainTarget[1] = reader.GetUInt32("effectChainTarget2");
                                spell.EffectChainTarget[2] = reader.GetUInt32("effectChainTarget3");

                                spell.EffectItemType[0] = reader.GetUInt64("effectItemType1");
                                spell.EffectItemType[1] = reader.GetUInt64("effectItemType2");
                                spell.EffectItemType[2] = reader.GetUInt64("effectItemType3");

                                spell.EffectMiscValue[0] = reader.GetInt32("effectMiscValue1");
                                spell.EffectMiscValue[1] = reader.GetInt32("effectMiscValue2");
                                spell.EffectMiscValue[2] = reader.GetInt32("effectMiscValue3");

                                spell.EffectTriggerSpell[0] = reader.GetUInt32("effectTriggerSpell1");
                                spell.EffectTriggerSpell[1] = reader.GetUInt32("effectTriggerSpell2");
                                spell.EffectTriggerSpell[2] = reader.GetUInt32("effectTriggerSpell3");

                                spell.EffectPointsPerComboPoint[0] = reader.GetFloat("effectPointsPerComboPoint1");
                                spell.EffectPointsPerComboPoint[1] = reader.GetFloat("effectPointsPerComboPoint2");
                                spell.EffectPointsPerComboPoint[2] = reader.GetFloat("effectPointsPerComboPoint3");

                                // Load visual data
                                spell.SpellVisual = reader.GetUInt32("spellVisual1");
                                spell.SpellVisual2 = reader.GetUInt32("spellVisual2");
                                spell.SpellIconID = reader.GetUInt32("spellIconId");
                                spell.activeIconID = reader.GetUInt32("activeIconId");
                                spell.spellPriority = reader.GetUInt32("spellPriority");

                                // Load text data
                                spell.SpellName = reader.GetString("name");
                                spell.nameFlags = reader.GetUInt32("nameFlags");
                                spell.nameSubtext = reader.GetString("nameSubtext");
                                spell.nameSubtextFlags = reader.GetUInt32("nameSubtextFlags");
                                spell.description = reader.GetString("description");
                                spell.descriptionFlags = reader.GetUInt32("descriptionFlags");
                                spell.auraDescription = reader.GetString("auraDescription");
                                spell.auraDescriptionFlags = reader.GetUInt32("auraDescriptionFlags");
                                spell.ToolTip[0] = spell.description; // Use description as tooltip

                                // Load misc data
                                spell.ManaCostPercentage = reader.GetUInt32("manaCostPercentage");
                                spell.StartRecoveryCategory = reader.GetUInt32("startRecoveryCategory");
                                spell.StartRecoveryTime = reader.GetUInt32("startRecoveryTime");
                                spell.MinTargetLevel = reader.GetUInt32("minTargetLevel");
                                spell.MaxTargetLevel = reader.GetUInt32("maxTargetLevel");
                                spell.SpellFamilyName = reader.GetUInt32("spellFamilyName");
                                spell.SpellFamilyFlags = reader.GetUInt64("spellFamilyFlags");
                                spell.MaxAffectedTargets = reader.GetUInt32("maxAffectedTargets");
                                spell.DmgClass = reader.GetUInt32("dmgClass");
                                spell.PreventionType = reader.GetUInt32("preventionType");
                                spell.stanceBarOrder = reader.GetInt32("stanceBarOrder");

                                spell.DmgMultiplier[0] = reader.GetFloat("dmgMultiplier1");
                                spell.DmgMultiplier[1] = reader.GetFloat("dmgMultiplier2");
                                spell.DmgMultiplier[2] = reader.GetFloat("dmgMultiplier3");

                                spell.MinFactionId = reader.GetUInt32("minFactionId");
                                spell.MinReputation = reader.GetUInt32("minReputation");
                                spell.RequiredAuraVision = reader.GetUInt32("requiredAuraVision");
                                spell.CustomFlags = reader.GetUInt32("customFlags");
                                spell.ScriptName = reader.GetString("script_name");

                                return spell;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving spell: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return null;
        }

        public async Task<bool> DeleteSpellAsync(uint spellId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (MySqlCommand command = new MySqlCommand("DELETE FROM spell_template WHERE entry = @spellId", connection))
                    {
                        command.Parameters.AddWithValue("@spellId", spellId);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting spell: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> SaveSpellAsync(SpellData spell)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Check if spell exists
                    bool exists = false;
                    using (MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM spell_template WHERE entry = @spellId", connection))
                    {
                        checkCommand.Parameters.AddWithValue("@spellId", spell.Id);
                        exists = Convert.ToInt32(await checkCommand.ExecuteScalarAsync()) > 0;
                    }

                    string query;
                    if (exists)
                    {
                        // UPDATE existing spell
                        query = "UPDATE spell_template SET " +
                                "school = @School, category = @Category, castUI = 0, dispel = @Dispel, mechanic = @Mechanic, " +
                                "attributes = @Attributes, attributesEx = @AttributesEx, attributesEx2 = @AttributesEx2, " +
                                "attributesEx3 = @AttributesEx3, attributesEx4 = @AttributesEx4, stances = @Stances, " +
                                "stancesNot = @StancesNot, targets = @Targets, targetCreatureType = @TargetCreatureType, " +
                                "requiresSpellFocus = @RequiresSpellFocus, casterAuraState = @CasterAuraState, " +
                                "targetAuraState = @TargetAuraState, castingTimeIndex = @CastingTimeIndex, " +
                                "recoveryTime = @RecoveryTime, categoryRecoveryTime = @CategoryRecoveryTime, " +
                                "interruptFlags = @InterruptFlags, auraInterruptFlags = @AuraInterruptFlags, " +
                                "channelInterruptFlags = @ChannelInterruptFlags, procFlags = @procFlags, " +
                                "procChance = @procChance, procCharges = @procCharges, maxLevel = @maxLevel, " +
                                "baseLevel = @baseLevel, spellLevel = @spellLevel, durationIndex = @DurationIndex, " +
                                "powerType = @powerType, manaCost = @manaCost, manCostPerLevel = @manaCostPerlevel, " +
                                "manaPerSecond = @manaPerSecond, manaPerSecondPerLevel = @manaPerSecondPerLevel, " +
                                "rangeIndex = @rangeIndex, speed = @speed, modelNextSpell = 0, stackAmount = @StackAmount, " +
                                "totem1 = @Totem1, totem2 = @Totem2, " +
                                "reagent1 = @Reagent1, reagent2 = @Reagent2, reagent3 = @Reagent3, reagent4 = @Reagent4, " +
                                "reagent5 = @Reagent5, reagent6 = @Reagent6, reagent7 = @Reagent7, reagent8 = @Reagent8, " +
                                "reagentCount1 = @ReagentCount1, reagentCount2 = @ReagentCount2, reagentCount3 = @ReagentCount3, " +
                                "reagentCount4 = @ReagentCount4, reagentCount5 = @ReagentCount5, reagentCount6 = @ReagentCount6, " +
                                "reagentCount7 = @ReagentCount7, reagentCount8 = @ReagentCount8, " +
                                "equippedItemClass = @EquippedItemClass, equippedItemSubClassMask = @EquippedItemSubClassMask, " +
                                "equippedItemInventoryTypeMask = @EquippedItemInventoryTypeMask, " +
                                "effect1 = @Effect1, effect2 = @Effect2, effect3 = @Effect3, " +
                                "effectDieSides1 = @EffectDieSides1, effectDieSides2 = @EffectDieSides2, effectDieSides3 = @EffectDieSides3, " +
                                "effectBaseDice1 = @EffectBaseDice1, effectBaseDice2 = @EffectBaseDice2, effectBaseDice3 = @EffectBaseDice3, " +
                                "effectDicePerLevel1 = @EffectDicePerLevel1, effectDicePerLevel2 = @EffectDicePerLevel2, " +
                                "effectDicePerLevel3 = @EffectDicePerLevel3, " +
                                "effectRealPointsPerLevel1 = @EffectRealPointsPerLevel1, effectRealPointsPerLevel2 = @EffectRealPointsPerLevel2, " +
                                "effectRealPointsPerLevel3 = @EffectRealPointsPerLevel3, " +
                                "effectBasePoints1 = @EffectBasePoints1, effectBasePoints2 = @EffectBasePoints2, " +
                                "effectBasePoints3 = @EffectBasePoints3, " +
                                "effectBonusCoefficient1 = @EffectBonusCoefficient1, effectBonusCoefficient2 = @EffectBonusCoefficient2, " +
                                "effectBonusCoefficient3 = @EffectBonusCoefficient3, " +
                                "effectMechanic1 = @EffectMechanic1, effectMechanic2 = @EffectMechanic2, effectMechanic3 = @EffectMechanic3, " +
                                "effectImplicitTargetA1 = @EffectImplicitTargetA1, effectImplicitTargetA2 = @EffectImplicitTargetA2, " +
                                "effectImplicitTargetA3 = @EffectImplicitTargetA3, " +
                                "effectImplicitTargetB1 = @EffectImplicitTargetB1, effectImplicitTargetB2 = @EffectImplicitTargetB2, " +
                                "effectImplicitTargetB3 = @EffectImplicitTargetB3, " +
                                "effectRadiusIndex1 = @EffectRadiusIndex1, effectRadiusIndex2 = @EffectRadiusIndex2, " +
                                "effectRadiusIndex3 = @EffectRadiusIndex3, " +
                                "effectApplyAuraName1 = @EffectApplyAuraName1, effectApplyAuraName2 = @EffectApplyAuraName2, " +
                                "effectApplyAuraName3 = @EffectApplyAuraName3, " +
                                "effectAmplitude1 = @EffectAmplitude1, effectAmplitude2 = @EffectAmplitude2, " +
                                "effectAmplitude3 = @EffectAmplitude3, " +
                                "effectMultipleValue1 = @EffectMultipleValue1, effectMultipleValue2 = @EffectMultipleValue2, " +
                                "effectMultipleValue3 = @EffectMultipleValue3, " +
                                "effectChainTarget1 = @EffectChainTarget1, effectChainTarget2 = @EffectChainTarget2, " +
                                "effectChainTarget3 = @EffectChainTarget3, " +
                                "effectItemType1 = @EffectItemType1, effectItemType2 = @EffectItemType2, effectItemType3 = @EffectItemType3, " +
                                "effectMiscValue1 = @EffectMiscValue1, effectMiscValue2 = @EffectMiscValue2, " +
                                "effectMiscValue3 = @EffectMiscValue3, " +
                                "effectTriggerSpell1 = @EffectTriggerSpell1, effectTriggerSpell2 = @EffectTriggerSpell2, " +
                                "effectTriggerSpell3 = @EffectTriggerSpell3, " +
                                "effectPointsPerComboPoint1 = @EffectPointsPerComboPoint1, " +
                                "effectPointsPerComboPoint2 = @EffectPointsPerComboPoint2, " +
                                "effectPointsPerComboPoint3 = @EffectPointsPerComboPoint3, " +
                                "spellVisual1 = @SpellVisual, spellVisual2 = @SpellVisual2, spellIconId = @SpellIconID, " +
                                "activeIconId = @activeIconID, spellPriority = @spellPriority, name = @SpellName, " +
                                "nameFlags = @nameFlags, nameSubtext = @nameSubtext, nameSubtextFlags = @nameSubtextFlags, " +
                                "description = @description, descriptionFlags = @descriptionFlags, " +
                                "auraDescription = @auraDescription, auraDescriptionFlags = @auraDescriptionFlags, " +
                                "manaCostPercentage = @ManaCostPercentage, startRecoveryCategory = @StartRecoveryCategory, " +
                                "startRecoveryTime = @StartRecoveryTime, minTargetLevel = @MinTargetLevel, " +
                                "maxTargetLevel = @MaxTargetLevel, spellFamilyName = @SpellFamilyName, " +
                                "spellFamilyFlags = @SpellFamilyFlags, maxAffectedTargets = @MaxAffectedTargets, " +
                                "dmgClass = @DmgClass, preventionType = @PreventionType, stanceBarOrder = @stanceBarOrder, " +
                                "dmgMultiplier1 = @DmgMultiplier1, dmgMultiplier2 = @DmgMultiplier2, dmgMultiplier3 = @DmgMultiplier3, " +
                                "minFactionId = @MinFactionId, minReputation = @MinReputation, " +
                                "requiredAuraVision = @RequiredAuraVision, customFlags = @CustomFlags, script_name = @ScriptName " +
                                "WHERE entry = @entry";
                    }
                    else
                    {
                        // INSERT new spell
                        query = "INSERT INTO spell_template (" +
                                "entry, school, category, castUI, dispel, mechanic, attributes, attributesEx, attributesEx2, " +
                                "attributesEx3, attributesEx4, stances, stancesNot, targets, targetCreatureType, requiresSpellFocus, " +
                                "casterAuraState, targetAuraState, castingTimeIndex, recoveryTime, categoryRecoveryTime, " +
                                "interruptFlags, auraInterruptFlags, channelInterruptFlags, procFlags, procChance, procCharges, " +
                                "maxLevel, baseLevel, spellLevel, durationIndex, powerType, manaCost, manCostPerLevel, " +
                                "manaPerSecond, manaPerSecondPerLevel, rangeIndex, speed, modelNextSpell, stackAmount, " +
                                "totem1, totem2, reagent1, reagent2, reagent3, reagent4, reagent5, reagent6, reagent7, reagent8, " +
                                "reagentCount1, reagentCount2, reagentCount3, reagentCount4, reagentCount5, reagentCount6, " +
                                "reagentCount7, reagentCount8, equippedItemClass, equippedItemSubClassMask, " +
                                "equippedItemInventoryTypeMask, effect1, effect2, effect3, effectDieSides1, effectDieSides2, " +
                                "effectDieSides3, effectBaseDice1, effectBaseDice2, effectBaseDice3, effectDicePerLevel1, " +
                                "effectDicePerLevel2, effectDicePerLevel3, effectRealPointsPerLevel1, effectRealPointsPerLevel2, " +
                                "effectRealPointsPerLevel3, effectBasePoints1, effectBasePoints2, effectBasePoints3, " +
                                "effectBonusCoefficient1, effectBonusCoefficient2, effectBonusCoefficient3, effectMechanic1, " +
                                "effectMechanic2, effectMechanic3, effectImplicitTargetA1, effectImplicitTargetA2, " +
                                "effectImplicitTargetA3, effectImplicitTargetB1, effectImplicitTargetB2, effectImplicitTargetB3, " +
                                "effectRadiusIndex1, effectRadiusIndex2, effectRadiusIndex3, effectApplyAuraName1, " +
                                "effectApplyAuraName2, effectApplyAuraName3, effectAmplitude1, effectAmplitude2, effectAmplitude3, " +
                                "effectMultipleValue1, effectMultipleValue2, effectMultipleValue3, effectChainTarget1, " +
                                "effectChainTarget2, effectChainTarget3, effectItemType1, effectItemType2, effectItemType3, " +
                                "effectMiscValue1, effectMiscValue2, effectMiscValue3, effectTriggerSpell1, effectTriggerSpell2, " +
                                "effectTriggerSpell3, effectPointsPerComboPoint1, effectPointsPerComboPoint2, effectPointsPerComboPoint3, " +
                                "spellVisual1, spellVisual2, spellIconId, activeIconId, spellPriority, name, nameFlags, " +
                                "nameSubtext, nameSubtextFlags, description, descriptionFlags, auraDescription, auraDescriptionFlags, " +
                                "manaCostPercentage, startRecoveryCategory, startRecoveryTime, minTargetLevel, maxTargetLevel, " +
                                "spellFamilyName, spellFamilyFlags, maxAffectedTargets, dmgClass, preventionType, stanceBarOrder, " +
                                "dmgMultiplier1, dmgMultiplier2, dmgMultiplier3, minFactionId, minReputation, requiredAuraVision, customFlags, script_name) " +
                                "VALUES (" +
                                "@entry, @School, @Category, 0, @Dispel, @Mechanic, @Attributes, @AttributesEx, @AttributesEx2, " +
                                "@AttributesEx3, @AttributesEx4, @Stances, @StancesNot, @Targets, @TargetCreatureType, " +
                                "@RequiresSpellFocus, @CasterAuraState, @TargetAuraState, @CastingTimeIndex, @RecoveryTime, " +
                                "@CategoryRecoveryTime, @InterruptFlags, @AuraInterruptFlags, @ChannelInterruptFlags, @procFlags, " +
                                "@procChance, @procCharges, @maxLevel, @baseLevel, @spellLevel, @DurationIndex, @powerType, " +
                                "@manaCost, @manaCostPerlevel, @manaPerSecond, @manaPerSecondPerLevel, @rangeIndex, @speed, 0, " +
                                "@StackAmount, @Totem1, @Totem2, @Reagent1, @Reagent2, @Reagent3, @Reagent4, @Reagent5, @Reagent6, " +
                                "@Reagent7, @Reagent8, @ReagentCount1, @ReagentCount2, @ReagentCount3, @ReagentCount4, " +
                                "@ReagentCount5, @ReagentCount6, @ReagentCount7, @ReagentCount8, @EquippedItemClass, " +
                                "@EquippedItemSubClassMask, @EquippedItemInventoryTypeMask, @Effect1, @Effect2, @Effect3, " +
                                "@EffectDieSides1, @EffectDieSides2, @EffectDieSides3, @EffectBaseDice1, @EffectBaseDice2, " +
                                "@EffectBaseDice3, @EffectDicePerLevel1, @EffectDicePerLevel2, @EffectDicePerLevel3, " +
                                "@EffectRealPointsPerLevel1, @EffectRealPointsPerLevel2, @EffectRealPointsPerLevel3, " +
                                "@EffectBasePoints1, @EffectBasePoints2, @EffectBasePoints3, @EffectBonusCoefficient1, " +
                                "@EffectBonusCoefficient2, @EffectBonusCoefficient3, @EffectMechanic1, @EffectMechanic2, " +
                                "@EffectMechanic3, @EffectImplicitTargetA1, @EffectImplicitTargetA2, @EffectImplicitTargetA3, " +
                                "@EffectImplicitTargetB1, @EffectImplicitTargetB2, @EffectImplicitTargetB3, @EffectRadiusIndex1, " +
                                "@EffectRadiusIndex2, @EffectRadiusIndex3, @EffectApplyAuraName1, @EffectApplyAuraName2, " +
                                "@EffectApplyAuraName3, @EffectAmplitude1, @EffectAmplitude2, @EffectAmplitude3, " +
                                "@EffectMultipleValue1, @EffectMultipleValue2, @EffectMultipleValue3, @EffectChainTarget1, " +
                                "@EffectChainTarget2, @EffectChainTarget3, @EffectItemType1, @EffectItemType2, @EffectItemType3, " +
                                "@EffectMiscValue1, @EffectMiscValue2, @EffectMiscValue3, @EffectTriggerSpell1, @EffectTriggerSpell2, " +
                                "@EffectTriggerSpell3, @EffectPointsPerComboPoint1, @EffectPointsPerComboPoint2, " +
                                "@EffectPointsPerComboPoint3, @SpellVisual, @SpellVisual2, @SpellIconID, @activeIconID, " +
                                "@spellPriority, @SpellName, @nameFlags, @nameSubtext, @nameSubtextFlags, @description, " +
                                "@descriptionFlags, @auraDescription, @auraDescriptionFlags, @ManaCostPercentage, " +
                                "@StartRecoveryCategory, @StartRecoveryTime, @MinTargetLevel, @MaxTargetLevel, @SpellFamilyName, " +
                                "@SpellFamilyFlags, @MaxAffectedTargets, @DmgClass, @PreventionType, @stanceBarOrder, " +
                                "@DmgMultiplier1, @DmgMultiplier2, @DmgMultiplier3, @MinFactionId, @MinReputation, " +
                                "@RequiredAuraVision, @CustomFlags, @ScriptName)";
                    }

                    // Create the command with parameters
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add all parameters
                        command.Parameters.AddWithValue("@entry", spell.Id);
                        command.Parameters.AddWithValue("@School", spell.School);
                        command.Parameters.AddWithValue("@Category", spell.Category);
                        command.Parameters.AddWithValue("@Dispel", spell.Dispel);
                        command.Parameters.AddWithValue("@Mechanic", spell.Mechanic);
                        command.Parameters.AddWithValue("@Attributes", spell.Attributes);
                        command.Parameters.AddWithValue("@AttributesEx", spell.AttributesEx);
                        command.Parameters.AddWithValue("@AttributesEx2", spell.AttributesEx2);
                        command.Parameters.AddWithValue("@AttributesEx3", spell.AttributesEx3);
                        command.Parameters.AddWithValue("@AttributesEx4", spell.AttributesEx4);
                        command.Parameters.AddWithValue("@Stances", spell.Stances);
                        command.Parameters.AddWithValue("@StancesNot", spell.StancesNot);
                        command.Parameters.AddWithValue("@Targets", spell.Targets);
                        command.Parameters.AddWithValue("@TargetCreatureType", spell.TargetCreatureType);
                        command.Parameters.AddWithValue("@RequiresSpellFocus", spell.RequiresSpellFocus);
                        command.Parameters.AddWithValue("@CasterAuraState", spell.CasterAuraState);
                        command.Parameters.AddWithValue("@TargetAuraState", spell.TargetAuraState);
                        command.Parameters.AddWithValue("@CastingTimeIndex", spell.CastingTimeIndex);
                        command.Parameters.AddWithValue("@RecoveryTime", spell.RecoveryTime);
                        command.Parameters.AddWithValue("@CategoryRecoveryTime", spell.CategoryRecoveryTime);
                        command.Parameters.AddWithValue("@InterruptFlags", spell.InterruptFlags);
                        command.Parameters.AddWithValue("@AuraInterruptFlags", spell.AuraInterruptFlags);
                        command.Parameters.AddWithValue("@ChannelInterruptFlags", spell.ChannelInterruptFlags);
                        command.Parameters.AddWithValue("@procFlags", spell.procFlags);
                        command.Parameters.AddWithValue("@procChance", spell.procChance);
                        command.Parameters.AddWithValue("@procCharges", spell.procCharges);
                        command.Parameters.AddWithValue("@maxLevel", spell.maxLevel);
                        command.Parameters.AddWithValue("@baseLevel", spell.baseLevel);
                        command.Parameters.AddWithValue("@spellLevel", spell.spellLevel);
                        command.Parameters.AddWithValue("@DurationIndex", spell.DurationIndex);
                        command.Parameters.AddWithValue("@powerType", spell.powerType);
                        command.Parameters.AddWithValue("@manaCost", spell.manaCost);
                        command.Parameters.AddWithValue("@manaCostPerlevel", spell.manaCostPerlevel);
                        command.Parameters.AddWithValue("@manaPerSecond", spell.manaPerSecond);
                        command.Parameters.AddWithValue("@manaPerSecondPerLevel", spell.manaPerSecondPerLevel);
                        command.Parameters.AddWithValue("@rangeIndex", spell.rangeIndex);
                        command.Parameters.AddWithValue("@speed", spell.speed);
                        command.Parameters.AddWithValue("@StackAmount", spell.StackAmount);

                        // Totem parameters
                        command.Parameters.AddWithValue("@Totem1", spell.Totem[0]);
                        command.Parameters.AddWithValue("@Totem2", spell.Totem[1]);

                        // Reagent parameters
                        command.Parameters.AddWithValue("@Reagent1", spell.Reagent[0]);
                        command.Parameters.AddWithValue("@Reagent2", spell.Reagent[1]);
                        command.Parameters.AddWithValue("@Reagent3", spell.Reagent[2]);
                        command.Parameters.AddWithValue("@Reagent4", spell.Reagent[3]);
                        command.Parameters.AddWithValue("@Reagent5", spell.Reagent[4]);
                        command.Parameters.AddWithValue("@Reagent6", spell.Reagent[5]);
                        command.Parameters.AddWithValue("@Reagent7", spell.Reagent[6]);
                        command.Parameters.AddWithValue("@Reagent8", spell.Reagent[7]);

                        command.Parameters.AddWithValue("@ReagentCount1", spell.ReagentCount[0]);
                        command.Parameters.AddWithValue("@ReagentCount2", spell.ReagentCount[1]);
                        command.Parameters.AddWithValue("@ReagentCount3", spell.ReagentCount[2]);
                        command.Parameters.AddWithValue("@ReagentCount4", spell.ReagentCount[3]);
                        command.Parameters.AddWithValue("@ReagentCount5", spell.ReagentCount[4]);
                        command.Parameters.AddWithValue("@ReagentCount6", spell.ReagentCount[5]);
                        command.Parameters.AddWithValue("@ReagentCount7", spell.ReagentCount[6]);
                        command.Parameters.AddWithValue("@ReagentCount8", spell.ReagentCount[7]);

                        // Equipment parameters
                        command.Parameters.AddWithValue("@EquippedItemClass", spell.EquippedItemClass);
                        command.Parameters.AddWithValue("@EquippedItemSubClassMask", spell.EquippedItemSubClassMask);
                        command.Parameters.AddWithValue("@EquippedItemInventoryTypeMask", spell.EquippedItemInventoryTypeMask);

                        // Effect parameters
                        command.Parameters.AddWithValue("@Effect1", spell.Effect[0]);
                        command.Parameters.AddWithValue("@Effect2", spell.Effect[1]);
                        command.Parameters.AddWithValue("@Effect3", spell.Effect[2]);

                        command.Parameters.AddWithValue("@EffectDieSides1", spell.EffectDieSides[0]);
                        command.Parameters.AddWithValue("@EffectDieSides2", spell.EffectDieSides[1]);
                        command.Parameters.AddWithValue("@EffectDieSides3", spell.EffectDieSides[2]);

                        command.Parameters.AddWithValue("@EffectBaseDice1", spell.EffectBaseDice[0]);
                        command.Parameters.AddWithValue("@EffectBaseDice2", spell.EffectBaseDice[1]);
                        command.Parameters.AddWithValue("@EffectBaseDice3", spell.EffectBaseDice[2]);

                        command.Parameters.AddWithValue("@EffectDicePerLevel1", spell.EffectDicePerLevel[0]);
                        command.Parameters.AddWithValue("@EffectDicePerLevel2", spell.EffectDicePerLevel[1]);
                        command.Parameters.AddWithValue("@EffectDicePerLevel3", spell.EffectDicePerLevel[2]);

                        command.Parameters.AddWithValue("@EffectRealPointsPerLevel1", spell.EffectRealPointsPerLevel[0]);
                        command.Parameters.AddWithValue("@EffectRealPointsPerLevel2", spell.EffectRealPointsPerLevel[1]);
                        command.Parameters.AddWithValue("@EffectRealPointsPerLevel3", spell.EffectRealPointsPerLevel[2]);

                        command.Parameters.AddWithValue("@EffectBasePoints1", spell.EffectBasePoints[0]);
                        command.Parameters.AddWithValue("@EffectBasePoints2", spell.EffectBasePoints[1]);
                        command.Parameters.AddWithValue("@EffectBasePoints3", spell.EffectBasePoints[2]);

                        command.Parameters.AddWithValue("@EffectBonusCoefficient1", spell.EffectBonusCoefficient[0]);
                        command.Parameters.AddWithValue("@EffectBonusCoefficient2", spell.EffectBonusCoefficient[1]);
                        command.Parameters.AddWithValue("@EffectBonusCoefficient3", spell.EffectBonusCoefficient[2]);

                        command.Parameters.AddWithValue("@EffectMechanic1", spell.EffectMechanic[0]);
                        command.Parameters.AddWithValue("@EffectMechanic2", spell.EffectMechanic[1]);
                        command.Parameters.AddWithValue("@EffectMechanic3", spell.EffectMechanic[2]);

                        command.Parameters.AddWithValue("@EffectImplicitTargetA1", spell.EffectImplicitTargetA[0]);
                        command.Parameters.AddWithValue("@EffectImplicitTargetA2", spell.EffectImplicitTargetA[1]);
                        command.Parameters.AddWithValue("@EffectImplicitTargetA3", spell.EffectImplicitTargetA[2]);

                        command.Parameters.AddWithValue("@EffectImplicitTargetB1", spell.EffectImplicitTargetB[0]);
                        command.Parameters.AddWithValue("@EffectImplicitTargetB2", spell.EffectImplicitTargetB[1]);
                        command.Parameters.AddWithValue("@EffectImplicitTargetB3", spell.EffectImplicitTargetB[2]);

                        command.Parameters.AddWithValue("@EffectRadiusIndex1", spell.EffectRadiusIndex[0]);
                        command.Parameters.AddWithValue("@EffectRadiusIndex2", spell.EffectRadiusIndex[1]);
                        command.Parameters.AddWithValue("@EffectRadiusIndex3", spell.EffectRadiusIndex[2]);

                        command.Parameters.AddWithValue("@EffectApplyAuraName1", spell.EffectApplyAuraName[0]);
                        command.Parameters.AddWithValue("@EffectApplyAuraName2", spell.EffectApplyAuraName[1]);
                        command.Parameters.AddWithValue("@EffectApplyAuraName3", spell.EffectApplyAuraName[2]);

                        command.Parameters.AddWithValue("@EffectAmplitude1", spell.EffectAmplitude[0]);
                        command.Parameters.AddWithValue("@EffectAmplitude2", spell.EffectAmplitude[1]);
                        command.Parameters.AddWithValue("@EffectAmplitude3", spell.EffectAmplitude[2]);

                        command.Parameters.AddWithValue("@EffectMultipleValue1", spell.EffectMultipleValue[0]);
                        command.Parameters.AddWithValue("@EffectMultipleValue2", spell.EffectMultipleValue[1]);
                        command.Parameters.AddWithValue("@EffectMultipleValue3", spell.EffectMultipleValue[2]);

                        command.Parameters.AddWithValue("@EffectChainTarget1", spell.EffectChainTarget[0]);
                        command.Parameters.AddWithValue("@EffectChainTarget2", spell.EffectChainTarget[1]);
                        command.Parameters.AddWithValue("@EffectChainTarget3", spell.EffectChainTarget[2]);

                        command.Parameters.AddWithValue("@EffectItemType1", spell.EffectItemType[0]);
                        command.Parameters.AddWithValue("@EffectItemType2", spell.EffectItemType[1]);
                        command.Parameters.AddWithValue("@EffectItemType3", spell.EffectItemType[2]);

                        command.Parameters.AddWithValue("@EffectMiscValue1", spell.EffectMiscValue[0]);
                        command.Parameters.AddWithValue("@EffectMiscValue2", spell.EffectMiscValue[1]);
                        command.Parameters.AddWithValue("@EffectMiscValue3", spell.EffectMiscValue[2]);

                        command.Parameters.AddWithValue("@EffectTriggerSpell1", spell.EffectTriggerSpell[0]);
                        command.Parameters.AddWithValue("@EffectTriggerSpell2", spell.EffectTriggerSpell[1]);
                        command.Parameters.AddWithValue("@EffectTriggerSpell3", spell.EffectTriggerSpell[2]);

                        command.Parameters.AddWithValue("@EffectPointsPerComboPoint1", spell.EffectPointsPerComboPoint[0]);
                        command.Parameters.AddWithValue("@EffectPointsPerComboPoint2", spell.EffectPointsPerComboPoint[1]);
                        command.Parameters.AddWithValue("@EffectPointsPerComboPoint3", spell.EffectPointsPerComboPoint[2]);

                        // Visual and text data
                        command.Parameters.AddWithValue("@SpellVisual", spell.SpellVisual);
                        command.Parameters.AddWithValue("@SpellVisual2", spell.SpellVisual2);
                        command.Parameters.AddWithValue("@SpellIconID", spell.SpellIconID);
                        command.Parameters.AddWithValue("@activeIconID", spell.activeIconID);
                        command.Parameters.AddWithValue("@spellPriority", spell.spellPriority);

                        command.Parameters.AddWithValue("@SpellName", spell.SpellName);
                        command.Parameters.AddWithValue("@nameFlags", spell.nameFlags);
                        command.Parameters.AddWithValue("@nameSubtext", spell.nameSubtext);
                        command.Parameters.AddWithValue("@nameSubtextFlags", spell.nameSubtextFlags);
                        command.Parameters.AddWithValue("@description", spell.description);
                        command.Parameters.AddWithValue("@descriptionFlags", spell.descriptionFlags);
                        command.Parameters.AddWithValue("@auraDescription", spell.auraDescription);
                        command.Parameters.AddWithValue("@auraDescriptionFlags", spell.auraDescriptionFlags);

                        // Misc data
                        command.Parameters.AddWithValue("@ManaCostPercentage", spell.ManaCostPercentage);
                        command.Parameters.AddWithValue("@StartRecoveryCategory", spell.StartRecoveryCategory);
                        command.Parameters.AddWithValue("@StartRecoveryTime", spell.StartRecoveryTime);
                        command.Parameters.AddWithValue("@MinTargetLevel", spell.MinTargetLevel);
                        command.Parameters.AddWithValue("@MaxTargetLevel", spell.MaxTargetLevel);
                        command.Parameters.AddWithValue("@SpellFamilyName", spell.SpellFamilyName);
                        command.Parameters.AddWithValue("@SpellFamilyFlags", spell.SpellFamilyFlags);
                        command.Parameters.AddWithValue("@MaxAffectedTargets", spell.MaxAffectedTargets);
                        command.Parameters.AddWithValue("@DmgClass", spell.DmgClass);
                        command.Parameters.AddWithValue("@PreventionType", spell.PreventionType);
                        command.Parameters.AddWithValue("@stanceBarOrder", spell.stanceBarOrder);

                        command.Parameters.AddWithValue("@DmgMultiplier1", spell.DmgMultiplier[0]);
                        command.Parameters.AddWithValue("@DmgMultiplier2", spell.DmgMultiplier[1]);
                        command.Parameters.AddWithValue("@DmgMultiplier3", spell.DmgMultiplier[2]);

                        command.Parameters.AddWithValue("@MinFactionId", spell.MinFactionId);
                        command.Parameters.AddWithValue("@MinReputation", spell.MinReputation);
                        command.Parameters.AddWithValue("@RequiredAuraVision", spell.RequiredAuraVision);
                        command.Parameters.AddWithValue("@CustomFlags", spell.CustomFlags);
                        command.Parameters.AddWithValue("@ScriptName", spell.CustomFlags);

                        // Execute the query
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving spell: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task LoadSpellDurationAsync()
        {
            // Start with a default "None" entry
            spellDurations = new List<SpellDuration> {
                    new SpellDuration { Id = 0, Base = 0, PerLevel = 0, Max = 0 }
                };

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // let's load the durations
                    string query = "SELECT id, base, perLevel, max FROM dbc_spell_duration ORDER BY id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                spellDurations.Add(new SpellDuration
                                {
                                    Id = reader.GetInt32("id"),
                                    Base = reader.GetInt32("base"),
                                    PerLevel = reader.GetInt32("perLevel"),
                                    Max = reader.GetInt32("max")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading spell durations: {ex.Message}\n", "Database Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async Task LoadSpellCastTimeAsync()
        {
            spellCastTimes = new List<SpellCastTime> {
                    new SpellCastTime { Id = 0, Base = 0, PerLevel = 0, Min = 0 }
                };

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // let's load the durations
                    string query = "SELECT id, base, perLevel, minimum FROM dbc_spell_cast_times ORDER BY id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                spellCastTimes.Add(new SpellCastTime
                                {
                                    Id = reader.GetInt32("id"),
                                    Base = reader.GetInt32("base"),
                                    PerLevel = reader.GetInt32("perLevel"),
                                    Min = reader.GetInt32("minimum")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading spell cast times: {ex.Message}\n", "Database Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async Task LoadSpellRangeAsync()
        {
            spellRanges = new List<SpellRange> {
                new SpellRange { Id = 0, RangeMin = 0, RangeMax = 0, Flags = 0, Name_enUS = "", ShortName_enUS = "" }
            };

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT id, rangeMin, rangeMax, flags, 
                                   name_enUS, name_koKR, name_frFR, name_deDE, 
                                   name_zhCN, name_zhTW, name_esES, name_ptPT, nameMask,
                                   shortName_enUS, shortName_koKR, shortName_frFR, shortName_deDE,
                                   shortName_zhCN, shortName_zhTW, shortName_esES, shortName_ptPT, shortNameMask
                            FROM dbc_spell_range 
                            ORDER BY id";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                spellRanges.Add(new SpellRange
                                {
                                    Id = reader.GetInt32("id"),
                                    RangeMin = reader.IsDBNull("rangeMin") ? 0 : reader.GetFloat("rangeMin"),
                                    RangeMax = reader.IsDBNull("rangeMax") ? 50000 : reader.GetFloat("rangeMax"),
                                    Flags = reader.IsDBNull("flags") ? 0 : reader.GetInt32("flags"),

                                    // Localized names
                                    Name_enUS = reader.IsDBNull("name_enUS") ? "" : reader.GetString("name_enUS"),
                                    Name_koKR = reader.IsDBNull("name_koKR") ? "" : reader.GetString("name_koKR"),
                                    Name_frFR = reader.IsDBNull("name_frFR") ? "" : reader.GetString("name_frFR"),
                                    Name_deDE = reader.IsDBNull("name_deDE") ? "" : reader.GetString("name_deDE"),
                                    Name_zhCN = reader.IsDBNull("name_zhCN") ? "" : reader.GetString("name_zhCN"),
                                    Name_zhTW = reader.IsDBNull("name_zhTW") ? "" : reader.GetString("name_zhTW"),
                                    Name_esES = reader.IsDBNull("name_esES") ? "" : reader.GetString("name_esES"),
                                    Name_ptPT = reader.IsDBNull("name_ptPT") ? "" : reader.GetString("name_ptPT"),
                                    NameMask = reader.IsDBNull("nameMask") ? 0 : reader.GetInt32("nameMask"),

                                    // Localized short names
                                    ShortName_enUS = reader.IsDBNull("shortName_enUS") ? "" : reader.GetString("shortName_enUS"),
                                    ShortName_koKR = reader.IsDBNull("shortName_koKR") ? "" : reader.GetString("shortName_koKR"),
                                    ShortName_frFR = reader.IsDBNull("shortName_frFR") ? "" : reader.GetString("shortName_frFR"),
                                    ShortName_deDE = reader.IsDBNull("shortName_deDE") ? "" : reader.GetString("shortName_deDE"),
                                    ShortName_zhCN = reader.IsDBNull("shortName_zhCN") ? "" : reader.GetString("shortName_zhCN"),
                                    ShortName_zhTW = reader.IsDBNull("shortName_zhTW") ? "" : reader.GetString("shortName_zhTW"),
                                    ShortName_esES = reader.IsDBNull("shortName_esES") ? "" : reader.GetString("shortName_esES"),
                                    ShortName_ptPT = reader.IsDBNull("shortName_ptPT") ? "" : reader.GetString("shortName_ptPT"),
                                    ShortNameMask = reader.IsDBNull("shortNameMask") ? 0 : reader.GetInt32("shortNameMask")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading spell ranges: {ex.Message}\n", "Database Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async Task LoadSpellRadiusAsync()
        {
            spellRadiuses = new List<SpellRadius> {
                new SpellRadius { Id = 0, Radius = 0, RadiusPerLevel = 0, RadiusMax = 0 }
            };

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT id, radius, radiusPerLevel, radiusMax
                            FROM dbc_spell_radius 
                            ORDER BY id";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                spellRadiuses.Add(new SpellRadius
                                {
                                    Id = reader.GetInt32("id"),
                                    Radius = reader.IsDBNull("radius") ? null : reader.GetFloat("radius"),
                                    RadiusPerLevel = reader.IsDBNull("radiusPerLevel") ? null : reader.GetFloat("radiusPerLevel"),
                                    RadiusMax = reader.IsDBNull("radiusMax") ? null : reader.GetFloat("radiusMax")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading spell radius: {ex.Message}\n", "Database Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }

    // Helper class for spell list
    public class SpellListItem
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint IconId { get; set; }
    }

    public class SpellDuration
    {
        public int Id { get; set; }
        public int Base { get; set; }
        public int PerLevel { get; set; }
        public int Max { get; set; }

        public override string ToString()
        {
            return $"{Id} - {FormatDurationTime(Base)}";
        }

        private string FormatDurationTime(int milliseconds)
        {
            if (milliseconds == 0) return "None";
            if (milliseconds < 1000) return $"{milliseconds}ms";
            if (milliseconds < 60000) return $"{milliseconds / 1000}s";
            if (milliseconds < 3600000) return $"{milliseconds / 60000}m";
            return $"{milliseconds / 3600000}h";
        }
    }

    public class SpellCastTime
    {
        public int Id { get; set; }
        public int Base { get; set; }
        public int PerLevel { get; set; }
        public int Min { get; set; }

        public override string ToString()
        {
            return $"{Id} - {FormatDurationTime(Base)}";
        }

        private string FormatDurationTime(int milliseconds)
        {
            if (milliseconds == 0) return "None";
            if (milliseconds < 1000) return $"{milliseconds}ms";
            if (milliseconds < 60000) return $"{milliseconds / 1000}s";
            if (milliseconds < 3600000) return $"{milliseconds / 60000}m";
            return $"{milliseconds / 3600000}h";
        }
    }

    public class SpellRange
    {
        public int Id { get; set; }
        public float? RangeMin { get; set; }
        public float? RangeMax { get; set; }
        public int? Flags { get; set; }

        // Localized names
        public string Name_enUS { get; set; }
        public string Name_koKR { get; set; }
        public string Name_frFR { get; set; }
        public string Name_deDE { get; set; }
        public string Name_zhCN { get; set; }
        public string Name_zhTW { get; set; }
        public string Name_esES { get; set; }
        public string Name_ptPT { get; set; }
        public int? NameMask { get; set; }

        // Localized short names
        public string ShortName_enUS { get; set; }
        public string ShortName_koKR { get; set; }
        public string ShortName_frFR { get; set; }
        public string ShortName_deDE { get; set; }
        public string ShortName_zhCN { get; set; }
        public string ShortName_zhTW { get; set; }
        public string ShortName_esES { get; set; }
        public string ShortName_ptPT { get; set; }
        public int? ShortNameMask { get; set; }

        public override string ToString()
        {
            return $"{Id} - {FormatRange()}";
        }

        private string FormatRange()
        {
            if (!RangeMin.HasValue && !RangeMax.HasValue) return "Unknown";
            if (RangeMin == 0 && RangeMax == 0) return "Self";
            if (RangeMin == RangeMax) return $"{RangeMax} yards";

            string minStr = RangeMin?.ToString() ?? "0";
            string maxStr = RangeMax?.ToString() ?? "∞";

            return $"{minStr}-{maxStr} yards - {Name_enUS}";
        }

        // Helper method to get localized name based on locale
        public string GetLocalizedName(string locale = "enUS")
        {
            return locale switch
            {
                "enUS" => Name_enUS,
                "koKR" => Name_koKR,
                "frFR" => Name_frFR,
                "deDE" => Name_deDE,
                "zhCN" => Name_zhCN,
                "zhTW" => Name_zhTW,
                "esES" => Name_esES,
                "ptPT" => Name_ptPT,
                _ => Name_enUS // Default to English
            };
        }

        // Helper method to get localized short name based on locale
        public string GetLocalizedShortName(string locale = "enUS")
        {
            return locale switch
            {
                "enUS" => ShortName_enUS,
                "koKR" => ShortName_koKR,
                "frFR" => ShortName_frFR,
                "deDE" => ShortName_deDE,
                "zhCN" => ShortName_zhCN,
                "zhTW" => ShortName_zhTW,
                "esES" => ShortName_esES,
                "ptPT" => ShortName_ptPT,
                _ => ShortName_enUS // Default to English
            };
        }
    }

    public class SpellRadius
    {
        public int Id { get; set; }
        public float? Radius { get; set; }
        public float? RadiusPerLevel { get; set; }
        public float? RadiusMax { get; set; }

        public override string ToString()
        {
            return $"{Id} - {FormatRadius()}";
        }

        private string FormatRadius()
        {
            if (!Radius.HasValue || Radius == 0) return "No radius";

            float baseRadius = Radius.Value;

            // If there's a per-level component, show it
            if (RadiusPerLevel.HasValue && RadiusPerLevel > 0)
            {
                string maxPart = RadiusMax.HasValue ? $" (max {RadiusMax} yards)" : "";
                return $"{baseRadius} + {RadiusPerLevel}/level yards{maxPart}";
            }

            // Just base radius
            return $"{baseRadius} yards";
        }

        // Helper method to calculate radius at a specific level
        public float CalculateRadiusAtLevel(int level)
        {
            if (!Radius.HasValue) return 0;

            float calculatedRadius = Radius.Value;

            if (RadiusPerLevel.HasValue)
            {
                calculatedRadius += RadiusPerLevel.Value * level;
            }

            // Apply maximum if specified
            if (RadiusMax.HasValue && calculatedRadius > RadiusMax.Value)
            {
                calculatedRadius = RadiusMax.Value;
            }

            return calculatedRadius;
        }
    }
}