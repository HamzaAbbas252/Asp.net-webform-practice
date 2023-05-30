<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableViewState="true"  CodeBehind="Countrycities.aspx.cs" Inherits="task5withGridviewfixed.Countrycities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<head>
       <title> Countries and Cities </title>
       <style>
         td,th{
             padding:15px;
         }
         .buttons{
             padding-left:25px;
         }

       </style>
   </head>
   
   <body>
    
    <table>
        <tr>
       
            <td> <asp:Label ID="countrycode_Label" runat="server" Text="CountryCode" ></asp:Label></td>
            <td> <asp:TextBox ID="countrycode_TextBox" runat="server" placeholder="Enter Country Code"></asp:TextBox> </td>
        </tr>

        <tr>
       
            <td> <asp:Label ID="countryname_label" runat="server" Text="CountryName" ></asp:Label></td>
            <td> <asp:TextBox ID="countryname_textbox" runat="server" placeholder="Enter Country Name"></asp:TextBox> </td>
        </tr>
    </table>
   
    <div class="buttons">
    
        <asp:Button ID="find_button" runat="server" Text="FIND" OnClick="find_button_Click" />
        <asp:Button ID="CANCEL_button" runat="server" Text="CANCEL" OnClick="CANCEL_button_Click" />
        <asp:Button ID="addnewrow" runat="server" Text="ADD" OnClick ="addnewrow_Click" />
        <asp:Button ID="Saveallrows" runat="server" Text="SAVE" OnClick="Saveallrows_Click" />
        <asp:Button ID="Delete_all_rows" runat="server" Text="DELETE-ALL" OnClick="Delete_all_rows_Click" />

    </div>

       <div class="gridviewdata">

           <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  SelectMethod="GridView1_GetData" UpdateMethod="GridView1_UpdateItem"
               ItemType="task5withGridviewfixed.City"   OnRowCommand ="GridView1_RowCommand" >
               <Columns>
                   <asp:TemplateField HeaderText="Delete">
                       <ItemTemplate>
                           <asp:LinkButton ID="lnkdelete" Text="Deleteitems" runat="server" CommandName="Deleteitems"  CommandArgument='<%# Container.DataItemIndex %>' />
                       </ItemTemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="citycode">
                       <ItemTemplate>
                           <asp:TextBox ID="txtCityCode" runat="server" Text='<%# BindItem.CityCode %>'></asp:TextBox>
                       </ItemTemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderText="cityname">
                       <ItemTemplate>
                           <asp:TextBox ID="txtCityName" runat="server" Text='<%# BindItem.CityName %>'></asp:TextBox>
                       </ItemTemplate>
                   </asp:TemplateField>
               </Columns>
           </asp:GridView>


              </div>




</asp:Content>
