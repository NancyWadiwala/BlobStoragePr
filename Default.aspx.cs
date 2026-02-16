using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace BlobStoragePr
{
    public partial class Default : System.Web.UI.Page
    {
        private string connectionString;
        private string containerName = "documents";

        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["BlobConnection"].ConnectionString;
            if (!IsPostBack)
            {
                LoadFiles();
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (!fileUpload.HasFile)
                {
                    lblMessage.Text = "Please select a file.";
                    return;
                }
                string fileName = Path.GetFileName(fileUpload.FileName);
                BlobContainerClient container = GetContainer();
                BlobClient blob = container.GetBlobClient(fileName);
                using (Stream stream = fileUpload.FileContent)
                {
                    blob.Upload(stream, true);
                }
                lblMessage.Text = "File uploaded successfully.";
                LoadFiles();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadFiles()
        {
            BlobContainerClient container = GetContainer();
            List<FileItem> files = new List<FileItem>();
            foreach (var blob in container.GetBlobs())
            {
                files.Add(new FileItem
                {
                    Name = blob.Name
                });
            }
            gvFiles.DataSource = files;
            gvFiles.DataBind();
        }

        protected void gvFiles_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string fileName = gvFiles.Rows[index].Cells[0].Text;
                DownloadFile(fileName);
            }
        }

        private void DownloadFile(string fileName)
        {
            BlobContainerClient container = GetContainer();
            BlobClient blob = container.GetBlobClient(fileName);
            var download = blob.Download();

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

            download.Value.Content.CopyTo(Response.OutputStream);
            Response.End();
        }

        private BlobContainerClient GetContainer()
        {
            BlobServiceClient service = new BlobServiceClient(connectionString);
            BlobContainerClient container = service.GetBlobContainerClient(containerName);
            container.CreateIfNotExists();
            return container;
        }
    }

    public class FileItem
    {
        public string Name { get; set; }
    }
}