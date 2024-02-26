﻿using DealNotifier.Core.Application.Exceptions;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Specification
{
    public class ItemSpecification : Specification<Item>
    {
        public ItemSpecification(ItemFilterAndPaginationRequest request):base(request)
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
                        expression = item => item.Title.Contains($"{currentName} gb");
                        expression = expression.Or(item => item.Title.Contains($"{currentName}gb"));
                    }
                    else
                    {
                        expression = expression.Or(item => item.Title.Contains($"{currentName} gb"));
                        expression = expression.Or(item => item.Title.Contains($"{currentName}gb"));
                    }
                }

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }

            #endregion Storages

            #region Stores

            if (request.Stores != null)
            {
                var storeList = request.Stores.Split(",").Select(int.Parse);
                Expression<Func<Item, bool>> expression = item => storeList.Contains(item.OnlineStoreId);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }

            #endregion Stores

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
                Expression<Func<Item, bool>> expression = item => typeList.Contains(item.ItemTypeId);

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

            /*            if (request.PhoneCarriers != null)
                        {
                            var phoneCarrierList = request.PhoneCarriers.Split(",").Select(int.Parse);
                            Expression<Func<Item, bool>> expression = item => phoneCarrierList.Equals(item.PhoneCarrierId);

                            Criteria = Criteria is null ? expression : Criteria.And(expression);
                        }*/

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
                bool parsed = decimal.TryParse(request.Min, out var min);

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
                Expression<Func<Item, bool>> expression = item => item.Title.Contains(request.Search);
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


            #region Status
            Expression<Func<Item, bool>> statusExpression;
            statusExpression = item => item.StockStatusId == (int) Enums.StockStatus.InStock;
            Criteria = Criteria is null ? statusExpression : Criteria.And(statusExpression);
            #endregion Status

            #endregion Criteria
        }
    }
}