﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CarsMaintenance.Common;
using TMS.Model;

namespace CarsMaintenance.OrderManagement
{
    public partial class ViewRepairOrderForm : BaseForm
    {
        public ViewRepairOrderForm()
        {
            InitializeComponent();
        }

        protected void LoadData()
        {
            var query = from o in SystemHelper.TMSContext.RepairOrders
                        orderby o.RepairDate
                        select o;

            dataGridViewRepairOrder.DataSource = query;
        }

        private void FormLoad(object sender, EventArgs e)
        {
            LoadData();
        }

        private RepairOrder GetSelectedOrder()
        {
            if (dataGridViewRepairOrder.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择修理单!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return null;
            }
            int id;
            if (!int.TryParse(Convert.ToString(dataGridViewRepairOrder.SelectedRows[0].Cells[0].Value), out id))
            {
                MessageBox.Show("请重新选择修理单!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return null;
            }

            return SystemHelper.TMSContext.RepairOrders.First(u => u.RepairOrderID == id);
        }

        private void Create()
        {
            using (CreateRepairOrderForm form = new CreateRepairOrderForm())
            {
                form.CurrentOrder = GetSelectedOrder();
                form.CurrentMode = CreateRepairOrderForm.MODE_CREATE;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void Browse()
        {
            using (CreateRepairOrderForm form = new CreateRepairOrderForm())
            {
                form.CurrentOrder = GetSelectedOrder();
                form.CurrentMode = CreateRepairOrderForm.MODE_BROWSE;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void CreateSelf()
        {
            using (CreateRepairOrderForm form = new CreateRepairOrderForm(CreateRepairOrderForm.MODE_CREATESELF))
            {
                form.CurrentOrder = new RepairOrder();
                form.CurrentOrder.Status = 1;
                form.CurrentMode = CreateRepairOrderForm.MODE_CREATESELF;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void toolStripMenuItemCreateRepairOrder_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(Create);
        }

        private void toolStripMenuItemCreateSelfRepairOrder_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(CreateSelf);
        }

        private void toolStripMenuItemBrowseRepairOrder_Click(object sender, EventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(Browse);
        }

        private void dataGridViewScrapOrder_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ExecuteActionHelper.ExecuteAction(Browse);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DateTime beginDate = dtBeginDate.Value.Date;
            DateTime endDate = dtEndDate.Value.Date.AddDays(1);
            var query = from o in SystemHelper.TMSContext.RepairOrders
                        where o.RepairDate >= beginDate && o.RepairDate <= endDate
                        orderby o.RepairDate
                        select o;

            dataGridViewRepairOrder.DataSource = query;
        }
    }
}
