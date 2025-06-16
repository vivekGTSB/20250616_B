<%@ Page Language="VB" AutoEventWireup="false"
    EnableEventValidation="false" Inherits="YTLWebApplication.FuelManagement" Codebehind="FuelManagement.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Fuel Management</title>
    <style media="print" type="text/css">
body {color : #000000;background : #ffffff;font-family : verdana,arial,sans-serif;font-size : 12pt;}
#fcimg
{display : none;}

</style>

    <script type="text/javascript" language="javascript" src="jsfiles/calendar.js"></script>

    <script type="text/javascript" language="javascript">
     var ec=<%=ec %>;
    function mysubmit()
    {
    
     var username=document.getElementById("ddluser").value;
    if (username=="--Select User Name--")
    {
         alert("Please select user name");
         return false;         
    }
    var plateno=document.getElementById("ddlpleate").value;
    if (plateno=="--Select Plate No--")
    {
         alert("Please select vehicle plate number");
         return false;         
    }
    var bigindatetime=document.getElementById("txtBeginDate").value+" "+document.getElementById("ddlbh").value+":"+document.getElementById("ddlbm").value;
    var enddatetime=document.getElementById("txtEndDate").value+" "+document.getElementById("ddleh").value+":"+document.getElementById("ddlem").value;
    
    var fdate=Date.parse(bigindatetime);
    var sdate=Date.parse(enddatetime);
    
    var diff=(sdate-fdate)*(1/(1000*60*60*24));
    var days=parseInt(diff)+1;
    if(days>5)
    {
        return confirm("You selected "+days+" days of data.So it will take more time to execute.\nAre you sure you want to proceed ? ");
    }
    return true;     
    }
    
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
		    var result=confirm("Are you delete checked fuel information ?");
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
        function ShowCalendar(strTargetDateField, intLeft, intTop)
        {
        txtTargetDateField = strTargetDateField;

        var divTWCalendarobj=document.getElementById("divTWCalendar");
        divTWCalendarobj.style.visibility = 'visible';
        divTWCalendarobj.style.left = intLeft+"px";
        divTWCalendarobj.style.top = intTop+"px"; selecteddate(txtTargetDateField);      
        }
      
    function ExcelReport()
    {
    if(ec==true)
    {
        var plateno=document.getElementById("ddlpleate").value;
       
        document.getElementById("plateno").value=plateno;

        var excelformobj=document.getElementById("excelform");
        excelformobj.submit();
    }
    else
    {
        alert("First click submit button");
    }
    }
        
    </script>

</head>
<body style="margin-left: 5px; margin-top: 0px; margin-bottom: 0px; margin-right: 5px;">
    <form id="fuelform" runat="server">

        <script type="text/javascript">javascript:DrawCalendarLayout();</script>

        <center>
            <br />
            <img src="images/FuelManagement.jpg" alt="Fuel Management" />
            <br />
            <br />
            <table style="font-family: Verdana; font-size: 11px;">
                <tr>
                    <td style="height: 20px; background-color: #465ae8;" align="left">
                        <b style="color: White;">&nbsp;Fuel Management &nbsp;:</b></td>
                </tr>
                <tr>
                    <td style="width: 420px; border: solid 1px #3952F9; height: 184px;">
                        <table style="width: 420px;">
                            <tbody>
                                <tr>
                                    <td align="left">
                                        <b style="color: #5f7afc;">Begin Date</b>
                                    </td>
                                    <td>
                                        <b style="color: #5f7afc;">:</b>
                                    </td>
                                    <td align="left">
                                        <input readonly="readonly" style="width: 70px;" type="text" value="<%=strBeginDate%>"
                                            id="txtBeginDate" runat="server" name="txtBeginDate" enableviewstate="false" />&nbsp;<a
                                                href="javascript:ShowCalendar('txtBeginDate', 250, 250);" style="text-decoration: none;">
                                                <img alt="Show calendar control" title="Show calendar control" height="14" src="images/Calendar.jpg"
                                                    width="19" style="border: solid 1px blue;" />
                                            </a><b style="color: #5f7afc;">&nbsp;Hour&nbsp;:&nbsp;</b>
                                        <asp:DropDownList ID="ddlbh" runat="server" Width="40px" Font-Size="12px" Font-Names="verdana"
                                            EnableViewState="False">
                                            <asp:ListItem Value="00">00</asp:ListItem>
                                            <asp:ListItem Value="01">01</asp:ListItem>
                                            <asp:ListItem Value="02">02</asp:ListItem>
                                            <asp:ListItem Value="03">03</asp:ListItem>
                                            <asp:ListItem Value="04">04</asp:ListItem>
                                            <asp:ListItem Value="05">05</asp:ListItem>
                                            <asp:ListItem Value="06">06</asp:ListItem>
                                            <asp:ListItem Value="07">07</asp:ListItem>
                                            <asp:ListItem Value="08">08</asp:ListItem>
                                            <asp:ListItem Value="09">09</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12</asp:ListItem>
                                            <asp:ListItem Value="13">13</asp:ListItem>
                                            <asp:ListItem Value="14">14</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="16">16</asp:ListItem>
                                            <asp:ListItem Value="17">17</asp:ListItem>
                                            <asp:ListItem Value="18">18</asp:ListItem>
                                            <asp:ListItem Value="19">19</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="21">21</asp:ListItem>
                                            <asp:ListItem Value="22">22</asp:ListItem>
                                            <asp:ListItem Value="23">23</asp:ListItem>
                                        </asp:DropDownList><b style="color: #5f7afc;">&nbsp;Min&nbsp;:&nbsp;</b>
                                        <asp:DropDownList ID="ddlbm" runat="server" Width="40px" Font-Size="12px" Font-Names="verdana"
                                            EnableViewState="False">
                                            <asp:ListItem Value="00">00</asp:ListItem>
                                            <asp:ListItem Value="01">01</asp:ListItem>
                                            <asp:ListItem Value="02">02</asp:ListItem>
                                            <asp:ListItem Value="03">03</asp:ListItem>
                                            <asp:ListItem Value="04">04</asp:ListItem>
                                            <asp:ListItem Value="05">05</asp:ListItem>
                                            <asp:ListItem Value="06">06</asp:ListItem>
                                            <asp:ListItem Value="07">07</asp:ListItem>
                                            <asp:ListItem Value="08">08</asp:ListItem>
                                            <asp:ListItem Value="09">09</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12</asp:ListItem>
                                            <asp:ListItem Value="13">13</asp:ListItem>
                                            <asp:ListItem Value="14">14</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="16">16</asp:ListItem>
                                            <asp:ListItem Value="17">17</asp:ListItem>
                                            <asp:ListItem Value="18">18</asp:ListItem>
                                            <asp:ListItem Value="19">19</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="21">21</asp:ListItem>
                                            <asp:ListItem Value="22">22</asp:ListItem>
                                            <asp:ListItem Value="23">23</asp:ListItem>
                                            <asp:ListItem Value="24">24</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="26">26</asp:ListItem>
                                            <asp:ListItem Value="27">27</asp:ListItem>
                                            <asp:ListItem Value="28">28</asp:ListItem>
                                            <asp:ListItem Value="29">29</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                            <asp:ListItem Value="31">31</asp:ListItem>
                                            <asp:ListItem Value="32">32</asp:ListItem>
                                            <asp:ListItem Value="33">33</asp:ListItem>
                                            <asp:ListItem Value="34">34</asp:ListItem>
                                            <asp:ListItem Value="35">35</asp:ListItem>
                                            <asp:ListItem Value="36">36</asp:ListItem>
                                            <asp:ListItem Value="37">37</asp:ListItem>
                                            <asp:ListItem Value="38">38</asp:ListItem>
                                            <asp:ListItem Value="39">39</asp:ListItem>
                                            <asp:ListItem Value="40">40</asp:ListItem>
                                            <asp:ListItem Value="41">41</asp:ListItem>
                                            <asp:ListItem Value="42">42</asp:ListItem>
                                            <asp:ListItem Value="43">43</asp:ListItem>
                                            <asp:ListItem Value="44">44</asp:ListItem>
                                            <asp:ListItem Value="45">45</asp:ListItem>
                                            <asp:ListItem Value="46">46</asp:ListItem>
                                            <asp:ListItem Value="47">47</asp:ListItem>
                                            <asp:ListItem Value="48">48</asp:ListItem>
                                            <asp:ListItem Value="49">49</asp:ListItem>
                                            <asp:ListItem Value="50">50</asp:ListItem>
                                            <asp:ListItem Value="51">51</asp:ListItem>
                                            <asp:ListItem Value="52">52</asp:ListItem>
                                            <asp:ListItem Value="53">53</asp:ListItem>
                                            <asp:ListItem Value="54">54</asp:ListItem>
                                            <asp:ListItem Value="55">55</asp:ListItem>
                                            <asp:ListItem Value="56">56</asp:ListItem>
                                            <asp:ListItem Value="57">57</asp:ListItem>
                                            <asp:ListItem Value="58">58</asp:ListItem>
                                            <asp:ListItem Value="59">59</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <b style="color: #5f7afc;">End Date</b>
                                    </td>
                                    <td>
                                        <b style="color: #5f7afc;">:</b>
                                    </td>
                                    <td align="left">
                                        <input style="width: 70px;" readonly="readonly" type="text" value="<%=strEndDate%>"
                                            id="txtEndDate" runat="server" name="txtEndDate" enableviewstate="false" />&nbsp;<a
                                                href="javascript:javascript:ShowCalendar('txtEndDate', 250, 250);" style="text-decoration: none;">
                                                <img alt="Show calendar control" title="Show calendar control" height="14" src="images/Calendar.jpg"
                                                    width="19" style="border: solid 1px blue;" />
                                            </a><b style="color: #5f7afc;">&nbsp;Hour&nbsp;:&nbsp;</b>
                                        <asp:DropDownList ID="ddleh" runat="server" Width="40px" Font-Size="12px" Font-Names="verdana"
                                            EnableViewState="False">
                                            <asp:ListItem Value="00">00</asp:ListItem>
                                            <asp:ListItem Value="01">01</asp:ListItem>
                                            <asp:ListItem Value="02">02</asp:ListItem>
                                            <asp:ListItem Value="03">03</asp:ListItem>
                                            <asp:ListItem Value="04">04</asp:ListItem>
                                            <asp:ListItem Value="05">05</asp:ListItem>
                                            <asp:ListItem Value="06">06</asp:ListItem>
                                            <asp:ListItem Value="07">07</asp:ListItem>
                                            <asp:ListItem Value="08">08</asp:ListItem>
                                            <asp:ListItem Value="09">09</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12</asp:ListItem>
                                            <asp:ListItem Value="13">13</asp:ListItem>
                                            <asp:ListItem Value="14">14</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="16">16</asp:ListItem>
                                            <asp:ListItem Value="17">17</asp:ListItem>
                                            <asp:ListItem Value="18">18</asp:ListItem>
                                            <asp:ListItem Value="19">19</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="21">21</asp:ListItem>
                                            <asp:ListItem Value="22">22</asp:ListItem>
                                            <asp:ListItem Value="23" Selected="True">23</asp:ListItem>
                                        </asp:DropDownList><b style="color: #5f7afc;">&nbsp;Min&nbsp;:&nbsp;</b>
                                        <asp:DropDownList ID="ddlem" runat="server" Width="40px" Font-Size="12px" Font-Names="verdana"
                                            EnableViewState="False">
                                            <asp:ListItem Value="00">00</asp:ListItem>
                                            <asp:ListItem Value="01">01</asp:ListItem>
                                            <asp:ListItem Value="02">02</asp:ListItem>
                                            <asp:ListItem Value="03">03</asp:ListItem>
                                            <asp:ListItem Value="04">04</asp:ListItem>
                                            <asp:ListItem Value="05">05</asp:ListItem>
                                            <asp:ListItem Value="06">06</asp:ListItem>
                                            <asp:ListItem Value="07">07</asp:ListItem>
                                            <asp:ListItem Value="08">08</asp:ListItem>
                                            <asp:ListItem Value="09">09</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="11">11</asp:ListItem>
                                            <asp:ListItem Value="12">12</asp:ListItem>
                                            <asp:ListItem Value="13">13</asp:ListItem>
                                            <asp:ListItem Value="14">14</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="16">16</asp:ListItem>
                                            <asp:ListItem Value="17">17</asp:ListItem>
                                            <asp:ListItem Value="18">18</asp:ListItem>
                                            <asp:ListItem Value="19">19</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="21">21</asp:ListItem>
                                            <asp:ListItem Value="22">22</asp:ListItem>
                                            <asp:ListItem Value="23">23</asp:ListItem>
                                            <asp:ListItem Value="24">24</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="26">26</asp:ListItem>
                                            <asp:ListItem Value="27">27</asp:ListItem>
                                            <asp:ListItem Value="28">28</asp:ListItem>
                                            <asp:ListItem Value="29">29</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                            <asp:ListItem Value="31">31</asp:ListItem>
                                            <asp:ListItem Value="32">32</asp:ListItem>
                                            <asp:ListItem Value="33">33</asp:ListItem>
                                            <asp:ListItem Value="34">34</asp:ListItem>
                                            <asp:ListItem Value="35">35</asp:ListItem>
                                            <asp:ListItem Value="36">36</asp:ListItem>
                                            <asp:ListItem Value="37">37</asp:ListItem>
                                            <asp:ListItem Value="38">38</asp:ListItem>
                                            <asp:ListItem Value="39">39</asp:ListItem>
                                            <asp:ListItem Value="40">40</asp:ListItem>
                                            <asp:ListItem Value="41">41</asp:ListItem>
                                            <asp:ListItem Value="42">42</asp:ListItem>
                                            <asp:ListItem Value="43">43</asp:ListItem>
                                            <asp:ListItem Value="44">44</asp:ListItem>
                                            <asp:ListItem Value="45">45</asp:ListItem>
                                            <asp:ListItem Value="46">46</asp:ListItem>
                                            <asp:ListItem Value="47">47</asp:ListItem>
                                            <asp:ListItem Value="48">48</asp:ListItem>
                                            <asp:ListItem Value="49">49</asp:ListItem>
                                            <asp:ListItem Value="50">50</asp:ListItem>
                                            <asp:ListItem Value="51">51</asp:ListItem>
                                            <asp:ListItem Value="52">52</asp:ListItem>
                                            <asp:ListItem Value="53">53</asp:ListItem>
                                            <asp:ListItem Value="54">54</asp:ListItem>
                                            <asp:ListItem Value="55">55</asp:ListItem>
                                            <asp:ListItem Value="56">56</asp:ListItem>
                                            <asp:ListItem Value="57">57</asp:ListItem>
                                            <asp:ListItem Value="58">58</asp:ListItem>
                                            <asp:ListItem Value="59" Selected="True">59</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <b style="color: #5f7afc;">User Name </b>
                                    </td>
                                    <td>
                                        <b style="color: #5f7afc;">:</b></td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlusername" runat="server" Width="200px" Font-Size="12px" Font-Names="verdana"
                                            EnableViewState="true" AutoPostBack="True">
                                            <asp:ListItem>--Select User Name--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <b style="color: #5f7afc;">Plate No </b>
                                    </td>
                                    <td>
                                        <b style="color: #5f7afc;">:</b></td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlpleate" runat="server" Width="200px" Font-Size="12px" Font-Names="verdana"
                                            EnableViewState="true">
                                            <asp:ListItem>--Select Plate No--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <b style="color: #5f7afc;">Records/Page</b>
                                    </td>
                                    <td>
                                        <b style="color: #5f7afc;">:</b></td>
                                    <td align="left">
                                        <asp:DropDownList ID="noofrecords" runat="server" Width="75px" Font-Size="12px" Font-Names="verdana"
                                            EnableViewState="False">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>30</asp:ListItem>
                                            <asp:ListItem>40</asp:ListItem>
                                            <asp:ListItem Selected="True">50</asp:ListItem>
                                            <asp:ListItem>75</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>200</asp:ListItem>
                                            <asp:ListItem>300</asp:ListItem>
                                            <asp:ListItem>400</asp:ListItem>
                                            <asp:ListItem>500</asp:ListItem>
                                            <asp:ListItem>1000</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <br />
                                        <br />
                                        <a href="Management.aspx">
                                            <img src="images/back.jpg" alt="Back" style="border: 0px; vertical-align: top; cursor: pointer"
                                                title="Back" />
                                        </a>
                                    </td>
                                    <td colspan="2" align="center">
                                        <br />
                                        <br />
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="images/Submit_s.jpg"
                                            ToolTip="Submit"></asp:ImageButton>&nbsp;&nbsp; <a href="javascript:ExcelReport();">
                                                <img alt="Save to Excel file" title="Save to Excel file" src="images/saveExcel.jpg"
                                                    style="border: solid 0px blue;" /></a>&nbsp;&nbsp; <a href="javascript:print();">
                                                        <img alt="Print" src="images/print.jpg" style="border: solid 0px blue;" /></a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <p style="margin-bottom: 15px; font-family: Verdana; font-size: 11px; color: #5373a2;">
                Copyright © 2009 Global Telematics Sdn Bhd. All rights reserved.</p>
            <table border="0" cellpadding="0" cellspacing="0" width="680px;" style="font-family: Verdana;
                font-size: 11px">
                <tr>
                    <td align="left" style="width: 27px; height: 24px;">
                        <asp:ImageButton ID="delete1" runat="server" ImageUrl="images/Delete.jpg" ToolTip="Delete Checked Information" /></td>
                    <td align="center" style="width: 334px; height: 24px;">
                    </td>
                    <td align="right" style="height: 24px;">
                        <a href="<%=addfuelpage %>">
                            <img id="add1" runat="server" src="images/Add.jpg" alt="Add Fuel" style="border: 0px; vertical-align: top;
                                cursor: pointer" title="Add Fuel" /></a></td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <div style="font-family: Verdana; font-size: 11px;">
                            <br />
                            <asp:GridView ID="fuelgrid" runat="server" AutoGenerateColumns="false" HeaderStyle-Font-Size="12px"
                                HeaderStyle-ForeColor="#FFFFFF" HeaderStyle-BackColor="#465AE8" HeaderStyle-Font-Bold="True"
                                HeaderStyle-Height="22px" HeaderStyle-HorizontalAlign="Center" Width="680px"
                                PageSize="50" AllowPaging="True">
                                <PagerSettings PageButtonCount="5" />
                                <PagerStyle Font-Bold="True" Font-Names="Verdana" Font-Size="Small" HorizontalAlign="Center"
                                    VerticalAlign="Middle" BackColor="White" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" />
                                <Columns>
                                    <asp:BoundField DataField="chk" HeaderText="&lt;input type='checkbox' onclick='javascript:checkall(this);' /&gt;"
                                        HtmlEncode="False">
                                        <ItemStyle Width="20px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="S No" HeaderText="S No">
                                        <ItemStyle HorizontalAlign="Center" Width="35px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Plate No" HeaderText="Plate No" HtmlEncode="False">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Date Time" HeaderText="Date Time">
                                        <ItemStyle Width="125px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fuel Id" HeaderText="Fuel Id" Visible="False"></asp:BoundField>
                                    <asp:BoundField DataField="Fuel Station Code" HeaderText="Fuel Station Code" HtmlEncode="False">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fuel Type" HeaderText="Fuel Type"></asp:BoundField>
                                    <asp:BoundField DataField="Liters" HeaderText="Liters">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cost" HeaderText="Cost">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <AlternatingRowStyle BackColor="Lavender" />
                                <HeaderStyle BackColor="#465AE8" Font-Bold="True" Font-Size="12px" ForeColor="White"
                                    Height="22px" HorizontalAlign="Center" />
                            </asp:GridView>
                            <% If show = True Then%>
                            <label id="pages" style="font-family: Verdana; font-size: 11px; font-weight: bold;">
                                Pages</label>
                            <%End If%>
                            <br />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 27px">
                        <asp:ImageButton ID="delete2" runat="server" ImageUrl="images/Delete.jpg" ToolTip="Delete Checked Information" />
                    </td>
                    <td align="center" style="width: 334px">
                    </td>
                    <td align="right" style="">
                        <a href="<%=addfuelpage %>">
                            <img id="add2"  runat="server" src="images/Add.jpg" alt="Add Fuel" style="border: 0px; vertical-align: top;
                                cursor: pointer;" title="Add Fuel" /></a>
                    </td>
                </tr>
            </table>
        </center>
    </form>
    <form id="excelform" method="get" action="ExcelReport.aspx">
        <input type="hidden" id="title" name="title" value="Vehicle Fuel Management" />
        <input type="hidden" id="plateno" name="plateno" value="" />
    </form>
</body>
</html>
