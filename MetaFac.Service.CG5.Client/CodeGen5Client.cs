using MessagePack;
using MetaFac.Conduits;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MetaFac.Service.CG5.Client
{
    public sealed class CodeGen5Client : ICodeGen5Service, IDisposable
    {
        private readonly IConduitClient _client;

        public CodeGen5Client(IConduitClient client)
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

        public ValueTask<GetGeneratorsInfoResponse> GetGeneratorsInfo(GetGeneratorsInfoRequest request, CancellationToken token)
        {
            return GenericRequestResponse<GetGeneratorsInfoRequest, GetGeneratorsInfoResponse>(request, token);
        }

        public ValueTask<GetTemplateNamesResponse> GetTemplateNames(GetTemplateNamesRequest request, CancellationToken token)
        {
            return GenericRequestResponse<GetTemplateNamesRequest, GetTemplateNamesResponse>(request, token);
        }

        public ValueTask<GetTemplateResponse> GetTemplate(GetTemplateRequest request, CancellationToken token)
        {
            return GenericRequestResponse<GetTemplateRequest, GetTemplateResponse>(request, token);
        }
    }
}
