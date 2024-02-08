using System.ComponentModel.DataAnnotations;
using Visitor_Management_System.Core.Domain.Entities;
using Visitor_Management_System.Core.Domain.Enum;


namespace Visitor_Management_System.Core.DTOs
{
    public class VisitorDto
    {
        public string Id { get;  set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Gender { get; set; }
        public string HostEmail { get; set; } = default!;
        public string Image { get; set; } = default!;
        public DateTime VisitDate { get; set; } = default!;
        public DateTime VisitTime { get; set; } = default!;

        public ICollection<VisitDto> Visits { get; set; } = new HashSet<VisitDto>();

        
    }

    public class VisitorRequestModel
    {
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public Gender Gender { get; set; }
        [DataType(DataType.EmailAddress)]
        public string HostEmail { get; set; } = default!;
        public IFormFile Image { get; set; } = default!;
        public string Number { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string? PostalCode { get; set; }
    }
}
