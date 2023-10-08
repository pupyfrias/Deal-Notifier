﻿using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Specification
{
    public class PhoneUnlockToolSpecification : Specification<PhoneUnlockTool>
    {
        public PhoneUnlockToolSpecification(PhoneUnlockToolFilterAndPaginationRequest request) : base(request)
        {
            #region Name
            if (request.Name != null)
            {
                Expression<Func<PhoneUnlockTool, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Name
        }
    }
}