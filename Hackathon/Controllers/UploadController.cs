using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Web.Configuration;

namespace Hackathon.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ViewResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname = files[i].FileName;
                        string ext = Path.GetExtension(fname);
                        if (ext == ".doc" || ext == ".docx" || ext == ".pdf" && files[i].ContentLength > 0)
                        {
                            // Get the complete folder path and store the file inside it.  
                            //fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                            //file.SaveAs(fname);
                            // Returns message that successfully uploaded  

                            string accessKey = WebConfigurationManager.AppSettings["AWSAccessKey"];
                            string secreteKey = WebConfigurationManager.AppSettings["AWSSecretKey"];
                            string s3BucketName = WebConfigurationManager.AppSettings["S3UploadBucket"];

                            AmazonS3Client client = new AmazonS3Client(accessKey, secreteKey, RegionEndpoint.USEast1);

                            PutObjectRequest putRequest = new PutObjectRequest
                            {
                                BucketName = s3BucketName,
                                Key = fname,
                                InputStream = file.InputStream,
                                ContentType = file.ContentType,
                            };

                            PutObjectResponse response = client.PutObject(putRequest);

                            var result = new { IsSuccess = true, Message = "File Uploaded Successfully!" };
                            return Json(result);
                        }
                        else
                        {
                            var result = new { IsSuccess = false, Message = "Currently this tool supports PDF..Coming soon!!" };
                            return Json(result);
                        }
                    }
                    return Json(" ");

                }
                catch (Exception ex)
                {
                    var result = new { IsSuccess = false, Message = ex.Message };
                    return Json(result);
                }
            }
            else
            {
                var result = new { IsSuccess = false, Message = "No files selected." };
                return Json(result);
            }
        }
    }
}