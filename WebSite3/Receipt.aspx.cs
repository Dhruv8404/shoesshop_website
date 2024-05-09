using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Receipt : System.Web.UI.Page
{
    // Field to store delivery date
    protected DateTime DeliveryDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Check if OrderId query parameter exists
            if (!string.IsNullOrEmpty(Request.QueryString["OrderId"]))
            {
                string orderId = Request.QueryString["OrderId"];
                PopulateReceipt(orderId);

                // Check if the current date is greater than or equal to the delivery date
                if (DateTime.Today >= DeliveryDate)
                {
                    // Show the feedback-related controls
                    FeedbackTextBox.Visible = true;
                    SubmitFeedbackButton.Visible = true;
                    FeedbackLabel.Visible = true;

                    // Check if the delivery date matches the current date
                    if (DateTime.Today == DeliveryDate)
                    {
                        DeliveryStatusLabel.Text = "Product is delivered today.";
                        DeliveryStatusLabel.ForeColor = System.Drawing.Color.Green;
                    }
                }
                else
                {
                    // Hide the feedback-related controls
                    FeedbackTextBox.Visible = false;
                    SubmitFeedbackButton.Visible = false;
                    FeedbackLabel.Visible = false;

                    // Hide the checkboxes for feedback
                    Checkbox1.Visible = false;
                    Checkbox2.Visible = false;
                    Checkbox3.Visible = false;
                    Checkbox4.Visible = false;
                    Checkbox5.Visible = false;
                    // Display message in DeliveryStatusLabel
                    DeliveryStatusLabel.Text = "Product is not delivered.";
                    DeliveryStatusLabel.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                // Handle case where OrderId query parameter is missing
                // Redirect or display an error message
            }
        }
    }


    protected void PopulateReceipt(string orderId)
    {
        string connectionString = @"Data Source=DESKTOP-B3AMDGK\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Fetch order details including order date
            string orderQuery = "SELECT * FROM UserOrder WHERE OrderId = @OrderId";

            using (SqlCommand orderCommand = new SqlCommand(orderQuery, connection))
            {
                orderCommand.Parameters.AddWithValue("@OrderId", orderId);
                SqlDataReader orderReader = orderCommand.ExecuteReader();

                if (orderReader.Read())
                {
                    OrderIdLabel.Text = "Order ID: " + orderId;

                    // Retrieve the order date from the database
                    DateTime orderDate = Convert.ToDateTime(orderReader["OrderDate"]);

                    // Calculate the delivery date by adding 5 days to the order date
                    DeliveryDate = orderDate.AddDays(5);

                    // Display order date and delivery date
                    OrderDateLabel.Text = "Order Date: " + orderDate.ToString("yyyy-MM-dd");
                    DeliveryDateLabel.Text = "Delivery Date: " + DeliveryDate.ToString("yyyy-MM-dd");
                }

                orderReader.Close();
            }

            // Fetch product details for the order
            string sqlQuery = @"
        SELECT UO.ProductId, UO.Quantity, UO.Size, UO.OrderDate, 
               PD.ProductName, PD.Price, PD.ProductImage
        FROM UserOrder AS UO
        INNER JOIN ProductDetail AS PD ON UO.ProductId = PD.ProductId
        WHERE UO.OrderId = @OrderId";

            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                ProductRepeater.DataSource = dt;
                ProductRepeater.DataBind();
            }
        }
    }

    protected void ProductRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView rowView = e.Item.DataItem as DataRowView;

            // Find controls in the repeater item
            Label productNameLabel = (Label)e.Item.FindControl("ProductNameLabel");
            Image productImage = (Image)e.Item.FindControl("ProductImage");
            Label sizeLabel = e.Item.FindControl("SizeLabel") as Label; // Find SizeLabel
            Label priceLabel = e.Item.FindControl("PriceLabel") as Label; // Find PriceLabel
            if (productNameLabel != null && productImage != null)
            {
                productNameLabel.Text = "Product Name: " + (rowView["ProductName"] != DBNull.Value ? rowView["ProductName"].ToString() : "");
                productImage.ImageUrl = ResolveUrl(rowView["ProductImage"].ToString());
                sizeLabel.Text = "Size: " + rowView["Size"].ToString(); // Set SizeLabel text
                priceLabel.Text = "Price: $" + rowView["Price"].ToString(); // Set PriceLabel text
            }
        }
    }


    protected void SubmitFeedbackButton_Click(object sender, EventArgs e)
    {
        // Retrieve user email from session
        string userEmail = Session["UserEmail"] as string;

        // Check if user email is available
        if (!string.IsNullOrEmpty(userEmail))
        {
            // Get feedback from the textbox
            string feedback = FeedbackTextBox.Text.Trim();
            string orderId = Request.QueryString["OrderId"];

            // Calculate rating
            int rating = CalculateRating();

            // Check if feedback for the same order ID and email already exists in the database
            bool feedbackExists = CheckFeedbackExists(orderId, userEmail);

            // If feedback exists, update the existing feedback; otherwise, insert a new record
            if (feedbackExists)
            {
                UpdateFeedback(orderId, userEmail, feedback, rating);
            }
            else
            {
                InsertFeedback(orderId, userEmail, feedback, rating);
            }

            // Update feedback in the receipt
            FeedbackLabel.Text = "Your feedback has been submitted successfully.";
            FeedbackTextBox.Text = ""; // Clear the feedback textbox
        }
    }

    private bool CheckFeedbackExists(string orderId, string userEmail)
    {
        // Check if feedback for the same order ID and email already exists in the database
        string connectionString = @"Data Source=DESKTOP-B3AMDGK\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";
        string sqlQuery = "SELECT COUNT(*) FROM Feedback WHERE OrderId = @OrderId AND Email = @Email";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@Email", userEmail);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }

    private void InsertFeedback(string orderId, string userEmail, string feedback, int rating)
    {
        // Insert feedback and rating into the database
        string connectionString = @"Data Source=DESKTOP-B3AMDGK\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";
        string sqlQuery = "INSERT INTO Feedback (Feedback, OrderId, Email, Rating) VALUES (@Feedback, @OrderId, @Email, @Rating)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Feedback", feedback);
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@Email", userEmail);
                command.Parameters.AddWithValue("@Rating", rating);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    private void UpdateFeedback(string orderId, string userEmail, string feedback, int rating)
    {
        // Update existing feedback in the database
        string connectionString = @"Data Source=DESKTOP-B3AMDGK\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";
        string sqlQuery = "UPDATE Feedback SET Feedback = @Feedback, Rating = @Rating WHERE OrderId = @OrderId AND Email = @Email";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@Feedback", feedback);
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@Email", userEmail);
                command.Parameters.AddWithValue("@Rating", rating);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    private int CalculateRating()
    {
        int rating = 0;
        if (Checkbox1.Checked)
            rating++;
        if (Checkbox2.Checked)
            rating++;
        if (Checkbox3.Checked)
            rating++;
        if (Checkbox4.Checked)
            rating++;
        if (Checkbox5.Checked)
            rating++;

        return rating;
    }



}
