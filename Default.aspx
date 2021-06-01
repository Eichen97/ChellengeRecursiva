<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ChallengeRecursiva.Default" %>

<!DOCTYPE html>  
<html  
    xmlns="http://www.w3.org/1999/xhtml">  
    <head runat="server">  
        <title>Challenge Recursiva</title>   
        <script src="js/bootstrap.min.js"></script>  
        <link href="css/bootstrap.min.css" rel="stylesheet" />  
        <link href="StyleSheet.css" rel="stylesheet" />
    </head>  
    <body>  
        <div id="TopBar">
            <svg id="Logo">
                <path d="M25.4 10.6c0-6.2 4.4-9.9 10.1-9.9C40.6.7 44 3.3 44 6.9c0 3.4-2.9 5.6-6.1 5.6-1.8 0-3.1-.3-4-1-.3-.3-.5-.2-.5.1 0 1.3.5 2.3 1.3 3.2.7.7 2 1.2 3.2 1.2 1.3 0 2.4-.3 3.4-.8s1.8-.3 2.3.5c.6.9-.2 2.1-.9 2.9-1.3 1.4-3.8 2.4-7 2.4-6.5-.2-10.3-4.6-10.3-10.4zm-13.3 4.1c.6 0 1 .3 1.4 1l1.8 2.9c.7 1.1 1.3 1.9 2.6 1.9s2-.5 2.6-2c.8-1.8 1.7-4.1 2.4-7.1.9-3.4 1.3-5.4 1.3-7.1s-.5-2.7-2.4-3c-2.5-.5-6-.7-9.7-.7s-7.2.2-9.7.6C.5 1.6 0 2.6 0 4.3S.4 8 1.2 11.4c.8 3 1.6 5.2 2.4 7.1.7 1.5 1.3 2 2.6 2s1.9-.8 2.6-1.9l1.8-2.9c.5-.6.9-1 1.5-1z"/>
            </svg>
        </div>
           <form id="formBase" runat="server">           
               <asp:FileUpload ID="FileUploadCSV" runat="server" onchange="UploadFile(this)"/>
               <asp:Button ID="btnPostBack" runat="server" OnClick="Upload" style="visibility:hidden"/>
               <asp:gridview ID="CSVTable" runat="server" BackColor="Gray" BorderColor="Black" BorderStyle="Solid"></asp:gridview>                                        
               <asp:Label runat="server" ID="lblMensaje"></asp:Label> 
               <asp:Button ID="JovenesUniversitariosCasados" runat="server" Text="Top 100 Jovenes Universitarios Casados" OnClick="JovenesUniversitariosCasados_Click"/>
               <asp:Button ID="NombresRiver" runat="server" Text="Top 5 Tempanos de Hielo" OnClick="NombresComunesRiver_Click"/>
               <asp:Button ID="EdadesEquipos" runat="server" Text="Piramide del descenso (poblacional) por hinchada" OnClick="EdadesPorEquipos_Click"/>
           </form>
            <p><asp:Label ID="CantidadTotal" runat="server"></asp:Label></p>
            <p><asp:Label ID="PromedioRacing" runat="server"></asp:Label></p>
    </body>     
</html>


<script type="text/javascript">
    function UploadFile(fileUpload)
    {
        if (fileUpload.value != '') {
            document.getElementById("<%=btnPostBack.ClientID %>").click();
        }
    }
</script>
