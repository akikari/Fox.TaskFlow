//==================================================================================================
// Response DTO for filtered task list.
// Contains list of tasks matching the filter criteria.
//==================================================================================================

namespace Fox.TaskFlow.Application.DTOs.Responses;

//==================================================================================================
/// <summary>
/// Response containing filtered list of tasks.
/// </summary>
//==================================================================================================
public sealed record GetTasksResponse
{
    //==============================================================================================
    /// <summary>
    /// Gets the list of tasks matching the filter.
    /// </summary>
    //==============================================================================================
    public IReadOnlyList<TaskResponse> Tasks { get; init; } = [];

    //==============================================================================================
    /// <summary>
    /// Gets the total count of tasks matching the filter.
    /// </summary>
    //==============================================================================================
    public int TotalCount { get; init; }
}
