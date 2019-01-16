using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net.Mail;
using JustBasic.Common;

namespace JustBasic.Web.Controllers
{
    public class UploaderController : Controller
    {
        //
        // POST: /Uploader/Upload
        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase uploadedFile)
        {
            try
            {
                if (uploadedFile != null && uploadedFile.ContentLength > 0)
                {
                    byte[] FileByteArray = new byte[uploadedFile.ContentLength];
                    uploadedFile.InputStream.Read(FileByteArray, 0, uploadedFile.ContentLength);

                    string strPath = Server.MapPath("~") + "\\UploadedFiles\\files\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\";
                    var fileName = string.Format("{0}{1}", strPath, uploadedFile.FileName);

                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    uploadedFile.SaveAs(fileName);
                    return Json(new
                    {
                        statusCode = 200,
                        status = "Upload thành công",
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    statusCode = 400,
                    status = "Bad Request! Upload Failed",
                    file = string.Empty
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    statusCode = 400,
                    status = "Bad Request! Upload Failed",
                    file = string.Empty
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SendMail(HttpPostedFileBase uploadedFile)
        {
            try
            {
                byte[] FileByteArray = new byte[uploadedFile.ContentLength];
                uploadedFile.InputStream.Read(FileByteArray, 0, uploadedFile.ContentLength);

                uploadedFile.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
                ////Copying file's content to FileStream
                //uploadedFile.CopyTo();
                //fileStream.Dispose();

                Attachment attachment = new Attachment(uploadedFile.InputStream, uploadedFile.FileName);
                attachment.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                MailHelper.SendMail("nguyenvantri.cntt@gmail.com", "Gửi mail thành công.", uploadedFile.FileName, attachment);
                return Json(new
                {
                    statusCode = 200,
                    status = "Gửi thành công."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    statusCode = 400,
                    status = "Bad Request! Upload Failed",
                    file = string.Empty
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //public FileContentResult DownloadAttachment(int id)
        //{
        //    return File(attachment.FileContent, attachment.FileType, attachment.FileName);
        //}

        //[HttpPost]
        //public ActionResult DeleteAttachment(int id)
        //{
        //    OperationResult OperationResult = new OperationResult();
        //    OperationResult = attachmentManager.Delete(id);
        //    if (OperationResult.Success)
        //    {
        //        return Json(new { ID = id });
        //    }
        //    else
        //    {
        //        return Json(new { ID = "", message = OperationResult.Message });
        //    }
        //}

        ////
        //// GET: /Uploader/Upload
        //public ActionResult UplodMultiple()
        //{
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_UplodMultiple");
        //    }
        //    else
        //    {
        //        return new HttpNotFoundResult();
        //    }
        //}

        //[HttpPost]
        //public JsonResult UplodMultiple(HttpPostedFileBase[] uploadedFiles)
        //{
        //    List<Attachment> newAttachmentList = new List<Attachment>();
        //    foreach (var File in uploadedFiles)
        //    {
        //        if (File != null && File.ContentLength > 0)
        //        {
        //            byte[] FileByteArray = new byte[File.ContentLength];
        //            File.InputStream.Read(FileByteArray, 0, File.ContentLength);
        //            Attachment newAttchment = new Attachment();
        //            newAttchment.FileName = File.FileName;
        //            newAttchment.FileType = File.ContentType;
        //            newAttchment.FileContent = FileByteArray;
        //            newAttachmentList.Add(newAttchment);
        //        }
        //    }
        //    OperationResult operationResult = attachmentManager.SaveAttachments(newAttachmentList);
        //    if (operationResult.Success)
        //    {
        //        string HTMLString = CaptureHelper.RenderViewToString("_AttachmentBulk", newAttachmentList, this.ControllerContext);
        //        return Json(new
        //        {
        //            statusCode = 200,
        //            status = operationResult.Message,
        //            NewRow = HTMLString
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            statusCode = 400,
        //            status = operationResult.Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}