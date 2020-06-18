using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppShop
{
    public partial class Form1 : Form
    {
        private string connStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        private readonly DataGridViewButtonColumn deleteBtn1 = new DataGridViewButtonColumn();
        private readonly DataGridViewButtonColumn deleteBtn2 = new DataGridViewButtonColumn();
  
        public Form1()
        {
            InitializeComponent();
            ConstructDataGridView1();
            ConstructDataGridView2();
        }
        private void ConstructDataGridView1()
        {
            //ADD COLUMNS
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "VendorNumber";
            dataGridView1.Columns[1].Name = "Name";
            dataGridView1.Columns[2].Name = "City";
            dataGridView1.Columns[3].Name = "Street";
            dataGridView1.Columns[4].Name = "ZipCode";
            dataGridView1.Columns[5].Name = "PhoneNumber";

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            AddDeleteButtonColumn1();
            dataGridView1.Columns.Add(deleteBtn1);
            AddRowVentors();
        }
        private void ConstructDataGridView2()
        {
            //ADD COLUMNS
            dataGridView2.ColumnCount = 5;
            dataGridView2.Columns[0].Name = "ItemId";
            dataGridView2.Columns[1].Name = "Item";
            dataGridView2.Columns[2].Name = "CurrentQuantity";
            dataGridView2.Columns[3].Name = "Description";
            dataGridView2.Columns[4].Name = "VentorNumber";

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            AddDeleteButtonColumn2();
            dataGridView2.Columns.Add(deleteBtn2);
            AddRowInventory();
        }
        private void AddRowVentors()
        {
            foreach (Ventor vd in GetAllVentors())
            {
                dataGridView1.Rows.Add(vd.ventorNumber, vd.Name, vd.City, vd.Street, vd.ZipCode, vd.PhoneNumber);
            }
        }
        private void AddRowInventory()
        {
            foreach (Inventory i in GetAllInventory())
            {
                dataGridView2.Rows.Add(i.ItemId, i.Item, i.CurrentQuantity, i.Description, i.VentorNumber);
            }
        }
        private void AddDeleteButtonColumn1()
        {
            deleteBtn1.HeaderText = @"";
            deleteBtn1.Name = "DeleteButton";
            deleteBtn1.Text = "Delete";
            deleteBtn1.UseColumnTextForButtonValue = true;
        }
        private void AddDeleteButtonColumn2()
        {
            deleteBtn2.HeaderText = @"";
            deleteBtn2.Name = "DeleteButton";
            deleteBtn2.Text = "Delete";
            deleteBtn2.UseColumnTextForButtonValue = true;
        }
        public List<Ventor> GetAllVentors()
        {
            List<Ventor> ventorDetails = new List<Ventor>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Ventor";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read() == true)
                    {
                        Ventor vd = new Ventor
                        {
                            ventorNumber = ((int)reader["VentorNumber"]).ToString(),
                            Name = reader["Name"].ToString().Trim(),
                            City = reader["City"].ToString().Trim(),
                            Street = reader["Street"].ToString().Trim(),
                            ZipCode = reader["ZipCode"].ToString().Trim(),
                            PhoneNumber = reader["PhoneNumber"].ToString().Trim(),

                        };
                        ventorDetails.Add(vd);
                    }
                }
            }
            return ventorDetails;

        }
        public List<Inventory> GetAllInventory()
        {
            List<Inventory> inventory = new List<Inventory>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Inventory";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read() == true)
                    {
                        Inventory i = new Inventory
                        {
                            VentorNumber = ((int)reader["VentorNumber"]).ToString(),
                            CurrentQuantity = reader["CurrentQuantity"].ToString().Trim(),
                            Item = reader["Item"].ToString().Trim(),
                            ItemId = (int)reader["ItemId"],
                            Description = reader["Description"].ToString().Trim()

                        };
                        inventory.Add(i);
                    }
                }
            }
            return inventory;

        }

        private void DeleteVentor()
        {
            string ventorNumber = (string)dataGridView1.CurrentRow.Cells[0].Value;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = $"DELETE FROM Ventor WHERE VentorNumber={ventorNumber}";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            dataGridView1.Rows.Clear();
            ConstructDataGridView1();
        }
        private void DeleteItem()
        {
            string ItemId = (string)dataGridView2.CurrentRow.Cells[0].Value;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = $"DELETE FROM Inventory WHERE ItemId={ItemId}";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            dataGridView1.Rows.Clear();
            ConstructDataGridView1();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "DeleteButton")
            {
                if (MessageBox.Show("Are you sure want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteVentor();
                }

            }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "DeleteButton")
            {
                if (MessageBox.Show("Are you sure want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteItem();
                }

            }
        }
      
        private void Add_Click1(object sender, EventArgs e)
        {
        //string Message;
        Ventor v = new Ventor();
        {
            v.ventorNumber = textBoxVentorNumber.Text;
            v.Name = textBoxName.Text;
            v.City = textBoxCity.Text;
            v.Street = textBoxStreet.Text;
            v.ZipCode = textBoxZipCode.Text;
            v.PhoneNumber = textBoxPhoneNumber.Text;
        };
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO Ventor(VentorNumber, Name, City, Street, ZipCode, PhoneNumber) values(@VentorNumber, @Name, @City, @Street, @ZipCode, @PhoneNumber)");
                string query = sb.ToString();
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@VentorNumber", v.ventorNumber);
                cmd.Parameters.AddWithValue("@Name", v.Name);
                cmd.Parameters.AddWithValue("@City", v.City);
                cmd.Parameters.AddWithValue("@Street", v.Street);
                cmd.Parameters.AddWithValue("@ZipCode", v.ZipCode);
                cmd.Parameters.AddWithValue("@PhoneNumber", v.PhoneNumber);
                cmd.ExecuteNonQuery();
                dataGridView1.Rows.Clear();
                ConstructDataGridView1();
            }
        }

        private void Add_Click2(object sender, EventArgs e)
        {
            //string Message;
            Inventory i = new Inventory();
            {
                i.VentorNumber = textBoxVentorNumberInventory.Text;
                i.Item = textBoxItem.Text;
                i.ItemId = Convert.ToInt32(textBoxItemId.Text);
                i.Description = textBoxDescription.Text;
                i.CurrentQuantity = textBoxCurrenQuantity.Text;
            
            };
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO Inventory(VentorNumber, Item, ItemId, Description, CurrentQuantity) values(@VentorNumber, @Item, @ItemId, @Description, @CurrentQuantity)");
                string query = sb.ToString();
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@VentorNumber", i.VentorNumber);
                cmd.Parameters.AddWithValue("@Item", i.Item);
                cmd.Parameters.AddWithValue("@ItemId", i.ItemId);
                cmd.Parameters.AddWithValue("@Description", i.Description);
                cmd.Parameters.AddWithValue("@CurrentQuantity", i.CurrentQuantity);
             
                cmd.ExecuteNonQuery();
                dataGridView2.Rows.Clear();
                ConstructDataGridView2();
            }
        }
    }
    public class Ventor
    {
        public string ventorNumber { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class Inventory
    {
        public int ItemId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string CurrentQuantity { get; set; }
        public string VentorNumber { get; set; }
    }
}
