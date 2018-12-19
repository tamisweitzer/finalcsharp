
using Payroll.DL;
using System;
using System.Collections.Generic;
using System.Data;

namespace Payroll.BL
{
    public class PayrollCollection : List<PayrollItem>
    {
        public PayrollCollection()
        {
            // Server=tcp:sweitzer.database.windows.net,1433;Initial Catalog=intermediate;Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
            DataAccess.ConnectionString = "Server=tcp:sweitzer.database.windows.net,1433;Initial Catalog=intermediate;Persist Security Info=False;User ID=tami;Password=tssPASS2014;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; // TODO: set connection string (from azure)
        }

        public void PopulateFromDB()
        {
            try
            {
                string sql = "SELECT * FROM Payroll;";
                DataTable table = DataAccess.Select(sql);
                foreach(DataRow row in table.Rows)
                {
                    Add(new PayrollItem(row));
                }
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Helper function to generate the next payroll id.
        /// </summary>
        /// <returns>The next id</returns>
        public int GetNextPayrollID()
        {
            int highest = 0;
            foreach (PayrollItem item in this)
            {
                if (item.ID > highest) highest = item.ID;
            }

            return (highest + 1);
        }

        /// <summary>
        /// Helper function to generate the next check number.
        /// </summary>
        /// <returns>The next check number</returns>
        public int GetNextCheckNumber()
        {
            int highest = 0;
            foreach (PayrollItem item in this)
            {
                if (item.CheckNumber > highest) highest = item.CheckNumber;
            }

            return (highest + 1);
        }
    }
}
