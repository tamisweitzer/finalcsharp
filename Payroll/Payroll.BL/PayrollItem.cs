
using Payroll.DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Payroll.BL
{
    public class PayrollItem
    {
        // Properties
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int CheckNumber { get; set; }
        public DateTime PayrollDate { get; set; }
        public double CheckAmount { get; set; }
        public double HoursWorked { get; set; }


        // Constructors
        public PayrollItem() { }

        public PayrollItem(DataRow payrollRow)
        {
            // TODO: Set the payroll item's properties.
            ID = Convert.ToInt32(payrollRow["ID"]);
            EmployeeID = Convert.ToInt32(payrollRow["EmployeeID"]);
            CheckNumber = Convert.ToInt32(payrollRow["CheckNumber"]);
            PayrollDate = Convert.ToDateTime(payrollRow["PayrollDate"]);
            CheckAmount = Convert.ToDouble(payrollRow["CheckAmount"]);
            HoursWorked = Convert.ToDouble(payrollRow["HoursWorked"]);
            

        }
        

        // Database Methods

        /// <summary>
        /// Inserts the payroll item into the database.
        /// </summary>
        /// <returns>Returns the result of the sql query.</returns>
        public int Insert()
        {
            try
            {
                string sql = "INSERT INTO Payroll VALUES(@id, @empId, @checkNum, @date, @amount, @hours);";
                int rowsAffected = DataAccess.RunSql(sql, GetSqlParams());
                return rowsAffected;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the payroll item in the database.
        /// </summary>
        /// <returns>Returns the result of the sql query.</returns>
        public int Update()
        {
            try
            {
                string sql = "UPDATE Payroll SET EmployeeID = @empId, CheckNumber = @checkNum, PayrollDate = @date, "
                    + "CheckAmount = @amount, HoursWorked = @hours WHERE ID = @id;";
                int rowsAffected = DataAccess.RunSql(sql, GetSqlParams());
                return rowsAffected;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Deletes the payroll item from the database.
        /// </summary>
        /// <returns>Returns the result of the sql query.</returns>
        public int Delete()
        {
            try
            {
                string sql = "DELETE FROM Payroll WHERE ID = @id;";
                int rowsAffected = DataAccess.RunSql(sql, GetSqlParams());
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private SqlParameter[] GetSqlParams()
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@id", ID));
            sqlParams.Add(new SqlParameter("@empId", EmployeeID));
            sqlParams.Add(new SqlParameter("@checkNum", CheckNumber));
            sqlParams.Add(new SqlParameter("@date", PayrollDate));
            sqlParams.Add(new SqlParameter("@amount", CheckAmount));
            sqlParams.Add(new SqlParameter("@hours", HoursWorked));
            return sqlParams.ToArray();
        }
    }
}
