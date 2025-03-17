using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpellWorker
{
    // Enum definitions based on the VMangos source
    public enum SpellSchool
    {
        SPELL_SCHOOL_NORMAL = 0,
        SPELL_SCHOOL_HOLY = 1,
        SPELL_SCHOOL_FIRE = 2,
        SPELL_SCHOOL_NATURE = 3,
        SPELL_SCHOOL_FROST = 4,
        SPELL_SCHOOL_SHADOW = 5,
        SPELL_SCHOOL_ARCANE = 6
    }

    public enum SpellEffects
    {
        SPELL_EFFECT_NONE = 0,
        SPELL_EFFECT_INSTAKILL = 1,
        SPELL_EFFECT_SCHOOL_DAMAGE = 2,
        SPELL_EFFECT_DUMMY = 3,
        SPELL_EFFECT_PORTAL_TELEPORT = 4,
        SPELL_EFFECT_TELEPORT_UNITS = 5,
        SPELL_EFFECT_APPLY_AURA = 6,
        SPELL_EFFECT_ENVIRONMENTAL_DAMAGE = 7,
        SPELL_EFFECT_POWER_DRAIN = 8,
        SPELL_EFFECT_HEALTH_LEECH = 9,
        SPELL_EFFECT_HEAL = 10,
        SPELL_EFFECT_BIND = 11,
        SPELL_EFFECT_PORTAL = 12,
        SPELL_EFFECT_RITUAL_BASE = 13,
        SPELL_EFFECT_RITUAL_SPECIALIZE = 14,
        SPELL_EFFECT_RITUAL_ACTIVATE_PORTAL = 15,
        SPELL_EFFECT_QUEST_COMPLETE = 16,
        SPELL_EFFECT_WEAPON_DAMAGE_NOSCHOOL = 17,
        SPELL_EFFECT_RESURRECT = 18,
        SPELL_EFFECT_ADD_EXTRA_ATTACKS = 19,
        SPELL_EFFECT_DODGE = 20,
        SPELL_EFFECT_EVADE = 21,
        SPELL_EFFECT_PARRY = 22,
        SPELL_EFFECT_BLOCK = 23,
        SPELL_EFFECT_CREATE_ITEM = 24,
        SPELL_EFFECT_WEAPON = 25,
        SPELL_EFFECT_DEFENSE = 26,
        SPELL_EFFECT_PERSISTENT_AREA_AURA = 27,
        SPELL_EFFECT_SUMMON = 28,
        SPELL_EFFECT_LEAP = 29,
        SPELL_EFFECT_ENERGIZE = 30,
        SPELL_EFFECT_WEAPON_PERCENT_DAMAGE = 31,
        SPELL_EFFECT_TRIGGER_MISSILE = 32,
        SPELL_EFFECT_OPEN_LOCK = 33,
        SPELL_EFFECT_SUMMON_CHANGE_ITEM = 34,
        SPELL_EFFECT_APPLY_AREA_AURA_PARTY = 35,
        SPELL_SPELL_EFFECT_APPLY_AREA_AURA_PARTY = 35,
        SPELL_EFFECT_LEARN_SPELL = 36,
        SPELL_EFFECT_SPELL_DEFENSE = 37,
        SPELL_EFFECT_DISPEL = 38,
        SPELL_EFFECT_LANGUAGE = 39,
        SPELL_EFFECT_DUAL_WIELD = 40,
        SPELL_EFFECT_SUMMON_WILD = 41,
        SPELL_EFFECT_SUMMON_GUARDIAN = 42,
        SPELL_EFFECT_TELEPORT_UNITS_FACE_CASTER = 43,
        SPELL_EFFECT_SKILL_STEP = 44,
        SPELL_EFFECT_ADD_HONOR = 45,
        SPELL_EFFECT_SPAWN = 46,
        SPELL_EFFECT_TRADE_SKILL = 47,
        SPELL_EFFECT_STEALTH = 48,
        SPELL_EFFECT_DETECT = 49,
        SPELL_EFFECT_TRANS_DOOR = 50,
        SPELL_EFFECT_FORCE_CRITICAL_HIT = 51,
        SPELL_EFFECT_GUARANTEE_HIT = 52,
        SPELL_EFFECT_ENCHANT_ITEM = 53,
        SPELL_EFFECT_ENCHANT_ITEM_TEMPORARY = 54,
        SPELL_EFFECT_TAMECREATURE = 55,
        SPELL_EFFECT_SUMMON_PET = 56,
        SPELL_EFFECT_LEARN_PET_SPELL = 57,
        SPELL_EFFECT_WEAPON_DAMAGE = 58,
        SPELL_EFFECT_OPEN_LOCK_ITEM = 59,
        SPELL_EFFECT_PROFICIENCY = 60,
        SPELL_EFFECT_SEND_EVENT = 61,
        SPELL_EFFECT_POWER_BURN = 62,
        SPELL_EFFECT_THREAT = 63,
        SPELL_EFFECT_TRIGGER_SPELL = 64,
        SPELL_EFFECT_HEALTH_FUNNEL = 65,
        SPELL_EFFECT_POWER_FUNNEL = 66,
        SPELL_EFFECT_HEAL_MAX_HEALTH = 67,
        SPELL_EFFECT_INTERRUPT_CAST = 68,
        SPELL_EFFECT_DISTRACT = 69,
        SPELL_EFFECT_PULL = 70,
        SPELL_EFFECT_PICKPOCKET = 71,
        SPELL_EFFECT_ADD_FARSIGHT = 72,
        SPELL_EFFECT_SUMMON_POSSESSED = 73,
        SPELL_EFFECT_SUMMON_TOTEM = 74,
        SPELL_EFFECT_HEAL_MECHANICAL = 75,
        SPELL_EFFECT_SUMMON_OBJECT_WILD = 76,
        SPELL_EFFECT_SCRIPT_EFFECT = 77,
        SPELL_EFFECT_ATTACK = 78,
        SPELL_EFFECT_SANCTUARY = 79,
        SPELL_EFFECT_ADD_COMBO_POINTS = 80,
        SPELL_EFFECT_CREATE_HOUSE = 81,
        SPELL_EFFECT_BIND_SIGHT = 82,
        SPELL_EFFECT_DUEL = 83,
        SPELL_EFFECT_STUCK = 84,
        SPELL_EFFECT_SUMMON_PLAYER = 85,
        SPELL_EFFECT_ACTIVATE_OBJECT = 86,
        SPELL_EFFECT_SUMMON_TOTEM_SLOT1 = 87,
        SPELL_EFFECT_SUMMON_TOTEM_SLOT2 = 88,
        SPELL_EFFECT_SUMMON_TOTEM_SLOT3 = 89,
        SPELL_EFFECT_SUMMON_TOTEM_SLOT4 = 90,
        SPELL_EFFECT_THREAT_ALL = 91,
        SPELL_EFFECT_ENCHANT_HELD_ITEM = 92,
        SPELL_EFFECT_SUMMON_PHANTASM = 93,
        SPELL_EFFECT_SELF_RESURRECT = 94,
        SPELL_EFFECT_SKINNING = 95,
        SPELL_EFFECT_CHARGE = 96,
        SPELL_EFFECT_SUMMON_CRITTER = 97,
        SPELL_EFFECT_KNOCK_BACK = 98,
        SPELL_EFFECT_DISENCHANT = 99,
        SPELL_EFFECT_INEBRIATE = 100,
        SPELL_EFFECT_FEED_PET = 101,
        SPELL_EFFECT_DISMISS_PET = 102,
        SPELL_EFFECT_REPUTATION = 103,
        SPELL_EFFECT_SUMMON_OBJECT_SLOT1 = 104,
        SPELL_EFFECT_SUMMON_OBJECT_SLOT2 = 105,
        SPELL_EFFECT_SUMMON_OBJECT_SLOT3 = 106,
        SPELL_EFFECT_SUMMON_OBJECT_SLOT4 = 107,
        SPELL_EFFECT_DISPEL_MECHANIC = 108,
        SPELL_EFFECT_SUMMON_DEAD_PET = 109,
        SPELL_EFFECT_DESTROY_ALL_TOTEMS = 110,
        SPELL_EFFECT_DURABILITY_DAMAGE = 111,
        SPELL_EFFECT_SUMMON_DEMON = 112,
        SPELL_EFFECT_RESURRECT_NEW = 113,
        SPELL_EFFECT_ATTACK_ME = 114,
        SPELL_EFFECT_DURABILITY_DAMAGE_PCT = 115,
        SPELL_EFFECT_SKIN_PLAYER_CORPSE = 116,
        SPELL_EFFECT_SPIRIT_HEAL = 117,
        SPELL_EFFECT_SKILL = 118,
        SPELL_EFFECT_APPLY_AREA_AURA_OWNER = 119,
        SPELL_EFFECT_TELEPORT_GRAVEYARD = 120,
        SPELL_EFFECT_NORMALIZED_WEAPON_DMG = 121,
        SPELL_EFFECT_122 = 122,
        SPELL_EFFECT_SEND_TAXI = 123,
        SPELL_EFFECT_PLAYER_PULL = 124,
        SPELL_EFFECT_MODIFY_THREAT_PERCENT = 125,
        SPELL_EFFECT_126 = 126,
        SPELL_EFFECT_127 = 127,
        SPELL_EFFECT_APPLY_AREA_AURA_FRIEND = 128,
        SPELL_EFFECT_APPLY_AREA_AURA_ENEMY = 129,
        SPELL_EFFECT_DESPAWN_OBJECT = 130,
        SPELL_EFFECT_NOSTALRIUS = 131,
        SPELL_EFFECT_APPLY_AREA_AURA_RAID = 132,
        SPELL_EFFECT_APPLY_AREA_AURA_PET = 133,
        SPELL_EFFECT_APPLY_AREA_AURA_MINION = 134,
        TOTAL_SPELL_EFFECTS = 135
    }

    enum AuraType
    {
        SPELL_AURA_NONE = 0,
        SPELL_AURA_BIND_SIGHT = 1,
        SPELL_AURA_MOD_POSSESS = 2,
        /**
         * The aura should do periodic damage, the function that handles
         * this is Aura::HandlePeriodicDamage, the amount is usually decided
         * by the Unit::SpellDamageBonusDone or Unit::MeleeDamageBonusDone
         * which increases/decreases the Modifier::m_amount
         */
        SPELL_AURA_PERIODIC_DAMAGE = 3,
        /**
         * Used by Aura::HandleAuraDummy
         */
        SPELL_AURA_DUMMY = 4,
        /**
         * Used by Aura::HandleModConfuse, will either confuse or unconfuse
         * the target depending on whether the apply flag is set
         */
        SPELL_AURA_MOD_CONFUSE = 5,
        SPELL_AURA_MOD_CHARM = 6,
        SPELL_AURA_MOD_FEAR = 7,
        /**
         * The aura will do periodic heals of a target, handled by
         * Aura::HandlePeriodicHeal, uses Unit::SpellHealingBonusDone
         * to calculate whether to increase or decrease Modifier::m_amount
         */
        SPELL_AURA_PERIODIC_HEAL = 8,
        /**
         * Changes the attackspeed, the Modifier::m_amount decides
         * how much we change in percent, ie, if the m_amount is
         * 50 the attackspeed will increase by 50%
         */
        SPELL_AURA_MOD_ATTACKSPEED = 9,
        /**
         * Modifies the threat that the Aura does in percent,
         * the Modifier::m_miscvalue decides which of the SpellSchools
         * it should affect threat for.
         * \see SpellSchoolMask
         */
        SPELL_AURA_MOD_THREAT = 10,
        /**
         * Just applies a taunt which will change the threat a mob has
         * Taken care of in Aura::HandleModThreat
         */
        SPELL_AURA_MOD_TAUNT = 11,
        /**
         * Stuns targets in different ways, taken care of in
         * Aura::HandleAuraModStun
         */
        SPELL_AURA_MOD_STUN = 12,
        /**
         * Changes the damage done by a weapon in any hand, the Modifier::m_miscvalue
         * will tell what school the damage is from, it's used as a bitmask
         * \see SpellSchoolMask
         */
        SPELL_AURA_MOD_DAMAGE_DONE = 13,
        /**
         * Not handled by the Aura class but instead this is implemented in
         * Unit::MeleeDamageBonusTaken and Unit::SpellBaseDamageBonusTaken
         */
        SPELL_AURA_MOD_DAMAGE_TAKEN = 14,
        /**
         * Not handled by the Aura class, implemented in Unit::DealMeleeDamage
         */
        SPELL_AURA_DAMAGE_SHIELD = 15,
        /**
         * Taken care of in Aura::HandleModStealth, take note that this
         * is not the same thing as invisibility
         */
        SPELL_AURA_MOD_STEALTH = 16,
        /**
         * Not handled by the Aura class, implemented in Unit::isVisibleForOrDetect
         * which does a lot of checks to determine whether the person is visible or not,
         * the SPELL_AURA_MOD_STEALTH seems to determine how in/visible ie a rogue is.
         */
        SPELL_AURA_MOD_STEALTH_DETECT = 17,
        /**
         * Handled by Aura::HandleInvisibility, the Modifier::m_miscvalue in the struct
         * seems to decide what kind of invisibility it is with a bitflag. the miscvalue
         * decides which bit is set, ie: 3 would make the 3rd bit be set.
         */
        SPELL_AURA_MOD_INVISIBILITY = 18,
        /**
         * Adds one of the kinds of detections to the possible detections.
         * As in SPEALL_AURA_MOD_INVISIBILITY the Modifier::m_miscvalue seems to decide
         * what kind of invisibility the Unit should be able to detect.
         */
        SPELL_AURA_MOD_INVISIBILITY_DETECTION = 19,
        SPELL_AURA_OBS_MOD_HEALTH = 20,                         //20,21 unofficial
        SPELL_AURA_OBS_MOD_MANA = 21,
        /**
         * Handled by Aura::HandleAuraModResistance, changes the resistance for a unit
         * the field Modifier::m_miscvalue decides which kind of resistance that should
         * be changed, for possible values see SpellSchools.
         * \see SpellSchools
         */
        SPELL_AURA_MOD_RESISTANCE = 22,
        /**
         * Currently just sets Aura::m_isPeriodic to apply and has a special case
         * for Curse of the Plaguebringer.
         */
        SPELL_AURA_PERIODIC_TRIGGER_SPELL = 23,
        /**
         * Just sets Aura::m_isPeriodic to apply
         */
        SPELL_AURA_PERIODIC_ENERGIZE = 24,
        /**
         * Changes whether the target is pacified or not depending on the apply flag.
         * Pacify makes the target silenced and have all it's attack skill disabled.
         * See: http://www.wowhead.com/spell=6462/pacified
         */
        SPELL_AURA_MOD_PACIFY = 25,
        /**
         * Roots or unroots the target
         */
        SPELL_AURA_MOD_ROOT = 26,
        /**
         * Silences the target and stops and spell casts that should be stopped,
         * they have the flag SpellPreventionType::SPELL_PREVENTION_TYPE_SILENCE
         */
        SPELL_AURA_MOD_SILENCE = 27,
        SPELL_AURA_REFLECT_SPELLS = 28,
        SPELL_AURA_MOD_STAT = 29,
        SPELL_AURA_MOD_SKILL = 30,
        SPELL_AURA_MOD_INCREASE_SPEED = 31,
        SPELL_AURA_MOD_INCREASE_MOUNTED_SPEED = 32,
        SPELL_AURA_MOD_DECREASE_SPEED = 33,
        SPELL_AURA_MOD_INCREASE_HEALTH = 34,
        SPELL_AURA_MOD_INCREASE_ENERGY = 35,
        SPELL_AURA_MOD_SHAPESHIFT = 36,
        SPELL_AURA_EFFECT_IMMUNITY = 37,
        SPELL_AURA_STATE_IMMUNITY = 38,
        SPELL_AURA_SCHOOL_IMMUNITY = 39,
        SPELL_AURA_DAMAGE_IMMUNITY = 40,
        SPELL_AURA_DISPEL_IMMUNITY = 41,
        SPELL_AURA_PROC_TRIGGER_SPELL = 42,
        SPELL_AURA_PROC_TRIGGER_DAMAGE = 43,
        SPELL_AURA_TRACK_CREATURES = 44,
        SPELL_AURA_TRACK_RESOURCES = 45,
        SPELL_AURA_MOD_PARRY_SKILL = 46,
        SPELL_AURA_MOD_PARRY_PERCENT = 47,
        SPELL_AURA_MOD_DODGE_SKILL = 48,
        SPELL_AURA_MOD_DODGE_PERCENT = 49,
        SPELL_AURA_MOD_BLOCK_SKILL = 50,
        SPELL_AURA_MOD_BLOCK_PERCENT = 51,
        SPELL_AURA_MOD_CRIT_PERCENT = 52,
        SPELL_AURA_PERIODIC_LEECH = 53,
        SPELL_AURA_MOD_HIT_CHANCE = 54,
        SPELL_AURA_MOD_SPELL_HIT_CHANCE = 55,
        SPELL_AURA_TRANSFORM = 56,
        SPELL_AURA_MOD_SPELL_CRIT_CHANCE = 57,
        SPELL_AURA_MOD_INCREASE_SWIM_SPEED = 58,
        SPELL_AURA_MOD_DAMAGE_DONE_CREATURE = 59,
        SPELL_AURA_MOD_PACIFY_SILENCE = 60,
        SPELL_AURA_MOD_SCALE = 61,
        SPELL_AURA_PERIODIC_HEALTH_FUNNEL = 62,
        SPELL_AURA_PERIODIC_MANA_FUNNEL = 63,
        SPELL_AURA_PERIODIC_MANA_LEECH = 64,
        SPELL_AURA_MOD_CASTING_SPEED_NOT_STACK = 65,
        SPELL_AURA_FEIGN_DEATH = 66,
        SPELL_AURA_MOD_DISARM = 67,
        SPELL_AURA_MOD_STALKED = 68,
        SPELL_AURA_SCHOOL_ABSORB = 69,
        SPELL_AURA_EXTRA_ATTACKS = 70,
        SPELL_AURA_MOD_SPELL_CRIT_CHANCE_SCHOOL = 71,
        SPELL_AURA_MOD_POWER_COST_SCHOOL_PCT = 72,
        SPELL_AURA_MOD_POWER_COST_SCHOOL = 73,
        SPELL_AURA_REFLECT_SPELLS_SCHOOL = 74,
        SPELL_AURA_MOD_LANGUAGE = 75,
        SPELL_AURA_FAR_SIGHT = 76,
        SPELL_AURA_MECHANIC_IMMUNITY = 77,
        SPELL_AURA_MOUNTED = 78,
        SPELL_AURA_MOD_DAMAGE_PERCENT_DONE = 79,
        SPELL_AURA_MOD_PERCENT_STAT = 80,
        SPELL_AURA_SPLIT_DAMAGE_PCT = 81,
        SPELL_AURA_WATER_BREATHING = 82,
        SPELL_AURA_MOD_BASE_RESISTANCE = 83,
        SPELL_AURA_MOD_REGEN = 84,
        SPELL_AURA_MOD_POWER_REGEN = 85,
        SPELL_AURA_CHANNEL_DEATH_ITEM = 86,
        SPELL_AURA_MOD_DAMAGE_PERCENT_TAKEN = 87,
        SPELL_AURA_MOD_HEALTH_REGEN_PERCENT = 88,
        SPELL_AURA_PERIODIC_DAMAGE_PERCENT = 89,
        SPELL_AURA_MOD_RESIST_CHANCE = 90,
        SPELL_AURA_MOD_DETECT_RANGE = 91,
        SPELL_AURA_PREVENTS_FLEEING = 92,
        SPELL_AURA_MOD_UNATTACKABLE = 93,
        SPELL_AURA_INTERRUPT_REGEN = 94,
        SPELL_AURA_GHOST = 95,
        SPELL_AURA_SPELL_MAGNET = 96,
        SPELL_AURA_MANA_SHIELD = 97,
        SPELL_AURA_MOD_SKILL_TALENT = 98,
        SPELL_AURA_MOD_ATTACK_POWER = 99,
        SPELL_AURA_AURAS_VISIBLE = 100,
        SPELL_AURA_MOD_RESISTANCE_PCT = 101,
        SPELL_AURA_MOD_MELEE_ATTACK_POWER_VERSUS = 102,
        SPELL_AURA_MOD_TOTAL_THREAT = 103,
        SPELL_AURA_WATER_WALK = 104,
        SPELL_AURA_FEATHER_FALL = 105,
        SPELL_AURA_HOVER = 106,
        SPELL_AURA_ADD_FLAT_MODIFIER = 107,
        SPELL_AURA_ADD_PCT_MODIFIER = 108,
        SPELL_AURA_ADD_TARGET_TRIGGER = 109,
        SPELL_AURA_MOD_POWER_REGEN_PERCENT = 110,
        SPELL_AURA_ADD_CASTER_HIT_TRIGGER = 111,
        SPELL_AURA_OVERRIDE_CLASS_SCRIPTS = 112,
        SPELL_AURA_MOD_RANGED_DAMAGE_TAKEN = 113,
        SPELL_AURA_MOD_RANGED_DAMAGE_TAKEN_PCT = 114,
        SPELL_AURA_MOD_HEALING = 115,
        SPELL_AURA_MOD_REGEN_DURING_COMBAT = 116,
        SPELL_AURA_MOD_MECHANIC_RESISTANCE = 117,
        SPELL_AURA_MOD_HEALING_PCT = 118,
        SPELL_AURA_SHARE_PET_TRACKING = 119,
        SPELL_AURA_UNTRACKABLE = 120,
        SPELL_AURA_EMPATHY = 121,
        SPELL_AURA_MOD_OFFHAND_DAMAGE_PCT = 122,
        SPELL_AURA_MOD_TARGET_RESISTANCE = 123,
        SPELL_AURA_MOD_RANGED_ATTACK_POWER = 124,
        SPELL_AURA_MOD_MELEE_DAMAGE_TAKEN = 125,
        SPELL_AURA_MOD_MELEE_DAMAGE_TAKEN_PCT = 126,
        SPELL_AURA_RANGED_ATTACK_POWER_ATTACKER_BONUS = 127,
        SPELL_AURA_MOD_POSSESS_PET = 128,
        SPELL_AURA_MOD_SPEED_ALWAYS = 129,
        SPELL_AURA_MOD_MOUNTED_SPEED_ALWAYS = 130,
        SPELL_AURA_MOD_RANGED_ATTACK_POWER_VERSUS = 131,
        SPELL_AURA_MOD_INCREASE_ENERGY_PERCENT = 132,
        SPELL_AURA_MOD_INCREASE_HEALTH_PERCENT = 133,
        SPELL_AURA_MOD_MANA_REGEN_INTERRUPT = 134,
        SPELL_AURA_MOD_HEALING_DONE = 135,
        SPELL_AURA_MOD_HEALING_DONE_PERCENT = 136,
        SPELL_AURA_MOD_TOTAL_STAT_PERCENTAGE = 137,
        SPELL_AURA_MOD_MELEE_HASTE = 138,
        SPELL_AURA_FORCE_REACTION = 139,
        SPELL_AURA_MOD_RANGED_HASTE = 140,
        SPELL_AURA_MOD_RANGED_AMMO_HASTE = 141,
        SPELL_AURA_MOD_BASE_RESISTANCE_PCT = 142,
        SPELL_AURA_MOD_RESISTANCE_EXCLUSIVE = 143,
        SPELL_AURA_SAFE_FALL = 144,
        SPELL_AURA_CHARISMA = 145,
        SPELL_AURA_PERSUADED = 146,
        SPELL_AURA_MECHANIC_IMMUNITY_MASK = 147,
        SPELL_AURA_RETAIN_COMBO_POINTS = 148,
        SPELL_AURA_RESIST_PUSHBACK = 149,                      //    Resist Pushback
        SPELL_AURA_MOD_SHIELD_BLOCKVALUE_PCT = 150,
        SPELL_AURA_TRACK_STEALTHED = 151,                      //    Track Stealthed
        SPELL_AURA_MOD_DETECTED_RANGE = 152,                    //    Mod Detected Range
        SPELL_AURA_SPLIT_DAMAGE_FLAT = 153,                     //    Split Damage Flat
        SPELL_AURA_MOD_STEALTH_LEVEL = 154,                     //    Stealth Level Modifier
        SPELL_AURA_MOD_WATER_BREATHING = 155,                   //    Mod Water Breathing
        SPELL_AURA_MOD_REPUTATION_GAIN = 156,                   //    Mod Reputation Gain
        SPELL_AURA_PET_DAMAGE_MULTI = 157,                      //    Mod Pet Damage
        SPELL_AURA_MOD_SHIELD_BLOCKVALUE = 158,
        SPELL_AURA_NO_PVP_CREDIT = 159,
        SPELL_AURA_MOD_AOE_AVOIDANCE = 160,
        SPELL_AURA_MOD_HEALTH_REGEN_IN_COMBAT = 161,
        SPELL_AURA_POWER_BURN_MANA = 162,
        SPELL_AURA_MOD_CRIT_DAMAGE_BONUS = 163,
        SPELL_AURA_164 = 164,
        SPELL_AURA_MELEE_ATTACK_POWER_ATTACKER_BONUS = 165,
        SPELL_AURA_MOD_ATTACK_POWER_PCT = 166,
        SPELL_AURA_MOD_RANGED_ATTACK_POWER_PCT = 167,
        SPELL_AURA_MOD_DAMAGE_DONE_VERSUS = 168,
        SPELL_AURA_MOD_CRIT_PERCENT_VERSUS = 169,
        SPELL_AURA_DETECT_AMORE = 170,
        SPELL_AURA_MOD_SPEED_NOT_STACK = 171,
        SPELL_AURA_MOD_MOUNTED_SPEED_NOT_STACK = 172,
        SPELL_AURA_ALLOW_CHAMPION_SPELLS = 173,
        SPELL_AURA_MOD_SPELL_DAMAGE_OF_SPIRIT_PCT = 174,
        SPELL_AURA_MOD_SPELL_HEALING_OF_STAT_PERCENT = 175,
        SPELL_AURA_SPIRIT_OF_REDEMPTION = 176,
        SPELL_AURA_AOE_CHARM = 177,
        SPELL_AURA_MOD_DEBUFF_RESISTANCE = 178,
        SPELL_AURA_MOD_ATTACKER_SPELL_CRIT_CHANCE = 179,
        SPELL_AURA_MOD_FLAT_SPELL_DAMAGE_VERSUS = 180,
        SPELL_AURA_MOD_FLAT_SPELL_CRIT_DAMAGE_VERSUS = 181,     // unused - possible flat spell crit damage versus
        SPELL_AURA_MOD_RESISTANCE_OF_STAT_PERCENT = 182,
        SPELL_AURA_MOD_CRITICAL_THREAT = 183,
        SPELL_AURA_MOD_ATTACKER_MELEE_HIT_CHANCE = 184,
        SPELL_AURA_MOD_ATTACKER_RANGED_HIT_CHANCE = 185,
        SPELL_AURA_MOD_ATTACKER_SPELL_HIT_CHANCE = 186,
        SPELL_AURA_MOD_ATTACKER_MELEE_CRIT_CHANCE = 187,
        SPELL_AURA_MOD_ATTACKER_RANGED_CRIT_CHANCE = 188,
        SPELL_AURA_MOD_RATING = 189,
        SPELL_AURA_MOD_FACTION_REPUTATION_GAIN = 190,
        SPELL_AURA_USE_NORMAL_MOVEMENT_SPEED = 191,
        // Custom
        SPELL_AURA_AURA_SPELL = 192, // Adds the auras of a spell while this aura is active.
        SPELL_AURA_SPLIT_DAMAGE_GROUP_PCT = 193, // Needed for Spirit Link.
        SPELL_AURA_MOD_AOE_DAMAGE_PERCENT_TAKEN = 194, // Needed for Pet Avoidance.
        SPELL_AURA_MOD_HONOR_GAIN = 195, // From WotLK.
        SPELL_AURA_ENABLE_FLYING = 196, // For flying mounts.
        SPELL_AURA_MOD_PERIODIC_DAMAGE_PERCENT_TAKEN = 197, // Modifies periodic damage taken.
        SPELL_AURA_MOD_CRIT_DAMAGE_BONUS_TAKEN = 198, // Modifies critical damage taken.
        SPELL_AURA_MOD_HEALING_DONE_FROM_ARMOR = 199, // Modifies healing power by percent of armor from items.
        SPELL_AURA_TRANSFER_THREAT_PERCENT = 200, // Percentage of threat caused is transferred to caster.
        SPELL_AURA_TRIGGER_SPELL_ON_CONTROLLED_UNIT = 201, // Spell gets cast on unit controlled by us.
        SPELL_AURA_SHARE_STAT_PCT = 202, // Percentage of caster's stats is added to the target.
        SPELL_AURA_SHARE_BASE_ARMOR_PCT = 203, // Percentage of caster's armor from items is added to the target.
        SPELL_AURA_SHARE_RESISTANCE_PCT = 204, // Percentage of caster's resistances are added to the target.
        SPELL_AURA_SHARE_MELEE_AP_BY_RANGED_AP_PCT = 205, // Percentage of caster's ranged attack power is added to the target as melee attack power.
        SPELL_AURA_SHARE_DAMAGE_BY_RANGED_AP_PCT = 206, // Percentage of caster's ranged attack power is added to the target as spell damage.
        SPELL_AURA_MOD_COMBAT_REACH = 207, // Increases the target's combat reach (melee range).
        SPELL_AURA_MOD_REAGENT_CONSUMPTION_CHANCE = 208, // A new On Equip effect for soul bags "Your spells have X% chance to not consume a reagent."
        SPELL_AURA_MECHANIC_DURATION_MOD = 209, // Changes aura duration with given mechanic value.
        SPELL_AURA_MOD_SELF_REZ_RESOURCES_PERCENT = 210, // Modifies health and mana you self rez with
        SPELL_AURA_SHARE_HIT_CHANCE_BY_SPELL_HIT_PCT = 211, // Percentage of caster's spell hit chance is added to the target's melee and spell hit chance.
        SPELL_AURA_MOD_TARGET_RESISTANCE_PCT = 212,
        SPELL_AURA_MOD_SPELL_DAMAGE_OF_INTELLECT_PCT = 213,
        SPELL_AURA_MOD_POWER_GAIN_PCT = 214,
        SPELL_AURA_SHARE_MELEE_CRIT_CHANCE_BY_SPELL_CRIT_PCT = 215, // Percentage of caster's melee crit is added to the target.
        SPELL_AURA_SHARE_SPELL_CRIT_CHANCE_PCT = 216, // Percentage of caster's spell crit is added to the target.
        TOTAL_AURAS = 217
    };

    public enum DispelType
    {
        DISPEL_NONE = 0,
        DISPEL_MAGIC = 1,
        DISPEL_CURSE = 2,
        DISPEL_DISEASE = 3,
        DISPEL_POISON = 4,
        DISPEL_STEALTH = 5,
        DISPEL_INVISIBILITY = 6,
        DISPEL_ALL = 7,
        DISPEL_SPE_NPC_ONLY = 8,
        DISPEL_ENRAGE = 9,
        DISPEL_ZG_TICKET = 10
    };

    public enum Mechanics
    {
        MECHANIC_NONE = 0,
        MECHANIC_CHARM = 1,
        MECHANIC_DISORIENTED = 2,
        MECHANIC_DISARM = 3,
        MECHANIC_DISTRACT = 4,
        MECHANIC_FEAR = 5,
        MECHANIC_FUMBLE = 6,
        MECHANIC_ROOT = 7,
        MECHANIC_PACIFY = 8,                          // 0 spells use this mechanic
        MECHANIC_SILENCE = 9,
        MECHANIC_SLEEP = 10,
        MECHANIC_SNARE = 11,
        MECHANIC_STUN = 12,
        MECHANIC_FREEZE = 13,
        MECHANIC_KNOCKOUT = 14,
        MECHANIC_BLEED = 15,
        MECHANIC_BANDAGE = 16,
        MECHANIC_POLYMORPH = 17,
        MECHANIC_BANISH = 18,
        MECHANIC_SHIELD = 19,
        MECHANIC_SHACKLE = 20,
        MECHANIC_MOUNT = 21,
        MECHANIC_PERSUADE = 22,                         // 0 spells use this mechanic
        MECHANIC_TURN = 23,
        MECHANIC_HORROR = 24,
        MECHANIC_INVULNERABILITY = 25,
        MECHANIC_INTERRUPT = 26,
        MECHANIC_DAZE = 27,
        MECHANIC_DISCOVERY = 28,
        MECHANIC_IMMUNE_SHIELD = 29,                         // Divine (Blessing) Shield/Protection and Ice Block
        MECHANIC_SAPPED = 30,

        // Custom
        MECHANIC_SLOW_CAST_SPEED = 31                          // Curse of Tongues
    }

    public enum SpellTarget
    {
        TARGET_NONE                                        = 0,
        TARGET_UNIT_CASTER                                 = 1,
        TARGET_UNIT_ENEMY_NEAR_CASTER                      = 2,
        TARGET_UNIT_FRIEND_NEAR_CASTER                     = 3,
        TARGET_UNIT_NEAR_CASTER                            = 4,
        TARGET_UNIT_CASTER_PET                             = 5,
        TARGET_UNIT_ENEMY                                  = 6,
        TARGET_ENUM_UNITS_SCRIPT_AOE_AT_SRC_LOC            = 7,
        TARGET_ENUM_UNITS_SCRIPT_AOE_AT_DEST_LOC           = 8,
        TARGET_LOCATION_CASTER_HOME_BIND                   = 9,
        TARGET_LOCATION_CASTER_DIVINE_BIND_NYI             = 10,
        TARGET_PLAYER_NYI                                  = 11,
        TARGET_PLAYER_NEAR_CASTER_NYI                      = 12,
        TARGET_PLAYER_ENEMY_NYI                            = 13,
        TARGET_PLAYER_FRIEND_NYI                           = 14,
        TARGET_ENUM_UNITS_ENEMY_AOE_AT_SRC_LOC             = 15,
        TARGET_ENUM_UNITS_ENEMY_AOE_AT_DEST_LOC            = 16,
        TARGET_LOCATION_DATABASE                           = 17,
        TARGET_LOCATION_CASTER_DEST                        = 18,
        TARGET_UNK_19                                      = 19,
        TARGET_ENUM_UNITS_PARTY_WITHIN_CASTER_RANGE        = 20,
        TARGET_UNIT_FRIEND                                 = 21,
        TARGET_LOCATION_CASTER_SRC                         = 22,
        TARGET_GAMEOBJECT                                  = 23,
        TARGET_ENUM_UNITS_ENEMY_IN_CONE_24                 = 24,
        TARGET_UNIT                                        = 25,
        TARGET_LOCKED                                      = 26,
        TARGET_UNIT_CASTER_MASTER                          = 27,
        TARGET_ENUM_UNITS_ENEMY_AOE_AT_DYNOBJ_LOC          = 28,
        TARGET_ENUM_UNITS_FRIEND_AOE_AT_DYNOBJ_LOC         = 29,
        TARGET_ENUM_UNITS_FRIEND_AOE_AT_SRC_LOC            = 30,
        TARGET_ENUM_UNITS_FRIEND_AOE_AT_DEST_LOC           = 31,
        TARGET_LOCATION_UNIT_MINION_POSITION               = 32,
        TARGET_ENUM_UNITS_PARTY_AOE_AT_SRC_LOC             = 33,
        TARGET_ENUM_UNITS_PARTY_AOE_AT_DEST_LOC            = 34,
        TARGET_UNIT_PARTY                                  = 35,
        TARGET_ENUM_UNITS_ENEMY_WITHIN_CASTER_RANGE        = 36, // TODO: only used with dest-effects - reinvestigate naming
        TARGET_UNIT_FRIEND_AND_PARTY                       = 37,
        TARGET_UNIT_SCRIPT_NEAR_CASTER                     = 38,
        TARGET_LOCATION_CASTER_FISHING_SPOT                = 39,
        TARGET_GAMEOBJECT_SCRIPT_NEAR_CASTER               = 40,
        TARGET_LOCATION_CASTER_FRONT_RIGHT                 = 41,
        TARGET_LOCATION_CASTER_BACK_RIGHT                  = 42,
        TARGET_LOCATION_CASTER_BACK_LEFT                   = 43,
        TARGET_LOCATION_CASTER_FRONT_LEFT                  = 44,
        TARGET_UNIT_FRIEND_CHAIN_HEAL                      = 45,
        TARGET_LOCATION_SCRIPT_NEAR_CASTER                 = 46,
        TARGET_LOCATION_CASTER_FRONT                       = 47,
        TARGET_LOCATION_CASTER_BACK                        = 48,
        TARGET_LOCATION_CASTER_LEFT                        = 49,
        TARGET_LOCATION_CASTER_RIGHT                       = 50,
        TARGET_ENUM_GAMEOBJECTS_SCRIPT_AOE_AT_SRC_LOC      = 51,
        TARGET_ENUM_GAMEOBJECTS_SCRIPT_AOE_AT_DEST_LOC     = 52,
        TARGET_LOCATION_CASTER_TARGET_POSITION             = 53,
        TARGET_ENUM_UNITS_ENEMY_IN_CONE_54                 = 54,
        TARGET_LOCATION_CASTER_FRONT_LEAP                  = 55,
        TARGET_ENUM_UNITS_RAID_WITHIN_CASTER_RANGE         = 56,
        TARGET_UNIT_RAID                                   = 57,
        TARGET_UNIT_RAID_NEAR_CASTER                       = 58,
        TARGET_ENUM_UNITS_FRIEND_IN_CONE                   = 59,
        TARGET_ENUM_UNITS_SCRIPT_IN_CONE_60                = 60,
        TARGET_UNIT_RAID_AND_CLASS                         = 61,
        TARGET_PLAYER_RAID_NYI                             = 62,
        TARGET_LOCATION_UNIT_POSITION                      = 63,

        MAX_SPELL_TARGETS
    }

    public enum Powers
    {
        POWER_MANA                          = 0,            // UNIT_FIELD_POWER1
        POWER_RAGE                          = 1,            // UNIT_FIELD_POWER2
        POWER_FOCUS                         = 2,            // UNIT_FIELD_POWER3
        POWER_ENERGY                        = 3,            // UNIT_FIELD_POWER4
        POWER_HAPPINESS                     = 4,            // UNIT_FIELD_POWER5
        POWER_HEALTH                        = -2            // (-2 as signed value)
    };

    public enum SpellFamily
    {
        SPELLFAMILY_GENERIC = 0,
        SPELLFAMILY_UNK1 = 1,                            // events, holidays
                                                         // 2 - unused
        SPELLFAMILY_MAGE = 3,
        SPELLFAMILY_WARRIOR = 4,
        SPELLFAMILY_WARLOCK = 5,
        SPELLFAMILY_PRIEST = 6,
        SPELLFAMILY_DRUID = 7,
        SPELLFAMILY_ROGUE = 8,
        SPELLFAMILY_HUNTER = 9,
        SPELLFAMILY_PALADIN = 10,
        SPELLFAMILY_SHAMAN = 11,
        SPELLFAMILY_UNK2 = 12,
        SPELLFAMILY_POTION = 13,
        // 14 - unused
        SPELLFAMILY_DEATHKNIGHT = 15,
        // 16 - unused
        SPELLFAMILY_UNK3 = 17
    };

    public enum ItemClass
    {
        ITEM_CLASS_CONSUMABLE = 0,
        ITEM_CLASS_CONTAINER = 1,
        ITEM_CLASS_WEAPON = 2,
        ITEM_CLASS_GEM = 3,
        ITEM_CLASS_ARMOR = 4,
        ITEM_CLASS_REAGENT = 5,
        ITEM_CLASS_PROJECTILE = 6,
        ITEM_CLASS_TRADE_GOODS = 7,
        ITEM_CLASS_GENERIC = 8,
        ITEM_CLASS_RECIPE = 9,
        ITEM_CLASS_MONEY = 10,
        ITEM_CLASS_QUIVER = 11,
        ITEM_CLASS_QUEST = 12,
        ITEM_CLASS_KEY = 13,
        ITEM_CLASS_PERMANENT = 14,
        ITEM_CLASS_JUNK = 15
    }

    public enum CreatureType
    {
        CREATURE_TYPE_BEAST = 1,
        CREATURE_TYPE_DRAGONKIN = 2,
        CREATURE_TYPE_DEMON = 3,
        CREATURE_TYPE_ELEMENTAL = 4,
        CREATURE_TYPE_GIANT = 5,
        CREATURE_TYPE_UNDEAD = 6,
        CREATURE_TYPE_HUMANOID = 7,
        CREATURE_TYPE_CRITTER = 8,
        CREATURE_TYPE_MECHANICAL = 9,
        CREATURE_TYPE_NOT_SPECIFIED = 10,
        CREATURE_TYPE_TOTEM = 11,
    };

    public // original names, do not edit
enum SpellCategories
    {
        SPELLCATEGORY_DEFAULT = 1,
        SPELLCATEGORY_DIRECT_DAMAGE_SPELL = 2,
        SPELLCATEGORY_ITEM_COMBAT_CONSUMABLE_POTION = 4,
        SPELLCATEGORY_ITEM_FOOD = 11,
        SPELLCATEGORY_HEALING_SPELL = 12,
        SPELLCATEGORY_QUICK_BUFF_RESIST_SPELL = 17,
        SPELLCATEGORY_DAMAGE_OVER_TIME_SPELL = 18,
        SPELLCATEGORY_QUICK_DAMAGE_SPELL = 19,
        SPELLCATEGORY_INVULNERABILITY_OTHER = 20,
        SPELLCATEGORY_QUICK_BUFF_SPELL = 21,
        SPELLCATEGORY_QUICK_DEBUFF_SPELL = 22,
        SPELLCATEGORY_SUMMONING = 23,
        SPELLCATEGORY_ITEM_COMBAT_CONSUMABLE_AGGRESSIVE = 24,
        SPELLCATEGORY_QUICK_HEAL_SPELL = 25,
        SPELLCATEGORY_RESURRECTION_FULL = 26,
        SPELLCATEGORY_ITEM_SCROLL = 27,
        SPELLCATEGORY_ITEM_QUICK_BUFF = 28,
        SPELLCATEGORY_ITEM_DEBUFF = 29,
        SPELLCATEGORY_ITEM_HEALING = 30,
        SPELLCATEGORY_CONJURE_SHORT = 31,
        SPELLCATEGORY_STUN = 32,
        SPELLCATEGORY_MEZ = 33,
        SPELLCATEGORY_ROOT = 34,
        SPELLCATEGORY_DIRECT_DAMAGE_AE_SPELL = 35,
        SPELLCATEGORY_DEBUFF_SPELL = 36,
        SPELLCATEGORY_INVULNERABILITY = 37,
        SPELLCATEGORY_AURA = 38,
        SPELLCATEGORY_SHAPESHIFT = 39,
        SPELLCATEGORY_MELEE_GENERIC = 40,
        SPELLCATEGORY_CRITICAL = 41,
        SPELLCATEGORY_SNARE = 42,
        SPELLCATEGORY_SHOUT = 43,
        SPELLCATEGORY_SPEED = 44,
        SPELLCATEGORY_TOTEM_STONECLAW = 45,
        SPELLCATEGORY_HEALING_GROUP_SPELL = 46,
        SPELLCATEGORY_COMBAT_STATES = 47,
        SPELLCATEGORY_DIRECT_DAMAGE_AE_ABILITY = 49,
        SPELLCATEGORY_DIRECT_DAMAGE_AECONE_ABILITY = 50,
        SPELLCATEGORY_QUICK_DEBUFF_DPS_SPELL = 51,
        SPELLCATEGORY_QUICK_DEBUFF_DR_SPELL = 52,
        SPELLCATEGORY_QUICK_BUFF_DR_SPELL = 54,
        SPELLCATEGORY_QUICK_BUFF_DPS_SPELL = 55,
        SPELLCATEGORY_INSTANT_HEAL_SPELL = 56,
        SPELLCATEGORY_QUICK_HEAL_GROUP_SPELL = 57,
        SPELLCATEGORY_INSTANT_HEAL_GROUP_SPELL = 58,
        SPELLCATEGORY_ITEM_DRINK = 59,
        SPELLCATEGORY_INVULNERABILITY_TEMP = 60,
        SPELLCATEGORY_ENERGIZE_GROUP_SPELL = 61,
        SPELLCATEGORY_ENERGIZE_SPELL = 62,
        SPELLCATEGORY_BIG_DIRECT_DAMAGE_SPELL = 63,
        SPELLCATEGORY_MINIMAP_SPECIAL = 64,
        SPELLCATEGORY_MELEE_SPECIAL = 65,
        SPELLCATEGORY_DODGE_MANEUVER = 66,
        SPELLCATEGORY_BLOCK_MANEUVER = 67,
        SPELLCATEGORY_PARRY_MANEUVER = 68,
        SPELLCATEGORY_DIRECT_DAMAGE_AEPERSISTENT_SPELL = 72,
        SPELLCATEGORY_MARTIAL_ARTS_GENERIC = 73,
        SPELLCATEGORY_MARTIAL_ARTS_SPECIAL = 74,
        SPELLCATEGORY_DETECT = 75,
        SPELLCATEGORY_SHOOT_THROW = 76,
        SPELLCATEGORY_TRADE_HERBALISM = 77,
        SPELLCATEGORY_TRADE_MINING = 78,
        SPELLCATEGORY_ITEM_POTION_NONCOMBAT = 79,
        SPELLCATEGORY_TAUNT_DETAUNT = 82,
        SPELLCATEGORY_TAMING = 83,
        SPELLCATEGORY_TAUNT_AE = 84,
        SPELLCATEGORY_DIRECT_DAMAGE_AECHAIN_ABILITY = 85,
        SPELLCATEGORY_PET = 86,
        SPELLCATEGORY_CONJURE_LONG = 87,
        SPELLCATEGORY_SILENCE = 88,
        SPELLCATEGORY_PORTAL = 89,
        SPELLCATEGORY_CHARM = 93,
        SPELLCATEGORY_ITEM_SUMMONING = 94,
        SPELLCATEGORY_RACIAL_ABILITY = 95,
        SPELLCATEGORY_RACIAL_ABILITY_2 = 96,
        SPELLCATEGORY_SECONDARY_SURVIVAL = 97,
        SPELLCATEGORY_BIG_DIRECT_DAMAGE_SPELL_2 = 98,
        SPELLCATEGORY_INSTANT_SPELL = 99,
        SPELLCATEGORY_ITEM_MANA_GEM = 100,
        SPELLCATEGORY_FELHUNTER = 101,
        SPELLCATEGORY_ITEM_LONG_BUFF = 102,
        SPELLCATEGORY_ITEM_EPIC = 103,
        SPELLCATEGORY_TOTEM_HEALING = 104,
        SPELLCATEGORY_TOTEM_SERPENT = 105,
        SPELLCATEGORY_TOTEM_SLOWING = 106,
        SPELLCATEGORY_TOTEM_MANA = 107,
        SPELLCATEGORY_TOTEM_INVISIBILITY = 108,
        SPELLCATEGORY_MELEE_DISARM = 109,
        SPELLCATEGORY_DISCIPLINE = 132,
        SPELLCATEGORY_GLOBAL = 133,
        SPELLCATEGORY_ITEM_BANDAGE = 150,
        SPELLCATEGORY_FINISHING_MOVE_WEAPONSCALED = 170,
        SPELLCATEGORY_BANISH = 190,
        SPELLCATEGORY_TOTEM_GROUNDING = 230,
        SPELLCATEGORY_BLAST_WAVE = 250,
        SPELLCATEGORY_KIDNEY_SHOT = 270,
        SPELLCATEGORY_PYROBLAST_REUSE = 290,
        SPELLCATEGORY_TRANSMUTE_ALCHEMY = 310,
        SPELLCATEGORY_MOUNT = 330,
        SPELLCATEGORY_INNER_RAGE = 350,
        SPELLCATEGORY_RANGED_WEAPON = 351,
        SPELLCATEGORY_LIGHTNING_SHIELD = 371,
        SPELLCATEGORY_QUEST_FELCURSE = 391,
        SPELLCATEGORY_TRAP = 411,
        SPELLCATEGORY_HOLY_NOVA = 431,
        SPELLCATEGORY_HOLY_FIRE = 451,
        SPELLCATEGORY_ICE_BARRIER = 471,
        SPELLCATEGORY_ASTRAL_RECALL = 511,
        SPELLCATEGORY_NATURES_GRASP = 531,
        SPELLCATEGORY_AURA_OF_THE_PIOUS = 551,
        SPELLCATEGORY_HURRICANE = 571,
        SPELLCATEGORY_TOTEM_MANA_TIDE = 591,
        SPELLCATEGORY_WINGS_OF_HOPE = 611,
        SPELLCATEGORY_SOUL_FIRE = 631,
        SPELLCATEGORY_DEATH_COIL = 633,
        SPELLCATEGORY_HOWL_OF_TERROR = 634,
        SPELLCATEGORY_SHADOWBURN = 651,
        SPELLCATEGORY_DESPERATE_PRAYER = 671,
        SPELLCATEGORY_CONFLAGRATE = 672,
        SPELLCATEGORY_DEVOURING_PLAGUE = 691,
        SPELLCATEGORY_SUMMON_INFERNAL = 731,
        SPELLCATEGORY_TREE_FORM = 751,
        SPELLCATEGORY_ITEM_SALT_SHAKER = 791,
        SPELLCATEGORY_DIVINE_INTERVENTION = 811,
        SPELLCATEGORY_SOULSTONE = 831,
        SPELLCATEGORY_RESTORATION = 851,
        SPELLCATEGORY_SHADOWMELD = 871,
        SPELLCATEGORY_INTERCEPT = 872,
        SPELLCATEGORY_WHIRLWIND = 891,
        SPELLCATEGORY_HOLY_SHOCK = 892,
        SPELLCATEGORY_DISTRACTING_SHOT = 911,
        SPELLCATEGORY_HOLY_SHIELD = 931,
        SPELLCATEGORY_CONSECRATION = 932,
        SPELLCATEGORY_PVP_BATTLEFIELD_ITEM_LONG_30_MINS = 951,
        SPELLCATEGORY_MORTAL_STRIKE = 971,
        SPELLCATEGORY_ITEM_SNOWMASTER = 991,
        SPELLCATEGORY_ARCANE_SURGE = 1010,
        SPELLCATEGORY_FRENZIED_HEALING = 1011,
        SPELLCATEGORY_ICICLES = 1012,
        SPELLCATEGORY_ARCANE_RUPTURE = 1013,
        SPELLCATEGORY_DARK_HARVEST = 1014,
        SPELLCATEGORY_STARSHARDS = 1015,
        SPELLCATEGORY_ITEM_HALF_HOUR = 1031,
        SPELLCATEGORY_ITEM_JUMPER_CABLES = 1051,
        SPELLCATEGORY_ITEM_HATCH_JUBLING = 1071,
        SPELLCATEGORY_BATTLEGROUNDS_RECALL = 1091,
        SPELLCATEGORY_WYVERN_STING = 1111,
        SPELLCATEGORY_HAMMER_OF_VENGEANCE = 1131,
        SPELLCATEGORY_INTIMIDATION = 1132,
        SPELLCATEGORY_FAERIE_FIRE_FERAL = 1133,
        SPELLCATEGORY_RIPOSTE = 1134,
        SPELLCATEGORY_COUNTERATTACK = 1135,
        SPELLCATEGORY_HOLIDAY_FIREWORK_ROCKETS = 1136,
        SPELLCATEGORY_HOLIDAY_FIRECRACKER = 1137,
        SPELLCATEGORY_RC_WEAPONS = 1138,
        SPELLCATEGORY_ITEM_QUEST_10_MINUTES = 1139,
        SPELLCATEGORY_ITEM_QUEST_1_MIN = 1140,
        SPELLCATEGORY_ITEM_BURST_TRINKET = 1141,
        SPELLCATEGORY_HOLIDAY_VALENTINE_PERFUME_COLOGNE = 1142,
        SPELLCATEGORY_ITEM_TARGET_DUMMY = 1143,
        SPELLCATEGORY_PRIEST_RACIAL = 1144,
        SPELLCATEGORY_LIGHTWELL = 1145,
        SPELLCATEGORY_QUEST_1_HOUR = 1149,
        SPELLCATEGORY_SHADOWTHUNDERSTRIKE = 1150,
        SPELLCATEGORY_TALENT_DPS = 1151,
        SPELLCATEGORY_CREATURE_SPECIAL = 1152,
        SPELLCATEGORY_ITEM_COMBAT_CONSUMABLE_NONAGGRESSIVE = 1153,
        SPELLCATEGORY_ARATHI_BASIN_TRINKET = 1155,
        SPELLCATEGORY_CREATURE_SPECIAL_2 = 1159,
        SPELLCATEGORY_ITEM_PRIEST_EPIC_STAFF = 1160,
        SPELLCATEGORY_REINCARNATION = 1161,
        SPELLCATEGORY_CHASTISE = 1162,
        SPELLCATEGORY_VIPER_STING = 1163,
        SPELLCATEGORY_WING_CLIP = 1164,
        SPELLCATEGORY_EXECUTE = 1165,
        SPELLCATEGORY_HAMSTRING = 1166,
        SPELLCATEGORY_WARLOCK_STONE = 1167,
        SPELLCATEGORY_BULWARK_OF_THE_RIGHTEOUS = 1168,
        SPELLCATEGORY_LIGHTNING_STRIKE = 1169,
        SPELLCATEGORY_REPENTANCE = 1170,
        SPELLCATEGORY_BLOODLUST = 1171,
    };

    // Class to hold all the spell data
    public class SpellData
    {
        public uint Id { get; set; }
        public uint School { get; set; }
        public uint Category { get; set; }
        public uint Dispel { get; set; }
        public uint Mechanic { get; set; }
        public uint Attributes { get; set; }
        public uint AttributesEx { get; set; }
        public uint AttributesEx2 { get; set; }
        public uint AttributesEx3 { get; set; }
        public uint AttributesEx4 { get; set; }
        public uint Stances { get; set; }
        public uint StancesNot { get; set; }
        public uint Targets { get; set; }
        public uint TargetCreatureType { get; set; }
        public uint RequiresSpellFocus { get; set; }
        public uint CasterAuraState { get; set; }
        public uint TargetAuraState { get; set; }
        public uint CastingTimeIndex { get; set; }
        public uint RecoveryTime { get; set; }
        public uint CategoryRecoveryTime { get; set; }
        public uint InterruptFlags { get; set; }
        public uint AuraInterruptFlags { get; set; }
        public uint ChannelInterruptFlags { get; set; }
        public uint procFlags { get; set; }
        public uint procChance { get; set; }
        public uint procCharges { get; set; }
        public uint maxLevel { get; set; }
        public uint baseLevel { get; set; }
        public uint spellLevel { get; set; }
        public uint DurationIndex { get; set; }
        public uint powerType { get; set; }
        public uint manaCost { get; set; }
        public uint manaCostPerlevel { get; set; }
        public uint manaPerSecond { get; set; }
        public uint manaPerSecondPerLevel { get; set; }
        public uint rangeIndex { get; set; }
        public float speed { get; set; }
        public uint StackAmount { get; set; }
        public uint[] Totem { get; set; } = new uint[2];
        public int[] Reagent { get; set; } = new int[8];
        public uint[] ReagentCount { get; set; } = new uint[8];
        public int EquippedItemClass { get; set; } = -1;
        public int EquippedItemSubClassMask { get; set; }
        public int EquippedItemInventoryTypeMask { get; set; }
        public uint[] Effect { get; set; } = new uint[3];
        public int[] EffectDieSides { get; set; } = new int[3];
        public uint[] EffectBaseDice { get; set; } = new uint[3];
        public float[] EffectDicePerLevel { get; set; } = new float[3];
        public float[] EffectRealPointsPerLevel { get; set; } = new float[3];
        public int[] EffectBasePoints { get; set; } = new int[3];
        public float[] EffectBonusCoefficient { get; set; } = new float[3];
        public uint[] EffectMechanic { get; set; } = new uint[3];
        public uint[] EffectImplicitTargetA { get; set; } = new uint[3];
        public uint[] EffectImplicitTargetB { get; set; } = new uint[3];
        public uint[] EffectRadiusIndex { get; set; } = new uint[3];
        public uint[] EffectApplyAuraName { get; set; } = new uint[3];
        public uint[] EffectAmplitude { get; set; } = new uint[3];
        public float[] EffectMultipleValue { get; set; } = new float[3];
        public uint[] EffectChainTarget { get; set; } = new uint[3];
        public ulong[] EffectItemType { get; set; } = new ulong[3];
        public int[] EffectMiscValue { get; set; } = new int[3];
        public uint[] EffectTriggerSpell { get; set; } = new uint[3];
        public float[] EffectPointsPerComboPoint { get; set; } = new float[3];
        public uint SpellVisual { get; set; }
        public uint SpellIconID { get; set; }
        public uint activeIconID { get; set; }
        public uint spellPriority { get; set; }
        public string SpellName { get; set; } = string.Empty;
        public uint nameFlags { get; set; } = 4128894; // Default value commonly used
        public string nameSubtext { get; set; } = string.Empty;
        public uint nameSubtextFlags { get; set; } = 4128894; // Default value commonly used
        public string description { get; set; } = string.Empty;
        public uint descriptionFlags { get; set; } = 4128894; // Default value commonly used
        public string auraDescription { get; set; } = string.Empty;
        public uint auraDescriptionFlags { get; set; } = 4128894; // Default value commonly used
        public string[] ToolTip { get; set; } = new string[8]; // Only index 0 is used in this application
        public uint ManaCostPercentage { get; set; }
        public uint StartRecoveryCategory { get; set; }
        public uint StartRecoveryTime { get; set; }
        public uint MinTargetLevel { get; set; }
        public uint MaxTargetLevel { get; set; }
        public uint SpellFamilyName { get; set; }
        public ulong SpellFamilyFlags { get; set; }
        public uint MaxAffectedTargets { get; set; }
        public uint DmgClass { get; set; }
        public uint PreventionType { get; set; }
        public int stanceBarOrder { get; set; }
        public float[] DmgMultiplier { get; set; } = new float[3];
        public uint MinFactionId { get; set; }
        public uint MinReputation { get; set; }
        public uint RequiredAuraVision { get; set; }
        public uint Custom { get; set; }

        // Constructor with default values
        public SpellData()
        {
            // Set default values
            Id = GetNextAvailableId();
            SpellName = "New Spell";
            School = (uint)SpellSchool.SPELL_SCHOOL_NORMAL;
            rangeIndex = 1; // Default range index is 1 (self)
            EquippedItemClass = -1; // Default is -1 (no requirement)
            ToolTip[0] = ""; // Initialize ToolTip array

            // Initialize arrays with default values
            for (int i = 0; i < 3; i++)
            {
                Effect[i] = 0;
                EffectApplyAuraName[i] = 0;
                EffectBasePoints[i] = 0;
                EffectDieSides[i] = 0;
                EffectBaseDice[i] = 0;
                EffectRealPointsPerLevel[i] = 0;
                EffectDicePerLevel[i] = 0;
                EffectMechanic[i] = 0;
                EffectImplicitTargetA[i] = 0;
                EffectImplicitTargetB[i] = 0;
                EffectRadiusIndex[i] = 0;
                EffectAmplitude[i] = 0;
                EffectMultipleValue[i] = 0;
                EffectChainTarget[i] = 0;
                EffectItemType[i] = 0;
                EffectMiscValue[i] = 0;
                EffectTriggerSpell[i] = 0;
                EffectPointsPerComboPoint[i] = 0;
                EffectBonusCoefficient[i] = 0;
                DmgMultiplier[i] = 1; // Default damage multiplier is 1.0
            }
        }

        // Generate a unique ID for a new spell
        private uint GetNextAvailableId()
        {
            // In a real application, this might check a database for the next available ID
            // For now, we'll just return a placeholder value that is unlikely to conflict
            Random random = new Random();
            return (uint)(60000 + random.Next(1, 5000)); // Avoid conflicts with existing spell IDs
        }

        // Convert to JSON for saving
        public string ToJson()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IgnoreNullValues = true
                };

                return JsonSerializer.Serialize(this, options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error serializing spell data: {ex.Message}");
            }
        }

        // Create from JSON for loading
        public static SpellData FromJson(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                };

                SpellData spellData = JsonSerializer.Deserialize<SpellData>(json, options);

                // Verify arrays are properly initialized (in case the JSON was missing some)
                if (spellData.Totem == null) spellData.Totem = new uint[2];
                if (spellData.Reagent == null) spellData.Reagent = new int[8];
                if (spellData.ReagentCount == null) spellData.ReagentCount = new uint[8];
                if (spellData.Effect == null) spellData.Effect = new uint[3];
                if (spellData.EffectDieSides == null) spellData.EffectDieSides = new int[3];
                if (spellData.EffectBaseDice == null) spellData.EffectBaseDice = new uint[3];
                if (spellData.EffectDicePerLevel == null) spellData.EffectDicePerLevel = new float[3];
                if (spellData.EffectRealPointsPerLevel == null) spellData.EffectRealPointsPerLevel = new float[3];
                if (spellData.EffectBasePoints == null) spellData.EffectBasePoints = new int[3];
                if (spellData.EffectBonusCoefficient == null) spellData.EffectBonusCoefficient = new float[3];
                if (spellData.EffectMechanic == null) spellData.EffectMechanic = new uint[3];
                if (spellData.EffectImplicitTargetA == null) spellData.EffectImplicitTargetA = new uint[3];
                if (spellData.EffectImplicitTargetB == null) spellData.EffectImplicitTargetB = new uint[3];
                if (spellData.EffectRadiusIndex == null) spellData.EffectRadiusIndex = new uint[3];
                if (spellData.EffectApplyAuraName == null) spellData.EffectApplyAuraName = new uint[3];
                if (spellData.EffectAmplitude == null) spellData.EffectAmplitude = new uint[3];
                if (spellData.EffectMultipleValue == null) spellData.EffectMultipleValue = new float[3];
                if (spellData.EffectChainTarget == null) spellData.EffectChainTarget = new uint[3];
                if (spellData.EffectItemType == null) spellData.EffectItemType = new ulong[3];
                if (spellData.EffectMiscValue == null) spellData.EffectMiscValue = new int[3];
                if (spellData.EffectTriggerSpell == null) spellData.EffectTriggerSpell = new uint[3];
                if (spellData.EffectPointsPerComboPoint == null) spellData.EffectPointsPerComboPoint = new float[3];
                if (spellData.DmgMultiplier == null) spellData.DmgMultiplier = new float[3];
                if (spellData.ToolTip == null) spellData.ToolTip = new string[8];

                return spellData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deserializing spell data: {ex.Message}");
            }
        }
    }
}
