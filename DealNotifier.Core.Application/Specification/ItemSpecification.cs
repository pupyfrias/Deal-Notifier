using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.Request;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;
using System.Linq;
using DealNotifier.Core.Application.Exceptions;
using System;

namespace DealNotifier.Core.Application.Specification
{
    public class ItemSpecification : Specification<Item>
    {
        public ItemSpecification(ItemRequest request)
        {
            #region Criteria


            #region Storages
            
            if (request.Storages != null)
            {
                Expression<Func<Item, bool>> expression = item => false;
                var storageList = request.Storages.Split(',');

                for (int i = 0; i < storageList.Length; i++)
                {
                    string currentName = storageList[i];
                    if (i == 0)
                    {
                        expression = item => item.Name.Contains(currentName);
                    }
                    else
                    {
                        expression = expression.Or(item => item.Name.Contains(currentName));
                    }
                }

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }

            #endregion Storages

            #region Shops
            
            if (request.Shops != null)
            {
                var shopList = request.Shops.Split(",").Select(int.Parse);
                Expression<Func<Item, bool>> expression = item => shopList.Contains(item.ShopId);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Shops

            #region Conditions

            if (request.Conditions != null)
            {
                var conditionList = request.Conditions.Split(",").Select(int.Parse);
                Expression<Func<Item, bool>> expression = item => conditionList.Contains(item.ConditionId);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Conditions

            #region Types

            if (request.Types != null)
            {
                var typeList = request.Types.Split(",").Select(int.Parse);
                Expression<Func<Item, bool>> expression = item => typeList.Contains(item.TypeId);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Types

            #region Brands

            if (request.Brands != null)
            {
                var brandList = request.Brands.Split(",").Select(int.Parse);
                Expression<Func<Item, bool>> expression = item => brandList.Contains(item.BrandId);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Brands

            #region PhoneCarriers

            if (request.PhoneCarriers != null)
            {
                var phoneCarrierList = request.PhoneCarriers.Split(",").Select(int.Parse);
                Expression<Func<Item, bool>> expression = item => phoneCarrierList.Contains(item.PhoneCarrierId);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion PhoneCarriers

            #region UnlockProbabilities

            if (request.UnlockProbabilities != null)
            {
                var phoneCarrierList = request.UnlockProbabilities.Split(",").Select(int.Parse);
                Expression<Func<Item, bool>> expression = item => phoneCarrierList.Contains(item.UnlockProbabilityId);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion UnlockProbabilities

            #region Min
            if (request.Min != null)
            {
               bool parsed =  decimal.TryParse(request.Min, out var min);

                if (!parsed) throw new BadRequestException("'Min' query parameter must be decimal");

                
                Expression<Func<Item, bool>> expression = item => item.Price > min;
                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Min

            #region Max
            if (request.Max != null)
            {
                bool parsed = decimal.TryParse(request.Max, out var max);

                if (!parsed) throw new BadRequestException("'Max' query parameter must be decimal");

                Expression<Func<Item, bool>> expression = item => item.Price < max;
                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Max

            #region Search
            if (request.Search != null)
            {
                Expression<Func<Item, bool>> expression = item => item.Name.Contains(request.Search);
                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Search

            #region Offer
            if (request.Offer != null)
            {
                Expression<Func<Item, bool>> expression;

                if (request.Offer == "today")
                {
                    expression = item => item.Saving > 0 && item.LastModified >= DateTime.Now.Date;
                }
                else
                {
                    expression = item => item.Saving > 0;
                }

                
                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Offer


            #endregion Criteria

            #region OrderBy

            switch (request.OrderBy?.ToLower())
            {
                case "name":
                    ApplyOrderBy(p => p.Name);
                    break;
                case "price":
                    ApplyOrderBy(p => p.Price);
                    break;
                case "savingspercentage":
                    ApplyOrderBy(p => p.SavingsPercentage);
                    break;
                case "saving":
                    ApplyOrderBy(p => p.Saving);
                    break;
                default:
                    ApplyOrderBy(p => p.Id);
                    break;
            }

            Descending = request?.Descending ?? false;

            #endregion OrderBy

            #region Pagination
            ApplyPaging(request.Offset, request.Limit);
            #endregion Pagination

        }
    }
}
