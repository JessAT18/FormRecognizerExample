<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="appFormRecognizer._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Form Recognizer</h1>
        <p class="lead">¡Bienvenido! A continuación, presiona el botón para cargar la factura que deseas analizar:</p>
        <asp:FileUpload ID="fuFoto" runat="server"/>
        <asp:TextBox ID="txtIDPath" runat="server"></asp:TextBox>
        <asp:Button ID="btnCargar" class="btn btn-default" runat="server" Text="Cargar factura &raquo;" OnClick="btnCargar_Click"/>&nbsp;
        <asp:Label ID="lblPrueba" class="lead" runat="server"></asp:Label>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Informacion Obtenida</h2>
            <p>
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>IDFactura</p>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtIDFactura" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>Nombre EESS</p>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtNombreEESS" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>No. Factura</p>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtNumFactura" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>Fecha</p>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtFecha" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>Hora</p>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtHora" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>NIT/CI</p>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCI" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>Nombre</p>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>Placa Vehiculo</p>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtPlaca" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <p>Total Bs</p>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtTotal" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </p>
            <p>
                <asp:Button ID="btnGuardar" class="btn btn-primary btn-lg" runat="server" Text="Guardar &raquo;" OnClick="btnGuardar_Click"/>&nbsp;
            </p>
        </div>
    </div>

</asp:Content>
