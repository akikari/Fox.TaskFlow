//==================================================================================================
// Core domain aggregate representing a task with workflow capabilities.
// Uses mutable properties for EF Core and MapKit projection support.
//==================================================================================================

namespace Fox.TaskFlow.Domain.Entities;

//==================================================================================================
/// <summary>
/// Represents a task in the task flow system.
/// </summary>
//==================================================================================================
public sealed class TaskItem
{
    #region Constructors

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskItem"/> class.
    /// </summary>
    /// <remarks>
    /// Parameterless constructor for EF Core and MapKit projection support.
    /// </remarks>
    //==============================================================================================
    public TaskItem()
    {
    }

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskItem"/> class.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="dueDate">The optional due date of the task.</param>
    //==============================================================================================
    public TaskItem(Guid id, string title, string description, DateTime? dueDate)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
    }

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskItem"/> class with full state.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="dueDate">The optional due date of the task.</param>
    /// <param name="status">The current status.</param>
    /// <param name="createdAt">The creation timestamp.</param>
    /// <param name="updatedAt">The last update timestamp.</param>
    /// <remarks>
    /// This constructor is used by the persistence layer to reconstruct entities from the database.
    /// </remarks>
    //==============================================================================================
    public TaskItem(Guid id, string title, string description, DateTime? dueDate, TaskStatus status, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    #endregion

    #region Properties

    //==============================================================================================
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    //==============================================================================================
    public Guid Id { get; set; }

    //==============================================================================================
    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    //==============================================================================================
    public string Title { get; set; } = string.Empty;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    //==============================================================================================
    public string Description { get; set; } = string.Empty;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the optional due date of the task.
    /// </summary>
    //==============================================================================================
    public DateTime? DueDate { get; set; }

    //==============================================================================================
    /// <summary>
    /// Gets or sets the current status of the task in the workflow.
    /// </summary>
    //==============================================================================================
    public TaskStatus Status { get; set; } = TaskStatus.Created;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the date and time when the task was created.
    /// </summary>
    //==============================================================================================
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the date and time when the task was last updated.
    /// </summary>
    //==============================================================================================
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Updates the task details.
    /// </summary>
    /// <param name="title">The new title.</param>
    /// <param name="description">The new description.</param>
    /// <param name="dueDate">The new optional due date.</param>
    //==============================================================================================
    public void Update(string title, string description, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    //==============================================================================================
    /// <summary>
    /// Transitions the task to a new status.
    /// </summary>
    /// <param name="newStatus">The new status to transition to.</param>
    //==============================================================================================
    public void TransitionTo(TaskStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    //==============================================================================================
    /// <summary>
    /// Determines whether the task can transition to the specified status.
    /// </summary>
    /// <param name="targetStatus">The target status to check.</param>
    /// <returns><c>true</c> if the transition is valid; otherwise, <c>false</c>.</returns>
    //==============================================================================================
    public bool CanTransitionTo(TaskStatus targetStatus)
    {
        return (Status, targetStatus) switch
        {
            (TaskStatus.Created, TaskStatus.InProgress) => true,
            (TaskStatus.Created, TaskStatus.Cancelled) => true,
            (TaskStatus.InProgress, TaskStatus.Completed) => true,
            (TaskStatus.InProgress, TaskStatus.Cancelled) => true,
            (TaskStatus.InProgress, TaskStatus.Created) => true,
            _ => false
        };
    }

    #endregion
}
