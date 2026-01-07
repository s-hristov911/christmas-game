using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SantaGiftListManager.Forms
{
    partial class MainForm
    {
        private IContainer? components = null;
        private Label lblGiftName;
        private TextBox txtGiftName;
        private Label lblChildName;
        private TextBox txtChildName;
        private Label lblDisposition;
        private ComboBox cmbDisposition;
        private Button btnAddOrUpdate;
        private Button btnRemove;
        private Button btnClear;
        private Label lblFilter;
        private ComboBox cmbFilter;
        private DataGridView gridGifts;
        private BindingSource giftsBindingSource;
        private DataGridViewTextBoxColumn colGift;
        private DataGridViewTextBoxColumn colChild;
        private DataGridViewTextBoxColumn colStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            lblGiftName = new Label();
            txtGiftName = new TextBox();
            lblChildName = new Label();
            txtChildName = new TextBox();
            lblDisposition = new Label();
            cmbDisposition = new ComboBox();
            btnAddOrUpdate = new Button();
            btnRemove = new Button();
            btnClear = new Button();
            lblFilter = new Label();
            cmbFilter = new ComboBox();
            gridGifts = new DataGridView();
            colGift = new DataGridViewTextBoxColumn();
            colChild = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            giftsBindingSource = new BindingSource(components);
            ((ISupportInitialize)gridGifts).BeginInit();
            ((ISupportInitialize)giftsBindingSource).BeginInit();
            SuspendLayout();
            // 
            // lblGiftName
            // 
            lblGiftName.AutoSize = true;
            lblGiftName.Location = new Point(24, 24);
            lblGiftName.Name = "lblGiftName";
            lblGiftName.Size = new Size(68, 15);
            lblGiftName.TabIndex = 0;
            lblGiftName.Text = "Gift Name";
            // 
            // txtGiftName
            // 
            txtGiftName.Location = new Point(24, 47);
            txtGiftName.Name = "txtGiftName";
            txtGiftName.Size = new Size(240, 23);
            txtGiftName.TabIndex = 1;
            // 
            // lblChildName
            // 
            lblChildName.AutoSize = true;
            lblChildName.Location = new Point(288, 24);
            lblChildName.Name = "lblChildName";
            lblChildName.Size = new Size(71, 15);
            lblChildName.TabIndex = 2;
            lblChildName.Text = "Child Name";
            // 
            // txtChildName
            // 
            txtChildName.Location = new Point(288, 47);
            txtChildName.Name = "txtChildName";
            txtChildName.Size = new Size(240, 23);
            txtChildName.TabIndex = 3;
            // 
            // lblDisposition
            // 
            lblDisposition.AutoSize = true;
            lblDisposition.Location = new Point(552, 24);
            lblDisposition.Name = "lblDisposition";
            lblDisposition.Size = new Size(78, 15);
            lblDisposition.TabIndex = 4;
            lblDisposition.Text = "Naughty/Nice";
            // 
            // cmbDisposition
            // 
            cmbDisposition.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDisposition.FormattingEnabled = true;
            cmbDisposition.Location = new Point(552, 47);
            cmbDisposition.Name = "cmbDisposition";
            cmbDisposition.Size = new Size(160, 23);
            cmbDisposition.TabIndex = 5;
            // 
            // btnAddOrUpdate
            // 
            btnAddOrUpdate.Location = new Point(24, 90);
            btnAddOrUpdate.Name = "btnAddOrUpdate";
            btnAddOrUpdate.Size = new Size(120, 32);
            btnAddOrUpdate.TabIndex = 6;
            btnAddOrUpdate.Text = "Add Gift";
            btnAddOrUpdate.UseVisualStyleBackColor = true;
            btnAddOrUpdate.Click += BtnAddOrUpdate_Click;
            // 
            // btnRemove
            // 
            btnRemove.Location = new Point(160, 90);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(120, 32);
            btnRemove.TabIndex = 7;
            btnRemove.Text = "Remove Gift";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += BtnRemove_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(296, 90);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(120, 32);
            btnClear.TabIndex = 8;
            btnClear.Text = "Clear Fields";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += BtnClear_Click;
            // 
            // lblFilter
            // 
            lblFilter.AutoSize = true;
            lblFilter.Location = new Point(456, 98);
            lblFilter.Name = "lblFilter";
            lblFilter.Size = new Size(69, 15);
            lblFilter.TabIndex = 9;
            lblFilter.Text = "Filter Board";
            // 
            // cmbFilter
            // 
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.FormattingEnabled = true;
            cmbFilter.Location = new Point(552, 95);
            cmbFilter.Name = "cmbFilter";
            cmbFilter.Size = new Size(160, 23);
            cmbFilter.TabIndex = 10;
            cmbFilter.SelectedIndexChanged += CmbFilter_SelectedIndexChanged;
            // 
            // gridGifts
            // 
            gridGifts.AllowUserToAddRows = false;
            gridGifts.AllowUserToDeleteRows = false;
            gridGifts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridGifts.AutoGenerateColumns = false;
            gridGifts.BackgroundColor = Color.White;
            gridGifts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridGifts.Columns.AddRange(new DataGridViewColumn[] { colGift, colChild, colStatus });
            gridGifts.DataSource = giftsBindingSource;
            gridGifts.Location = new Point(24, 144);
            gridGifts.MultiSelect = false;
            gridGifts.Name = "gridGifts";
            gridGifts.ReadOnly = true;
            gridGifts.RowHeadersVisible = false;
            gridGifts.RowTemplate.Height = 25;
            gridGifts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridGifts.Size = new Size(688, 320);
            gridGifts.TabIndex = 11;
            gridGifts.SelectionChanged += GridGifts_SelectionChanged;
            // 
            // colGift
            // 
            colGift.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colGift.DataPropertyName = "GiftName";
            colGift.HeaderText = "Gift";
            colGift.Name = "colGift";
            colGift.ReadOnly = true;
            // 
            // colChild
            // 
            colChild.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colChild.DataPropertyName = "ChildName";
            colChild.HeaderText = "Child";
            colChild.Name = "colChild";
            colChild.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Disposition";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 120;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(736, 492);
            Controls.Add(gridGifts);
            Controls.Add(cmbFilter);
            Controls.Add(lblFilter);
            Controls.Add(btnClear);
            Controls.Add(btnRemove);
            Controls.Add(btnAddOrUpdate);
            Controls.Add(cmbDisposition);
            Controls.Add(lblDisposition);
            Controls.Add(txtChildName);
            Controls.Add(lblChildName);
            Controls.Add(txtGiftName);
            Controls.Add(lblGiftName);
            MinimumSize = new Size(752, 531);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Santa's Gift List Manager";
            ((ISupportInitialize)gridGifts).EndInit();
            ((ISupportInitialize)giftsBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
