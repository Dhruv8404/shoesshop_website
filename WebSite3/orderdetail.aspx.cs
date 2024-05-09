using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;


public partial class orderdetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Check if the user is logged in
            if (Session["UserEmail"] != null)
            {
                string userEmail = Session["UserEmail"].ToString();
                FetchAndDisplayUserOrders(userEmail);

                // Populate user profile information
                PopulateUserProfile(userEmail);
            }
            else
            {
                // Redirect to login page if user is not logged in
                Response.Redirect("login.aspx");
            }
        }
    }

    private void FetchAndDisplayUserOrders(string userEmail)
    {
        string connectionString = @"Data Source=DESKTOP-B3AMDGK\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string sqlQuery = @"SELECT UO.OrderId, UO.Email, UO.ProductId, UO.Quantity, UO.Size, UO.OrderDate, PD.ProductName, PD.Price, PD.ProductImage 
                            FROM UserOrder AS UO 
                            INNER JOIN ProductDetail AS PD ON UO.ProductId = PD.ProductId 
                            WHERE UO.Email = @UserEmail
                            ORDER BY UO.OrderId"; // Ensure data is sorted by OrderId

            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@UserEmail", userEmail);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Create a DataTable to hold grouped orders
                    DataTable groupedOrders = new DataTable();
                    groupedOrders.Columns.Add("OrderId", typeof(int));
                    groupedOrders.Columns.Add("Orders", typeof(DataTable));

                    int currentOrderId = -1;
                    DataTable currentGroup = null;

                    while (reader.Read())
                    {
                        int orderId = Convert.ToInt32(reader["OrderId"]);

                        if (orderId != currentOrderId)
                        {
                            // Start a new group
                            if (currentGroup != null)
                                groupedOrders.Rows.Add(currentOrderId, currentGroup);

                            currentOrderId = orderId;
                            currentGroup = new DataTable();
                            currentGroup.Columns.Add("Email");
                            currentGroup.Columns.Add("ProductId");
                            currentGroup.Columns.Add("Quantity");
                            currentGroup.Columns.Add("Size");
                            currentGroup.Columns.Add("OrderDate");
                            currentGroup.Columns.Add("ProductName");
                            currentGroup.Columns.Add("Price");
                        }

                        // Add order details to the current group
                        currentGroup.Rows.Add(
                            reader["Email"],
                            reader["ProductId"],
                            reader["Quantity"],
                            reader["Size"],
                            reader["OrderDate"],
                            reader["ProductName"],
                            reader["Price"]
                        );
                    }

                    // Add the last group
                    if (currentGroup != null)
                        groupedOrders.Rows.Add(currentOrderId, currentGroup);

                    // Bind the grouped data to the Repeater
                    RepeaterOrders.DataSource = groupedOrders;
                    RepeaterOrders.DataBind();
                }
            }
        }
    }


    protected void RepeaterOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Receipt")
        {
            string orderId = e.CommandArgument.ToString();
            // Handle Receipt button click here
            // Redirect to payment success page with the orderId
            Response.Redirect("PaymentSuccess.aspx?OrderId=" + orderId);
        }
    }

    private void PopulateUserProfile(string userEmail)
    {

        string connectionString = @"Data Source=DESKTOP-B3AMDGK\SQLEXPRESS;Initial Catalog=shoesshop;Integrated Security=True";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string sqlQuery = "SELECT Name, Email, Contact, Address FROM Users WHERE Email = @UserEmail";

            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.Parameters.AddWithValue("@UserEmail", userEmail);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblName.Text = reader["Name"].ToString();
                        lblEmail.Text = reader["Email"].ToString();
                        lblContact.Text = reader["Contact"].ToString();
                        lblAddress.Text = reader["Address"].ToString();
                    }
                }
            }
        }
    }


  

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        // Handle logout
        Session.Remove("UserEmail");
        Session.Abandon();
        Response.Redirect("shop.aspx");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        // Redirect to update page
        Response.Redirect("update.aspx");
    }
 

    protected void ReceiptButton_Click(object sender, EventArgs e)
    {
        // Get the Order ID from the command argument
        string orderId = ((Button)sender).CommandArgument;

        // Redirect to the receipt page with the Order ID as a query parameter
        Response.Redirect("Receipt.aspx?OrderId=" + orderId);
    }


    protected void RepeaterOrders_ItemCommand1(object source, RepeaterCommandEventArgs e)
    {

    }
}