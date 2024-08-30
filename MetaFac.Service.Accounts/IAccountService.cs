using System.Threading;
using System.Threading.Tasks;

namespace MetaFac.Service.Accounts
{
    public interface IAccountService
    {
        ValueTask<GetServerInfoResponse> GetServerInfo(GetServerInfoRequest request, CancellationToken token);
        ValueTask<GetNewAccountIdResponse> GetNewAccountId(GetNewAccountIdRequest request, CancellationToken token);
        ValueTask<CreateAccountResponse> CreateAccount(CreateAccountRequest request, CancellationToken token);
        ValueTask<VerifyAccountResponse> VerifyAccount(VerifyAccountRequest request, CancellationToken token);
        ValueTask<GetAdminTokenResponse> GetAdminToken(GetAdminTokenRequest request, CancellationToken token);
        ValueTask<GetSecretResponse> GetSecret(GetSecretRequest request, CancellationToken token);
        ValueTask<AddSecretResponse> AddSecret(AddSecretRequest request, CancellationToken token);
        ValueTask<CommencePurchaseResponse> CommencePurchase(CommencePurchaseRequest request, CancellationToken token);
        ValueTask<CompletePurchaseResponse> CompletePurchase(CompletePurchaseRequest request, CancellationToken token);
    }
}
