using System;
using System.Data;
using System.Data.SqlClient;
using WiseWay.Core;
using WiseWay.Extender;

namespace WiseWay.Facade
{
    public class CustomerFacade
    {
        public static Customer AddUpdateCustomer(Customer objcustomer)
        {
            DataSet dataSet = new DataSet();
            Customer tempCustomer = new Customer();
            using (SqlConnection con = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_AddUpdateCustomer", con))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FirstName", objcustomer.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", objcustomer.LastName);
                        cmd.Parameters.AddWithValue("@Password", objcustomer.Password);
                        cmd.Parameters.AddWithValue("@Phone", objcustomer.PhoneNo);
                        cmd.Parameters.AddWithValue("@Email", objcustomer.Email);
                        cmd.Parameters.AddWithValue("@Address", objcustomer.Address);
                        cmd.Parameters.AddWithValue("@Area", objcustomer.Area);
                        cmd.Parameters.AddWithValue("@City", objcustomer.City);
                        cmd.Parameters.AddWithValue("@CustomerType", objcustomer.CustomerType);
                        cmd.Parameters.AddWithValue("@UserId", objcustomer.UserId);
                        cmd.Parameters.AddWithValue("@ID", objcustomer.Id);

                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                        con.Open();
                        sqlDataAdapter.Fill(dataSet);
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        tempCustomer.Msg = ex.Message;
                        return tempCustomer;
                    }
                }
            }
            tempCustomer = GetCustomerData(dataSet, tempCustomer);
            if (objcustomer.Id == 0)
            { tempCustomer.Msg = "Customer data added successfully"; }
            else { tempCustomer.Msg = "Customer data updated successfully"; }
            return tempCustomer;
        }

        private static Customer GetCustomerData(DataSet dataSet, Customer customer)
        {
            if (dataSet.HasData())
            {
                DataRow dataRow = dataSet.Tables[0].Rows[0];
                try
                {
                    customer = new Customer()
                    {
                        Id = dataRow["Id"].ToLong(),
                        Password = dataRow["Password"].ToString(),
                        FirstName = dataRow["FirstName"].ToString(),
                        LastName = dataRow["LastName"].ToString(),
                        Email = dataRow["Email"].ToString(),
                        PhoneNo = dataRow["PhoneNo"].ToString(),
                        Address = dataRow["Address"].ToString(),
                        Area = dataRow["Area"].ToString(),
                        City = dataRow["City"].ToString(),
                        CustomerType = dataRow["CustomerType"].ToString()                        
                    };
                    customer.Msg = "Get customer data successfully";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("no data is present in database");
                }
            }
            else
            {
                customer.Msg = "Invalid customer id";
            }
            return customer;
        }

        public static string GetCustomerList()
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("usp_GetCustomerList", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlDataAdapter.Fill(dataSet);
                    sqlConnection.Close();
                }
            }
            string JSONresult = "{\"msg\":\"Data is not available\"}";
            if (dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    JSONresult = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        JSONresult += dt.Rows[i][0].ToString();
                    }
                }
            }
            return JSONresult;
        }

        public static Customer GetCustomerDetailById(int CustomerId)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("usp_GetCustomerById", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Id", CustomerId);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlDataAdapter.Fill(dataSet);
                    sqlConnection.Close();
                }
            }
            Customer tempCustomer = new Customer();
            tempCustomer = GetCustomerData(dataSet, tempCustomer);
            return tempCustomer;
        }

        public static string DeleteCustomer(int Id)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection con = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_DeleteCustomer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                    con.Open();
                    sqlDataAdapter.Fill(dataSet);
                    con.Close();
                }
            }
            string JSONresult = "{\"msg\":\"Customer id is not available\"}";
            if (dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    JSONresult = "{\"msg\":\"Customer Deleted Successfully.\"}";
                }
            }
            return JSONresult;
        }

        public static string GetCustomerTypeWiseCount()
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(DBUtil.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("usp_GetCustomerTypeWiseCount", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlDataAdapter.Fill(dataSet);
                    sqlConnection.Close();
                }
            }
            string JSONresult = "{\"msg\":\"Data is not available\"}";
            if (dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    JSONresult = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        JSONresult += dt.Rows[i][0].ToString();
                    }
                }
            }
            return JSONresult;
        }
    }
}
