<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public class Handler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";//"application/json";
        var r = new List<EnhancedViewDataUploadFilesResult>();
        JavaScriptSerializer js = new JavaScriptSerializer();

        if (context.Request.ServerVariables["REQUEST_METHOD"] == "DELETE")
        {
            DeleteFiles(context,r);
        }
        else
        {
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                HttpPostedFile hpf = context.Request.Files[i] as HttpPostedFile;
                string FileName = string.Empty;
                if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                {
                    string[] files = hpf.FileName.Split(new char[] { '\\' });
                    FileName = files[files.Length - 1];
                }
                else
                {
                    FileName = hpf.FileName;
                }
                if (hpf.ContentLength == 0)
                    continue;
                string savedFileName = System.IO.Path.Combine(context.Request.MapPath(uploadFolderPath), FileName);
                hpf.SaveAs(savedFileName);

                r.Add(new EnhancedViewDataUploadFilesResult()
                {
                    //Thumbnail_url = savedFileName,
                    name = FileName,
                    size = hpf.ContentLength,
                    type = hpf.ContentType,
                    url = savedFileName,
                    deleteType = "DELETE",
                    deleteUrl = HttpContext.Current.Request.Url.AbsoluteUri + "?files=" + FileName
                });
                var uploadedFiles = new
                {
                    files = r.ToArray()
                };
                var jsonObj = js.Serialize(uploadedFiles);
                //jsonObj.ContentEncoding = System.Text.Encoding.UTF8;
                //jsonObj.ContentType = "application/json;";
                context.Response.Write(jsonObj.ToString());
            }
        }
    }
    public string uploadFolderPath
    {
        get{
            try{
                return string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["BulkListUploadFolder"]) ? "~/uploadedFiles" : System.Configuration.ConfigurationSettings.AppSettings["BulkListUploadFolder"];
            }
            catch
            {
                return "~/uploadedFiles";
            }
        }
   
    }
    public bool IsReusable {
        get {
            return false;
        }
    }


    private void DeleteFiles(HttpContext context,List<EnhancedViewDataUploadFilesResult> r)
    {
        // try to delete the file from the uploaded folder

        try
        {
            // get the filename
            var fileName = context.Request.Params["files"];
            string savedFileName = System.IO.Path.Combine(context.Request.MapPath(uploadFolderPath), fileName);

            System.IO.FileInfo fileToDelete = new System.IO.FileInfo(savedFileName);

            fileToDelete.Delete();
                r.Add(new EnhancedViewDataUploadFilesResult()
                {
                    //Thumbnail_url = savedFileName,
                    name = context.Request.Params["files"]

                });
                var uploadedFiles = new
                {
                    files = r.ToArray()
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var jsonObj = js.Serialize(uploadedFiles);
                context.Response.Write(jsonObj.ToString());
        }
        catch(Exception ex)
        {
            
        }
        
    }
 

}




public class EnhancedViewDataUploadFilesResult
{
    public string url { get; set; }
    public string name { get; set; }
    public int size { get; set; }
    public string error { get; set; }
    public string type { get; set; }
    public string deleteUrl { get; set; }
    public string deleteType { get; set; }
    public string thumbnailUrl { get; set; }
    
}


public class ViewDataUploadFilesResult
{
    public string Thumbnail_url { get; set; }
    public string Name { get; set; }
    public int Length { get; set; }
    public string Type { get; set; }
}