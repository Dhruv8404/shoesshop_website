<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="home" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <title>Home Page</title>
    <style>
      
     

        /* Style for the container */
        .container {
            position: relative;
            text-align: center;
        }

        /* Style for the background image */
        .bg-image {
            width: 100%;
            height: auto;
            display: block;
            margin-top: 20px; /* Adjust the margin as needed */
            object-fit: cover; /* Ensures the image covers the container */
        }

        /* Style for the centered button */
        .centered-button {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background-color: #663399;
            color: white;
            padding: 10px 20px;
            text-decoration: none;
            border-radius: 5px; /* Rounded button corners */
            font-weight: bold;
        }

        .custom-image {
            max-width: 100px; /* Adjust the maximum width as needed */
            max-height: 100px; /* Adjust the maximum height as needed */
            display: block;
            margin: 0 auto; /* Center the images horizontally */
            border: 1px solid #ccc; /* Add a border for styling */
            border-radius: 5px; /* Add rounded corners */
        }
             /* Add specific styles for the shop page here */
        .classic-dataitem-container {
            width: 100%;
            margin: 20px auto; /* Center the container and add margin for better spacing */
            overflow: auto; /* Clear the float */
        }

        .classic-dataitem {
            width: 100%; /* Adjusted width for three columns with some spacing */
            float: left;
            box-sizing: border-box;
            padding: 10px;
            margin: 10px; /* Add margin for better spacing */
            background-color: #fff; /* Background color for each item */
            border-radius: 8px; /* Rounded corners */
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); /* Box shadow for a subtle effect */
            text-align: center; /* Center content inside each item */
        }

       .classic-image {
    width: 100%;
    height: 400px; /* Set a fixed height for all images */
    display: block;
    margin-bottom: 10px; /* Add space below the image */
    border-radius: 8px; /* Rounded corners for the image */
    object-fit: cover; /* Maintain aspect ratio and cover the entire container */
}


        .classic-column {
            display: block;
            margin-bottom: 5px;
            font-weight: bold; /* Make text bold for better visibility */
        }

        .classic-dropdown,
        .classic-textbox {
            width: 50px; /* Adjusted width */
            margin: 5px 0;
            padding: 8px;
        }

        .classic-button {
            background-color: #007bff;
            color: #fff;
            padding: 10px;
            border: none;
            cursor: pointer;
            width: 100%;
            box-sizing: border-box;
            border-radius: 8px;
        }

        .classic-button:hover {
            background-color: #0056b3;
        }
    
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="container">
            <h1>Welcome to Our Online Store</h1> 
            <img src="https://media.istockphoto.com/id/1279108197/photo/variety-of-womens-fashion-comfortable-shoes-of-all-seasons-on-a-light-background-top-view.jpg?s=612x612&w=is&k=20&c=qxCXUXW9SA8On2Kij0gubrVe97DS-mcTxoz-QfkeBto=" alt="Background Image" class="bg-image" />
            <a href="shop.aspx" class="centered-button">Shop Now</a>
        </div>
    </div>
    <div class="classic-dataitem-container">
        <asp:DataList ID="ProductDataList" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="classic-dataitem">
            <ItemTemplate>
                <div class="classic-dataitem">
                    <asp:Image ID="ProductImage" runat="server" CssClass="classic-image" ImageUrl='<%# Eval("ProductImage") %>' AlternateText="Product Image" />
                    <asp:Label ID="ProductIdLabel" runat="server" Text='<%# Eval("ProductId") %>' CssClass="classic-column" />
                    <asp:Label ID="ProductNameLabel" runat="server" Text='<%# Eval("ProductName") %>' CssClass="classic-column" />
                    <asp:Label ID="PriceLabel" runat="server" Text='<%# Eval("Price") %>' CssClass="classic-column" />

                    <asp:DropDownList ID="SizeDropDown" runat="server" CssClass="classic-dropdown">
                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="QuantityTextBox" runat="server" CssClass="classic-textbox" Text="1" />
                    <asp:Button ID="AddToCartButton" runat="server" Text="Add To Cart" OnClick="AddToCartButton_Click" CommandArgument='<%# Eval("ProductId") %>' CssClass="classic-button" />
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>
