﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using qlsb_mini.BLL;
using qlsb_mini.DAL;
using qlsb_mini.demo;
using qlsb_mini.DTO;

namespace qlsb_mini
{
    public partial class Manager : Form
    {
        public Manager()
        {
            InitializeComponent();
            GetAllBill();
            LoadField();
            LoadTypeField();
            LoadFieldType();
        }
        public delegate void Mydel();
        public Mydel d { get; set; }
        void GetAllBill()
        { 
            dataGridView1.DataSource = BillBLL.Instance.ShowBill();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Value.ToShortDateString();
            dataGridView1.DataSource = BillBLL.Instance.ShowBill(date);
        }
        void LoadField()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[]
            {
                    new DataColumn {ColumnName = "id", DataType = typeof(int)},
                    new DataColumn {ColumnName = "TypeName", DataType = typeof(string)},
                    new DataColumn {ColumnName = "Name", DataType = typeof(string)},
                    new DataColumn {ColumnName = "idType", DataType = typeof(int)},
                //  new DataColumn {ColumnName = "status", DataType = typeof(string)}
            });
            List<Field> listField = FieldDAL.Instance.LoadFieldList();
            List<FieldType> listFieldType = FieldTypeDAL.Instance.LoadFieldType();
            foreach (Field field in listField)
            {
                foreach (FieldType fieldType in listFieldType)
                {
                    if (fieldType.Id == field.IdFieldType)
                    {
                        dataTable.Rows.Add(field.Id, fieldType.TypeName, field.Name, field.IdFieldType);
                    }

                }
            }
            dataGridView2.DataSource = dataTable;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[3].Visible = false;
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Lấy hàng đã click
                DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];
                // Tiếp theo, bạn có thể lấy giá trị từ các ô trong hàng đã chọn
                txtIdField.Text = selectedRow.Cells[0].Value.ToString();
                txtNameField.Text = selectedRow.Cells[2].Value.ToString();
                cbIdType.Text = selectedRow.Cells[3].Value.ToString(); 
                int id = int.Parse(cbIdType.Text);
                showTypeName(id);
            }
        }
        void showTypeName(int id)
        {
            List<FieldType> listFieldType = FieldTypeDAL.Instance.LoadFieldType();
            foreach (FieldType fieldType in listFieldType)
            {
                if (fieldType.Id == id)
                {
                    txtTypeName.Text = fieldType.TypeName;
                }
            }
        }
        void LoadTypeField()
        {
            List<FieldType> listFieldType = FieldTypeDAL.Instance.LoadFieldType();
            //  cbIdType.DataSource = listFieldType;
            //    cbIdType.DisplayMember = "id";
            cbIdType.Items.AddRange(listFieldType.ToArray());
        }
        private void cbIdType_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            ComboBox cb = sender as ComboBox;
            FieldType choosed = cb.SelectedItem as FieldType;
            if (choosed != null)
            {
                int id = choosed.Id;
                showTypeName(id);
            }
            
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = int.Parse(cbIdType.Text);
            bool check = true;
            List<Field> listField = FieldDAL.Instance.GetFieldByIdFieldType(id);
            foreach(Field field in listField)
            {
                if(field.Name == txtNameField.Text)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                // insert vào
                FieldDAL.Instance.insertField(txtNameField.Text, int.Parse(cbIdType.Text));
                LoadField();
                d();
            }
            else
            {
                MessageBox.Show("Sân đã trùng tên");
            }
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            FieldDAL.Instance.DelField(int.Parse(txtIdField.Text));
            LoadField();
            d();
        }
        // Table FieldType
        void LoadFieldType()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[]
            {
                    new DataColumn {ColumnName = "id", DataType = typeof(int)},
                    new DataColumn {ColumnName = "Name", DataType = typeof(string)},
                    new DataColumn {ColumnName = "normalDayPrice", DataType = typeof(float)},
                    new DataColumn {ColumnName = "specialDayPrice", DataType = typeof(float)}
            });
            List<FieldType> listFieldType = FieldTypeDAL.Instance.LoadFieldType();
            foreach (FieldType fieldType in listFieldType)
            {
                dataTable.Rows.Add(fieldType.Id, fieldType.TypeName, fieldType.NormalPrice, fieldType.SpecialPrice);
            }
            dataGridView3.DataSource = dataTable;
            dataGridView3.Columns[0].Visible = false;
        }
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Lấy hàng đã click
                DataGridViewRow selectedRow = dataGridView3.Rows[e.RowIndex];
                // Tiếp theo, bạn có thể lấy giá trị từ các ô trong hàng đã chọn
                txtIdType.Text = selectedRow.Cells[0].Value.ToString();
                txtTenLoai.Text = selectedRow.Cells[1].Value.ToString();
                txtPriceNormal.Text = selectedRow.Cells[2].Value.ToString();
                txtPriceSpecial.Text = selectedRow.Cells[3].Value.ToString();
            }
        }
        private void btnEditType_Click(object sender, EventArgs e)
        {
            FieldTypeDAL.Instance.updateFieldType(int.Parse(txtIdType.Text), txtTenLoai.Text.ToString(), float.Parse(txtPriceNormal.Text), float.Parse(txtPriceSpecial.Text));
            LoadFieldType();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            int id = int.Parse(cbIdType.Text);
            bool check = true;
            List<Field> listField = FieldDAL.Instance.GetFieldByIdFieldType(id);
            foreach (Field field in listField)
            {
                if (field.Name == txtNameField.Text)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                FieldDAL.Instance.updateField(int.Parse(txtIdField.Text), txtNameField.Text.ToString(), int.Parse(cbIdType.SelectedItem.ToString()));
                LoadField();
                d();
            }
            else
            {
                MessageBox.Show("Sân đã trùng tên");
            }
        }
        private void btnRf_Click(object sender, EventArgs e)
        {
            dataGridView1.Controls.Clear();
            GetAllBill();
        }
    }
}
