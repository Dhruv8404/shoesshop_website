using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Load products from the database and bind them to the DataList
            BindProductDataList();
        }
    }
    private void BindProductDataList()
    {
        string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 6 ProductId, ProductName, Price, ProductImage FROM ProductDetail", con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ProductDataList.DataSource = reader;
                    ProductDataList.DataBind();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            // Handle the exception (e.g., log the error, display a user-friendly message)
            // You can also use debugging tools to inspect the exception details
            Console.WriteLine("An error occurred while binding ProductDataList: " + ex.Message);
        }
    }



    protected void AddToCartButton_Click(object sender, EventArgs e)
    {
        // Get the ProductId from the CommandArgument of the button
        Button addToCartButton = (Button)sender;
        int productId = Convert.ToInt32(addToCartButton.CommandArgument);

        // Use the userEmail variable to associate the product with the user
        string userEmail = Session["UserEmail"] as string;

        if (!string.IsNullOrEmpty(userEmail))
        {
            // Find the QuantityTextBox and SizeDropDown in the DataList item that corresponds to the clicked button
            DataListItem dataListItem = (DataListItem)addToCartButton.NamingContainer;
            TextBox quantityTextBox = (TextBox)dataListItem.FindControl("QuantityTextBox");
            DropDownList sizeDropDown = (DropDownList)dataListItem.FindControl("SizeDropDown");

            if (quantityTextBox != null && sizeDropDown != null)
            {
                int quantity = Convert.ToInt32(quantityTextBox.Text);
                string size = sizeDropDown.SelectedValue;

                string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if the product is available (stock > 0)
                    using (SqlCommand checkStockCmd = new SqlCommand("SELECT Stock FROM ProductStock WHERE ProductId = @ProductId", con))
                    {
                        checkStockCmd.Parameters.AddWithValue("@ProductId", productId);
                        int stock = Convert.ToInt32(checkStockCmd.ExecuteScalar());

                        if (stock < 1)
                        {
                            // Product is unavailable, display a message
                            // You can use a label to display the message on the page
                            // For demonstration purposes, let's use JavaScript alert
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('This product is currently unavailable.');", true);
                            return; // Stop further processing
                        }
                    }

                    // Check if the CartItem already exists for the user, product, and size
                    using (SqlCommand checkCmd = new SqlCommand("SELECT Quantity FROM CartItem4 WHERE Email = @Email AND ProductId = @ProductId AND Size = @Size", con))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", userEmail);
                        checkCmd.Parameters.AddWithValue("@ProductId", productId);
                        checkCmd.Parameters.AddWithValue("@Size", size);

                        object existingQuantityObj = checkCmd.ExecuteScalar();

                        if (existingQuantityObj != null)
                        {
                            int existingQuantity = Convert.ToInt32(existingQuantityObj);

                            if (quantity > existingQuantity)
                            {
                                // Update the quantity if necessary
                                using (SqlCommand updateCmd = new SqlCommand("UPDATE CartItem4 SET Quantity = @Quantity WHERE Email = @Email AND ProductId = @ProductId AND Size = @Size", con))
                                {
                                    updateCmd.Parameters.AddWithValue("@Email", userEmail);
                                    updateCmd.Parameters.AddWithValue("@ProductId", productId);
                                    updateCmd.Parameters.AddWithValue("@Quantity", quantity);
                                    updateCmd.Parameters.AddWithValue("@Size", size);

                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                            // Handle other cases if needed
                        }
                        else
                        {
                            // Insert the product into the cart
                            using (SqlCommand insertCmd = new SqlCommand("INSERT INTO CartItem4 (Email, ProductId, Quantity, Size) VALUES (@Email, @ProductId, @Quantity, @Size)", con))
                            {
                                insertCmd.Parameters.AddWithValue("@Email", userEmail);
                                insertCmd.Parameters.AddWithValue("@ProductId", productId);
                                insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                                insertCmd.Parameters.AddWithValue("@Size", size);

                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    con.Close();
                }
            }
        }
        else
        {
            // Handle the case where the user is not logged in or the session variable is not set.
            // You can redirect the user to the login page or display a message.
        }
        Response.Redirect(Request.Url.AbsoluteUri);
    }
}
