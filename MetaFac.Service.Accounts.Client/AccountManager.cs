using MetaFac.Conduits;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MetaFac.Service.Accounts.Client
{
    public sealed class AccountManager : IDisposable
    {
        private readonly ILogger _logger;
        private readonly IConduitClient _conduit;

        public AccountManager(ILogger logger, IConduitClient conduit)
        {
            _logger = logger;
            _conduit = conduit;
        }

        public void Dispose()
        {
            // nothing to dispose yet
        }

        private void CheckResponse(BaseResponse response)
        {
            if (response.Succeeded)
            {
                if (response.Messages is not null)
                {
                    foreach (var message in response.Messages)
                    {
                        LogToLogger(message);
                    }
                }
            }
            else
                throw new Exception(response.FailReason ?? "");
        }

        private void LogToLogger(LogMessage? message)
        {
            if (message is not null)
            {
                LogLevel logLevel = (LogLevel)message.Severity;
                string? template = message.MessageTemplate;
                object?[] parameters = message.MessageParameters is null
                    ? (new object?[0])
                    : message.MessageParameters.ToArray();
                switch (logLevel)
                {
                    case LogLevel.Trace:
                        _logger.LogTrace(template, parameters);
                        break;
                    case LogLevel.Debug:
                        _logger.LogDebug(template, parameters);
                        break;
                    case LogLevel.Information:
                        _logger.LogInformation(template, parameters);
                        break;
                    case LogLevel.Warning:
                        _logger.LogWarning(template, parameters);
                        break;
                    case LogLevel.Error:
                        _logger.LogError(template, parameters);
                        break;
                    case LogLevel.Critical:
                        _logger.LogCritical(template, parameters);
                        break;
                    case LogLevel.None:
                        break;
                    default:
                        break;
                }
            }
        }

        public async ValueTask<Dictionary<string, string?>> GetServerProps(CancellationToken ct)
        {
            var request = new GetServerInfoRequest() { RequestId = Guid.NewGuid() };
            using var client = new AccountClient(_conduit);
            GetServerInfoResponse response = await client.GetServerInfo(request, ct);
            CheckResponse(response);
            var result = new Dictionary<string, string?>();
            if (response.ServerProperties is not null)
            {
                foreach (var nvp in response.ServerProperties)
                {
                    if (nvp is not null && nvp.Name is not null)
                    {
                        result[nvp.Name] = nvp.Value;
                    }
                }
            }
            return result;
        }

        public async ValueTask<string> GetNewAccountId(CancellationToken ct)
        {
            var request = new GetNewAccountIdRequest() { RequestId = Guid.NewGuid() };
            using var client = new AccountClient(_conduit);
            GetNewAccountIdResponse? response = await client.GetNewAccountId(request, ct);
            CheckResponse(response);
            string createToken = response.CreateToken ?? throw new InvalidDataException("CreateToken missing");
            return createToken;
        }

        public async ValueTask<string> CreateAccount(
            string createToken,
            string shortName,
            string primaryEmail,
            string backupEmail,
            CancellationToken ct)
        {
            var request = new CreateAccountRequest()
            {
                RequestId = Guid.NewGuid(),
                CreateToken = createToken,
                ShortName = shortName,
                PrimaryEmailAddress = primaryEmail,
                BackupEmailAddress = backupEmail,
            };
            using var client = new AccountClient(_conduit);
            CreateAccountResponse? response = await client.CreateAccount(request, ct);
            CheckResponse(response);
            string verifyToken = response.VerifyToken ?? throw new InvalidDataException("VerifyToken missing");
            return verifyToken;
        }

        public async ValueTask<string> VerifyAccount(
            string verifyToken,
            string primaryVerifyCode,
            string backupVerifyCode,
            CancellationToken ct)
        {
            var request = new VerifyAccountRequest()
            {
                RequestId = Guid.NewGuid(),
                VerifyToken = verifyToken,
                PrimaryEmailVerifyCode = primaryVerifyCode,
                BackupEmailVerifyCode = backupVerifyCode,
            };
            using var client = new AccountClient(_conduit);
            VerifyAccountResponse response = await client.VerifyAccount(request, ct);
            CheckResponse(response);
            string accessToken = response.AccessToken ?? throw new InvalidDataException("AccessToken missing");
            return accessToken;
        }

        public async ValueTask<int> CommencePurchaseFree(
            string accessToken,
            CancellationToken ct)
        {
            var request = new CommencePurchaseRequest()
            {
                RequestId = Guid.NewGuid(),
                AccessToken = accessToken,
                LicenseTier = "Free",
            };
            using var client = new AccountClient(_conduit);
            CommencePurchaseResponse response = await client.CommencePurchase(request, ct);
            CheckResponse(response);
            return 0;
        }

        public async ValueTask<int> CommencePurchasePaid(
            string accessToken,
            string creditCardNumber,
            ushort creditCardExpYear,
            byte creditCardExpMonth,
            string creditCardCvc,
            CancellationToken ct)
        {
            var request = new CommencePurchaseRequest()
            {
                RequestId = Guid.NewGuid(),
                AccessToken = accessToken,
                LicenseTier = "Paid",
                CreditCard = new CreditCardDetails()
                {
                    Number = creditCardNumber,
                    ExpiryYear = creditCardExpYear,
                    ExpiryMonth = creditCardExpMonth,
                    CVC = creditCardCvc
                }
            };
            using var client = new AccountClient(_conduit);
            CommencePurchaseResponse response = await client.CommencePurchase(request, ct);
            CheckResponse(response);
            return 0;
        }

        public async ValueTask<string> CompletePurchase(
            string accessToken,
            string verifyCode,
            CancellationToken ct)
        {
            var request = new CompletePurchaseRequest()
            {
                RequestId = Guid.NewGuid(),
                AccessToken = accessToken,
                VerifyCode = verifyCode,
            };
            using var client = new AccountClient(_conduit);
            CompletePurchaseResponse response = await client.CompletePurchase(request, ct);
            CheckResponse(response);
            string licenseCode = response.LicenseCode ?? throw new InvalidDataException("LicenseCode missing");
            return licenseCode;
        }

        //public async ValueTask<string> GetSecretAsync(
        //    string licenseCode,
        //    Guid secretGuid,
        //    CancellationToken ct)
        //{
        //    using var client = new MetaCodeApiClient(_conduit);
        //    var request = new GetSecretRequest()
        //    {
        //        RequestId = Guid.NewGuid(),
        //        LicenseCode = licenseCode,
        //        SecretGuid = secretGuid,
        //    };
        //    GetSecretResponse response = await client.GetSecret(request, ct);
        //    CheckResponse(response);
        //    return response.SecretText ?? throw new InvalidDataException("SecretText missing");
        //}

        //public async ValueTask AddSecretAsync(
        //    string adminToken,
        //    Guid secretGuid,
        //    string secretText,
        //    CancellationToken ct)
        //{
        //    var request = new AddSecretRequest()
        //    {
        //        RequestId = Guid.NewGuid(),
        //        AdminToken = adminToken,
        //        SecretGuid = secretGuid,
        //        SecretText = secretText,
        //    };
        //    using var client = new MetaCodeApiClient(_conduit);
        //    AddSecretResponse response = await client.AddSecret(request, ct);
        //    CheckResponse(response);
        //}
    }
}
