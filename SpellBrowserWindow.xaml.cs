using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SpellWorker
{
    public partial class SpellBrowserWindow : Window
    {
        private DatabaseConnector dbConnector;
        private List<SpellListItem> allSpells;
        public SpellData SelectedSpell { get; private set; }

        public SpellBrowserWindow(DatabaseConnector connector)
        {
            InitializeComponent();
            dbConnector = connector;
            LoadSpellsAsync();
        }

        private async void LoadSpellsAsync()
        {
            try
            {
                // Show loading indicator
                Mouse.OverrideCursor = Cursors.Wait;
                
                // Load all spells from database
                allSpells = await dbConnector.GetSpellListAsync();
                dgSpells.ItemsSource = allSpells;
                
                // Update UI
                Title = $"Spell Browser - {allSpells.Count} spells loaded";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading spells: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Hide loading indicator
                Mouse.OverrideCursor = null;
            }
        }

        private void FilterSpells(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // No filter, show all spells
                dgSpells.ItemsSource = allSpells;
            }
            else
            {
                // Filter by ID or name
                bool isNumeric = uint.TryParse(searchText, out uint spellId);
                
                var filteredList = allSpells.Where(s => 
                    (isNumeric && s.Id == spellId) || 
                    s.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();
                
                dgSpells.ItemsSource = filteredList;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterSpells(txtSearch.Text.Trim());
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FilterSpells(txtSearch.Text.Trim());
        }

        private void dgSpells_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Enable/disable buttons based on selection
            bool hasSelection = dgSpells.SelectedItem != null;
            btnEdit.IsEnabled = hasSelection;
            btnDelete.IsEnabled = hasSelection;
        }

        private async void dgSpells_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgSpells.SelectedItem is SpellListItem selectedItem)
            {
                await LoadSelectedSpell(selectedItem.Id);
                if (SelectedSpell != null)
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgSpells.SelectedItem is SpellListItem selectedItem)
            {
                await LoadSelectedSpell(selectedItem.Id);
                if (SelectedSpell != null)
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

        private async Task LoadSelectedSpell(uint spellId)
        {
            try
            {
                // Show loading indicator
                Mouse.OverrideCursor = Cursors.Wait;
                
                // Load the selected spell
                SelectedSpell = await dbConnector.GetSpellByIdAsync(spellId);
                
                if (SelectedSpell == null)
                {
                    MessageBox.Show($"Could not load spell ID {spellId}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading spell: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SelectedSpell = null;
            }
            finally
            {
                // Hide loading indicator
                Mouse.OverrideCursor = null;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            // Create a new spell
            SelectedSpell = new SpellData();
            DialogResult = true;
            Close();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgSpells.SelectedItem is SpellListItem selectedItem)
            {
                if (MessageBox.Show($"Are you sure you want to delete spell ID {selectedItem.Id} ({selectedItem.Name})?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Show loading indicator
                        Mouse.OverrideCursor = Cursors.Wait;
                        
                        // Delete the spell
                        bool success = await dbConnector.DeleteSpellAsync(selectedItem.Id);
                        
                        if (success)
                        {
                            MessageBox.Show($"Spell deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            // Reload the spell list
                            LoadSpellsAsync();
                        }
                        else
                        {
                            MessageBox.Show($"Failed to delete spell.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting spell: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        // Hide loading indicator
                        Mouse.OverrideCursor = null;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}