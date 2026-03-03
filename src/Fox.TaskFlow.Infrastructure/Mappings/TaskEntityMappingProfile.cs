//==================================================================================================
// INACTIVE MAPPING PROFILE - Fox.MapKit Configuration
// This profile was designed for compile-time code generation with Fox.MapKit package.
// Fox.MapKit is not yet publicly available, so the generator is not active.
// This class serves documentation purposes only and is not used at runtime.
// Actual mapping logic is provided by Fox.MapKit runtime library (libs/Fox.MapKit.dll).
//==================================================================================================
using Fox.MapKit;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Infrastructure.Data.Entities;

namespace Fox.TaskFlow.Infrastructure.Mappings;

//==================================================================================================
/// <summary>
/// Defines mappings between domain models and database entities.
/// </summary>
//==================================================================================================
[MapProfile]
public sealed class TaskEntityMappingProfile : MapProfile
{
    //==============================================================================================
    /// <summary>
    /// Configures the mappings for task entities.
    /// </summary>
    /// <param name="config">The mapping configuration.</param>
    //==============================================================================================
    public override void Configure(IMapConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        config.CreateMap<TaskItem, TaskEntity>()
            .IgnoreOnUpdate(x => x.Id)        // Non-reversible: TaskItem -> TaskEntity only
            .IgnoreOnUpdate(x => x.CreatedAt) // Non-reversible: TaskItem -> TaskEntity only
            .Reversible();
    }
}
