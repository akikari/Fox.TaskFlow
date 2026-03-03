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
using Fox.TaskFlow.Application.DTOs.Responses;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Domain.Enums;

namespace Fox.MapKit.Generated;

public static class GeneratedMapper_TaskItem_TaskResponse
{
    public static TaskResponse Map(TaskItem source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        
        var target = new TaskResponse
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
    
    public static TaskResponse MapToExisting(TaskItem source, TaskResponse target)
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
        MapperCache<TaskItem, TaskResponse>.SetMapper(Map);
        
        MapperCache<TaskItem, TaskResponse>.SetExistingMapper(MapToExisting);
    }
}
