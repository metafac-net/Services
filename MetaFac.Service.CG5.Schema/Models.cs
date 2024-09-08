using FluentModels;
using System;

namespace MetaFac.Service.CG5.Schema
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

    [Entity(6)]
    public class GeneratorsInfo
    {
        [Member(1)] string? AssmVersion { get; }
        [Member(2)] string? InfoVersion { get; }
        [Member(3)] string? FileVersion { get; }
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

    [Entity(13)]
    public class GetGeneratorsInfoRequest : BaseRequest
    {
    }

    [Entity(14)]
    public class GetGeneratorsInfoResponse : BaseResponse
    {
        [Member(11)] public GeneratorsInfo? VersionInfo { get; }
    }

    [Entity(15)]
    public class GetTemplateNamesRequest : BaseRequest
    {
    }

    [Entity(16)]
    public class GetTemplateNamesResponse : BaseResponse
    {
        [Member(11)] public string[]? TemplateNames { get; }
    }

    [Entity(17)]
    public class GetTemplateRequest : BaseRequest
    {
        [Member(11)] public string? TemplateName { get; }
    }

    [Entity(18)]
    public class GetTemplateResponse : BaseResponse
    {
        [Member(11)] public string[]? Template { get; }
    }

    //==================== Level 2 fields ====================

}
