<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditTab.ascx.cs" Inherits="EPiServer.Reference.Commerce.Manager.Apps.Order.Payments.MetaData.PayPal.EditTab" %>
<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td class="FormLabelCell" style="color: Red">
                <asp:Literal ID="Literal1" runat="server" Text="The current payment method does not support this order type." />
            </td>
        </tr>
        <tr>
            <td class="FormFieldCell" style="display: none;">
                <asp:TextBox MaxLength="255" Width="350" ID="creditCardName" runat="server"></asp:TextBox><br />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="creditCardName"
                    ErrorMessage="*" EnableClientScript="False" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
</div>
