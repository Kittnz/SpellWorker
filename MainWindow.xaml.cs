using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Globalization;

namespace SpellWorker
{
    // Class to hold reagent data for the DataGrid
    public class ReagentItem
    {
        public int Index { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
    }

    public partial class MainWindow : Window
    {
        private SpellData currentSpell;
        private ObservableCollection<ReagentItem> reagentItems;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the reagents grid first
            reagentItems = new ObservableCollection<ReagentItem>();
            for (int i = 0; i < 8; i++)
            {
                reagentItems.Add(new ReagentItem { Index = i });
            }
            dgReagents.ItemsSource = reagentItems;

            // Initialize enums and set default selections
            InitializeEnums();

            // Defer loading the spell data until after initialization is complete
            this.Loaded += (s, e) =>
            {
                NewSpell();
                txtStatus.Text = "Ready";
            };
        }

        private void InitializeEnums()
        {
            // Initialize School dropdown
            cmbSchool.ItemsSource = Enum.GetValues(typeof(SpellSchool))
                .Cast<SpellSchool>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbSchool.DisplayMemberPath = "Name";
            cmbSchool.SelectedValuePath = "Value";
            cmbSchool.SelectedIndex = 0;

            // Initialize Category dropdown
            cmbCategory.ItemsSource = Enum.GetValues(typeof(SpellCategories))
                .Cast<SpellCategories>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbCategory.DisplayMemberPath = "Name";
            cmbCategory.SelectedValuePath = "Value";
            cmbCategory.SelectedIndex = 0;

            // Initialize Dispel dropdown
            cmbDispel.ItemsSource = Enum.GetValues(typeof(DispelType))
                .Cast<DispelType>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbDispel.DisplayMemberPath = "Name";
            cmbDispel.SelectedValuePath = "Value";
            cmbDispel.SelectedIndex = 0;

            // Initialize Mechanic dropdown
            cmbMechanic.ItemsSource = Enum.GetValues(typeof(Mechanics))
                .Cast<Mechanics>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbMechanic.DisplayMemberPath = "Name";
            cmbMechanic.SelectedValuePath = "Value";
            cmbMechanic.SelectedIndex = 0;

            // Initialize Targets dropdown
            cmbTargets.ItemsSource = Enum.GetValues(typeof(SpellTarget))
                .Cast<SpellTarget>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbTargets.DisplayMemberPath = "Name";
            cmbTargets.SelectedValuePath = "Value";
            cmbTargets.SelectedIndex = 0;

            // Initialize TargetCreatureType dropdown
            cmbTargetCreatureType.ItemsSource = Enum.GetValues(typeof(CreatureType))
                .Cast<CreatureType>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbTargetCreatureType.DisplayMemberPath = "Name";
            cmbTargetCreatureType.SelectedValuePath = "Value";
            cmbTargetCreatureType.SelectedIndex = 0;

            // Initialize InterruptFlags dropdown (simplified for example)
            cmbInterruptFlags.ItemsSource = new List<dynamic>
            {
                new { Value = 0, Name = "NONE" },
                new { Value = 1, Name = "SPELL_INTERRUPT_FLAG_MOVEMENT" },
                new { Value = 2, Name = "SPELL_INTERRUPT_FLAG_DAMAGE" },
                new { Value = 4, Name = "SPELL_INTERRUPT_FLAG_INTERRUPT" },
                new { Value = 8, Name = "SPELL_INTERRUPT_FLAG_AUTOATTACK" },
                new { Value = 16, Name = "SPELL_INTERRUPT_FLAG_ABORT_ON_DMG" }
            };
            cmbInterruptFlags.DisplayMemberPath = "Name";
            cmbInterruptFlags.SelectedValuePath = "Value";
            cmbInterruptFlags.SelectedIndex = 0;

            // Initialize AuraInterruptFlags dropdown (simplified)
            cmbAuraInterruptFlags.ItemsSource = new List<dynamic>
            {
                new { Value = 0, Name = "NONE" },
                new { Value = 1, Name = "AURA_INTERRUPT_FLAG_HITBYSPELL" },
                new { Value = 2, Name = "AURA_INTERRUPT_FLAG_DAMAGE" },
                new { Value = 4, Name = "AURA_INTERRUPT_FLAG_CAST" },
                new { Value = 8, Name = "AURA_INTERRUPT_FLAG_MOVE" }
            };
            cmbAuraInterruptFlags.DisplayMemberPath = "Name";
            cmbAuraInterruptFlags.SelectedValuePath = "Value";
            cmbAuraInterruptFlags.SelectedIndex = 0;

            // Initialize ChannelInterruptFlags dropdown (simplified)
            cmbChannelInterruptFlags.ItemsSource = new List<dynamic>
            {
                new { Value = 0, Name = "NONE" },
                new { Value = 1, Name = "CHANNEL_FLAG_ENTER_COMBAT" },
                new { Value = 2, Name = "CHANNEL_FLAG_DAMAGE" },
                new { Value = 4, Name = "CHANNEL_FLAG_INTERRUPT" },
                new { Value = 8, Name = "CHANNEL_FLAG_MOVEMENT" }
            };
            cmbChannelInterruptFlags.DisplayMemberPath = "Name";
            cmbChannelInterruptFlags.SelectedValuePath = "Value";
            cmbChannelInterruptFlags.SelectedIndex = 0;

            // Initialize ProcFlags dropdown (simplified)
            cmbProcFlags.ItemsSource = new List<dynamic>
            {
                new { Value = 0, Name = "NONE" },
                new { Value = 1, Name = "PROC_FLAG_HEARTBEAT" },
                new { Value = 2, Name = "PROC_FLAG_KILL" },
                new { Value = 4, Name = "PROC_FLAG_DEAL_MELEE_SWING" },
                new { Value = 8, Name = "PROC_FLAG_TAKE_MELEE_SWING" }
            };
            cmbProcFlags.DisplayMemberPath = "Name";
            cmbProcFlags.SelectedValuePath = "Value";
            cmbProcFlags.SelectedIndex = 0;

            // Initialize PowerType dropdown
            cmbPowerType.ItemsSource = Enum.GetValues(typeof(Powers))
                .Cast<Powers>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbPowerType.DisplayMemberPath = "Name";
            cmbPowerType.SelectedValuePath = "Value";
            cmbPowerType.SelectedIndex = 0;

            // Initialize SpellFamilyName dropdown
            cmbSpellFamilyName.ItemsSource = Enum.GetValues(typeof(SpellFamily))
                .Cast<SpellFamily>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbSpellFamilyName.DisplayMemberPath = "Name";
            cmbSpellFamilyName.SelectedValuePath = "Value";
            cmbSpellFamilyName.SelectedIndex = 0;

            // Initialize EquippedItemClass dropdown
            cmbEquippedItemClass.ItemsSource = Enum.GetValues(typeof(ItemClass))
                .Cast<ItemClass>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbEquippedItemClass.DisplayMemberPath = "Name";
            cmbEquippedItemClass.SelectedValuePath = "Value";
            cmbEquippedItemClass.SelectedIndex = 0;

            // Initialize Effect dropdowns for each effect tab
            InitializeEffectControls(0);
            InitializeEffectControls(1);
            InitializeEffectControls(2);
        }

        private void InitializeEffectControls(int effectIndex)
        {
            // Get the UI elements for this effect (0-based index)
            ComboBox cmbEffect = FindName($"cmbEffect{effectIndex}") as ComboBox;
            ComboBox cmbAura = FindName($"cmbAura{effectIndex}") as ComboBox;
            ComboBox cmbEffectMechanic = FindName($"cmbEffectMechanic{effectIndex}") as ComboBox;
            ComboBox cmbEffectImplicitTargetA = FindName($"cmbEffectImplicitTargetA{effectIndex}") as ComboBox;
            ComboBox cmbEffectImplicitTargetB = FindName($"cmbEffectImplicitTargetB{effectIndex}") as ComboBox;

            // Initialize Effect dropdown
            cmbEffect.ItemsSource = Enum.GetValues(typeof(SpellEffects))
                .Cast<SpellEffects>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbEffect.DisplayMemberPath = "Name";
            cmbEffect.SelectedValuePath = "Value";
            cmbEffect.SelectedIndex = 0;

            // Initialize Aura dropdown
            cmbAura.ItemsSource = Enum.GetValues(typeof(AuraType))
                .Cast<AuraType>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbAura.DisplayMemberPath = "Name";
            cmbAura.SelectedValuePath = "Value";
            cmbAura.SelectedIndex = 0;

            // Initialize EffectMechanic dropdown
            cmbEffectMechanic.ItemsSource = Enum.GetValues(typeof(Mechanics))
                .Cast<Mechanics>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();
            cmbEffectMechanic.DisplayMemberPath = "Name";
            cmbEffectMechanic.SelectedValuePath = "Value";
            cmbEffectMechanic.SelectedIndex = 0;

            // Initialize Target A & B dropdowns
            var targetValues = Enum.GetValues(typeof(SpellTarget))
                .Cast<SpellTarget>()
                .Select(e => new { Value = (int)e, Name = e.ToString() })
                .ToList();

            cmbEffectImplicitTargetA.ItemsSource = targetValues;
            cmbEffectImplicitTargetA.DisplayMemberPath = "Name";
            cmbEffectImplicitTargetA.SelectedValuePath = "Value";
            cmbEffectImplicitTargetA.SelectedIndex = 0;

            cmbEffectImplicitTargetB.ItemsSource = targetValues;
            cmbEffectImplicitTargetB.DisplayMemberPath = "Name";
            cmbEffectImplicitTargetB.SelectedValuePath = "Value";
            cmbEffectImplicitTargetB.SelectedIndex = 0;

            // Add event handler for Effect combobox to update the UI when selected effect changes
            cmbEffect.SelectionChanged += (s, e) => UpdateEffectControls(effectIndex);
        }

        private void UpdateEffectControls(int effectIndex)
        {
            ComboBox cmbEffect = FindName($"cmbEffect{effectIndex}") as ComboBox;
            ComboBox cmbAura = FindName($"cmbAura{effectIndex}") as ComboBox;

            if (cmbEffect.SelectedValue == null)
                return;

            var selectedEffect = (int)cmbEffect.SelectedValue;

            // Enable or disable the Aura dropdown based on whether the selected effect is SPELL_EFFECT_APPLY_AURA
            if (selectedEffect == (int)SpellEffects.SPELL_EFFECT_APPLY_AURA)
            {
                cmbAura.IsEnabled = true;
            }
            else
            {
                cmbAura.IsEnabled = false;
                cmbAura.SelectedIndex = 0; // Set to "SPELL_AURA_NONE"
            }

            // Similar logic could be implemented for other controls based on the effect
        }

        private void NewSpell_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Create a new spell? Unsaved changes will be lost.", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                NewSpell();
            }
        }

        private void NewSpell()
        {
            currentSpell = new SpellData();
            LoadSpellDataToUI();
            txtStatus.Text = "New spell created";
        }

        private void LoadSpell_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Spell Files (*.spell)|*.spell|All Files (*.*)|*.*",
                Title = "Load Spell Data"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    string json = File.ReadAllText(dialog.FileName);
                    currentSpell = SpellData.FromJson(json);
                    LoadSpellDataToUI();
                    txtStatus.Text = $"Loaded spell ID {currentSpell.Id} from {System.IO.Path.GetFileName(dialog.FileName)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading spell: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtStatus.Text = "Error loading spell";
                }
            }
        }

        private void SaveSpell_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Spell Files (*.spell)|*.spell|All Files (*.*)|*.*",
                Title = "Save Spell Data",
                DefaultExt = ".spell",
                FileName = $"spell_{currentSpell.Id}.spell"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    SaveUIDataToSpell();
                    string json = currentSpell.ToJson();
                    File.WriteAllText(dialog.FileName, json);
                    MessageBox.Show("Spell saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtStatus.Text = $"Saved spell ID {currentSpell.Id} to {System.IO.Path.GetFileName(dialog.FileName)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving spell: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtStatus.Text = "Error saving spell";
                }
            }
        }

        private void GenerateSQL_Click(object sender, RoutedEventArgs e)
        {
            SaveUIDataToSpell();
            string sql = GenerateSqlStatement();
            txtSqlPreview.Text = sql;
            txtStatus.Text = "SQL generated";
        }

        private void CopySql_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSqlPreview.Text))
            {
                Clipboard.SetText(txtSqlPreview.Text);
                MessageBox.Show("SQL copied to clipboard.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                txtStatus.Text = "SQL copied to clipboard";
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Exit the application? Unsaved changes will be lost.", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Spell Worker \n\nA tool for creating and editing spells.\n\nVersion 1.0.0", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadSpellDataToUI()
        {
            // Load basic properties
            txtSpellId.Text = currentSpell.Id.ToString();
            txtSpellName.Text = currentSpell.SpellName;
            cmbSchool.SelectedValue = (int)currentSpell.School;
            cmbCategory.SelectedValue = (int)currentSpell.Category;
            cmbDispel.SelectedValue = (int)currentSpell.Dispel;
            cmbMechanic.SelectedValue = (int)currentSpell.Mechanic;

            // Load attributes (with bit flags)
            LoadAttributeCheckboxes(currentSpell.Attributes, "chkAttribute");

            // Load other properties
            cmbTargets.SelectedValue = (int)currentSpell.Targets;
            cmbTargetCreatureType.SelectedValue = (int)currentSpell.TargetCreatureType;
            txtCastingTimeIndex.Text = currentSpell.CastingTimeIndex.ToString();
            txtRecoveryTime.Text = currentSpell.RecoveryTime.ToString();
            txtCategoryRecoveryTime.Text = currentSpell.CategoryRecoveryTime.ToString();
            cmbInterruptFlags.SelectedValue = (int)currentSpell.InterruptFlags;
            cmbAuraInterruptFlags.SelectedValue = (int)currentSpell.AuraInterruptFlags;
            cmbChannelInterruptFlags.SelectedValue = (int)currentSpell.ChannelInterruptFlags;
            cmbProcFlags.SelectedValue = (int)currentSpell.procFlags;
            txtProcChance.Text = currentSpell.procChance.ToString();
            txtProcCharges.Text = currentSpell.procCharges.ToString();
            txtMaxLevel.Text = currentSpell.maxLevel.ToString();
            txtBaseLevel.Text = currentSpell.baseLevel.ToString();
            txtSpellLevel.Text = currentSpell.spellLevel.ToString();
            txtDurationIndex.Text = currentSpell.DurationIndex.ToString();
            cmbPowerType.SelectedValue = (int)currentSpell.powerType;
            txtManaCost.Text = currentSpell.manaCost.ToString();
            txtManaCostPerLevel.Text = currentSpell.manaCostPerlevel.ToString();
            txtManaPerSecond.Text = currentSpell.manaPerSecond.ToString();
            txtRangeIndex.Text = currentSpell.rangeIndex.ToString();
            txtSpeed.Text = currentSpell.speed.ToString();
            txtStackAmount.Text = currentSpell.StackAmount.ToString();

            // Load effect data for each effect
            LoadEffectData(0, currentSpell.Effect[0], currentSpell.EffectApplyAuraName[0],
                currentSpell.EffectBasePoints[0], currentSpell.EffectDieSides[0],
                currentSpell.EffectBaseDice[0], currentSpell.EffectRealPointsPerLevel[0],
                currentSpell.EffectDicePerLevel[0], currentSpell.EffectMechanic[0],
                currentSpell.EffectImplicitTargetA[0], currentSpell.EffectImplicitTargetB[0],
                currentSpell.EffectRadiusIndex[0], currentSpell.EffectAmplitude[0],
                currentSpell.EffectMultipleValue[0], currentSpell.EffectChainTarget[0],
                currentSpell.EffectItemType[0], currentSpell.EffectMiscValue[0],
                currentSpell.EffectTriggerSpell[0], currentSpell.EffectPointsPerComboPoint[0],
                currentSpell.DmgMultiplier[0], currentSpell.EffectBonusCoefficient[0]);

            LoadEffectData(1, currentSpell.Effect[1], currentSpell.EffectApplyAuraName[1],
                currentSpell.EffectBasePoints[1], currentSpell.EffectDieSides[1],
                currentSpell.EffectBaseDice[1], currentSpell.EffectRealPointsPerLevel[1],
                currentSpell.EffectDicePerLevel[1], currentSpell.EffectMechanic[1],
                currentSpell.EffectImplicitTargetA[1], currentSpell.EffectImplicitTargetB[1],
                currentSpell.EffectRadiusIndex[1], currentSpell.EffectAmplitude[1],
                currentSpell.EffectMultipleValue[1], currentSpell.EffectChainTarget[1],
                currentSpell.EffectItemType[1], currentSpell.EffectMiscValue[1],
                currentSpell.EffectTriggerSpell[1], currentSpell.EffectPointsPerComboPoint[1],
                currentSpell.DmgMultiplier[1], currentSpell.EffectBonusCoefficient[1]);

            LoadEffectData(2, currentSpell.Effect[2], currentSpell.EffectApplyAuraName[2],
                currentSpell.EffectBasePoints[2], currentSpell.EffectDieSides[2],
                currentSpell.EffectBaseDice[2], currentSpell.EffectRealPointsPerLevel[2],
                currentSpell.EffectDicePerLevel[2], currentSpell.EffectMechanic[2],
                currentSpell.EffectImplicitTargetA[2], currentSpell.EffectImplicitTargetB[2],
                currentSpell.EffectRadiusIndex[2], currentSpell.EffectAmplitude[2],
                currentSpell.EffectMultipleValue[2], currentSpell.EffectChainTarget[2],
                currentSpell.EffectItemType[2], currentSpell.EffectMiscValue[2],
                currentSpell.EffectTriggerSpell[2], currentSpell.EffectPointsPerComboPoint[2],
                currentSpell.DmgMultiplier[2], currentSpell.EffectBonusCoefficient[2]);

            // Load reagent data
            for (int i = 0; i < 8; i++)
            {
                reagentItems[i].ItemId = currentSpell.Reagent[i];
                reagentItems[i].Count = (int)currentSpell.ReagentCount[i];
                // Update item name based on ID
                reagentItems[i].Name = LookupItemName(currentSpell.Reagent[i]);
            }

            // Load totem data
            txtTotem1.Text = currentSpell.Totem[0].ToString();
            txtTotem2.Text = currentSpell.Totem[1].ToString();

            // Load equipment requirements
            cmbEquippedItemClass.SelectedValue = currentSpell.EquippedItemClass;
            txtEquippedItemSubClassMask.Text = currentSpell.EquippedItemSubClassMask.ToString();
            txtEquippedItemInventoryTypeMask.Text = currentSpell.EquippedItemInventoryTypeMask.ToString();

            // Load additional properties
            txtSpellVisual.Text = currentSpell.SpellVisual.ToString();
            txtSpellIconID.Text = currentSpell.SpellIconID.ToString();
            txtActiveIconID.Text = currentSpell.activeIconID.ToString();
            txtSpellPriority.Text = currentSpell.spellPriority.ToString();
            txtToolTip.Text = currentSpell.ToolTip[0];
            txtDescription.Text = currentSpell.description;
            txtAuraDescription.Text = currentSpell.auraDescription;
            cmbSpellFamilyName.SelectedValue = (int)currentSpell.SpellFamilyName;
            txtSpellFamilyFlags.Text = currentSpell.SpellFamilyFlags.ToString();
            txtMaxAffectedTargets.Text = currentSpell.MaxAffectedTargets.ToString();
            txtCustom.Text = currentSpell.Custom.ToString();

            // Update the SQL preview
            GenerateSQL_Click(null, null);
        }

        private void LoadAttributeCheckboxes(uint attributes, string checkboxPrefix)
        {
            for (int i = 0; i < 32; i++)
            {
                CheckBox chk = FindName($"{checkboxPrefix}{i + 1}") as CheckBox;
                if (chk != null)
                {
                    chk.IsChecked = (attributes & (1u << i)) != 0;
                }
            }
        }

        private void LoadEffectData(int index, uint effect, uint aura, int basePoints, int dieSides,
            uint baseDice, float realPointsPerLevel, float dicePerLevel, uint mechanic,
            uint targetA, uint targetB, uint radiusIndex, uint amplitude,
            float multipleValue, uint chainTarget, ulong itemType, int miscValue,
            uint triggerSpell, float pointsPerComboPoint, float damageMultiplier,
            float bonusCoefficient)
        {
            ComboBox cmbEffect = FindName($"cmbEffect{index}") as ComboBox;
            ComboBox cmbAura = FindName($"cmbAura{index}") as ComboBox;
            TextBox txtBasePoints = FindName($"txtEffectBasePoints{index}") as TextBox;
            TextBox txtDieSides = FindName($"txtEffectDieSides{index}") as TextBox;
            TextBox txtBaseDice = FindName($"txtEffectBaseDice{index}") as TextBox;
            TextBox txtRealPointsPerLevel = FindName($"txtEffectRealPointsPerLevel{index}") as TextBox;
            TextBox txtDicePerLevel = FindName($"txtEffectDicePerLevel{index}") as TextBox;
            ComboBox cmbMechanic = FindName($"cmbEffectMechanic{index}") as ComboBox;
            ComboBox cmbTargetA = FindName($"cmbEffectImplicitTargetA{index}") as ComboBox;
            ComboBox cmbTargetB = FindName($"cmbEffectImplicitTargetB{index}") as ComboBox;
            TextBox txtRadiusIndex = FindName($"txtEffectRadiusIndex{index}") as TextBox;
            TextBox txtAmplitude = FindName($"txtEffectAmplitude{index}") as TextBox;
            TextBox txtMultipleValue = FindName($"txtEffectMultipleValue{index}") as TextBox;
            TextBox txtChainTarget = FindName($"txtEffectChainTarget{index}") as TextBox;
            TextBox txtItemType = FindName($"txtEffectItemType{index}") as TextBox;
            TextBox txtMiscValue = FindName($"txtEffectMiscValue{index}") as TextBox;
            TextBox txtTriggerSpell = FindName($"txtEffectTriggerSpell{index}") as TextBox;
            TextBox txtPointsPerComboPoint = FindName($"txtEffectPointsPerComboPoint{index}") as TextBox;
            TextBox txtDmgMultiplier = FindName($"txtDmgMultiplier{index}") as TextBox;
            TextBox txtBonusCoefficient = FindName($"txtEffectBonusCoefficient{index}") as TextBox;

            cmbEffect.SelectedValue = (int)effect;
            cmbAura.SelectedValue = (int)aura;
            txtBasePoints.Text = basePoints.ToString();
            txtDieSides.Text = dieSides.ToString();
            txtBaseDice.Text = baseDice.ToString();
            txtRealPointsPerLevel.Text = realPointsPerLevel.ToString(CultureInfo.InvariantCulture);
            txtDicePerLevel.Text = dicePerLevel.ToString(CultureInfo.InvariantCulture);
            cmbMechanic.SelectedValue = (int)mechanic;
            cmbTargetA.SelectedValue = (int)targetA;
            cmbTargetB.SelectedValue = (int)targetB;
            txtRadiusIndex.Text = radiusIndex.ToString();
            txtAmplitude.Text = amplitude.ToString();
            txtMultipleValue.Text = multipleValue.ToString(CultureInfo.InvariantCulture);
            txtChainTarget.Text = chainTarget.ToString();
            txtItemType.Text = itemType.ToString();
            txtMiscValue.Text = miscValue.ToString();
            txtTriggerSpell.Text = triggerSpell.ToString();
            txtPointsPerComboPoint.Text = pointsPerComboPoint.ToString(CultureInfo.InvariantCulture);
            txtDmgMultiplier.Text = damageMultiplier.ToString(CultureInfo.InvariantCulture);
            txtBonusCoefficient.Text = bonusCoefficient.ToString(CultureInfo.InvariantCulture);

            // Update dependent controls based on the effect
            UpdateEffectControls(index);
        }

        private void SaveUIDataToSpell()
        {
            // Save basic properties
            currentSpell.Id = ParseUInt(txtSpellId.Text);
            currentSpell.SpellName = txtSpellName.Text;
            currentSpell.School = (uint)GetSelectedValue(cmbSchool);
            currentSpell.Category = (uint)GetSelectedValue(cmbCategory);
            currentSpell.Dispel = (uint)GetSelectedValue(cmbDispel);
            currentSpell.Mechanic = (uint)GetSelectedValue(cmbMechanic);

            // Save attributes (from checkboxes)
            currentSpell.Attributes = SaveAttributeCheckboxes("chkAttribute");

            // Save other properties
            currentSpell.Targets = (uint)GetSelectedValue(cmbTargets);
            currentSpell.TargetCreatureType = (uint)GetSelectedValue(cmbTargetCreatureType);
            currentSpell.CastingTimeIndex = ParseUInt(txtCastingTimeIndex.Text);
            currentSpell.RecoveryTime = ParseUInt(txtRecoveryTime.Text);
            currentSpell.CategoryRecoveryTime = ParseUInt(txtCategoryRecoveryTime.Text);
            currentSpell.InterruptFlags = (uint)GetSelectedValue(cmbInterruptFlags);
            currentSpell.AuraInterruptFlags = (uint)GetSelectedValue(cmbAuraInterruptFlags);
            currentSpell.ChannelInterruptFlags = (uint)GetSelectedValue(cmbChannelInterruptFlags);
            currentSpell.procFlags = (uint)GetSelectedValue(cmbProcFlags);
            currentSpell.procChance = ParseUInt(txtProcChance.Text);
            currentSpell.procCharges = ParseUInt(txtProcCharges.Text);
            currentSpell.maxLevel = ParseUInt(txtMaxLevel.Text);
            currentSpell.baseLevel = ParseUInt(txtBaseLevel.Text);
            currentSpell.spellLevel = ParseUInt(txtSpellLevel.Text);
            currentSpell.DurationIndex = ParseUInt(txtDurationIndex.Text);
            currentSpell.powerType = (uint)GetSelectedValue(cmbPowerType);
            currentSpell.manaCost = ParseUInt(txtManaCost.Text);
            currentSpell.manaCostPerlevel = ParseUInt(txtManaCostPerLevel.Text);
            currentSpell.manaPerSecond = ParseUInt(txtManaPerSecond.Text);
            currentSpell.manaPerSecondPerLevel = 0; // Not included in the UI
            currentSpell.rangeIndex = ParseUInt(txtRangeIndex.Text);
            currentSpell.speed = ParseFloat(txtSpeed.Text);
            currentSpell.StackAmount = ParseUInt(txtStackAmount.Text);

            // Save effect data for each effect
            SaveEffectData(0);
            SaveEffectData(1);
            SaveEffectData(2);

            // Save reagent data
            for (int i = 0; i < 8; i++)
            {
                currentSpell.Reagent[i] = reagentItems[i].ItemId;
                currentSpell.ReagentCount[i] = (uint)reagentItems[i].Count;
            }

            // Save totem data
            currentSpell.Totem[0] = ParseUInt(txtTotem1.Text);
            currentSpell.Totem[1] = ParseUInt(txtTotem2.Text);

            // Save equipment requirements
            currentSpell.EquippedItemClass = (int)GetSelectedValue(cmbEquippedItemClass);
            currentSpell.EquippedItemSubClassMask = ParseInt(txtEquippedItemSubClassMask.Text);
            currentSpell.EquippedItemInventoryTypeMask = ParseInt(txtEquippedItemInventoryTypeMask.Text);

            // Save additional properties
            currentSpell.SpellVisual = ParseUInt(txtSpellVisual.Text);
            currentSpell.SpellIconID = ParseUInt(txtSpellIconID.Text);
            currentSpell.activeIconID = ParseUInt(txtActiveIconID.Text);
            currentSpell.spellPriority = ParseUInt(txtSpellPriority.Text);
            currentSpell.ToolTip[0] = txtToolTip.Text;
            currentSpell.description = txtDescription.Text;
            currentSpell.auraDescription = txtAuraDescription.Text;
            currentSpell.SpellFamilyName = (uint)GetSelectedValue(cmbSpellFamilyName);
            currentSpell.SpellFamilyFlags = ParseULong(txtSpellFamilyFlags.Text);
            currentSpell.MaxAffectedTargets = ParseUInt(txtMaxAffectedTargets.Text);
            currentSpell.Custom = ParseUInt(txtCustom.Text);
        }
        private void SaveEffectData(int index)
        {
            ComboBox cmbEffect = FindName($"cmbEffect{index}") as ComboBox;
            ComboBox cmbAura = FindName($"cmbAura{index}") as ComboBox;
            TextBox txtBasePoints = FindName($"txtEffectBasePoints{index}") as TextBox;
            TextBox txtDieSides = FindName($"txtEffectDieSides{index}") as TextBox;
            TextBox txtBaseDice = FindName($"txtEffectBaseDice{index}") as TextBox;
            TextBox txtRealPointsPerLevel = FindName($"txtEffectRealPointsPerLevel{index}") as TextBox;
            TextBox txtDicePerLevel = FindName($"txtEffectDicePerLevel{index}") as TextBox;
            ComboBox cmbMechanic = FindName($"cmbEffectMechanic{index}") as ComboBox;
            ComboBox cmbTargetA = FindName($"cmbEffectImplicitTargetA{index}") as ComboBox;
            ComboBox cmbTargetB = FindName($"cmbEffectImplicitTargetB{index}") as ComboBox;
            TextBox txtRadiusIndex = FindName($"txtEffectRadiusIndex{index}") as TextBox;
            TextBox txtAmplitude = FindName($"txtEffectAmplitude{index}") as TextBox;
            TextBox txtMultipleValue = FindName($"txtEffectMultipleValue{index}") as TextBox;
            TextBox txtChainTarget = FindName($"txtEffectChainTarget{index}") as TextBox;
            TextBox txtItemType = FindName($"txtEffectItemType{index}") as TextBox;
            TextBox txtMiscValue = FindName($"txtEffectMiscValue{index}") as TextBox;
            TextBox txtTriggerSpell = FindName($"txtEffectTriggerSpell{index}") as TextBox;
            TextBox txtPointsPerComboPoint = FindName($"txtEffectPointsPerComboPoint{index}") as TextBox;
            TextBox txtDmgMultiplier = FindName($"txtDmgMultiplier{index}") as TextBox;
            TextBox txtBonusCoefficient = FindName($"txtEffectBonusCoefficient{index}") as TextBox;

            currentSpell.Effect[index] = (uint)GetSelectedValue(cmbEffect);
            currentSpell.EffectApplyAuraName[index] = (uint)GetSelectedValue(cmbAura);
            currentSpell.EffectBasePoints[index] = ParseInt(txtBasePoints.Text);
            currentSpell.EffectDieSides[index] = ParseInt(txtDieSides.Text);
            currentSpell.EffectBaseDice[index] = ParseUInt(txtBaseDice.Text);
            currentSpell.EffectRealPointsPerLevel[index] = ParseFloat(txtRealPointsPerLevel.Text);
            currentSpell.EffectDicePerLevel[index] = ParseFloat(txtDicePerLevel.Text);
            currentSpell.EffectMechanic[index] = (uint)GetSelectedValue(cmbMechanic);
            currentSpell.EffectImplicitTargetA[index] = (uint)GetSelectedValue(cmbTargetA);
            currentSpell.EffectImplicitTargetB[index] = (uint)GetSelectedValue(cmbTargetB);
            currentSpell.EffectRadiusIndex[index] = ParseUInt(txtRadiusIndex.Text);
            currentSpell.EffectAmplitude[index] = ParseUInt(txtAmplitude.Text);
            currentSpell.EffectMultipleValue[index] = ParseFloat(txtMultipleValue.Text);
            currentSpell.EffectChainTarget[index] = ParseUInt(txtChainTarget.Text);
            currentSpell.EffectItemType[index] = ParseULong(txtItemType.Text);
            currentSpell.EffectMiscValue[index] = ParseInt(txtMiscValue.Text);
            currentSpell.EffectTriggerSpell[index] = ParseUInt(txtTriggerSpell.Text);
            currentSpell.EffectPointsPerComboPoint[index] = ParseFloat(txtPointsPerComboPoint.Text);
            currentSpell.DmgMultiplier[index] = ParseFloat(txtDmgMultiplier.Text);
            currentSpell.EffectBonusCoefficient[index] = ParseFloat(txtBonusCoefficient.Text);
        }

        private string GenerateSqlStatement()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("REPLACE INTO `spell_template` (");

            // Column names
            sql.AppendLine("  `entry`, `school`, `category`, `castUI`, `dispel`, `mechanic`, `attributes`, " +
                           "`attributesEx`, `attributesEx2`, `attributesEx3`, `attributesEx4`, `stances`, " +
                           "`stancesNot`, `targets`, `targetCreatureType`, `requiresSpellFocus`, `casterAuraState`, " +
                           "`targetAuraState`, `castingTimeIndex`, `recoveryTime`, `categoryRecoveryTime`, " +
                           "`interruptFlags`, `auraInterruptFlags`, `channelInterruptFlags`, `procFlags`, " +
                           "`procChance`, `procCharges`, `maxLevel`, `baseLevel`, `spellLevel`, `durationIndex`, " +
                           "`powerType`, `manaCost`, `manCostPerLevel`, `manaPerSecond`, `manaPerSecondPerLevel`, " +
                           "`rangeIndex`, `speed`, `modelNextSpell`, `stackAmount`, `totem1`, `totem2`, `reagent1`, " +
                           "`reagent2`, `reagent3`, `reagent4`, `reagent5`, `reagent6`, `reagent7`, `reagent8`, " +
                           "`reagentCount1`, `reagentCount2`, `reagentCount3`, `reagentCount4`, `reagentCount5`, " +
                           "`reagentCount6`, `reagentCount7`, `reagentCount8`, `equippedItemClass`, " +
                           "`equippedItemSubClassMask`, `equippedItemInventoryTypeMask`, `effect1`, `effect2`, " +
                           "`effect3`, `effectDieSides1`, `effectDieSides2`, `effectDieSides3`, `effectBaseDice1`, " +
                           "`effectBaseDice2`, `effectBaseDice3`, `effectDicePerLevel1`, `effectDicePerLevel2`, " +
                           "`effectDicePerLevel3`, `effectRealPointsPerLevel1`, `effectRealPointsPerLevel2`, " +
                           "`effectRealPointsPerLevel3`, `effectBasePoints1`, `effectBasePoints2`, `effectBasePoints3`, " +
                           "`effectBonusCoefficient1`, `effectBonusCoefficient2`, `effectBonusCoefficient3`, " +
                           "`effectMechanic1`, `effectMechanic2`, `effectMechanic3`, `effectImplicitTargetA1`, " +
                           "`effectImplicitTargetA2`, `effectImplicitTargetA3`, `effectImplicitTargetB1`, " +
                           "`effectImplicitTargetB2`, `effectImplicitTargetB3`, `effectRadiusIndex1`, " +
                           "`effectRadiusIndex2`, `effectRadiusIndex3`, `effectApplyAuraName1`, " +
                           "`effectApplyAuraName2`, `effectApplyAuraName3`, `effectAmplitude1`, " +
                           "`effectAmplitude2`, `effectAmplitude3`, `effectMultipleValue1`, " +
                           "`effectMultipleValue2`, `effectMultipleValue3`, `effectChainTarget1`, " +
                           "`effectChainTarget2`, `effectChainTarget3`, `effectItemType1`, " +
                           "`effectItemType2`, `effectItemType3`, `effectMiscValue1`, " +
                           "`effectMiscValue2`, `effectMiscValue3`, `effectTriggerSpell1`, " +
                           "`effectTriggerSpell2`, `effectTriggerSpell3`, `effectPointsPerComboPoint1`, " +
                           "`effectPointsPerComboPoint2`, `effectPointsPerComboPoint3`, `spellVisual1`, " +
                           "`spellVisual2`, `spellIconId`, `activeIconId`, `spellPriority`, `name`, `nameFlags`, " +
                           "`nameSubtext`, `nameSubtextFlags`, `description`, `descriptionFlags`, `auraDescription`, " +
                           "`auraDescriptionFlags`, `manaCostPercentage`, `startRecoveryCategory`, `startRecoveryTime`, " +
                           "`minTargetLevel`, `maxTargetLevel`, `spellFamilyName`, `spellFamilyFlags`, " +
                           "`maxAffectedTargets`, `dmgClass`, `preventionType`, `stanceBarOrder`, " +
                           "`dmgMultiplier1`, `dmgMultiplier2`, `dmgMultiplier3`, `minFactionId`, " +
                           "`minReputation`, `requiredAuraVision`, `customFlags`) VALUES (");

            // Values
            sql.AppendLine($"  {currentSpell.Id}, {currentSpell.School}, {currentSpell.Category}, 0, {currentSpell.Dispel}, " +
                          $"{currentSpell.Mechanic}, {currentSpell.Attributes}, {currentSpell.AttributesEx}, " +
                          $"{currentSpell.AttributesEx2}, {currentSpell.AttributesEx3}, {currentSpell.AttributesEx4}, " +
                          $"{currentSpell.Stances}, {currentSpell.StancesNot}, {currentSpell.Targets}, " +
                          $"{currentSpell.TargetCreatureType}, {currentSpell.RequiresSpellFocus}, " +
                          $"{currentSpell.CasterAuraState}, {currentSpell.TargetAuraState}, {currentSpell.CastingTimeIndex}, " +
                          $"{currentSpell.RecoveryTime}, {currentSpell.CategoryRecoveryTime}, {currentSpell.InterruptFlags}, " +
                          $"{currentSpell.AuraInterruptFlags}, {currentSpell.ChannelInterruptFlags}, {currentSpell.procFlags}, " +
                          $"{currentSpell.procChance}, {currentSpell.procCharges}, {currentSpell.maxLevel}, " +
                          $"{currentSpell.baseLevel}, {currentSpell.spellLevel}, {currentSpell.DurationIndex}, " +
                          $"{currentSpell.powerType}, {currentSpell.manaCost}, {currentSpell.manaCostPerlevel}, " +
                          $"{currentSpell.manaPerSecond}, {currentSpell.manaPerSecondPerLevel}, {currentSpell.rangeIndex}, " +
                          $"{FormatFloat(currentSpell.speed)}, 0, {currentSpell.StackAmount}, {currentSpell.Totem[0]}, " +
                          $"{currentSpell.Totem[1]}, {currentSpell.Reagent[0]}, {currentSpell.Reagent[1]}, " +
                          $"{currentSpell.Reagent[2]}, {currentSpell.Reagent[3]}, {currentSpell.Reagent[4]}, " +
                          $"{currentSpell.Reagent[5]}, {currentSpell.Reagent[6]}, {currentSpell.Reagent[7]}, " +
                          $"{currentSpell.ReagentCount[0]}, {currentSpell.ReagentCount[1]}, {currentSpell.ReagentCount[2]}, " +
                          $"{currentSpell.ReagentCount[3]}, {currentSpell.ReagentCount[4]}, {currentSpell.ReagentCount[5]}, " +
                          $"{currentSpell.ReagentCount[6]}, {currentSpell.ReagentCount[7]}, {currentSpell.EquippedItemClass}, " +
                          $"{currentSpell.EquippedItemSubClassMask}, {currentSpell.EquippedItemInventoryTypeMask}, " +
                          $"{currentSpell.Effect[0]}, {currentSpell.Effect[1]}, {currentSpell.Effect[2]}, " +
                          $"{currentSpell.EffectDieSides[0]}, {currentSpell.EffectDieSides[1]}, {currentSpell.EffectDieSides[2]}, " +
                          $"{currentSpell.EffectBaseDice[0]}, {currentSpell.EffectBaseDice[1]}, {currentSpell.EffectBaseDice[2]}, " +
                          $"{FormatFloat(currentSpell.EffectDicePerLevel[0])}, {FormatFloat(currentSpell.EffectDicePerLevel[1])}, " +
                          $"{FormatFloat(currentSpell.EffectDicePerLevel[2])}, {FormatFloat(currentSpell.EffectRealPointsPerLevel[0])}, " +
                          $"{FormatFloat(currentSpell.EffectRealPointsPerLevel[1])}, {FormatFloat(currentSpell.EffectRealPointsPerLevel[2])}, " +
                          $"{currentSpell.EffectBasePoints[0]}, {currentSpell.EffectBasePoints[1]}, {currentSpell.EffectBasePoints[2]}, " +
                          $"{FormatFloat(currentSpell.EffectBonusCoefficient[0])}, {FormatFloat(currentSpell.EffectBonusCoefficient[1])}, " +
                          $"{FormatFloat(currentSpell.EffectBonusCoefficient[2])}, {currentSpell.EffectMechanic[0]}, " +
                          $"{currentSpell.EffectMechanic[1]}, {currentSpell.EffectMechanic[2]}, " +
                          $"{currentSpell.EffectImplicitTargetA[0]}, {currentSpell.EffectImplicitTargetA[1]}, " +
                          $"{currentSpell.EffectImplicitTargetA[2]}, {currentSpell.EffectImplicitTargetB[0]}, " +
                          $"{currentSpell.EffectImplicitTargetB[1]}, {currentSpell.EffectImplicitTargetB[2]}, " +
                          $"{currentSpell.EffectRadiusIndex[0]}, {currentSpell.EffectRadiusIndex[1]}, " +
                          $"{currentSpell.EffectRadiusIndex[2]}, {currentSpell.EffectApplyAuraName[0]}, " +
                          $"{currentSpell.EffectApplyAuraName[1]}, {currentSpell.EffectApplyAuraName[2]}, " +
                          $"{currentSpell.EffectAmplitude[0]}, {currentSpell.EffectAmplitude[1]}, " +
                          $"{currentSpell.EffectAmplitude[2]}, {FormatFloat(currentSpell.EffectMultipleValue[0])}, " +
                          $"{FormatFloat(currentSpell.EffectMultipleValue[1])}, {FormatFloat(currentSpell.EffectMultipleValue[2])}, " +
                          $"{currentSpell.EffectChainTarget[0]}, {currentSpell.EffectChainTarget[1]}, " +
                          $"{currentSpell.EffectChainTarget[2]}, {currentSpell.EffectItemType[0]}, " +
                          $"{currentSpell.EffectItemType[1]}, {currentSpell.EffectItemType[2]}, " +
                          $"{currentSpell.EffectMiscValue[0]}, {currentSpell.EffectMiscValue[1]}, " +
                          $"{currentSpell.EffectMiscValue[2]}, {currentSpell.EffectTriggerSpell[0]}, " +
                          $"{currentSpell.EffectTriggerSpell[1]}, {currentSpell.EffectTriggerSpell[2]}, " +
                          $"{FormatFloat(currentSpell.EffectPointsPerComboPoint[0])}, {FormatFloat(currentSpell.EffectPointsPerComboPoint[1])}, " +
                          $"{FormatFloat(currentSpell.EffectPointsPerComboPoint[2])}, {currentSpell.SpellVisual}, " +
                          $"0, {currentSpell.SpellIconID}, {currentSpell.activeIconID}, {currentSpell.spellPriority}, " +
                          $"'{EscapeSql(currentSpell.SpellName)}', {currentSpell.nameFlags}, '{EscapeSql(currentSpell.nameSubtext)}', " +
                          $"{currentSpell.nameSubtextFlags}, '{EscapeSql(currentSpell.description)}', " +
                          $"{currentSpell.descriptionFlags}, '{EscapeSql(currentSpell.auraDescription)}', " +
                          $"{currentSpell.auraDescriptionFlags}, {currentSpell.ManaCostPercentage}, " +
                          $"{currentSpell.StartRecoveryCategory}, {currentSpell.StartRecoveryTime}, " +
                          $"{currentSpell.MinTargetLevel}, {currentSpell.MaxTargetLevel}, {currentSpell.SpellFamilyName}, " +
                          $"{currentSpell.SpellFamilyFlags}, {currentSpell.MaxAffectedTargets}, {currentSpell.DmgClass}, " +
                          $"{currentSpell.PreventionType}, {currentSpell.stanceBarOrder}, " +
                          $"{FormatFloat(currentSpell.DmgMultiplier[0])}, {FormatFloat(currentSpell.DmgMultiplier[1])}, " +
                          $"{FormatFloat(currentSpell.DmgMultiplier[2])}, {currentSpell.MinFactionId}, " +
                          $"{currentSpell.MinReputation}, {currentSpell.RequiredAuraVision}, {currentSpell.Custom});");

            return sql.ToString();
        }

        private int GetSelectedValue(ComboBox cmb)
        {
            return cmb.SelectedValue != null ? (int)cmb.SelectedValue : 0;
        }

        private uint SaveAttributeCheckboxes(string checkboxPrefix)
        {
            uint attributes = 0;
            for (int i = 0; i < 32; i++)
            {
                CheckBox chk = FindName($"{checkboxPrefix}{i + 1}") as CheckBox;
                if (chk != null && chk.IsChecked == true)
                {
                    attributes |= (1u << i);
                }
            }
            return attributes;
        }
        private string EscapeSql(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace("'", "''");
        }

        private string FormatFloat(float value)
        {
            return value.ToString("0.#####", System.Globalization.CultureInfo.InvariantCulture);
        }

        private string LookupItemName(int itemId)
        {
            // In a real application, this would query a database or use a dictionary lookup
            // For now, just return a placeholder
            if (itemId <= 0)
                return string.Empty;

            return $"Item #{itemId}";
        }

        // Helper methods for parsing numeric values
        private int ParseInt(string text, int defaultValue = 0)
        {
            if (int.TryParse(text, out int result))
                return result;
            return defaultValue;
        }

        private uint ParseUInt(string text, uint defaultValue = 0)
        {
            if (uint.TryParse(text, out uint result))
                return result;
            return defaultValue;
        }

        private float ParseFloat(string text, float defaultValue = 0)
        {
            if (float.TryParse(text, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float result))
                return result;
            return defaultValue;
        }

        private ulong ParseULong(string text, ulong defaultValue = 0)
        {
            if (ulong.TryParse(text, out ulong result))
                return result;
            return defaultValue;
        }
    }
}