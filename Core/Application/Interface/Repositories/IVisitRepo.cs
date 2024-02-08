using System.Linq.Expressions;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.Domain.Enum;

namespace Visitor_Management_System.Core.Application.Interface.Repositories
{
    public interface IVisitRepo : IBaseResponse<Visit>
    {
        Task<Visit> Get(string id);
        Task<Visit> Get(Expression<Func<Visit, bool>> predicate);
        Task<ICollection<Visit>> GetAll();
        Task<ICollection<Visit>> GetAll(Expression<Func<Visit, bool>> predicate);
        
    }
}
