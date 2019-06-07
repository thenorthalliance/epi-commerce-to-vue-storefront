<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurePayment.ascx.cs" Inherits="EPiServer.Reference.Commerce.Manager.Apps.Order.Payments.Plugins.PayPal.ConfigurePayment" %>
<div id="DataForm">
    <table cellpadding="0" cellspacing="2">
	    <tr>
		    <td class="FormLabelCell" colspan="2"><b><asp:Literal ID="Literal1" runat="server" Text="Configure PayPal Account" /></b></td>
	    </tr>
    </table>
    <br />
    <table class="DataForm">
<%--         <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal4" runat="server" Text="Business email" />:</td>
	          <td class="FormFieldCell">
		            <asp:TextBox Runat="server" ID="BusinessEmail" Width="300px" MaxLength="250"></asp:TextBox><br/>
		            <asp:RequiredFieldValidator ControlToValidate="BusinessEmail" Display="dynamic" Font-Name="verdana" Font-Size="9pt"
			                ErrorMessage="Business email required" runat="server" id="Requiredfieldvalidator2"></asp:RequiredFieldValidator>
	          </td>
        </tr>--%>
	     <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal5" runat="server" Text="ClientId" />:</td>
	          <td class="FormFieldCell">
		            <asp:TextBox Runat="server" ID="ClientId" Width="300px" MaxLength="250"></asp:TextBox><br/>
		            <asp:RequiredFieldValidator ControlToValidate="ClientId" Display="dynamic" Font-Name="verdana" Font-Size="9pt"
			                ErrorMessage="ClientId required" runat="server" id="Requiredfieldvalidator4"></asp:RequiredFieldValidator>
	          </td>
        </tr>
	     <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal3" runat="server" Text="Secret" />:</td>
	          <td class="FormFieldCell">
		            <asp:TextBox Runat="server" ID="Secret" Width="300px" TextMode="Password" MaxLength="250"></asp:TextBox><br/>
		            <asp:RequiredFieldValidator ControlToValidate="Secret" Display="dynamic" Font-Name="verdana" Font-Size="9pt"
			                ErrorMessage="Secret Required" runat="server" id="Requiredfieldvalidator3"></asp:RequiredFieldValidator>
	          </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <%--<tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal2" runat="server" Text="API Signature" />:</td>
	          <td class="FormFieldCell">
		            <asp:TextBox Runat="server" ID="Signature" Width="300px" MaxLength="250"></asp:TextBox><br/>
		            <asp:RequiredFieldValidator ControlToValidate="Signature" Display="dynamic" Font-Name="verdana" Font-Size="9pt"
			                ErrorMessage="API Signature required" runat="server" id="Requiredfieldvalidator1"></asp:RequiredFieldValidator>
	          </td>
        </tr>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>--%>
        <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal6" runat="server" Text="Use test environment (sandbox)" />:</td>
	          <td class="FormFieldCell">
                    <asp:CheckBox ID="CheckBoxTest" runat="server" Checked="true" />
	          </td>
        </tr>
         <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
<%--        <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal8" runat="server" Text="Allow buyers to change shipping address at PayPal" />:</td>
	          <td class="FormFieldCell">
                    <asp:CheckBox ID="CheckBoxAllowChangeAddress" runat="server" Checked="false" />
	          </td>
        </tr>--%>
        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal9" runat="server" Text="Payment action" />:</td>
	          <td class="FormFieldCell">
                  <asp:DropDownList ID="DropDownListPaymentAction" runat="server">
                <asp:ListItem Text ="Authorization" Value ="Authorization" Selected ="True"></asp:ListItem>
                   <asp:ListItem Text ="Capture" Value ="Capture" Selected ="True"></asp:ListItem>
                  </asp:DropDownList>
	          </td>
        </tr>
         <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
<%--        <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal11" runat="server" Text="Allow guest checkout" />:</td>
	          <td class="FormFieldCell">
                   <asp:CheckBox ID="CheckBoxGuestCheckout" runat="server" Checked="true" />
	          </td>
        </tr>--%>
         <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
<%--        <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal7" runat="server" Text="ExpressCheckout URL" />:</td>
	          <td class="FormFieldCell">
	                <asp:TextBox Runat="server" ID="ExpChkoutURL" Width="300px" MaxLength="250"></asp:TextBox><br/>
                    <asp:RequiredFieldValidator ControlToValidate="ExpChkoutURL" Display="dynamic" Font-Name="verdana" Font-Size="9pt"
			                ErrorMessage="ExpressCheckout URL required" runat="server" id="Requiredfieldvalidator5"></asp:RequiredFieldValidator>
	          </td>
        </tr>--%>
<%--        <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal12" runat="server" Text="Skip OrderConfirm page" />:</td>
	          <td class="FormFieldCell">
	                <asp:CheckBox ID="CheckBoxSkipConfirmPage" runat="server" Checked="true" />
	          </td>
        </tr>
	    <tr>
            <td colspan="2" class="FormSpacerCell"></td>
        </tr>
        <tr>
              <td class="FormLabelCell"><asp:Literal ID="Literal10" runat="server" Text="PayPal Secure Merchant Account ID (PAL) (optional)" />:</td>
	          <td class="FormFieldCell">
		            <asp:TextBox Runat="server" ID="PAL" Width="300px" MaxLength="250"></asp:TextBox>
	          </td>
        </tr>--%>
       
    </table>
</div>