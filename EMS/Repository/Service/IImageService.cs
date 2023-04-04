namespace EMS.Repository.Service
{
    public interface IImageService
    {   
        public Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFilename);
    }
}
