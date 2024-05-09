﻿using Hệ_thống_quản_lý_sân_bóng_mini.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hệ_thống_quản_lý_sân_bóng_mini.DAL
{
    public class BillDAL
    {
        private static BillDAL _Instance;
        public static BillDAL Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = new BillDAL();
                }
                return _Instance;
            }
            private set
            {

            }
            
        }

        private BillDAL() { }

        public List<Bill> LoadBill()
        {
            List<Bill> listBill = new List<Bill>();
            string sql = " select * from dbo.Bill";
            DataTable data = DataProvider.Instance.ExcuteQuery(sql);
            foreach (DataRow row in data.Rows)
            {
                Bill bill = new Bill(row);
                listBill.Add(bill);

            }
            return listBill;

        }
        public void insertBill(int idCustomerBooking,float totalPrice)
        {
            string sql  = "insert into dbo.Bill(idCustomerBooking,totalPrice) values"+ "(" + idCustomerBooking+","+totalPrice+")";
            DataProvider.Instance.ExcuteNonQuery(sql);
        }
    }
}
