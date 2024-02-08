using System.Linq.Expressions;
using Visitor_Management_System.Core.Domain.Entities;

namespace Visitor_Management_System.Core.Application.Interface.Repositories
{
    public interface IVisitorRepo : IBaseResponse<Visitor>
    {
        Task<Visitor> Get(string id);
        Task<Visitor> Get(Expression<Func<Visitor, bool>> predicate);
        Task<ICollection<Visitor>> GetAll();
        Task<ICollection<Visitor>> GetAll(Expression<Func<Visitor, bool>> predicate);
    }
}
