using MessagePack;
using MetaFac.Conduits;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MetaFac.Service.Accounts.Client
{
    public sealed class AccountClient : IAccountService, IDisposable
    {
        private readonly IConduitClient _client;

        public AccountClient(IConduitClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public void Dispose()
        {
            // nothing yet
        }

        private async ValueTask<TResponse> GenericRequestResponse<TRequest, TResponse>(TRequest request, CancellationToken token)
            where TRequest : BaseRequest
            where TResponse : BaseResponse
        {
            DateTime deadlineUtc = default;
            byte[] outgoing = MessagePackSerializer.Serialize<BaseRequest>(request);
            var incoming = await _client.SimpleUnaryCall(outgoing, new CallContext(token, deadlineUtc));
            BaseResponse response = MessagePackSerializer.Deserialize<BaseResponse>(incoming);

            Guid requestId = request.RequestId;
            Guid responseId = response.RequestId;
            if (responseId != requestId)
                throw new ArgumentException($"Unexpected response id: {responseId}, expected: {requestId}");

            if (response is TResponse typedResponse)
            {
                return typedResponse;
            }

            throw new ArgumentException($"Unexpected response type: {response.GetType().Name}, expected: {typeof(TResponse).Name}");
        }

        public ValueTask<GetServerInfoResponse> GetServerInfo(GetServerInfoRequest request, CancellationToken token)
        {
            return GenericRequestResponse<GetServerInfoRequest, GetServerInfoResponse>(request, token);
        }

        public ValueTask<GetNewAccountIdResponse> GetNewAccountId(GetNewAccountIdRequest request, CancellationToken token)
        {
            return GenericRequestResponse<GetNewAccountIdRequest, GetNewAccountIdResponse>(request, token);
        }

        public ValueTask<CreateAccountResponse> CreateAccount(CreateAccountRequest request, CancellationToken token)
        {
            return GenericRequestResponse<CreateAccountRequest, CreateAccountResponse>(request, token);
        }

        public ValueTask<VerifyAccountResponse> VerifyAccount(VerifyAccountRequest request, CancellationToken token)
        {
            return GenericRequestResponse<VerifyAccountRequest, VerifyAccountResponse>(request, token);
        }

        public ValueTask<GetSecretResponse> GetSecret(GetSecretRequest request, CancellationToken token)
        {
            return GenericRequestResponse<GetSecretRequest, GetSecretResponse>(request, token);
        }

        public ValueTask<GetAdminTokenResponse> GetAdminToken(GetAdminTokenRequest request, CancellationToken token)
        {
            return GenericRequestResponse<GetAdminTokenRequest, GetAdminTokenResponse>(request, token);
        }

        public ValueTask<AddSecretResponse> AddSecret(AddSecretRequest request, CancellationToken token)
        {
            return GenericRequestResponse<AddSecretRequest, AddSecretResponse>(request, token);
        }

        public ValueTask<CommencePurchaseResponse> CommencePurchase(CommencePurchaseRequest request, CancellationToken token)
        {
            return GenericRequestResponse<CommencePurchaseRequest, CommencePurchaseResponse>(request, token);
        }

        public ValueTask<CompletePurchaseResponse> CompletePurchase(CompletePurchaseRequest request, CancellationToken token)
        {
            return GenericRequestResponse<CompletePurchaseRequest, CompletePurchaseResponse>(request, token);
        }

    }
}
