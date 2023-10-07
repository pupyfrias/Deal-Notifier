﻿using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.BanKeyword;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Specification
{
    public class BanKeywordSpecification : Specification<BanKeyword>
    {
        public BanKeywordSpecification(BanKeywordFilterAndPaginationRequest request): base(request)
        {


            #region Keywords

            if (request.Keyword != null)
            {
                Expression<Func<BanKeyword, bool>> expression = item => item.Keyword.Contains(request.Keyword);

                Criteria =  expression;
            }

            #endregion Keywords

            
        }

    }
}