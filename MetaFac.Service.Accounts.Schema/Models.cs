using FluentModels;
using System;

namespace MetaFac.Service.Accounts.Schema
{
    // ==================== Level 0 fields ====================

    [Entity(1)]
    public abstract class BaseMessage
    {
        [Member(1)] public Guid RequestId { get; }
    }

    [Entity(2)]
    public abstract class BaseRequest : BaseMessage
    {
    }

    [Entity(3)]
    public abstract class BaseResponse : BaseMessage
    {
        [Member(2)] public bool Succeeded { get; }
        [Member(3)] public string? FailReason { get; }
        [Member(4)] public LogMessage[]? Messages { get; }
    }

    [Entity(4)]
    public class LogMessage
    {
        [Member(1)] public int Severity { get; }
        [Member(2)] public string? MessageTemplate { get; }
        [Member(3)] public string[]? MessageParameters { get; }
    }

    [Entity(5)]
    public class NameValuePair
    {
        [Member(1)] public string? Name { get; }
        [Member(2)] public string? Value { get; }
    }

    //==================== Level 1 fields ====================

    [Entity(11)]
    public class GetServerInfoRequest : BaseRequest
    {
    }

    [Entity(12)]
    public class GetServerInfoResponse : BaseResponse
    {
        [Member(11, ModelState.Deleted)] public string? Obsolete01 { get; } // was ServerInfo

        [Member(12)] public NameValuePair[]? ServerProperties { get; }
    }

    [Entity(21)]
    public abstract class AccountRequest : BaseRequest
    {
    }

    [Entity(22)]
    public abstract class AccountResponse : BaseResponse
    {
    }

    //==================== Level 2 fields ====================

    [Entity(23)]
    public class GetNewAccountIdRequest : AccountRequest
    {
    }

    [Entity(24)]
    public class GetNewAccountIdResponse : AccountResponse
    {
        [Member(21)] public string? CreateToken { get; }
    }

    [Entity(25)]
    public class CreateAccountRequest : AccountRequest
    {
        [Member(21)] public string? CreateToken { get; }
        [Member(22)] public string? ShortName { get; }
        [Member(23)] public string? PrimaryEmailAddress { get; }
        [Member(24)] public string? BackupEmailAddress { get; }
    }

    [Entity(26)]
    public class CreateAccountResponse : AccountResponse
    {
        [Member(21)] public string? VerifyToken { get; }
    }

    [Entity(27)]
    public class VerifyAccountRequest : AccountRequest
    {
        [Member(21)] public string? VerifyToken { get; }
        [Member(25)] public string? PrimaryEmailVerifyCode { get; }
        [Member(26)] public string? BackupEmailVerifyCode { get; }
    }

    [Entity(28)]
    public class VerifyAccountResponse : AccountResponse
    {
        [Member(21)] public string? AccessToken { get; }
    }

    [Entity(29)]
    public class ModifyAccountRequest : AccountRequest
    {
        [Member(21)] public string? AccessToken { get; }
        [Member(22)] public string? ShortName { get; }
        [Member(23)] public string? PrimaryEmailAddress { get; }
        [Member(24)] public string? BackupEmailAddress { get; }
    }

    [Entity(30)]
    public class ModifyAccountResponse : AccountResponse
    {
        [Member(21)] public string? VerifyToken { get; }
    }

    [Entity(33)]
    public class GetSecretRequest : AccountRequest
    {
        [Member(20)] public string? LicenseCode { get; }
        [Member(21)] public Guid SecretGuid { get; }
    }

    [Entity(34)]
    public class GetSecretResponse : AccountResponse
    {
        [Member(21)] public Guid SecretGuid { get; }
        [Member(22)] public string? SecretText { get; }
    }

    [Entity(35)]
    public class GetAdminTokenRequest : AccountRequest
    {
        [Member(21)] public string? AccessToken { get; }
        [Member(22)] public string? MasterToken { get; }
    }

    [Entity(36)]
    public class GetAdminTokenResponse : AccountResponse
    {
        [Member(21)] public string? AdminToken { get; }
    }

    [Entity(37)]
    public class AddSecretRequest : AccountRequest
    {
        [Member(20)] public string? AdminToken { get; }
        [Member(21)] public Guid SecretGuid { get; }
        [Member(22)] public string? SecretText { get; }
    }

    [Entity(38)]
    public class AddSecretResponse : AccountResponse
    {
    }

    [Entity(41)]
    public class CommencePurchaseRequest : AccountRequest
    {
        [Member(21)] public string? AccessToken { get; }
        [Member(22)] public CreditCardDetails? CreditCard { get; }
        [Member(23)] public string? LicenseTier { get; }
    }

    [Entity(42)]
    public class CommencePurchaseResponse : AccountResponse
    {
    }

    [Entity(43)]
    public class CompletePurchaseRequest : AccountRequest
    {
        [Member(21)] public string? AccessToken { get; }
        [Member(25)] public string? VerifyCode { get; }
    }

    [Entity(44)]
    public class CompletePurchaseResponse : AccountResponse
    {
        [Member(21)] public string? LicenseCode { get; }
    }

    [Entity(45)]
    public class CreditCardDetails
    {
        [Member(1)] public string? Number { get; }
        [Member(2)] public ushort ExpiryYear { get; } // YYYY
        [Member(3)] public byte ExpiryMonth { get; } // MM
        [Member(4)] public string? CVC { get; }
    }
}
