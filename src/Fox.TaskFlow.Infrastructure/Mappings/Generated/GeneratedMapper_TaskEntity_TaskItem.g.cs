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

public static class GeneratedMapper_TaskEntity_TaskItem
{
    public static TaskItem Map(TaskEntity source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        
        var target = new TaskItem
        (
            id: source.Id,
            title: source.Title,
            description: source.Description,
            dueDate: source.DueDate,
            status: source.Status,
            createdAt: source.CreatedAt,
            updatedAt: source.UpdatedAt
        );
        
        return target;
    }
    
    public static TaskItem MapToExisting(TaskEntity source, TaskItem target)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        
        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }
        
        target.Id = source.Id;
        target.Title = source.Title;
        target.Description = source.Description;
        target.DueDate = source.DueDate;
        target.Status = source.Status;
        target.CreatedAt = source.CreatedAt;
        target.UpdatedAt = source.UpdatedAt;
        
        return target;
    }
    
    [ModuleInitializer]
    internal static void Initialize()
    {
        MapperCache<TaskEntity, TaskItem>.SetMapper(Map);
        
        MapperCache<TaskEntity, TaskItem>.SetExistingMapper(MapToExisting);
    }
}
