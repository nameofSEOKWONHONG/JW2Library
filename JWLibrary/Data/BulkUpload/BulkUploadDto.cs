using eXtensionSharp;

namespace JWLibrary
{
    public class BulkUploadDto<T>
        where T : class
    {
        public T Data { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMsg { get; set; }
        public XHDictionary<string, object> Options { get; set; }
    }
}