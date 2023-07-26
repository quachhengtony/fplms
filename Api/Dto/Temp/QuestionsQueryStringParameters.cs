using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repositories.Contracts;
using Repositories.Enum;

namespace Api.Dto.Temp
{
    public class QuestionsQueryStringParameters : IQueryStringParameters
    {
        private const int MAX_ITEMS_PER_PAGE = 50;
        private int pageSize;
        public string? Question { get; set; }
        public string? Subject { get; set; }
        public SortType Sort { get; set; } = SortType.NEW;

        [BindRequired]
        public int PageNumber { get; set; } = 1;

        [BindRequired]
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MAX_ITEMS_PER_PAGE ? MAX_ITEMS_PER_PAGE : value;
        }
    }
}

// using Repositories.Enum;

// namespace Api.Dto.Temp
// {
//     public class QuestionsQueryStringParameters
//     {
//         private const int MAX_ITEMS_PER_PAGE = 50;
//         private int pageSize;
//         public string? Question { get; set; }
//         public string? Subject { get; set; }
//         public SortType Sort { get; set; } = SortType.NEW;

//         public int PageNumber { get; set; } = 1;

//         public int PageSize
//         {
//             get => pageSize;
//             set => pageSize = value > MAX_ITEMS_PER_PAGE ? MAX_ITEMS_PER_PAGE : value;
//         }
//     }
// }