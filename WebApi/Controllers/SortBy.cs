using System.Linq.Expressions;
using DealNotifier.Core.Domain.Entities;

namespace WebApi.Controllers
{
    public class SortBy
    {
        public string Sort { get; set; }
        public Expression<Func<Item, decimal>> expression { get; set; }
    }
   

}