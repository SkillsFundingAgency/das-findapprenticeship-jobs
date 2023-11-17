using System;
using Microsoft.Azure.WebJobs.Description;

namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.DependencyInjection;
[Binding]
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class InjectAttribute : Attribute
{
}
