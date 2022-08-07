﻿using AutoMapper;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.SSO.Application.AutoMapper;
using ByLearning.SSO.Application.Interfaces;
using ByLearning.SSO.Application.ViewModels;
using ByLearning.SSO.Application.ViewModels.UserViewModels;
using ByLearning.SSO.Domain.Commands.User;
using ByLearning.SSO.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ByLearning.SSO.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEventBus Bus;

        public UserAppService(
            IUserService userService,
            IEventBus bus)
        {

            _mapper = UserMapping.Mapper;
            _userService = userService;
            Bus = bus;
        }

        /// <summary>
        /// Register user as an admin. Bypass many validation rules
        /// </summary>
        public Task<bool> AdminRegister(AdminRegisterUserViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterNewUserCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        /// <summary>
        /// Register regular user. With password and without Provider
        /// </summary>
        public Task<bool> Register(RegisterUserViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterNewUserCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        /// <summary>
        /// Register user from LDAP connection
        /// </summary>
        public Task<bool> Register(RegisterUserLdapViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterNewUserWithoutPassCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        /// <summary>
        /// Register user and add a new Login for him. Usually for federation logins
        /// </summary>
        public Task<bool> RegisterWithoutPassword(SocialViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterNewUserWithoutPassCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        /// <summary>
        /// Register user with password and add a new Login for him.
        /// </summary>
        public Task<bool> RegisterWithPasswordAndProvider(RegisterUserViewModel model)
        {
            var registerCommand = _mapper.Map<RegisterNewUserWithProviderCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> SendResetLink(ForgotPasswordViewModel model)
        {
            var registerCommand = _mapper.Map<SendResetLinkCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> ResetPassword(ResetPasswordViewModel model)
        {
            var registerCommand = _mapper.Map<ResetPasswordCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> ConfirmEmail(ConfirmEmailViewModel model)
        {
            var registerCommand = _mapper.Map<ConfirmEmailCommand>(model);
            return Bus.SendCommand(registerCommand);
        }


        public Task<bool> AddLogin(SocialViewModel model)
        {
            var registerCommand = _mapper.Map<AddLoginCommand>(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> CheckUsername(string userName)
        {
            return _userService.UsernameExist(userName);
        }

        public Task<bool> CheckEmail(string email)
        {
            return _userService.EmailExist(email);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
