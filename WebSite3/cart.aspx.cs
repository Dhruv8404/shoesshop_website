using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class cart : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Retrieve the total amount from the query string
            string totalAmount = Request.QueryString["totalAmount"];

            // Load and bind the CartItem data to the GridView
            BindCartGridView();

            // Check if the cart is empty
            if (IsCartEmpty())
            {
                // Cart is empty, hide the delivery message, total amount label, and buttons
                CartEmptyMessage.Text = "Cart Is Empty";
                DeliveryMessage.Visible = false;
                TotalAmountLabel.Visible = false;
                CashOnDeliveryButton.Visible = false;
                PaymentButton.Visible = false;
            }
            else
            {
                // Cart is not empty, show the delivery message, total amount label, and buttons
                CartEmptyMessage.Text = ""; // Clear the message if there are items in the cart
                DeliveryMessage.Visible = true;
                TotalAmountLabel.Visible = true;
                CashOnDeliveryButton.Visible = true;
                PaymentButton.Visible = true;
            }
        }
    }

    private bool IsCartEmpty()
    {
        // Implement your logic to check if the cart is empty.
        // You can check if the GridView has no rows, for example.
        return CartGridView.Rows.Count == 0;
    }

    protected void CashOnDeliveryButton_Click(object sender, EventArgs e)
    {
        // Get the user's email from the session
        string userEmail = Session["UserEmail"] as string;

        if (!string.IsNullOrEmpty(userEmail))
        {
            // Initialize a DataTable to store the order details
            DataTable orderTable = new DataTable();
            orderTable.Columns.Add("Email", typeof(string));
            orderTable.Columns.Add("ProductId", typeof(int));
            orderTable.Columns.Add("Quantity", typeof(int));
            orderTable.Columns.Add("Size", typeof(string));
            orderTable.Columns.Add("OrderDate", typeof(DateTime));
            orderTable.Columns.Add("OrderId", typeof(int)); // Add OrderId column

            // Initialize a DataTable to store the updated stock information
            DataTable stockUpdateTable = new DataTable();
            stockUpdateTable.Columns.Add("ProductId", typeof(int));
            stockUpdateTable.Columns.Add("Quantity", typeof(int));

            // Iterate through the GridView rows to extract data
            foreach (GridViewRow row in CartGridView.Rows)
            {
                int productId = Convert.ToInt32(row.Cells[0].Text); // Assuming ProductId is in the first column
                int quantity = Convert.ToInt32(row.Cells[4].Text);  // Assuming Quantity is in the fifth column
                string size = row.Cells[2].Text;                    // Assuming Size is in the third column
                DateTime orderDate = DateTime.Now;

                // Generate a random order ID
                Random random = new Random();
                int orderId = random.Next(10000, 99999); // Generates a random 5-digit number

                // Add the data to the DataTable
                orderTable.Rows.Add(userEmail, productId, quantity, size, orderDate, orderId);

                // Add the data to the stock update DataTable
                stockUpdateTable.Rows.Add(productId, quantity);
            }

            // Define your connection string
            string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Start a transaction to ensure that both inserts and updates are atomic
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Use SqlBulkCopy to efficiently insert the order data into the UserOrder table
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = "UserOrder"; // Specify the destination table
                        bulkCopy.WriteToServer(orderTable);          // Write the data from the DataTable to the table
                    }

                    // Update the stock in the ProductStock table
                    foreach (DataRow stockRow in stockUpdateTable.Rows)
                    {
                        int productId = Convert.ToInt32(stockRow["ProductId"]);
                        int quantity = Convert.ToInt32(stockRow["Quantity"]);

                        // Define your UPDATE query to decrement the stock
                        string updateStockQuery = "UPDATE ProductStock SET Stock = Stock - @Quantity WHERE ProductId = @ProductId";

                        using (SqlCommand cmdStockUpdate = new SqlCommand(updateStockQuery, con, transaction))
                        {
                            // Set the parameters for the update
                            cmdStockUpdate.Parameters.AddWithValue("@ProductId", productId);
                            cmdStockUpdate.Parameters.AddWithValue("@Quantity", quantity);

                            // Execute the UPDATE query
                            cmdStockUpdate.ExecuteNonQuery();
                        }
                    }

                    // Define your DELETE query to remove products from CartItem4 table
                    string deleteQuery = "DELETE FROM CartItem4 WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con, transaction))
                    {
                        // Set the parameter for the user's email
                        cmd.Parameters.AddWithValue("@Email", userEmail);

                        // Execute the DELETE query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Commit the transaction if all operations succeed
                            transaction.Commit();

                            // Refresh the GridView to reflect the changes
                            BindCartGridView();

                            // You can also display a confirmation message here
                            PlaceOrderMessageLabel.Text = "Order Placed Successfully! Your cart is now empty.";
                        }
                        else
                        {
                            // Rollback the transaction if the delete operation fails
                            transaction.Rollback();

                            // Handle the case where no rows were deleted
                            PlaceOrderMessageLabel.Text = "Failed to place the order. Please try again later.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during insert or update operations
                    transaction.Rollback();
                    PlaceOrderMessageLabel.Text = "An error occurred: " + ex.Message;
                }
                finally
                {
                    transaction.Dispose();
                    con.Close();
                }
            }
        }
        else
        {
            // Handle the case where the user is not logged in or the session variable is not set.
            // You can redirect the user to the login page or display a message.
        }
        DeliveryMessage.Visible = false;

        // Hide the Total Amount label
        TotalAmountLabel.Visible = false;

        // Hide both buttons after purchasing
        CashOnDeliveryButton.Visible = false;
        PaymentButton.Visible = false;

        // Update the CartEmptyMessage to indicate that the cart is empty
        CartEmptyMessage.Text = "Cart Is Empty";
        CartEmptyMessage.Visible = true;
    }

    private void BindCartGridView()
    {
        // Get the user's email from the session
        string userEmail = Session["UserEmail"] as string;

        if (!string.IsNullOrEmpty(userEmail))
        {
            string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT c.ProductId, c.Quantity, c.Size, p.ProductName, p.Price, p.ProductImage " +
                                                        "FROM CartItem4 c " +
                                                        "INNER JOIN ProductDetail p ON c.ProductId = p.ProductId " +
                                                        "WHERE c.Email = @Email", con))
                {
                    cmd.Parameters.AddWithValue("@Email", userEmail);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    CartGridView.DataSource = reader;
                    CartGridView.DataBind();
                    con.Close();
                }
            }

            // Calculate the total amount
            decimal totalAmount = 0;
            bool isFreeDelivery = false;

            foreach (GridViewRow row in CartGridView.Rows)
            {
                decimal price = Convert.ToDecimal(row.Cells[3].Text); // Price is in the fourth column (zero-based index)
                int quantity = Convert.ToInt32(row.Cells[4].Text); // Quantity is in the fifth column (zero-based index)
                decimal rowTotal = price * quantity;
                totalAmount += rowTotal;
            }

            // Check if totalAmount is greater than 10000 for free delivery
            if (totalAmount > 10000)
            {
                isFreeDelivery = true;
                TotalAmountLabel.Text = totalAmount.ToString("C");
            }

            if (isFreeDelivery)
            {
                DeliveryMessage.InnerHtml = "<p>Delivery: Free</p>";
            }
            else
            {
                totalAmount += 100; // Add delivery charge
                TotalAmountLabel.Text = totalAmount.ToString("C");
                DeliveryMessage.InnerHtml = "<p>Delivery Charge: Rs100</p>";
            }
        }
        else
        {
            // Handle the case where the user is not logged in or the session variable is not set.
            // You can redirect the user to the login page or display a message.
        }
    }


    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        // Get the ProductId, Size, and Quantity from the CommandArgument of the button
        Button deleteButton = (Button)sender;
        string[] args = deleteButton.CommandArgument.Split(',');
        int productId = Convert.ToInt32(args[0]);
        string size = args[1];
        int quantity = Convert.ToInt32(args[2]);

        // Define your connection string
        string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            // Define your DELETE query
            string deleteQuery = "DELETE FROM CartItem4 WHERE ProductId = @ProductId AND Size = @Size AND Quantity = @Quantity";

            using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
            {
                // Set the parameters
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Size", size);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                // Execute the DELETE query
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Rows were deleted successfully; you can provide feedback to the user if needed
                    // Refresh the GridView to reflect the changes
                    BindCartGridView();
                }
                else
                {
                    // Handle the case where no rows were deleted (e.g., the specified conditions did not match any rows)
                }
            }

            con.Close();
        }
        if (IsCartEmpty())
        {
            // Update UI for an empty cart
            CartEmptyMessage.Text = "Cart Is Empty";
            DeliveryMessage.Visible = false;
            TotalAmountLabel.Visible = false;
            CashOnDeliveryButton.Visible = false;
            PaymentButton.Visible = false;
        }
        else
        {
            // Update UI for a non-empty cart
            CartEmptyMessage.Text = "";
            DeliveryMessage.Visible = true;
            TotalAmountLabel.Visible = true;
            CashOnDeliveryButton.Visible = true;
            PaymentButton.Visible = true;
        }
    }

    protected void PaymentButton_Click(object sender, EventArgs e)
    {
        // Construct the query string with additional parameters
        string queryString = "totalAmount=" + TotalAmountLabel.Text;

        // Iterate through each row in the GridView to add product information to the query string
        foreach (GridViewRow row in CartGridView.Rows)
        {
            // Get the ProductId, Quantity, and Size from each row
            string productId = row.Cells[0].Text;
            string quantity = row.Cells[4].Text;
            string size = row.Cells[2].Text;

            // Append the product information to the query string
            queryString += "&productId=" + productId;
            queryString += "&quantity=" + quantity;
            queryString += "&size=" + size;
        }

        // Redirect the user to the payment page along with the query string
        Response.Redirect("PaymentForm.aspx?" + queryString);
    }
}
