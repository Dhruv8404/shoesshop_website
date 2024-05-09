using System;
using System.IO;
using System.Web.UI;
using System.Globalization; // Make sure to include this namespace for CultureInfo

public partial class PaymentSuccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Retrieve parameters from query string
        string paymentID = Request.QueryString["PaymentID"];
        string amount = Request.QueryString["totalAmount"];
        string date = Request.QueryString["date"];
        string orderId = Request.QueryString["OrderId"];

        amountLabel.Text = "Amount: " + amount;
        orderIdLabel.Text = "Order ID: " + orderId; // Add this line to display OrderId

        // Parse and format the date
        DateTime parsedDate;
        if (DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            // Date was successfully parsed, now you can format and display it as needed
            dateLabel.Text = "Date: " + parsedDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            // If parsing fails, display an error message
            dateLabel.Text = "Invalid date format";
        }
    }
    protected void btnDownloadReceipt_Click(object sender, EventArgs e)
    {
        // Generate the receipt content
        string receiptContent = GenerateReceiptContent();

        // Set the appropriate headers for file download
        Response.Clear();
        Response.ContentType = "text/plain";
        Response.AddHeader("Content-Disposition", "attachment; filename=Receipt.txt");

        // Write the receipt content to the response stream
        Response.Write(receiptContent);
        Response.End();
    }
    private string GenerateReceiptContent()
    {
        // Generate the receipt content based on your requirement
        string paymentID = Request.QueryString["PaymentID"];
        string amount = Request.QueryString["totalAmount"];
        string date = Request.QueryString["date"];
        string orderId = Request.QueryString["OrderId"];

        // Format the receipt content as needed
        string receiptContent = "Payment ID: " + paymentID + "\r\n";
        receiptContent += "Amount: " + amount + "\r\n";
        receiptContent += "Order ID: " + orderId + "\r\n";
        receiptContent += "Date: " + date + "\r\n";

        return receiptContent;
    }


}
