//==================================================================================================
// GENERATED CODE - Fox.MapKit Source Generator
// This file was generated at compile-time by Fox.MapKit.Generator package (not yet published).
// Manual modifications will be preserved as the generator is not active in this project.
// The Fox.MapKit runtime library (libs/Fox.MapKit.dll) is required for execution.
//==================================================================================================
#nullable enable
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Fox.MapKit;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Infrastructure.Data.Entities;

namespace Fox.MapKit.Generated;

public static class GeneratedProjection_TaskItem_TaskEntity
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        Expression<Func<TaskItem, TaskEntity>> projection = source => 
        new TaskEntity
        {
            Id = source.Id,
            Title = source.Title,
            Description = source.Description,
            DueDate = source.DueDate,
            Status = source.Status,
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt
        };
        
        ProjectionCache<TaskItem, TaskEntity>.SetProjection(projection);
    }
}
