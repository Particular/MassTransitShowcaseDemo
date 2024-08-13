// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "<Pending>", Scope = "member", Target = "~M:Shipping.Program.RunUserInterfaceLoop(Shipping.SimulationEffects)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Code", "PS0018:A task-returning method should have a CancellationToken parameter unless it has a parameter implementing ICancellableContext", Justification = "<Pending>", Scope = "member", Target = "~M:Shipping.Program.RunUserInterfaceLoop(Shipping.SimulationEffects)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Code", "PS0018:A task-returning method should have a CancellationToken parameter unless it has a parameter implementing ICancellableContext", Justification = "<Pending>", Scope = "member", Target = "~M:Shipping.OrderBilledHandler.Consume(MassTransit.ConsumeContext{Messages.OrderBilled})~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Code", "PS0018:A task-returning method should have a CancellationToken parameter unless it has a parameter implementing ICancellableContext", Justification = "<Pending>", Scope = "member", Target = "~M:Shipping.OrderPlacedHandler.Consume(MassTransit.ConsumeContext{Messages.OrderPlaced})~System.Threading.Tasks.Task")]
