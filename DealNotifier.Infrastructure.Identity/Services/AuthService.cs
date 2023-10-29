using AutoMapper;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.ViewModels.V1.Auth;
using Catalog.Application.Wrappers;
using Catalog.Domain.Configs;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace DealNotifier.Infrastructure.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthProto.AuthService.AuthServiceClient _authClient;
        private readonly IMapper _mapper;

        public AuthService(IMapper mapper, IOptions<SecurityServiceConfig> options)
        {
            _mapper = mapper;
            var channel = GrpcChannel.ForAddress(options.Value.Address);
            channel.ConnectAsync().Wait();
            _authClient = new AuthProto.AuthService.AuthServiceClient(channel);

        }

        public async Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request)
        {
            var mappedRequest = _mapper.Map<AuthProto.AuthenticationRequest>(request);
            var response = await _authClient.loginAsync(mappedRequest);
            var mappedResponse = _mapper.Map<AuthenticationResponse>(response);
            return new Response<AuthenticationResponse>(mappedResponse);
        }
    }
}