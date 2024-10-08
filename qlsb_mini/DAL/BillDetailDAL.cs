﻿using qlsb_mini.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace qlsb_mini.DAL
{
    public class BillDetailDAL
    {
        private static BillDetailDAL _Instance;
        public static BillDetailDAL Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new BillDetailDAL();
                }
                return _Instance;
            }
            private set
            {

            }
        }
        private BillDetailDAL() { }
        public List<BillDetail> LoadBill(string date = null)
        {
            
            string sql =
                "SELECT Customer.name as CustomerName,\r\n   " +
                "Customer.phone,\r\n    " +
                "FieldType.TypeName AS FieldType, \r\n    " +
                "FieldName.name AS FieldName, \r\n   " +
                "CustomerBooking.startTime,\r\n    " +
                "CustomerBooking.endTime,\r\n   " +
                "CustomerBooking.priceBooking,\r\n   " +
                "CustomerBooking.status,\r\n  " +
                "Bill.totalPrice,\r\n   " +
                "Bill.paymentDay\r\nFROM \r\n  " +
                "FieldType\r\n" +
                "INNER JOIN \r\n   " +
                "FieldName ON FieldType.id = FieldName.idFieldType\r\n" +
                "INNER JOIN \r\n   " +
                "CustomerBooking ON FieldName.id = CustomerBooking.idFieldName\r\n" +
                "INNER JOIN \r\n    " +
                "Customer ON CustomerBooking.idCustomer = Customer.id \r\n" +
                "INNER JOIN \r\n    " +
                "Bill ON CustomerBooking.id = Bill.idCustomerBooking";
            if (date != null)
            {
                sql += " WHERE Bill.paymentDay = @date";
            }
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@date", date ?? (object)DBNull.Value)
            };

            DataTable data = DataProvider.Instance.ExecuteQuery(sql,parameters);
            List<BillDetail>   listBill = new List<BillDetail>();
            foreach (DataRow row in data.Rows)
            {
                BillDetail bill = new BillDetail(row);
                listBill.Add(bill);
            }
            return listBill;
        }

    }
}
