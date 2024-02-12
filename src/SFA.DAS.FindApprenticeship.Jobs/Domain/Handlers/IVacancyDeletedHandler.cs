﻿using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Events;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Events;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Handlers;
public interface IVacancyDeletedHandler
{
    Task Handle(VacancyDeletedEvent vacancyDeletedEvent, ILogger log);
}
