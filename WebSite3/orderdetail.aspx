<%@ Page Language="C#" AutoEventWireup="true" CodeFile="orderdetail.aspx.cs" Inherits="orderdetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style>
        /* Your CSS styles here */
        body, div, h2, hr {
            margin: 0;
            padding: 0;
        }

        /* Apply a background color */
        body {
            background-color: #f5f5f5;
            font-family: Arial, sans-serif;
        }

        /* Style the profile container */
        .profile-container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        /* Style the profile labels */
        .profile-label {
            font-weight: bold;
            display: block;
            margin-bottom: 10px;
            font-family: Arial;
            text-align: left; /* Align left for profile labels */
            font-size: 16px; /* Increase font size */
            color: #333; /* Dark gray color */
        }

        /* Style the GridView */
        .order-gridview {
            margin-top: 20px;
            border-collapse: collapse;
            width: 100%;
        }

        .order-gridview th, .order-gridview td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }

        .order-gridview tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        /* Style the Product Image */
        .product-image {
            max-width: 100px;
            max-height: 100px;
        }

        .registration-button {
            font-family: Arial, sans-serif;
            background-color: #007bff; /* Blue color, adjust as needed */
            color: #fff; /* White text color */
            padding: 10px 20px;
            border: none;
            cursor: pointer;
            font-size: 16px;
            width: auto;
        }

        /* Apply hover effect to the Save button */
        .registration-button:hover {
            background-color: #0056b3; /* Darker blue on hover */
        }

        .profile-box {
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
            padding: 20px;
            width: 97%;
            max-width: 600px;
            text-align: center;
            margin-top: auto;
            margin-bottom: auto;
            height: auto; /* Set height to auto */
            overflow: hidden; /* Ensure content does not overflow */
        }

        .date-container {
            margin-bottom: 20px;
            padding: 10px;
            border: 1px solid #ccc;
            background-color: #fff;
            width: 50%;
            text-align: center;
            margin: 0 auto; /* Center align horizontally */
        }

        .date-group {
            margin-bottom: 20px;
        }

        /* Style the Order container */
        .order-container {
            margin: 20px auto;
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 10px;
            background-color: #fff;
            max-width: 600px;
        }

        /* Style the Order ID label */
        .order-container label {
            font-weight: bold; /* Make the label bold */
            display: block;
            margin-bottom: 10px;
            font-family: Arial;
            text-align: center;
        }

        .order-container .order-gridview {
            margin-top: 10px;
        }

        .receipt-button {
            font-family: Arial, sans-serif;
            background-color: #28a745; /* Green color, adjust as needed */
            color: #fff; /* White text color */
            padding: 8px 16px;
            border: none;
            cursor: pointer;
            font-size: 14px;
            border-radius: 5px;
            margin-bottom: 10px;
            transition: background-color 0.3s; /* Add transition for smoother hover effect */
        }

        /* Apply hover effect to the Receipt button */
        .receipt-button:hover {
            background-color: #218838; /* Darker green on hover */
        }

    
  .order-details {
    /* Styles for the entire container */
    margin-bottom: 20px; /* Example margin */
    padding: 20px; /* Example padding */
    border: 1px solid #ccc; /* Example border */
    background-color: #fff; /* Example background color */
}

.order-details label {
    /* Styles for all labels inside the order-details div */
    font-family: Arial, sans-serif;
    font-size: 14px;
    color: #333; /* Example color */
    display: block;
    margin-bottom: 5px; /* Adjust the margin bottom to add spacing between labels */
}




        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="profile-container">
            <div class="profile-box">
                <h2>User Profile</h2>
                <br />
                <hr />
                <br />
                <div class="profile-info">
                    <asp:Label ID="lblName" runat="server" CssClass="profile-label"></asp:Label>
                    <asp:Label ID="lblEmail" runat="server" CssClass="profile-label"></asp:Label>
                    <asp:Label ID="lblContact" runat="server" CssClass="profile-label"></asp:Label>
                    <asp:Label ID="lblAddress" runat="server" CssClass="profile-label"></asp:Label>
                    <hr width="600px" />
                </div>
                <br />
                <asp:Button ID="Button1" runat="server" CssClass="registration-button" Text="Update Data" OnClick="Button1_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnLogout" runat="server" CssClass="registration-button" Text="Logout" OnClick="btnLogout_Click" />
            </div>
        </div>
        <asp:Repeater ID="RepeaterOrders" runat="server" 
            onitemcommand="RepeaterOrders_ItemCommand1">
            <ItemTemplate>
                <div class="order-container">
                    <asp:Label ID="Label1" runat="server" Text='<%# "Order ID: " + Eval("OrderId") %>' CssClass="profile-label"></asp:Label>
                    <br />
                    <asp:Repeater ID="InnerRepeater" runat="server" DataSource='<%# ((System.Data.DataRowView)Container.DataItem)["Orders"] %>'>
                        <ItemTemplate>
                           <div class="order-details">
    <asp:Label ID="EmailLabel" runat="server" Text='<%# "Email: " + Eval("Email") %>'></asp:Label>&nbsp;&nbsp;&nbsp;
    <asp:Label ID="ProductIdLabel" runat="server" Text='<%# "Product ID: " + Eval("ProductId") %>'></asp:Label>&nbsp;&nbsp;
    <asp:Label ID="QuantityLabel" runat="server" Text='<%# "Quantity: " + Eval("Quantity") %>'></asp:Label>&nbsp;&nbsp;
    <asp:Label ID="SizeLabel" runat="server" Text='<%# "Size: " + Eval("Size") %>'></asp:Label><br>
    <asp:Label ID="OrderDateLabel" runat="server" Text='<%# "Order Date: " + Eval("OrderDate", "{0:yyyy-MM-dd}") %>'></asp:Label>&nbsp;&nbsp;
    <asp:Label ID="ProductNameLabel" runat="server" Text='<%# "Product Name: " + Eval("ProductName") %>'></asp:Label>&nbsp;&nbsp;
    <div class="price-label">
        <asp:Label ID="PriceLabel" runat="server" Text='<%# "Product Name: " + Eval("Price") %>'></asp:Label>
    </div>
</div>

                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Button ID="ReceiptButton" runat="server" Text="Receipt" CssClass="receipt-button"
                        OnClick="ReceiptButton_Click" CommandName="Receipt" CommandArgument='<%# Eval("OrderId") %>' />
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
