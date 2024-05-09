using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

public partial class productupdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Bind data to the GridView
            BindGridView();
        }
    }

    protected void BindGridView()
    {
        string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True"; // Replace with your database connection string
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM ProductDetail", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
  

    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        // Implement the database delete operation using productId
        string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True"; // Replace with your database connection string
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            // Delete from ProductDetail table
            SqlCommand cmd1 = new SqlCommand("DELETE FROM ProductDetail WHERE ProductID = @ProductID", con);
            cmd1.Parameters.AddWithValue("@ProductID", TextBox1.Text);
            cmd1.ExecuteNonQuery();

            // Delete from ProductStock table
            SqlCommand cmd2 = new SqlCommand("DELETE FROM ProductStock WHERE ProductID = @ProductID", con);
            cmd2.Parameters.AddWithValue("@ProductID", TextBox1.Text);
            cmd2.ExecuteNonQuery();
        }
    }

    protected void InsertButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TextBox1.Text) && !string.IsNullOrWhiteSpace(TextBox2.Text) && !string.IsNullOrWhiteSpace(TextBox3.Text) && !string.IsNullOrWhiteSpace(TextBox4.Text) && FileUpload1.HasFile)
        {
            try
            {
                // Save the file to the server
                string fileName = Path.GetFileName(FileUpload1.FileName);
                string filePath = "~/image/" + fileName; // Relative path to save in database
                FileUpload1.SaveAs(Server.MapPath(filePath));

                // Insert data into the database (ProductDetail table)
                string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO ProductDetail (ProductId, ProductName, Price, ProductImage) VALUES (@ProductId, @ProductName, @Price, @ProductImage)", con);
                    cmd.Parameters.AddWithValue("@ProductId", TextBox1.Text);
                    cmd.Parameters.AddWithValue("@ProductName", TextBox2.Text);
                    cmd.Parameters.AddWithValue("@Price", TextBox3.Text);
                    cmd.Parameters.AddWithValue("@ProductImage", filePath); // Save file path in the database
                    cmd.ExecuteNonQuery();
                }

                // Insert data into the database (ProductStock table)
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO ProductStock (ProductId, ProductName, Price, ProductImage, Stock) VALUES (@ProductId, @ProductName, @Price, @ProductImage, @Stock)", con);
                    cmd.Parameters.AddWithValue("@ProductId", TextBox1.Text);
                    cmd.Parameters.AddWithValue("@ProductName", TextBox2.Text);
                    cmd.Parameters.AddWithValue("@Price", TextBox3.Text);
                    cmd.Parameters.AddWithValue("@ProductImage", filePath); // Save file path in the database
                    cmd.Parameters.AddWithValue("@Stock", int.Parse(TextBox4.Text)); // Parse stock from TextBox4
                    cmd.ExecuteNonQuery();
                }

                Label5.Text = "Data inserted successfully!";
            }
            catch (Exception ex)
            {
                Label5.Text = "Error: " + ex.Message;
            }
        }
        else
        {
            Label5.Text = "Please fill out all fields and select an image to insert data.";
        }

        // Refresh the GridView for both tables
        BindGridView();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile && !string.IsNullOrWhiteSpace(TextBox1.Text) && !string.IsNullOrWhiteSpace(TextBox2.Text) && !string.IsNullOrWhiteSpace(TextBox3.Text) && !string.IsNullOrWhiteSpace(TextBox4.Text))
        {
            try
            {
                // Save the file to the server
                string fileName = Path.GetFileName(FileUpload1.FileName);
                string filePath = "~/image/" + fileName; // Relative path to save in database
                FileUpload1.SaveAs(Server.MapPath(filePath));

                // Update data in the database (ProductDetail table)
                string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE ProductDetail SET ProductName = @ProductName, Price = @Price, ProductImage = @ProductImage WHERE ProductID = @ProductID", con);
                    cmd.Parameters.AddWithValue("@ProductID", TextBox1.Text);
                    cmd.Parameters.AddWithValue("@ProductName", TextBox2.Text);
                    cmd.Parameters.AddWithValue("@Price", TextBox3.Text); // Assuming Price is a decimal type in the database

                    // Store the file path in the database
                    cmd.Parameters.AddWithValue("@ProductImage", filePath);

                    cmd.ExecuteNonQuery();
                }

                // Update data in the database (ProductStock table)
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE ProductStock SET ProductName = @ProductName, Price = @Price, ProductImage = @ProductImage, Stock = @Stock WHERE ProductID = @ProductID", con);
                    cmd.Parameters.AddWithValue("@ProductID", TextBox1.Text);
                    cmd.Parameters.AddWithValue("@ProductName", TextBox2.Text);
                    cmd.Parameters.AddWithValue("@Price", TextBox3.Text); // Assuming Price is a decimal type in the database
                    cmd.Parameters.AddWithValue("@ProductImage", filePath);
                    cmd.Parameters.AddWithValue("@Stock", int.Parse(TextBox4.Text));

                    cmd.ExecuteNonQuery();
                }

                Label5.Text = "Data updated successfully!";
            }
            catch (Exception ex)
            {
                Label5.Text = "Error: " + ex.Message;
            }
        }
        else
        {
            Label5.Text = "Please fill out all fields and select an image to update.";
        }

        // Refresh the GridView
        BindGridView();
    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
       
        if (e.CommandName == "EditRecord")
        {
            // Handle edit operation
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            string productId = GridView1.DataKeys[rowIndex]["ProductID"].ToString();

            // Retrieve the record to edit (similar to your existing code)
            // Populate the form fields with the data
            // ...

            // You can implement an update operation based on user input here
            // ...
        }
    }
    protected void GetDataButton_Click(object sender, EventArgs e)
    {
        // Call a method to retrieve and display all products
        DisplayAllProducts();
    }

  

    private void DisplayAllProducts()
    {
        // Implement code to retrieve all products from the database and display them
        string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True"; // Replace with your database connection string
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM ProductDetail", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }

  


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
