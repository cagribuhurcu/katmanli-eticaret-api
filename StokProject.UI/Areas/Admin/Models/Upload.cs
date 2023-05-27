namespace StokProject.UI.Areas.Admin.Models
{
    public class Upload
    {
        public static string ImageUpload(List<IFormFile> files, IWebHostEnvironment env, out bool result)
        {
            result= false;
            var upload = Path.Combine(env.WebRootPath, "Uploads");

            foreach (var file in files)
            {
                if (file.ContentType.Contains("image"))
                {
                    //Content type
                    //image/jpg
                    //image/png
                    //image/tiff

                    if(file.Length <= 4194304)
                    {
                        //F6762C28-AC23-4215-B414-85F9EE6C00D9 => GUID
                        //f6762c28-ac23_4215_b414_85f9ee6c00d9.jpg => oluşturulacak tekil isim

                        string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{file.ContentType.Split("/")[1]}";//split sonrası / karakterinden bölerek bize bir array verir. Oluşan array => {"image","jpeg"} gibidir. Bir uzantıyı almak için 1. indexteki değeri kullanırız.

                        var filePath = Path.Combine(upload, uniqueName); // ~/Uploads/f6762c28-ac23_4215_b414_85f9ee6c00d9.jpg

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            result= true;
                            return filePath.Substring(filePath.IndexOf("\\Uploads\\"));
                        }
                    }
                    else
                    {
                        return "Boyut 4 MB'tan büyük olamaz!";
                    }
                }
                else
                {
                    return "Lütfen resim formatında bir dosya seçiniz!";
                }
            }

            return "Dosya seçilmedi!";
        }
    }
}
