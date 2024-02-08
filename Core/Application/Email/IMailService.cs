using Visitor_Management_System.Core.DTOs;

namespace Visitor_Management_System.Core.Application.Email
{
    public interface IMailServices
    {
        public void SendEMail(EMailDto mailRequest);
        public void QRCodeEMail(EMailDto mailRequest, string qrCodeImagePath);
    }
}
