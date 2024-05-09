<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentForm.aspx.cs" Inherits="PaymentForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Credit Card Payment</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }

        .payment-container {
            width: 50%;
            margin: 50px auto;
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
            padding: 20px;
            text-align: center;
        }

        h2 {
            margin-bottom: 20px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-group label {
            font-weight: bold;
        }

        .form-group input[type="text"] {
            width: 100%;
            padding: 10px;
            box-sizing: border-box;
            border-radius: 5px;
            border: 1px solid #ccc;
        }

        .form-group input[type="submit"] {
            background-color: #007bff;
            color: #fff;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.3s ease;
        }

        .form-group input[type="submit"]:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="payment-container">
        <asp:Label ID="ErrorMessageLabel" runat="server" ForeColor="Red"></asp:Label>
            <h2>Enter Credit Card Information</h2>
          <div class="form-group">
    <label for="txtCardNumber">Credit Card Number:</label><br />
    <asp:TextBox ID="txtCardNumber" runat="server" placeholder="Credit Card Number" MaxLength="12" />
</div>
<div class="form-group">
    <label for="txtExpDate">Expiration Date (MM/YY):</label><br />
    <asp:TextBox ID="txtExpDate" runat="server" placeholder="Expiration Date (MM/YY)" MaxLength="4" />
</div>




            <div class="form-group">
                <label for="txtTotalAmount">Total Amount:</label><br />
                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="total-amount" ReadOnly="true"></asp:TextBox>
            </div>
   <div class="form-group">
    <label for="txtCVV">CVV:</label><br />
    <asp:TextBox ID="txtCVV" runat="server" placeholder="CVV" MaxLength="3" />
</div>

            <div class="form-group">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit Payment" OnClick="btnSubmit_Click" />
            </div>
        </div>
        

    </form>
</body>
</html>
