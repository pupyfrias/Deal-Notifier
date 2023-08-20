

using AuthProto;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.Wrappers;
using Grpc.Net.Client;

namespace DealNotifier.Infrastructure.Identity.Services
{
    public class AuthService: IAuthService
    {
        private GrpcChannel channel;
        private AuthProto.AuthService.AuthServiceClient authClient;

        public AuthService()
        {

            const string address = "https://localhost:8082/";
            channel = GrpcChannel.ForAddress(address);
            channel.ConnectAsync().Wait();
            authClient = new AuthProto.AuthService.AuthServiceClient(channel);

        }

        public async Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request)
        {
            var response = await authClient.loginAsync(request);
            return new Response<AuthenticationResponse>(response);
        }

        public Task<Response<Core.Application.DTOs.Auth.AuthenticationResponse>> LoginAsync(Core.Application.DTOs.Auth.AuthenticationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
