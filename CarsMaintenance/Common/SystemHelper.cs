﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TMS.Model;

namespace CarsMaintenance.Common
{
    class SystemHelper
    {
        private static TMSEntities tmsContext = new TMSEntities();

        public static TMSEntities TMSContext
        {
            get
            {
                return tmsContext;
            }
        }

        public static SystemUser CurrentUser
        {
            get;
            set;
        }

        public static void BindComboxToSupply(ComboBox cbSupply)
        {
            var querySupply = from s in SystemHelper.TMSContext.Supplies
                              where s.Deleted == false
                              orderby s.Code
                              select s;

            cbSupply.DataSource = querySupply;

            cbSupply.DisplayMember = "Name";
            cbSupply.ValueMember = "Code";
        }

        public static void BindComboxToTool(ComboBox cbTool)
        {
            var queryTools = from t in SystemHelper.TMSContext.Tools
                             where t.Deleted == false
                             orderby t.Code
                             select t;

            cbTool.DataSource = queryTools;
            cbTool.DisplayMember = "Name";
            cbTool.ValueMember = "Code";
        }

        public static void BindComboxToToolCategory(ComboBox cbToolCategory)
        {
            var queryToolCategories = from t in SystemHelper.TMSContext.ToolCategories
                             where t.Deleted == false
                             orderby t.Code
                             select t;

            cbToolCategory.DataSource = queryToolCategories;
            cbToolCategory.DisplayMember = "Name";
            cbToolCategory.ValueMember = "Code";
        }

        public static void BindComboxToCustomer(ComboBox cbCustomer)
        {
            var queryCustomer = from s in SystemHelper.TMSContext.Units
                              where s.Deleted == false
                              orderby s.Code
                              select s;

            cbCustomer.DataSource = queryCustomer;

            cbCustomer.DisplayMember = "Name";
            cbCustomer.ValueMember = "Code";
        }


        public static void BindComboxToSystemUser(ComboBox cbSystemUser)
        {
            var querySystemUser = from s in SystemHelper.TMSContext.SystemUsers
                                where s.Deleted == false
                                orderby s.Code
                                select s;

            cbSystemUser.DataSource = querySystemUser;

            cbSystemUser.DisplayMember = "Name";
            cbSystemUser.ValueMember = "Code";
        }

        public static bool ValidateComboxForSupply(ComboBox cbSupply)
        {
            bool isValid = true;
            if (cbSupply.SelectedItem == null)
            {
                string code = cbSupply.Text;
                Supply cs = SystemHelper.TMSContext.Supplies.FirstOrDefault(s => s.Code == code);
                if (cs == null)
                {
                    isValid = false;
                    cbSupply.Text = "";
                    cbSupply.SelectedItem = null;
                }
                else
                    cbSupply.SelectedItem = cs;
            }

            return isValid;
        }

        public static bool ValidateComboxForTool(ComboBox cbTool)
        {
            bool isValid = true;
            if (cbTool.SelectedItem == null)
            {
                string code = cbTool.Text;
                Tool cs = SystemHelper.TMSContext.Tools.FirstOrDefault(s => s.Code == code);
                if (cs == null)
                {
                    isValid = false;
                    cbTool.Text = "";
                    cbTool.SelectedItem = null;
                }
                else
                    cbTool.SelectedItem = cs;
            }

            return isValid;
        }

        public static bool ValidateComboxForCustomer(ComboBox cbCustomer)
        {
            bool isValid = true;
            if (cbCustomer.SelectedItem == null)
            {
                string code = cbCustomer.Text;
                Unit cs = SystemHelper.TMSContext.Units.FirstOrDefault(s => s.Code == code);
                if (cs == null)
                {
                    isValid = false;
                    cbCustomer.Text = "";
                    cbCustomer.SelectedItem = null;
                }
                else
                    cbCustomer.SelectedItem = cs;
            }

            return isValid;
        }

        public static bool ValidateComboxForSystemUser(ComboBox cbSystemUser)
        {
            bool isValid = true;
            if (cbSystemUser.SelectedItem == null)
            {
                string code = cbSystemUser.Text;
                SystemUser cs = SystemHelper.TMSContext.SystemUsers.FirstOrDefault(s => s.Code == code);
                if (cs == null)
                {
                    isValid = false;
                    cbSystemUser.Text = "";
                    cbSystemUser.SelectedItem = null;
                }
                else
                    cbSystemUser.SelectedItem = cs;
            }

            return isValid;
        }
    }
}
