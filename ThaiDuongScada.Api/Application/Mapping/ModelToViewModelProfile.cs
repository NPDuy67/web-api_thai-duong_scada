using AutoMapper;
using ThaiDuongScada.Api.Application.Queries;
using ThaiDuongScada.Api.Application.Queries.DeviceMoulds;
using ThaiDuongScada.Api.Application.Queries.Devices;
using ThaiDuongScada.Api.Application.Queries.MachineStatus;
using ThaiDuongScada.Api.Application.Queries.Moulds;
using ThaiDuongScada.Api.Application.Queries.ShiftReports;
using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
using ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;
using ThaiDuongScada.Domain.AggregateModels.MouldAggregate;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;

namespace ThaiDuongScada.Api.Application.Mapping;
public class ModelToViewModelProfile : Profile
{
    public ModelToViewModelProfile() 
    {
        CreateMap<Device, DeviceViewModel>();

        CreateMap<Mould, MouldViewModel>();

        CreateMap<DeviceMould, DeviceMouldViewModel>()
            .ForMember(dest => dest.MouldInfo, o => o.MapFrom(src => src.Mould))
            .ForMember(dest => dest.MouldId, o => o.MapFrom(src => src.Mould.MouldId));

        CreateMap<ShiftReport, ShiftReportViewModel>();
        CreateMap<ShiftReport, ShiftReportWithShotViewModel>();

        CreateMap<MachineStatus, MachineStatusViewModel>();
    }
}
