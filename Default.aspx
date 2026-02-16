<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BlobStoragePr.Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Blob Upload & Download</title>
</head>
<body>
    <form runat="server">
        <h2>Upload File</h2>
        <asp:FileUpload ID="fileUpload" runat="server" />
        <br /><br />
        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
        <br /><br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
        <hr />
        <h2>Uploaded Files</h2>
        <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="false" OnRowCommand="gvFiles_RowCommand">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="File Name" />
                <asp:ButtonField Text="Download" ButtonType="Button" CommandName="Download" HeaderText="Action" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>