namespace Sales.Application.Extensions
{
    public static class SystemModelExtensions
    {
        public static IEnumerable<SystemModelDto> ToSystemModelDtoList(this IEnumerable<SystemModel> systemModels)
        {
            return systemModels.Select(sm => sm.ToSystemModelDto());
        }

        public static SystemModelDto ToSystemModelDto(this SystemModel systemModel)
        {
            return DtoFromSystemModel(systemModel);
        }
        private static SystemModelDto DtoFromSystemModel(SystemModel systemModel)
        {
            return new SystemModelDto(
                Id: systemModel.Id.Value,
                Name: systemModel.Name,
                PanelType: systemModel.PanelType,
                CapacityKW: systemModel.CapacityKW,
                BasePrice: systemModel.BasePrice);
        }
    }
}
