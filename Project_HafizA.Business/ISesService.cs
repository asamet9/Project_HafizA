namespace Project_HafizA.Business
{
    public interface ISesService
    {
        Task<string> SesiMetneÇevir(byte[] sesVerisi, string uzanti = "wav");
    }
}