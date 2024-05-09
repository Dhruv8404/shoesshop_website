using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

public partial class PaymentForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Retrieve the total amount from the query string
            string totalAmount = Request.QueryString["totalAmount"];
            if (!string.IsNullOrEmpty(totalAmount))
            {
                // Display or use the total amount as needed
                txtTotalAmount.Text = totalAmount;
            }
        }

        // Ensure this is not inside the !IsPostBack block so that it is executed every time the page is loaded
        // Retrieve the user's email from the session and ensure the user is logged in
        string userEmail = Session["UserEmail"] as string;
        if (string.IsNullOrEmpty(userEmail))
        {
            // Redirect the user to the login page or handle the case where the user is not logged in
            Response.Redirect("LoginPage.aspx");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // Retrieve payment details from the form
        string cardNumber = txtCardNumber.Text;
        string expDate = txtExpDate.Text;
        string cvv = txtCVV.Text;
        string totalAmount = txtTotalAmount.Text; // Retrieve total amount from the textbox

        // Check if input lengths are valid
        if (cardNumber.Length != 12 || expDate.Length != 4 || cvv.Length != 3)
        {
            ErrorMessageLabel.Text = "Please enter valid values for Credit Card Number (12 digits), Expiration Date (4 digits), and CVV (3 digits).";
            return;
        }

        // Retrieve user email from session
        string userEmail = Session["UserEmail"] as string;

        // Define your connection string
        string connectionString = "Data Source=DESKTOP-B3AMDGK\\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

        // Define your UPDATE query
        string updateQuery = "UPDATE ProductStock SET Stock = Stock - @Quantity WHERE ProductId = @ProductId";

        // Generate a random OrderId
        Random random = new Random();
        int orderId = random.Next(10000, 99999); // Generates a random 5-digit number

        // Get the next available PaymentID
        int nextPaymentID;
        // Define your INSERT query for PaymentDetails
        string insertPaymentQuery = "INSERT INTO PaymentDetails (PaymentID, OrderId, CardNumber, ExpirationDate, CVV, Amount, PaymentDate, UserEmail) " +
                                    "VALUES (@PaymentID, @OrderId, @CardNumber, @ExpirationDate, @CVV, @Amount, GETDATE(), @UserEmail)";

        // Define your INSERT query for UserOrder with the generated OrderId
        string insertUserOrderQuery = "INSERT INTO UserOrder (OrderId, ProductId, Quantity, Size, Orderdate, Email) " +
                                       "SELECT @OrderId, ProductId, Quantity, Size, GETDATE(), @UserEmail FROM CartItem4 WHERE Email = @UserEmail";

        try
        {
            // Create a SqlConnection object
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Open the connection
                con.Open();

                // Begin a transaction
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Retrieve cart items for the user from the database using the userEmail
                    DataTable cartItems = RetrieveCartItems(userEmail, con, transaction);

                    if (cartItems != null && cartItems.Rows.Count > 0)
                    {
                        // Create a SqlCommand object for the UPDATE query
                        using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, con, transaction))
                        {
                            // Iterate through each cart item
                            foreach (DataRow row in cartItems.Rows)
                            {
                                int productId = Convert.ToInt32(row["ProductId"]);
                                int quantity = Convert.ToInt32(row["Quantity"]);

                                // Add parameters to the UPDATE command
                                cmdUpdate.Parameters.Clear(); // Clear previous parameters
                                cmdUpdate.Parameters.AddWithValue("@Quantity", quantity);
                                cmdUpdate.Parameters.AddWithValue("@ProductId", productId);

                                // Execute the UPDATE command
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }
                    }

                    // Get the next available PaymentID
                    nextPaymentID = GetNextPaymentID(con, transaction);

                    // Insert payment details into PaymentDetails table
                    using (SqlCommand cmdInsertPayment = new SqlCommand(insertPaymentQuery, con, transaction))
                    {
                        // Add parameters to the INSERT command for PaymentDetails
                        cmdInsertPayment.Parameters.AddWithValue("@PaymentID", nextPaymentID);
                        cmdInsertPayment.Parameters.AddWithValue("@OrderId", orderId);
                        cmdInsertPayment.Parameters.AddWithValue("@CardNumber", cardNumber);
                        cmdInsertPayment.Parameters.AddWithValue("@ExpirationDate", expDate);
                        cmdInsertPayment.Parameters.AddWithValue("@CVV", cvv);
                        cmdInsertPayment.Parameters.AddWithValue("@Amount", totalAmount);
                        cmdInsertPayment.Parameters.AddWithValue("@UserEmail", userEmail);

                        // Execute the INSERT command for PaymentDetails
                        cmdInsertPayment.ExecuteNonQuery();
                    }

                    // Insert user order details into UserOrder table
                    using (SqlCommand cmdInsertUserOrder = new SqlCommand(insertUserOrderQuery, con, transaction))
                    {
                        // Add parameters for OrderId and the user's email
                        cmdInsertUserOrder.Parameters.AddWithValue("@OrderId", orderId);
                        cmdInsertUserOrder.Parameters.AddWithValue("@UserEmail", userEmail);

                        // Execute the INSERT command for UserOrder
                        cmdInsertUserOrder.ExecuteNonQuery();
                    }

                    // Delete cart items for the user from the CartItem4 table
                    DeleteCartItems(userEmail, con, transaction);
                    // Commit the transaction
                    transaction.Commit();
                    // Redirect to payment success page with PaymentID, totalAmount, and date as query parameters

                
                    DateTime currentDate = DateTime.Now;
                    string redirectUrl = "PaymentSuccess.aspx?PaymentID=" + nextPaymentID + "&OrderId=" + orderId + "&totalAmount=" + totalAmount + "&date=" + currentDate.ToString("yyyy-MM-dd HH:mm:ss");
                    Response.Redirect(redirectUrl);


                    // Clear form data after successful submission
                    txtCardNumber.Text = string.Empty;
                    txtExpDate.Text = string.Empty;
                    txtCVV.Text = string.Empty;
                    txtTotalAmount.Text = string.Empty;

                }
                catch (Exception ex)
                {
                    // Rollback the transaction if an exception occurs
                    transaction.Rollback();
                    ErrorMessageLabel.Text = "An error occurred: " + ex.Message;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any other exceptions
            ErrorMessageLabel.Text = "An error occurred: " + ex.Message;
        }
    }

    // Method to retrieve cart items for the user from the database using the userEmail
    private DataTable RetrieveCartItems(string userEmail, SqlConnection con, SqlTransaction transaction)
    {
        // Define your SELECT query to retrieve cart items for the user
        string selectQuery = "SELECT ProductId, Quantity FROM CartItem4 WHERE Email = @Email";

        // Create a SqlCommand object for the SELECT query
        using (SqlCommand cmd = new SqlCommand(selectQuery, con, transaction))
        {
            // Add parameter for the user's email
            cmd.Parameters.AddWithValue("@Email", userEmail);

            // Create a DataTable to store the cart items
            DataTable cartItems = new DataTable();

            // Use a SqlDataAdapter to fill the DataTable with the results of the query
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(cartItems);
            }

            return cartItems;
        }
    }
    private void DeleteCartItems(string userEmail, SqlConnection con, SqlTransaction transaction)
    {
        // Define your DELETE query to delete cart items for the user
        string deleteQuery = "DELETE FROM CartItem4 WHERE Email = @Email";

        // Create a SqlCommand object for the DELETE query
        using (SqlCommand cmd = new SqlCommand(deleteQuery, con, transaction))
        {
            // Add parameter for the user's email
            cmd.Parameters.AddWithValue("@Email", userEmail);

            // Execute the DELETE command
            cmd.ExecuteNonQuery();
        }
    }
    // Method to get the next available PaymentID
    private int GetNextPaymentID(SqlConnection con, SqlTransaction transaction)
    {
        string selectQuery = "SELECT ISNULL(MAX(PaymentID), 9999) + 1 FROM PaymentDetails";
        using (SqlCommand cmd = new SqlCommand(selectQuery, con, transaction))
        {
            object result = cmd.ExecuteScalar();
            int nextPaymentID;
            if (result != DBNull.Value)
            {
                nextPaymentID = Convert.ToInt32(result);
                if (nextPaymentID < 10000)
                {
                    nextPaymentID = 10000; // Ensure the minimum PaymentID is 10000
                }
            }
            else
            {
                nextPaymentID = 10000; // If no PaymentID exists yet, start from 10000
            }
            return nextPaymentID;
        }
    }
}
