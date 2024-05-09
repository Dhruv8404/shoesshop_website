<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Receipt.aspx.cs" Inherits="Receipt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Receipt</title>
    <style>
        /* CSS styles */
        body {
            font-family: Arial, sans-serif;
            background-color: #f0f0f0;
            margin: 0;
            padding: 0;
        }
        
        .container {
            width: 800px;
            margin: 20px auto;
            background-color: #fff;
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 20px;
        }
        
        .label {
            font-weight: bold;
            font-size: 16px;
            color: #333;
        }
        
        .product-details {
            margin-bottom: 20px;
            overflow: hidden;
        }
        
        .product-image {
            max-width: 100%;
            max-height: 200px;
            margin-bottom: 10px;
        }
        
        .product-details .label {
            display: block;
            margin-bottom: 5px;
        }
        
        .product-name {
            font-size: 18px;
            color: #333;
        }
        
        .product-info {
            float: left;
            width: 70%;
        }
        
        .product-image-container {
            float: right;
            width: 30%;
            text-align: center;
        }
        
        .star-checkbox {
            display: inline-block;
            font-size: 30px;
            cursor: pointer;
        }
        
        .star-checkbox input[type="checkbox"] {
            display: none;
        }
        
        .star-checkbox label:before {
            content: "\2605";
            margin-right: 5px;
            color: #aaa;
        }
        
        .star-checkbox input[type="checkbox"]:checked + label:before {
            color: #f9b916;
        }
        
        .clear {
            clear: both;
        }
        
        .feedback-container {
            margin-top: 20px;
        }
        
        .feedback-label {
            font-size: 16px;
            color: green;
        }
        
        .feedback-textbox {
            margin-top: 10px;
            width: 100%;
            padding: 5px;
        }
        
        .submit-button {
            margin-top: 10px;
            padding: 10px 20px;
            background-color: #007bff;
            color: #fff;
            border: none;
            border-radius: 3px;
            cursor: pointer;
        }
        
        .submit-button:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- Order details -->
            <div class="order-details">
                <div class="label">Order ID:</div>
                <asp:Label ID="OrderIdLabel" runat="server" CssClass="label" />
            </div>

            <!-- Product list -->
            <asp:Repeater ID="ProductRepeater" runat="server" OnItemDataBound="ProductRepeater_ItemDataBound">
                <ItemTemplate>
                    <div class="product-details">
                        <div class="product-info">
                            <div class="label product-name">
                                <asp:Label ID="ProductNameLabel" runat="server" CssClass="label" />
                            </div>
                            <div class="label">Size:</div>
                            <asp:Label ID="SizeLabel" runat="server" CssClass="label" />
                            <br />
                            <div class="label">Price:</div>
                            <asp:Label ID="PriceLabel" runat="server" CssClass="label" />
                        </div>
                        <div class="product-image-container">
                            <asp:Image ID="ProductImage" runat="server" CssClass="product-image" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            
            <!-- Order and delivery dates -->
            <div class="order-dates">
                <div class="label">Order Date:</div>
                <asp:Label ID="OrderDateLabel" runat="server" CssClass="label" />
                <br />
                <div class="label">Delivery Date:</div>
                <asp:Label ID="DeliveryDateLabel" runat="server" CssClass="label" />
            </div>
            <!-- Delivery status label -->
<asp:Label ID="DeliveryStatusLabel" runat="server" CssClass="feedback-label" />

            <!-- Checkboxes for feedback -->
           <div class="star-checkbox">
                <input type="checkbox" id="Checkbox1" runat="server" />
                <label for="Checkbox1"></label>
            </div>
            <div class="star-checkbox">
                <input type="checkbox" id="Checkbox2" runat="server" />
                <label for="Checkbox2"></label>
            </div>
            <div class="star-checkbox">
                <input type="checkbox" id="Checkbox3" runat="server" />
                <label for="Checkbox3"></label>
            </div>
            <div class="star-checkbox">
                <input type="checkbox" id="Checkbox4" runat="server" />
                <label for="Checkbox4"></label>
            </div>
            <div class="star-checkbox">
                <input type="checkbox" id="Checkbox5" runat="server" />
                <label for="Checkbox5"></label>
            </div>

                <!-- Feedback textbox and submit button -->
                <asp:TextBox ID="FeedbackTextBox" runat="server" CssClass="feedback-textbox" TextMode="MultiLine" Rows="4" Columns="50" />
                <br />
                <asp:Button ID="SubmitFeedbackButton" runat="server" Text="Submit Feedback" OnClick="SubmitFeedbackButton_Click" CssClass="submit-button" />
                <br />
                <asp:Label ID="FeedbackLabel" runat="server" CssClass="feedback-label" />
            </div>
        </div>
    </form>
</body>
</html>
