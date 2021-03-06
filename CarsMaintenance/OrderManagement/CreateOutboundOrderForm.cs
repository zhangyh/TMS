﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CarsMaintenance.Common;
using CarsMaintenance.Common.Validation;
using TMS.Model;

namespace CarsMaintenance.OrderManagement
{
    public partial class CreateOutboundOrderForm : BaseForm
    {
        public OutboundOrder CurrentOrder { get; set; }
        public string CurrentMode { get; set; }
        public int ItemCount { get; set; }

        public static string MODE_BROWSE = "BROWSE";
        public static string MODE_CREATE = "CREATE";
        public static string MODE_APPEND = "APPEND";

        public CreateOutboundOrderForm()
        {
            InitializeComponent();
            SetControls();
            RegisterControlsForValidation();
        }

        private void SetControls()
        {
            txtBerth.Enabled = true;
            txtMachine.Enabled = true;
            txtShip.Enabled = true;
            txtHatch.Enabled = true;
            txtCargo.Enabled = true;
            txtQuantity.Enabled = true;
            txtProcess.Enabled = true;
        }

        private ValidationManager _validationManager;

        private void RegisterControlsForValidation()
        {
            _validationManager = new ValidationManager()
            {
                Provider = _errorProvider
            };
            // Do not validate code because of auto generation
            //_validationManager.Validators.Add(new RequiredValidator()
            //{
            //    Control = txtCode,
            //    ErrorMessage = string.Format(CarsMaintenance.Properties.Resources.RequiredErrorMessage, lblCode.Text)
            //});
            _validationManager.Validators.Add(new RequiredValidator()
            {
                Control = txtJob,
                ErrorMessage = string.Format(CarsMaintenance.Properties.Resources.RequiredErrorMessage, lblJob.Text)
            });
            _validationManager.Validators.Add(new RequiredValidator()
            {
                Control = cbCustomer,
                ErrorMessage = string.Format(CarsMaintenance.Properties.Resources.RequiredErrorMessage, lblCustomer.Text)
            });
        }

        private void LoadData()
        {
            SystemHelper.BindComboxToCustomer(cbCustomer);
            SystemHelper.BindComboxToSystemUser(cbSystemUser);

            // Set data object value
            if (CurrentOrder == null)
            {
                CurrentOrder = new OutboundOrder();
                CurrentMode = MODE_CREATE;
            }

            if (CurrentMode == MODE_CREATE)
            {
                CurrentOrder.Version = 0;
                CurrentOrder.SystemUser = SystemHelper.CurrentUser;
                ItemCount = 0;
            }
            else if (CurrentMode == MODE_APPEND)
            {
                CurrentOrder.Version ++;
                CurrentOrder.SystemUser = SystemHelper.CurrentUser;
                ItemCount = CurrentOrder.Items.Count;
            }
            else if (CurrentMode == MODE_BROWSE)
            {
                ItemCount = CurrentOrder.Items.Count;
            }

            txtCode.Text = CurrentOrder.Code;
            dtOutboundDate.Value = CurrentOrder.OutboundDate;
            txtVersion.Text = CurrentOrder.Version.ToString();
            txtJob.Text = CurrentOrder.Job;

            txtBerth.Text = CurrentOrder.Berth;
            txtMachine.Text = CurrentOrder.Machine;
            txtShip.Text = CurrentOrder.Ship;
            txtHatch.Text = CurrentOrder.Hatch;
            txtCargo.Text = CurrentOrder.Cargo;
            txtQuantity.Text = CurrentOrder.Quantity;
            txtProcess.Text = CurrentOrder.Process;

            cbCustomer.SelectedItem = CurrentOrder.Customer;
            cbSystemUser.SelectedItem = CurrentOrder.SystemUser;

            foreach (OutboundOrderDetail item in CurrentOrder.Items)
            {
                DataGridViewRow dgvr = new DataGridViewRow();
                //dgvr.Cells["ItemCode"].Value = item.Tool.Code;
                //dgvr.Cells["ItemQuantity"].Value = item.Quantity;
                //dgvr.Cells["ItemName"].Value = item.Tool.Name;
                //dgvr.Cells["ItemDimensions"].Value = item.Tool.Dimensions;
                object[] row = { item.Tool.Code, item.Quantity, item.Tool.Name, item.Tool.Dimensions };
                dataGridViewDetail.Rows.Add(row);
            }
        }

        private void EnableForm()
        {
            if (CurrentMode == MODE_BROWSE)
                _saveButton.Enabled = false;
        }

        private void CreateOutboundOrderForm_Load(object sender, EventArgs e)
        {
            LoadData();
            EnableForm();
        }

        private void _saveButton_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(delegate()
            {
                if (!_validationManager.Validate())
                {
                    return;
                }

                CurrentOrder.Code = txtCode.Text;
                CurrentOrder.OutboundDate = dtOutboundDate.Value;

                int version = 0;
                int.TryParse(txtVersion.Text, out version);
                CurrentOrder.Version = version;

                CurrentOrder.Job = txtJob.Text;
                CurrentOrder.Berth = txtBerth.Text;
                CurrentOrder.Machine = txtMachine.Text;
                CurrentOrder.Ship = txtShip.Text;
                CurrentOrder.Hatch = txtHatch.Text;
                CurrentOrder.Cargo = txtCargo.Text;
                CurrentOrder.Quantity = txtQuantity.Text;
                CurrentOrder.Process = txtProcess.Text;

                CurrentOrder.Customer = cbCustomer.SelectedItem as Unit;
                CurrentOrder.SystemUser = cbSystemUser.SelectedItem as SystemUser;

                if (CurrentOrder.EntityKey == null)
                    SystemHelper.TMSContext.AddToOutboundOrders(CurrentOrder);

                CurrentOrder.SystemUser = SystemHelper.CurrentUser;
                CurrentOrder.LastUpdateTime = System.DateTime.Now;

                // Iterate all rows
                foreach (DataGridViewRow dgvr in dataGridViewDetail.Rows)
                {
                    if (!dgvr.IsNewRow && dgvr.Index >= ItemCount)
                    {
                        // for outbound detail
                        decimal quantity = 0;
                        decimal.TryParse(dgvr.Cells["ItemQuantity"].Value.ToString(), out quantity);

                        string code = dgvr.Cells["ItemCode"].Value.ToString();
                        Tool t = SystemHelper.TMSContext.Tools.FirstOrDefault(s => s.Code == code);

                        OutboundOrderDetail item = SystemHelper.TMSContext.OutboundOrderDetails.CreateObject();
                        item.Version = CurrentOrder.Version;
                        item.Tool = t;
                        item.Quantity = quantity;
                        item.UnitPrice = t.ToolInventory.UnitPrice;
                        item.OutboundDate = CurrentOrder.LastUpdateTime;

                        CurrentOrder.Items.Add(item);

                        // for inventory and inventory history
                        ToolInventory inventory = SystemHelper.TMSContext.ToolInventories.FirstOrDefault(ti => ti.ToolID == item.ToolID);
                        inventory.Tool = item.Tool;
                        inventory.OutQuantity = inventory.OutQuantity + item.Quantity;

                        ToolInventoryHistory inventoryHistory = SystemHelper.TMSContext.ToolInventoryHistories.CreateObject();
                        inventoryHistory.Customer = CurrentOrder.Customer;
                        inventoryHistory.ToolInventoryHistoryDate = CurrentOrder.OutboundDate;
                        inventoryHistory.Tool = item.Tool;
                        inventoryHistory.Quantity = item.Quantity;
                        inventoryHistory.UnitPrice = item.UnitPrice;
                        inventoryHistory.OutboundOrder = CurrentOrder;
                        inventoryHistory.OutboundOrderDetail = item;
                    }
                }

                SystemHelper.TMSContext.SaveChanges();

                DialogResult = DialogResult.OK;

                ExecuteActionHelper.ExecuteAction(delegate()
                {
                    FormsManager.OpenForm(typeof(CarsMaintenance.Reports.OutboundOrderReport), new object[] { "ID", CurrentOrder.OutboundOrderID });
                });
            });
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            if (CurrentOrder.EntityState == EntityState.Modified)
                SystemHelper.TMSContext.Refresh(System.Data.Objects.RefreshMode.StoreWins, CurrentOrder);
        }


        private void dataGridViewDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < ItemCount)
                dataGridViewDetail.CurrentCell.ReadOnly = true;
        }

        private void dataGridViewDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.FormattedValue == null || e.FormattedValue.ToString().Length == 0)
                return;

            switch (e.ColumnIndex)
            {
                case 0:
                    string code = e.FormattedValue.ToString();
                    Tool t = SystemHelper.TMSContext.Tools.FirstOrDefault(s => s.Code == code);
                    if (t == null)
                    {
                        e.Cancel = true;              
                    }
                    else
                    {
                        dataGridViewDetail.Rows[e.RowIndex].Cells["ItemName"].Value = t.Name;
                        dataGridViewDetail.Rows[e.RowIndex].Cells["ItemDimensions"].Value = t.Dimensions;
                    }
                    break;
                case 1:
                    decimal quantity = 0;
                    if (!decimal.TryParse(e.FormattedValue.ToString(), out quantity))
                        e.Cancel = true;
                    break;

            }
        }

        private void cbCustomer_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !SystemHelper.ValidateComboxForCustomer(cbCustomer);
        }

        private void cbSystemUser_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !SystemHelper.ValidateComboxForSystemUser(cbSystemUser);
        }
    }
}
