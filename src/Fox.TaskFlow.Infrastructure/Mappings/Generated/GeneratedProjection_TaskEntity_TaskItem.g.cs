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

public static class GeneratedProjection_TaskEntity_TaskItem
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        Expression<Func<TaskEntity, TaskItem>> projection = source => 
        new TaskItem(id: source.Id, title: source.Title, description: source.Description, dueDate: source.DueDate, status: source.Status, createdAt: source.CreatedAt, updatedAt: source.UpdatedAt);
        
        ProjectionCache<TaskEntity, TaskItem>.SetProjection(projection);
    }
}
