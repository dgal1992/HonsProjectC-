using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

namespace HonsProject
{
    public partial class Form1 : Form
    {
        // connection string to connect to SqlServer database
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MyInstance;Initial Catalog=HonsProject;Integrated Security=True");
        
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
          
        }
        
        public void display_tblPerson_data()
        {
            // fill datatable from tblPerson
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql statement to select record containing username from tblPerson
            cmd.CommandText = "Select * from tblPerson WHERE ((username) = '" + txtUsername.Text + "')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        public void display_tblAccount_data()
        {
            // fill datatable from tblAccount
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql statement to select record containing username from tblAccount
            cmd.CommandText = "Select * from tblAccount WHERE ((username) = @username)";
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = txtUsername.Text;
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView5.DataSource = dt;
            con.Close();
        }

        public void display_tblBank_data()
        {
            // fill datatable from tblBank using the bankID from tblPerson
            String bankID = (String)dataGridView1["bankID", 0].Value.ToString();

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql statement to select record containing username from tblbank
            cmd.CommandText = "Select * from tblBank WHERE ((bankID) = '" + bankID + "')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView3.DataSource = dt;
            con.Close();
        }

        public void display_tblAddress_data()
        {
            // fill datatable from tblAddress using the addressID from tblPerson
            String addressID = (String)dataGridView1["addressID", 0].Value.ToString();

            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql statement to select record containing username from tblAddress
            cmd.CommandText = "Select * from tblAddress WHERE ((AddressID) = '" + addressID + "')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView4.DataSource = dt;
            con.Close();
        }

        public void display_NosqlData()
        {
            //method to collect data from NoSQL database 
            getData gd = new getData
            {
                username = txtUsername.Text + ""
            }; // retrieves data containing the username entered
            dal d = new dal();
            List<getData> list = d.SearchByData(gd);
            //calls method to generate a list of data from the NoSQL database
            dataGridViewNosql.DataSource = list;
        }

        public void Delete_NoSQL_Data()
        {
            //new dal form is called
            dal dl = new dal();
            //strings the data from the datagridview containing the data
            String x = dataGridViewNosql.Rows[0].Cells[0].Value.ToString();
            //DeleteData is executed
            dl.DeleteData(x);
        }
        

        public void delete_rec_tblAccount()
        {
            //opens connection
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql query to delete the record
            cmd.CommandText = "Delete from tblAccount WHERE ((username) = '" + txtUsername.Text + "')";
            //query is executed
            cmd.ExecuteNonQuery();
            //connection is closed
            con.Close();
        }

        public void delete_rec_tblAddress()
        {
            // applies variable to delete the addressID which is within the tblPerson gridview
            String addressID = (String)dataGridView1["addressID", 0].Value.ToString();
            //connection opens
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql query to delete the record
            cmd.CommandText = "Delete from tblAddress WHERE ((addressID) = '" + addressID + "')";
            //query is executed
            cmd.ExecuteNonQuery();
            //connection closes
            con.Close();
        }

        public void delete_rec_tblBank()
        {
            // applies variable to delete the bankID which is within the tblPerson gridview
            String bankID = (String)dataGridView1["bankID", 0].Value.ToString();
            //connection opens
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql query to delete the record
            cmd.CommandText = "Delete from tblBank WHERE ((bankID) = '" + bankID + "')";
            //query is executed
            cmd.ExecuteNonQuery();
            //connection closes
            con.Close();
        }

        public void delete_rec_tblPerson()
        {
            //connection opens
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql query to delete to record
            cmd.CommandText = "Delete from tblPerson WHERE ((username) = '" + txtUsername.Text + "')";
            //query is executed
            cmd.ExecuteNonQuery();
            //connection closes
            con.Close();
        }
               
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }

        private void btnIns_Click(object sender, EventArgs e)
        {
            insertNewRecord insForm = new insertNewRecord();
            insForm.Show();
            this.Hide();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            //if the datagrid is empty
            if (dataGridView1.DataSource == null)
            {
                //display error
                MessageBox.Show("Please search for username before deleting record");
            }
            else
            {
                //confirm with dialog box in whether to begin deletion process
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the record contained by the user '" 
                    + txtUsername.Text.ToUpper() + "'? ", "Delete Record", MessageBoxButtons.YesNo);
                //if yes is selected
                if (dialogResult == DialogResult.Yes)
                {
                    //run methods
                    Delete_NoSQL_Data();
                    delete_rec_tblAccount();
                    delete_rec_tblAddress();
                    delete_rec_tblBank();
                    delete_rec_tblPerson();

                    //clear data grids
                    dataGridView1.DataSource = null;
                    dataGridView5.DataSource = null;
                    dataGridView3.DataSource = null;
                    dataGridView4.DataSource = null;
                    dataGridViewNosql.DataSource = null;
                    //clear username txtbox
                    txtUsername.Clear();
                    //success message
                    MessageBox.Show("Record deleted successfully");
                }
                else if (dialogResult == DialogResult.No)
                {
                    //return to previous interface
                }
            }
        }
        
        private void dataGridViewNoSQL_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        
        private void btnSearchData_Click(object sender, EventArgs e)
        {
            // call display_tblPerson_data
            display_tblPerson_data();
            //searches the datagrid to determine if any data is found
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                //counts the cells within the datagrid
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    //if the datagrid is null OR is a database NULL OR is a whitespace or standard NULL
                    if (row.Cells[i].Value == null || row.Cells[i].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells[i].Value.ToString()))
                    {
                        //error message shows since no username was found
                        MessageBox.Show("No username found");
                        //clears datagrids so no column names are displayed
                        dataGridView1.DataSource = null;
                        dataGridView5.DataSource = null;
                        dataGridView3.DataSource = null;
                        dataGridView4.DataSource = null;
                    }
                    else
                    {
                        //run other methods since username exists
                        display_NosqlData();
                        display_tblAccount_data();
                        display_tblBank_data();
                        display_tblAddress_data();
                    }
                    break; // breaks cell count for loop
                }
                break;// breaks row foreach loop
            }
        }
        
        private void button5_Click_1(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);  // terminates program
        }
        
    }
}