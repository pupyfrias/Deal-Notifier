﻿

using AuthProto;
using AutoMapper;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.Wrappers;
using Grpc.Net.Client;

namespace DealNotifier.Infrastructure.Identity.Services
{
    public class AuthService: IAuthService
    {
        private GrpcChannel channel;
        private AuthProto.AuthService.AuthServiceClient authClient;
        private IMapper _mapper;

        public AuthService(IMapper mapper)
        {

            _mapper = mapper;
            const string address = "https://localhost:8001/";
            //const string address = "https://localhost:7199";
            channel = GrpcChannel.ForAddress(address);
            channel.ConnectAsync().Wait();
            authClient = new AuthProto.AuthService.AuthServiceClient(channel);

        }


        public async Task<Response<Core.Application.DTOs.Auth.AuthenticationResponse>> LoginAsync(Core.Application.DTOs.Auth.AuthenticationRequest request)
        {
            var mappedRequest = _mapper.Map<AuthenticationRequest>(request);
            var response = await authClient.loginAsync(mappedRequest);
            var mappedResponse = _mapper.Map<Core.Application.DTOs.Auth.AuthenticationResponse>(response);
            return new Response<Core.Application.DTOs.Auth.AuthenticationResponse>(mappedResponse);
        }
    }
}
