//==================================================================================================
// GENERATED CODE - Fox.MapKit Source Generator
// This file was generated at compile-time by Fox.MapKit.Generator package (not yet published).
// Manual modifications will be preserved as the generator is not active in this project.
// The Fox.MapKit runtime library (libs/Fox.MapKit.dll) is required for execution.
//==================================================================================================
#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Fox.MapKit;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Domain.Enums;
using Fox.TaskFlow.Infrastructure.Data.Entities;

namespace Fox.MapKit.Generated;

public static class GeneratedMapper_TaskItem_TaskEntity
{
    public static TaskEntity Map(TaskItem source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        
        var target = new TaskEntity
        {
            Id = source.Id,
            Title = source.Title,
            Description = source.Description,
            DueDate = source.DueDate,
            Status = source.Status,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };
        
        return target;
    }
    
    public static TaskEntity MapToExisting(TaskItem source, TaskEntity target)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        
        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }
        
        target.Title = source.Title;
        target.Description = source.Description;
        target.DueDate = source.DueDate;
        target.Status = source.Status;
        target.UpdatedAt = source.UpdatedAt;
        
        return target;
    }
    
    [ModuleInitializer]
    internal static void Initialize()
    {
        MapperCache<TaskItem, TaskEntity>.SetMapper(Map);
        
        MapperCache<TaskItem, TaskEntity>.SetExistingMapper(MapToExisting);
    }
}
