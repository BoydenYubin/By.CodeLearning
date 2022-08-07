using AutoMapper;
using ByLearning.SSO.Application.ViewModels.EmailViewModels;
using ByLearning.SSO.Domain.Commands.Email;
using ByLearning.SSO.Domain.Models;

namespace ByLearning.SSO.Application.AutoMapper
{
    public class EmailMapperProfile : Profile
    {
        public EmailMapperProfile()
        {
            /*
             * Email commands
             */
            CreateMap<EmailViewModel, SaveEmailCommand>().ConstructUsing(c => new SaveEmailCommand(c.Content, c.Sender, c.Subject, c.Type, c.Bcc, c.Username));
            CreateMap<TemplateViewModel, SaveTemplateCommand>().ConstructUsing(c => new SaveTemplateCommand(c.Subject, c.Content, c.Name, c.Username));
            CreateMap<TemplateViewModel, UpdateTemplateCommand>().ConstructUsing(c => new UpdateTemplateCommand(c.OldName, c.Subject, c.Content, c.Name, c.Username));


            CreateMap<Email, EmailViewModel>(MemberList.Destination);
            CreateMap<Template, TemplateViewModel>(MemberList.Destination);
        }
    }
}