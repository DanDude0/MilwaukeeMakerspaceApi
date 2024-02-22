using System;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;

namespace Mms.Api.Jobs
{
	public class PullMembersFromWildApricot : IScheduledJob
	{
		public string Name { get; } = nameof(PullMembersFromWildApricot);

		public Task ExecuteAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
