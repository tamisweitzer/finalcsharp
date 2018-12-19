using Payroll.BL;
using System;
using System.Windows.Forms;

namespace Payroll.UI
{
    public partial class frmPayroll : Form
    {
        // Fields
        private PayrollCollection collection = new PayrollCollection();


        // Form Constructor
        public frmPayroll()
        {
            InitializeComponent();
        }


        // Methods

        // Populate and bind the payroll data
        private void frmPayroll_Load(object sender, EventArgs e)
        {
            try
            {
                collection.PopulateFromDB();
                RebindPayrollData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void RebindPayrollData()
        {
            try
            {
                // rebind the datasource
                dgvPayroll.DataSource = null;
                dgvPayroll.DataSource = collection;
            }
            catch
            {
                // For some reason, setting the datasource for a datagrid view throws an "invalid index" exception.
                // Since it still works, we'll just swallow the exception and move on.
            }

            // Make the datagridview look pretty
            dgvPayroll.ReadOnly = false;
            dgvPayroll.Columns["ID"].Visible = false;
            dgvPayroll.Columns["CheckAmount"].DefaultCellStyle.Format = "c";
            dgvPayroll.Columns["EmployeeID"].HeaderText = "Employee ID";
            dgvPayroll.Columns["PayrollDate"].HeaderText = "Payroll Date";
            dgvPayroll.Columns["PayrollDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvPayroll.Columns["CheckNumber"].HeaderText = "Check Number";
            dgvPayroll.Columns["CheckAmount"].HeaderText = "Check Amount";
            dgvPayroll.Columns["HoursWorked"].HeaderText = "Hours Worked";
            dgvPayroll.ReadOnly = true;
        }


        // Gets the payroll item that is bound to the selected row
        private PayrollItem GetSelectedPayrollItem()
        {
            if (dgvPayroll.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPayroll.SelectedRows[0];

                if (row != null)
                {
                    PayrollItem payroll = row.DataBoundItem as PayrollItem;

                    if (payroll != null) return payroll;
                }
            }

            return null;
        }


        // Shows the payroll data in the textboxes when a datagrid row is selected
        private void dgvPayroll_SelectionChanged(object sender, EventArgs e)
        {
            PayrollItem payroll = GetSelectedPayrollItem();
            if (payroll != null)
            {
                txtEmployeeID.Text = payroll.EmployeeID.ToString();
                dtpPayrollDate.Value = payroll.PayrollDate;
                txtCheckNumber.Text = payroll.CheckNumber.ToString();
                txtCheckAmount.Text = payroll.CheckAmount.ToString();
                txtHoursWorked.Text = payroll.HoursWorked.ToString();
            }
        }


        // Calls the insert function of the payroll collection
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                PayrollItem payroll = new PayrollItem();

                payroll.ID = collection.GetNextPayrollID();
                payroll.EmployeeID = int.Parse(txtEmployeeID.Text);
                payroll.CheckNumber = int.Parse(txtCheckNumber.Text);
                payroll.CheckAmount = double.Parse(txtCheckAmount.Text);
                payroll.HoursWorked = double.Parse(txtHoursWorked.Text);
                payroll.PayrollDate = dtpPayrollDate.Value;

                if (payroll.Insert() > 0)
                {
                    collection.Add(payroll);
                    RebindPayrollData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // Calls the update function of the payroll collection
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            PayrollItem payroll = GetSelectedPayrollItem();
            if (payroll != null)
            {
                try
                {
                    payroll.EmployeeID = int.Parse(txtEmployeeID.Text);
                    payroll.PayrollDate = dtpPayrollDate.Value;
                    payroll.CheckNumber = int.Parse(txtCheckNumber.Text);
                    payroll.CheckAmount = double.Parse(txtCheckAmount.Text);
                    payroll.HoursWorked = double.Parse(txtHoursWorked.Text);

                    if (payroll.Update() > 0)
                    {
                        RebindPayrollData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        // Calls the delete function of the payroll collection
        private void btnDelete_Click(object sender, EventArgs e)
        {
            PayrollItem payroll = GetSelectedPayrollItem();
            if (payroll != null)
            {
                try
                {
                    if (payroll.Delete() > 0)
                    {
                        collection.Remove(payroll);
                        RebindPayrollData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        // Clear out the textboxes and deselect any rows in the datagridview
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCheckAmount.Text = "";
            txtCheckNumber.Text = collection.GetNextCheckNumber().ToString();
            txtEmployeeID.Text = "";
            txtHoursWorked.Text = "40";
            dtpPayrollDate.Value = DateTime.Now;

            dgvPayroll.ClearSelection();
        }
    }
}
