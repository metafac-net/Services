using System.Threading;
using System.Threading.Tasks;

namespace MetaFac.Service.CG5
{
    public interface ICodeGen5Service
    {
        ValueTask<GetServerInfoResponse> GetServerInfo(GetServerInfoRequest request, CancellationToken token);
        ValueTask<GetGeneratorsInfoResponse> GetGeneratorsInfo(GetGeneratorsInfoRequest request, CancellationToken token);
        ValueTask<GetTemplateNamesResponse> GetTemplateNames(GetTemplateNamesRequest request, CancellationToken token);
        ValueTask<GetTemplateResponse> GetTemplate(GetTemplateRequest request, CancellationToken token);
    }
}
