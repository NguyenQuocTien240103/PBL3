﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hệ_thống_quản_lý_sân_bóng_mini.demo;
using Hệ_thống_quản_lý_sân_bóng_mini.DTO;

namespace Hệ_thống_quản_lý_sân_bóng_mini.DAL
{
    public class CustomerDAL
    {
        private static CustomerDAL _Instance;

        public static CustomerDAL Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CustomerDAL();
                }
                return _Instance;
            }
            private set
            {

            }
        }
        private CustomerDAL() { }

        public List<Customer> LoadCustomerList()
        {
            String sql = "Select * FROM dbo.Customer ";
            List<Customer> ListCustomer = new List<Customer>();

            DataTable dataTable = DataProvider.Instance.ExcuteQuery(sql);

            foreach (DataRow row in dataTable.Rows)
            {
                Customer customer = new Customer(row);
                ListCustomer.Add(customer);
            }
            return ListCustomer;
        }

        public void InsertCustomer(String name,String phone)
        {
            String sql = "INSERT INTO dbo.Customer(name,phone)" +
                "VALUES "+"('" +name.ToString() +"',"+ "'"+ phone.ToString() +"')";
            DataProvider.Instance.ExcuteNonQuery(sql);
        }

        public int getIdCustomerLast()
        {
            String sql = "SELECT TOP 1 * FROM dbo.Customer  ORDER BY id DESC";
            DataTable dataTable = DataProvider.Instance.ExcuteQuery(sql);
            if (dataTable.Rows.Count > 0)
            {
                Customer customer = new Customer(dataTable.Rows[0]);

                return customer.Id;
            }


            return -1;
        }

        public void updateCustomer(int idCustomer,string name,string phone)
        {
            String sql = "update dbo.Customer " +
                "set name = '" + name + "', phone = '" + phone + "' " +
                "where id = " + idCustomer;
            DataProvider.Instance.ExcuteNonQuery(sql);
        }
    }
}
