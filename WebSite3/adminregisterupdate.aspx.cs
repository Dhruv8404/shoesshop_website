using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class adminregisterupdate : System.Web.UI.Page
{
    private readonly string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Load user data into the GridView when the page first loads
            LoadUserData();
        }
    }

    protected void gvUserData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteUser")
        {
            int rowIndex;
            if (int.TryParse(e.CommandArgument.ToString(), out rowIndex) && rowIndex >= 0 && rowIndex < gvUserData.Rows.Count)
            {
                string email = gvUserData.DataKeys[rowIndex]["Email"].ToString(); // Assuming Email is a unique identifier

                // Implement the delete operation here
                DeleteUserByEmail(email);

                // Reload user data after deletion
                LoadUserData();
            }
        }
    }

    protected void DeleteUserByEmail(string email)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string deleteQuery = "DELETE FROM Users WHERE Email = @Email";
            SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection);
            deleteCmd.Parameters.AddWithValue("@Email", email);

            int rowsAffected = deleteCmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                ShowMessage("User data deleted successfully!", System.Drawing.Color.Green);
            }
            else
            {
                ShowMessage("User data deletion failed.", System.Drawing.Color.Red);
            }
        }
    }

    protected void LoadUserData()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string selectQuery = "SELECT * FROM Users"; // Select all columns
            SqlCommand selectCmd = new SqlCommand(selectQuery, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(selectCmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                // Bind the data to the GridView control
                gvUserData.DataSource = dataTable;
                gvUserData.DataBind();
            }
            else
            {
                ShowMessage("No users found.", System.Drawing.Color.Red);
            }
        }
    }

    protected void btnUpdateUser_Click(object sender, EventArgs e)
    {
        // Retrieve user input
        string email = txtEmail.Text.Trim();
        string name = txtName.Text.Trim();
        string contact = txtContact.Text.Trim();
        string password = txtPassword.Text;
        string address = txtAddress.Text.Trim();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string updateQuery = "UPDATE Users SET Name = @Name, Contact = @Contact, Password = @Password, Address = @Address WHERE Email = @Email";
            SqlCommand updateCmd = new SqlCommand(updateQuery, connection);
            updateCmd.Parameters.AddWithValue("@Name", name);
            updateCmd.Parameters.AddWithValue("@Contact", contact);
            updateCmd.Parameters.AddWithValue("@Password", password);
            updateCmd.Parameters.AddWithValue("@Address", address);
            updateCmd.Parameters.AddWithValue("@Email", email);

            int rowsAffected = updateCmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                ShowMessage("User data updated successfully!", System.Drawing.Color.Green);
            }
            else
            {
                ShowMessage("User data update failed.", System.Drawing.Color.Red);
            }
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        // Retrieve user input
        string name = txtName.Text.Trim();
        string email = txtEmail.Text.Trim();
        string contact = txtContact.Text.Trim();
        string password = txtPassword.Text;
        string address = txtAddress.Text.Trim();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
            checkCmd.Parameters.AddWithValue("@Email", email);
            int emailCount = (int)checkCmd.ExecuteScalar();

            if (emailCount > 0)
            {
                ShowMessage("This email address is already registered.", System.Drawing.Color.Red);
            }
            else
            {
                // Email address is not in the database, proceed with registration
                string insertQuery = "INSERT INTO Users (Name, Email, Contact, Password, Address) VALUES (@Name, @Email, @Contact, @Password, @Address)";
                SqlCommand cmd = new SqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Contact", contact);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Address", address);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Registration successful
                        ShowMessage("Registration successful!", System.Drawing.Color.Green);
                        Session["UserEmail"] = email;

                        // Other code...
                    }
                    else
                    {
                        // Registration failed
                        ShowMessage("Registration failed. Please try again later.", System.Drawing.Color.Red);
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error: " + ex.Message, System.Drawing.Color.Red);
                }
            }
        }
    }

    protected void btnGetUserData_Click(object sender, EventArgs e)
    {
        // Reload user data when the "Get User Data" button is clicked
        LoadUserData();
    }

    private void ShowMessage(string message, System.Drawing.Color color)
    {
        lblErrorMessage.Visible = true;
        lblErrorMessage.Text = message;
        lblErrorMessage.ForeColor = color;
    }
}
