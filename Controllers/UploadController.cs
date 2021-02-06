using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using figma.OutFile;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using figma.Models;

namespace figma.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public Firebasekey _firebase { get; }
        public UploadController(IOptions<Firebasekey> firebase, IWebHostEnvironment hostEnvironment)
        {
            _firebase = firebase.Value;
            _hostingEnvironment = hostEnvironment;
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateImage(List<IFormFile> filesadd, int width, int height)
        {
            if (filesadd == null || filesadd.Count == 0)
                return Ok(new { imgNode = "Không có hình ảnh được chọn", result = false });
            string createFolderDate = DateTime.Now.ToString("yyyy/MM/dd");
            string path = _hostingEnvironment.WebRootPath + @"\uploads\" + createFolderDate + "";
            CreateFolder(path);
            if (path == null)
                path = "image";
            var filePaths = new List<string>();
            string sql = "";
            foreach (var formFile in filesadd)
            {
                if (FormFileExtensions.IsImage(formFile) && formFile.Length > 0 && formFile.Length <= 4096000)
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "" + path + "");
                    filePaths.Add(filePath);
                    var randomname = DateTime.Now.ToFileTime() + Path.GetExtension(formFile.FileName);
                    var fileNameWithPath = string.Concat(filePath, "\\", randomname);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    Resize(fileNameWithPath, width > 0 ? width : 1200, height > 0 ? height : 900);
                    //FileStream ms;
                    //ms = new FileStream(fileNameWithPath, FileMode.Open);
                    //var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebase.ApiKey));
                    //var a = await auth.SignInWithEmailAndPasswordAsync(_firebase.AuthEmail, _firebase.AuthPassword);
                    //var cancellation = new CancellationTokenSource();
                    //var task = new FirebaseStorage(
                    //    _firebase.Bucket,
                    //    new FirebaseStorageOptions
                    //    {
                    //        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    //        ThrowOnCancel = true
                    //    })
                    //    .Child($"uploads/" + createFolderDate + "/" + randomname + "")
                    //    .PutAsync(ms, cancellation.Token);

                    //task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");
                    // ms.Close();
                    if (sql.Length > 1)
                        sql = "" + sql + ",uploads/" + createFolderDate + "/" + randomname + "";
                    else
                        sql = "uploads/" + createFolderDate + "/" + randomname + "";
                }
                else
                    return Ok(new { imgNode = "File được chọn có định dạng không phải là hình ảnh hoặc file được chọn phải có kích thước > 0 và <4M !", result = false });
            }
            return Ok(new
            {
                imgNode = sql,
                result = true
            });
        }

        //delete file

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult DeleteImage(string filesadd)
        {
            var result = false;
            if (filesadd != null)
            {

                string filepath = Path.Combine(_hostingEnvironment.WebRootPath, filesadd.Replace("/", "\\"));
                Console.WriteLine(filepath);
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                    return Ok(new { result = true, content = "Xóa thành công !" });
                }
            }
            return Ok(new { result, content = "Xóa thất bại !" });


        }

        // resize

        public void Resize(string h, int w, int he)
        {
            using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(h))
            {
                image.Mutate(x => x.Resize(w, he));
                image.Save(h);
            }
        }

        public void CreateFolder(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine("Path đã tồn tại !");
                }
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}
