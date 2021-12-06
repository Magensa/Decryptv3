using DecryptV3.Dtos;
using System.Threading.Tasks;

namespace DecryptV3.ServiceFactory
{
    public interface IDecryptV3Client
    {
        Task<(DecryptCardSwipeResponseDto Response, RawSoapDetails SoapDetails)> DecryptCardSwipe(DecryptCardSwipeRequestDto dto);
        Task<(DecryptDataResponseDto Response, RawSoapDetails SoapDetails)> DecryptData(DecryptDataRequestDto dto);
        Task<(GenerateMacResponseDto Response, RawSoapDetails SoapDetails)> GenerateMac(GenerateMacRequestDto dto);
    }
}
