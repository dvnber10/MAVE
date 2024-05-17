namespace MAVE.Utilities
{
    public class ImageUtility
    {
        private readonly string _imageR;

            #nullable disable
        public ImageUtility(IConfiguration config)
        {
            _imageR = config.GetSection("ImageConfig").GetSection("ImagesRoutes").Value;
        }
            #nullable enable

        public async Task<string> ImageRoute(IFormFile image)
        {
            try
            {
                string imagesRoutes = Path.Combine(_imageR, image.FileName);
                await using (FileStream newFile = File.Create(imagesRoutes))
                {
                    image.CopyTo(newFile);
                    newFile.Flush();
                }
                return imagesRoutes;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public async Task<string> ImageUpload(IFormFile img){
            var Name = Guid.NewGuid().ToString()+".jpg";
            string Route = $"Upload/{Name}";
            using (var stream = new FileStream(Route,FileMode.Create))
            {
                await img.CopyToAsync(stream);
            }
            return Route;
        }
    }
}