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
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HonsProject
{
    public partial class insertNewRecord : Form
    {
        public insertNewRecord()
        {
            InitializeComponent();
        }
        
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MyInstance;Initial Catalog=HonsProject;Integrated Security=True");

        char gender;
        String addressID;
        String bankID;
        bool sub;
      
        private void insertNewRecord_Load(object sender, EventArgs e)
        {

        }

        public void insert_tblAccount()
        {
            //opens sql connection
            con.Open();
            //set subsciption variable
            setSub();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql insertion query using parameters set below
            cmd.CommandText = "insert into tblAccount(username, email, website, is_subscriber) values(@username, @email, @website, @isSub)";
            //creates values from parameters from the textboxes
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = txtUsername.Text;
            cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = txtEmail.Text;
            //if the textbox is empty the parameters value will equal null
            if (String.IsNullOrWhiteSpace(txtWebsite.Text))
            {
                //declares variable as null
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@website", Value = DBNull.Value });
            }
            else
            {
                //decalres variable within the textbox 
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@website", Value = txtWebsite.Text });
            }
            cmd.Parameters.Add("@website", SqlDbType.VarChar).Value = txtWebsite.Text;
            cmd.Parameters.Add("@isSub", SqlDbType.Bit).Value = sub;
            cmd.ExecuteNonQuery();
            //closes the connection
            con.Close();
        }

        public void insert_tblBank()
        {
            //opens sql connection 
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //creates values from parameters from the textboxes
            cmd.Parameters.Add("@bank_account", SqlDbType.VarChar).Value = txtBankAcc.Text;
            cmd.Parameters.Add("@card_type", SqlDbType.VarChar).Value = txtBankCard.Text;
            //sql insertion query using parameters set above
            cmd.CommandText = @"insert into tblBank(bank_account, card_type) values(@bank_account, @card_type)";
            cmd.ExecuteNonQuery();
            //closes connection
            con.Close();
        }

        public void insert_tblAddress()
        {
            //opens sql connection
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql insertion query using parameters set below
            cmd.CommandText = "insert into tblAddress(address, post_code, country) values(@address, @post_code, @country)";
            //creates values from parameters from the textboxes
            cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = txtAddress.Text;
            //if the textbox is empty the parameters value will equal null
            if (String.IsNullOrWhiteSpace(txtpCode.Text))
            {
                //declares variable as null
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@post_code", Value = DBNull.Value });
            }
            else
            {
                //decalres variable within the textbox 
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@post_code", Value = txtpCode.Text });
            }
            cmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = txtCountry.Text;
            cmd.ExecuteNonQuery();
            //closes connection
            con.Close();
        }

        public void insert_tblPerson()
        {
            //opens the connection
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //sql insert query using parameters set below
            cmd.CommandText = "insert into tblPerson(first_name, last_name, gender, username, addressID, bankID) values(@first_name, @last_name, @gender, @username, @addressID, @bankID)";
            //creates values from parameters from the textboxes and set variables 
            cmd.Parameters.Add("@first_name", SqlDbType.VarChar).Value = txtfName.Text;
            cmd.Parameters.Add("@last_name", SqlDbType.VarChar).Value = txtsName.Text;
            cmd.Parameters.Add("@gender", SqlDbType.Char).Value = gender;
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = txtUsername.Text;
            cmd.Parameters.Add("@addressID", SqlDbType.Int).Value = addressID;
            cmd.Parameters.Add("@bankID", SqlDbType.Int).Value = bankID;
            //executes the query
            cmd.ExecuteNonQuery();
            //closes the connection
            con.Close();
        }

        public void get_AddressID()
        {
            con.Open();
            //sql query to select the last entered identity
            SqlCommand sqlcmd = new SqlCommand("SELECT IDENT_CURRENT('tblAddress')", con);
            //command executes
            sqlcmd.ExecuteNonQuery();
            //declares new datatable
            DataTable dt = new DataTable();
            //loads data into the sqldataadapter
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            //fills the data table with the data contained in the data adapter
            da.Fill(dt);
            //strings the data to addressID
            addressID = dt.Rows[0][0].ToString();
            //closes connection
            con.Close();
        }

        public void get_BankID()
        {
            con.Open();
            //sql query to select the last entered identity
            SqlCommand sqlcmd = new SqlCommand("SELECT IDENT_CURRENT('tblBank')", con);
            //command executes
            sqlcmd.ExecuteNonQuery();
            //declares new datatable
            DataTable dt = new DataTable();
            //loads data into the sqldataadapter
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            //strings the data to bankID
            bankID = dt.Rows[0][0].ToString();
            //closes connection
            con.Close();
        }

        public void insertNoSQL()
        {
            //decalre variable to parse
            double lat;
            double lon;
            //parses the textbox variables to double
            lat = double.Parse(txtLatitude.Text);
            lon = double.Parse(txtLongitude.Text);

            //declare forms
            dal d = new dal();
            getData gd = new getData
            {
                //sets variables from getData
                username = txtUsername.Text,
                latitude = lat,
                longitude = lon,
                imageUploaded = txtURL.Text,
                lastComment = txtComment.Text

            };
                //calls Create record from dal using the data set from getData
                d.CreateRecord(gd);
                txtUsername.Text = txtUsername.Text;
                txtLatitude.Text = "";
                txtLongitude.Text = "";
                txtURL.Text = "";
                txtComment.Text = "";
            }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Show();
            this.Hide();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // textboxes must have data in order to write to the databases
            if (txtUsername.Text == "" || txtfName.Text == "" || txtsName.Text == "" || txtAddress.Text == "" || txtCountry.Text == "" 
                || txtBankAcc.Text == "" || txtBankCard.Text == "" || txtEmail.Text == "" || txtSub.Text == "" || txtLongitude.Text == "" 
                || txtLatitude.Text == "" || txtURL.Text == "" || txtComment.Text == "")
            {
                //displays error
                MessageBox.Show("Please fill out all mandatory criteria");
            }
            //one radio button must be selected
            else if (!rdoFemale.Checked && !rdoMale.Checked)
            {   
                //displays error
                MessageBox.Show("Please select a gender");
            }
            //if the subsciption does not contain the specified values
            else if (!(txtSub.Text.Contains("True") || txtSub.Text.Contains("true") || txtSub.Text.Contains("False") || txtSub.Text.Contains("false")))
            {
                //displays error
                MessageBox.Show("Subcription must be 'True' or 'False'");
            }
            else
            {
                //executes insertion methods
                insertNoSQL();
                insert_tblAccount();
                insert_tblAddress();
                insert_tblBank();
                //collects ID to insert into tblPerson
                get_AddressID();
                get_BankID();
                insert_tblPerson();
                //displays message of successful insertion
                MessageBox.Show("New Record inserted successfully");
                Form1 frm1 = new Form1();
                this.Close();
                //shows main form
                frm1.Show();
            }
        }

        private void rdoMale_CheckedChanged_1(object sender, EventArgs e)
        {
            gender = 'M';
        }

        private void rdoFemale_CheckedChanged(object sender, EventArgs e)
        {
            gender = 'F';
        }

        private void txtpCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSub_TextChanged(object sender, EventArgs e)
        {

        }

        public void setSub()
        {
            if (txtSub.Text == "True" || txtSub.Text == "true")
            {
                sub = true;
            }
            else if (txtSub.Text == "False" || txtSub.Text == "false")
            {
                sub = false;
            }
        }
    }
 }
