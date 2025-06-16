<%@ Page Language="vb" AutoEventWireup="false" EnableEventValidation="false" Inherits="YTLWebApplication.AVLS.FuelFormulaManagement" Codebehind="FuelFormulaManagement.aspx.vb" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Fuel Formula Management</title>
    <link type="text/css" href="cssfiles/balloontip.css" rel="stylesheet" />

    <script type="text/javascript" src="jsfiles/balloontip.js"></script>

    <script type="text/javascript" language="javascript">
   var ec=<%=ec %>; 
    function checkall(chkobj)
	{
	    var chkvalue=chkobj.checked;
	    for(i = 0; i < document.forms[0].elements.length; i++) 
        {
            elm = document.forms[0].elements[i]
            if (elm.type == 'checkbox') 
            {
                document.forms[0].elements[i].checked =chkvalue;
            }
        }
    }
   
   function ExcelReport()
    {
        if(ec==true)
        {
            var plateno=document.getElementById("ddlusers").value;
           
            document.getElementById("plateno").value=plateno;

            var excelformobj=document.getElementById("excelform");
            excelformobj.submit();
        }
        else
        {
            alert("First click submit button");
        }
}
    
    function deleteconfirmation()
	{
	    var checked=false;
	    for(i = 0; i < document.forms[0].elements.length; i++) 
        {
           elm = document.forms[0].elements[i]
           if (elm.type == 'checkbox') 
            {
                if(elm.checked == true)
                {
                    checked=true;
                    break;
                }
            }
        }
        if(checked)
        {
		    var result=confirm("Are you delete checked vehicles ?");
		    if(result)
		    {
		        return true;
		    }
		    return false;
		}
		else
		{
		    alert("Please select checkboxes");
		    return false;
		}
	}
    function mouseover(path)
    {
        document.getElementById("bigimage").src="vehiclebigimages\\"+path+"?rnd="+Math.random();
    }
    </script>

</head>
<body style="margin-left: 5px; margin-top: 0px; margin-bottom: 0px; margin-right: 5px;">
    <form id="vehicleform" runat="server">
        <center>
            <br />
            <img src="images/fuelformulamanagement.gif" alt="Fuel Formula Management" />
            <br />
            <br />
            <table border="0" width="800px;" style="font-family: Verdana; font-size: 11px">
                <tr>
                    <td align="left">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="images/Delete.jpg" ToolTip="delete checked vehicles' formula" /></td>
                    <td align="center">
                        <b style="color: #5f7afc;">Select User Name&nbsp;:&nbsp;</b>
                        <asp:DropDownList ID="ddlusers" runat="server" Width="200px" AutoPostBack="True"
                            Font-Size="12px" Font-Names="verdana" EnableViewState="False">
                            <asp:ListItem Value="--Select User Name--">--Select User Name--</asp:ListItem>
                            <asp:ListItem Value="--All Server 1 Users--">--All Server 1 Users--</asp:ListItem>
                            <asp:ListItem>--All Server 2 Users--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right"> <a href="javascript:ExcelReport();">
                                                            </a>&nbsp;<a href="javascript:print();"></a>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a href="AddFuelFormula.aspx">
                            <img src="images/Add.jpg" alt="add new vehicle's formula" style="border: 0px; cursor: pointer"
                                title="add new vehicle's formula" />
                        </a>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                        <div style="font-family: Verdana; font-size: 11px;">
                            <br />
                            <asp:GridView ID="vehiclesgrid" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="12px"
                                HeaderStyle-ForeColor="#FFFFFF" HeaderStyle-BackColor="#465AE8" HeaderStyle-Font-Bold="True"
                                HeaderStyle-Height="22px" EnableViewState="False" HeaderStyle-HorizontalAlign="Center"
                                Width="900px">
                                <Columns>
                                    <asp:BoundField DataField="chk" HeaderText="<input type='checkbox' onclick='javascript:checkall(this);' />"
                                        HtmlEncode="False">
                                        <ItemStyle Width="20" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sno" HeaderText="No">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="Plate No" HeaderText="Plate No" HtmlEncode="False">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="Level ID" HeaderText="Level ID" HtmlEncode="False" />
                                    <asp:BoundField DataField="Formula" HeaderText="Formula" />
                                    <asp:BoundField DataField="Offset Value" HeaderText="Offset">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="Type" HeaderText="Type">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="Tank" HeaderText="Tank">
                                             <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField> 
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                </Columns>
                                <AlternatingRowStyle BackColor="Lavender" />
                            </asp:GridView>
                            <br />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="images/Delete.jpg" ToolTip="delete checked vehicles' formula" />
                    </td>
                    <td>
                    </td>
                    <td align="right">
                        <a href="AddFuelFormula.aspx">
                            <img src="images/Add.jpg" alt="add new vehicle's formula" style="border: 0px; cursor: pointer"
                                title="add new vehicle's formula" />
                        </a>
                    </td>
                </tr>
            </table>
            <p style="margin-bottom: 15px; font-family: Verdana; font-size: 11px; color: #5373a2;">
                Copyright © 2013 Global Telematics Sdn Bhd. All rights reserved.</p>
        </center>
        <div id="balloon1" class="balloonstyle" style="width: 102px; vertical-align: middle;">
            <img id="bigimage" src="vehiclebigimages/bigvehicle.gif" alt="" style="border: 1px solid silver;
                width: 100px; height: 100px; vertical-align: middle;" />
        </div>
    </form>
       <form id="excelform" method="get" action="ExcelReport.aspx">
        <input type="hidden" id="title" name="title" value="Fuel Formula Management" />
    </form> 
</body>
</html>
