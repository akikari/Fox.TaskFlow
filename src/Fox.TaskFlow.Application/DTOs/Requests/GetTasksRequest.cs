//==================================================================================================
// Request DTO for filtering and searching tasks.
// Uses OptionKit to represent optional filter parameters.
//==================================================================================================
using Fox.OptionKit;

namespace Fox.TaskFlow.Application.DTOs.Requests;

//==================================================================================================
/// <summary>
/// Request for filtering and searching tasks.
/// </summary>
//==================================================================================================
public sealed record GetTasksRequest
{
    //==============================================================================================
    /// <summary>
    /// Gets optional search term for task title.
    /// </summary>
    //==============================================================================================
    public Option<string> SearchTitle { get; init; } = Option.None<string>();

    //==============================================================================================
    /// <summary>
    /// Gets optional filter by task status.
    /// </summary>
    //==============================================================================================
    public Option<TaskStatus> FilterByStatus { get; init; } = Option.None<TaskStatus>();

    //==============================================================================================
    /// <summary>
    /// Gets optional filter for tasks due before this date.
    /// </summary>
    //==============================================================================================
    public Option<DateTime> DueBefore { get; init; } = Option.None<DateTime>();

    //==============================================================================================
    /// <summary>
    /// Gets optional filter for tasks due after this date.
    /// </summary>
    //==============================================================================================
    public Option<DateTime> DueAfter { get; init; } = Option.None<DateTime>();

    //==============================================================================================
    /// <summary>
    /// Gets optional filter to show or hide completed tasks.
    /// </summary>
    //==============================================================================================
    public Option<bool> ShowCompleted { get; init; } = Option.None<bool>();
}
