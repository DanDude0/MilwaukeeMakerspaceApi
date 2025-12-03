using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Mms.Database;
using System.Security.Claims;

namespace Mms.Api
{
	public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
													   RolesAuthorizationRequirement requirement)
		{
			if (context.User == null || !context.User.Identity.IsAuthenticated) {
				context.Fail();
				return Task.CompletedTask;
			}

			var validRole = false;
			if (requirement.AllowedRoles == null ||
				requirement.AllowedRoles.Any() == false) {
				validRole = true;
			}
			else {
				var claims = context.User.Claims;
				var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
				var roles = requirement.AllowedRoles.ToList();

				// Always add "Super Admin" to the list of allowed roles, people in that group can see everything.
				roles.Add("Super Admin");

				// If WA Admin Status is set, they are automatically granted this role.
				if (roles.Contains("MMS Admin")) {
					//if (claims.FirstOrDefault(c => c.Type == "AccountAdministrator").Value == "True")
						validRole = true;
				}
				else {
					using (var db = new AccessControlDatabase()) {
						var sql = @"
						SELECT 
							COUNT(*)
						FROM
							`group` g
							INNER JOIN group_member gm
								ON g.group_id = gm.group_id
						WHERE
							g.name IN (@0)
							AND gm.member_id = @1
						LIMIT 1;";

						validRole = db.ExecuteScalar<int>(sql, roles.ToArray(), userId) == 1;
					}
				}
			}

			if (validRole) {
				context.Succeed(requirement);
			}
			else {
				context.Fail();
			}
			return Task.CompletedTask;
		}
	}
}
