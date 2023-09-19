using AutoMapper;
using Grpc.Core;
using PlatformWebService.Data;

namespace PlatformWebService.SyncDataServices.Grpc
{
    public class GrpcPlatformServices:GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;

        public GrpcPlatformServices(IPlatformRepo platformRepo,IMapper mapper)
        {
            _platformRepo= platformRepo;
            _mapper=mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request,ServerCallContext context)
        {
            var response= new PlatformResponse();
            var platforms= _platformRepo.GetAllPlatForm();

            foreach (var plat in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}