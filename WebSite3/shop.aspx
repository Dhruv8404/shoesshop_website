<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="shop.aspx.cs" Inherits="shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
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

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
