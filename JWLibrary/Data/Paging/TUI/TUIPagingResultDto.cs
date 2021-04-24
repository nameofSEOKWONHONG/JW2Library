namespace JWLibrary {
    public class TUIPagingResultDto<T> : ResultDto<T>, ITUIPagingBase {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int VisiblePages { get; set; }
        public int Page { get; set; }
    }
}