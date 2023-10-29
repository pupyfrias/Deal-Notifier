using Grpc.Net.Client;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Empty = Google.Protobuf.WellKnownTypes.Empty;

namespace ApiGateway.Providers
{
    public static class PublicKeyProvider
    {
        public static async Task<RsaSecurityKey> GetIdentityMicroServicePublicKeyAsync(string address)
        {
            try
            {
                using (var channel = GrpcChannel.ForAddress(address))
                {
                    await channel.ConnectAsync();

                    var authClient = new AuthProto.AuthService.AuthServiceClient(channel);
                    var response = authClient.PublicKeyAsync(new Empty()).GetAwaiter().GetResult();

                    if (response == null)
                    {
                        throw new InvalidOperationException("The gRPC response from SecurityService is null.");
                    }

                    if (string.IsNullOrEmpty(response.PublicKey))
                    {
                        throw new InvalidOperationException("The SecurityService public key is empty or null.");
                    }

                    byte[] publicKeyBytes = Convert.FromBase64String(response.PublicKey);

                    RSA rsa = RSA.Create();
                    rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                    return new RsaSecurityKey(rsa);

                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the public key from SecurityService.", ex);
            }
        }
    }
}
