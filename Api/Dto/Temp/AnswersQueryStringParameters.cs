using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repositories.Contracts;

namespace Api.Dto.Temp
{
    public class AnswersQueryStringParameters : IQueryStringParameters
    {
        private const int MAX_ITEMS_PER_PAGE = 50;
        private int pageSize;

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

// namespace Api.Dto.Temp
// {
//     public class AnswersQueryStringParameters
//     {
//         private const int MAX_ITEMS_PER_PAGE = 50;
//         private int pageSize;

//         public int PageNumber { get; set; } = 1;

//         public int PageSize
//         {
//             get => pageSize;
//             set => pageSize = value > MAX_ITEMS_PER_PAGE ? MAX_ITEMS_PER_PAGE : value;
//         }

//     }
// }