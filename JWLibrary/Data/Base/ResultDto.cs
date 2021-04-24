using System;
using Newtonsoft.Json;

namespace JWLibrary {
    public class ResultDtoBase<TResultDto> {
        [JsonProperty("data")] public TResultDto Data { get; set; }
    }

    public class ResultDto<TResultDto> : ResultDtoBase<TResultDto> {
    }

    public class PagingResultDto<TResultDto> : ResultDto<TResultDto> {
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalCount { get; set; }

        public int TotalPageNumber => (int) Math.Ceiling((double) TotalCount / Size);
    }
}