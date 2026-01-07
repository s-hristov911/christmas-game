using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using SantaGiftListManager.Models;

namespace SantaGiftListManager.Forms
{
    public partial class MainForm : Form
    {
        private readonly BindingList<GiftItem> _items = new();
        private readonly BindingList<GiftItem> _viewItems = new();
        private GiftItem? _selectedItem;

        public MainForm()
        {
            InitializeComponent();
            InitializeFormState();
        }

        private void InitializeFormState()
        {
            cmbDisposition.DataSource = Enum.GetValues(typeof(GiftDisposition));
            cmbFilter.Items.Add("All");
            cmbFilter.Items.Add(GiftDisposition.Nice);
            cmbFilter.Items.Add(GiftDisposition.Naughty);
            cmbFilter.SelectedIndex = 0;

            giftsBindingSource.DataSource = _viewItems;
            gridGifts.AutoGenerateColumns = false;

            btnRemove.Enabled = false;

            SeedSampleData();
            ApplyFilter();
        }

        private void SeedSampleData()
        {
            _items.Add(new GiftItem { GiftName = "Wooden Train", ChildName = "Ava", Disposition = GiftDisposition.Nice });
            _items.Add(new GiftItem { GiftName = "Puzzle Box", ChildName = "Ben", Disposition = GiftDisposition.Nice });
            _items.Add(new GiftItem { GiftName = "Coal Briquette", ChildName = "Max", Disposition = GiftDisposition.Naughty });
        }

        private void BtnAddOrUpdate_Click(object? sender, EventArgs e)
        {
            if (!TryReadFormInputs(out var giftName, out var childName, out var disposition))
            {
                return;
            }

            if (_selectedItem == null)
            {
                _items.Add(new GiftItem
                {
                    GiftName = giftName,
                    ChildName = childName,
                    Disposition = disposition
                });
            }
            else
            {
                _selectedItem.GiftName = giftName;
                _selectedItem.ChildName = childName;
                _selectedItem.Disposition = disposition;
            }

            ApplyFilter();
            ClearInputs();
        }

        private void BtnRemove_Click(object? sender, EventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            _items.Remove(_selectedItem);
            _selectedItem = null;
            ApplyFilter();
            ClearInputs();
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            ClearInputs();
        }

        private void CmbFilter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void GridGifts_SelectionChanged(object? sender, EventArgs e)
        {
            if (gridGifts.CurrentRow?.DataBoundItem is not GiftItem item)
            {
                return;
            }

            _selectedItem = item;
            txtGiftName.Text = item.GiftName;
            txtChildName.Text = item.ChildName;
            cmbDisposition.SelectedItem = item.Disposition;
            btnAddOrUpdate.Text = "Update Gift";
            btnRemove.Enabled = true;
        }

        private bool TryReadFormInputs(out string giftName, out string childName, out GiftDisposition disposition)
        {
            giftName = txtGiftName.Text.Trim();
            childName = txtChildName.Text.Trim();
            disposition = cmbDisposition.SelectedItem is GiftDisposition d ? d : GiftDisposition.Nice;

            if (string.IsNullOrWhiteSpace(giftName))
            {
                MessageBox.Show("Please enter a gift name before saving.", "Missing Gift", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiftName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(childName))
            {
                MessageBox.Show("Please enter the recipient child name.", "Missing Child", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtChildName.Focus();
                return false;
            }

            return true;
        }

        private void ApplyFilter()
        {
            var filtered = cmbFilter.SelectedItem switch
            {
                GiftDisposition disposition => _items.Where(item => item.Disposition == disposition),
                _ => _items
            };

            _viewItems.RaiseListChangedEvents = false;
            _viewItems.Clear();
            foreach (var gift in filtered)
            {
                _viewItems.Add(gift);
            }

            _viewItems.RaiseListChangedEvents = true;
            _viewItems.ResetBindings();

            if (_selectedItem != null && !_viewItems.Any(item => item.Id == _selectedItem.Id))
            {
                ClearInputs();
            }
        }

        private void ClearInputs()
        {
            txtGiftName.Clear();
            txtChildName.Clear();
            cmbDisposition.SelectedIndex = 0;
            gridGifts.ClearSelection();
            _selectedItem = null;
            btnRemove.Enabled = false;
            btnAddOrUpdate.Text = "Add Gift";
        }
    }
}
