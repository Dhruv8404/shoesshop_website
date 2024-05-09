<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentSuccess.aspx.cs" Inherits="PaymentSuccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Payment Success</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }
.receipt-container {
    width: 50%;
    margin: 50px auto;
    background-color: #fff;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.2), 0 0 20px rgba(255, 255, 255, 0.5); /* Add box-shadow for shining effect */
    padding: 20px;
    text-align: center;
    border: 4px solid black; /* First border */
    outline: 10px solid white; /* Second border */
}


.success-message {
    color: green;
    font-size: 32px; /* Increase font size */
    margin-bottom: 20px;
}

.checkmark {
    font-size: 64px; /* Increase font size */
    line-height: 1;
}

.content {
    margin-top: 20px;
    font-size: 24px; /* Increase font size */
}

    </style>
</head>
<body>
 <form id="form1" runat="server">
    <div class="receipt-container">
        <div class="success-message">
            Payment Successful <span class="checkmark">&#10004;</span>
        </div>
        <asp:Label ID="orderIdLabel" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="amountLabel" runat="server" Text=""></asp:Label><br />
        <asp:Label ID="dateLabel" runat="server" Text=""></asp:Label><br />
     <div class="content">
    <p>
        Thank you for your payment. Your transaction has been processed successfully.
    </p>
    <p>
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla fringilla accumsan metus a hendrerit.
    </p>
    <p>
        Visit our <strong>Shoes Shop</strong> for the latest trends and best deals!
    </p>
</div>

    </div>
     <asp:Button ID="btnDownloadReceipt" runat="server" Text="Download Receipt" OnClick="btnDownloadReceipt_Click" />
     </form>
</body>
</html>
