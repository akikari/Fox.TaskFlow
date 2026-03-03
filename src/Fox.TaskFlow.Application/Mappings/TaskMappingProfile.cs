//==================================================================================================
// INACTIVE MAPPING PROFILE - Fox.MapKit Configuration
// This profile was designed for compile-time code generation with Fox.MapKit package.
// Fox.MapKit is not yet publicly available, so the generator is not active.
// This class serves documentation purposes only and is not used at runtime.
// Actual mapping logic is provided by Fox.MapKit runtime library (libs/Fox.MapKit.dll).
//==================================================================================================
using Fox.MapKit;
using Fox.TaskFlow.Application.DTOs.Responses;
using Fox.TaskFlow.Domain.Entities;

namespace Fox.TaskFlow.Application.Mappings;

//==================================================================================================
/// <summary>
/// Defines mappings between domain models and DTOs for task operations.
/// </summary>
//==================================================================================================
[MapProfile]
public sealed class TaskMappingProfile : MapProfile
{
    //==============================================================================================
    /// <summary>
    /// Configures the mappings for tasks.
    /// </summary>
    /// <param name="config">The mapping configuration.</param>
    //==============================================================================================
    public override void Configure(IMapConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        config.CreateMap<TaskItem, TaskResponse>();
    }
}
