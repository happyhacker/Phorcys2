
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Phorcys.Data;
using Phorcys.Domain;

public class UserServices
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly PhorcysContext _context;
	private readonly ILogger _logger;

	public UserServices(IHttpContextAccessor httpContextAccessor, PhorcysContext context, ILogger<UserServices> logger)
	{
		_httpContextAccessor = httpContextAccessor;
		_context = context;
		_logger = logger;
	}

	public string? GetLoggedInUserId()
	{
		HttpContext? httpContext = _httpContextAccessor.HttpContext;
		ClaimsPrincipal? user = httpContext?.User;
		Claim? userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
		string? userId = userIdClaim?.Value;

		return userId;
	}

	public User? GetUser(int userId)
	{
		try
		{
			var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
			return user;
		} catch (Exception ex)
		{
			_logger.LogError("Error retreiving user " +  userId + ex.Message);
			throw;
		}
    }

    public int GetUserId()
	{
		int retVal = 0;
		try
		{
			var loginId = GetLoggedInUserId();
			var user = _context.Users.FirstOrDefault(d => d.AspNetUserId == loginId);
			if (user != null)
			{
				retVal = user.UserId;
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error retreiving user: {Message}", ex.Message);
			throw;
		}
		return retVal;
	}
	public string GetUserName()
	{
		try
		{
			var loginId = GetLoggedInUserId();
			var User = _context.Users.FirstOrDefault(d => d.AspNetUserId == loginId);
			if (User != null)
			{
				return User.LoginId;
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error retreiving user name: {Message}", ex.Message);
		}
		return "";
	}

	/// <summary>
	/// Provisions the domain-side records for a newly registered ASP.NET Core Identity user:
	/// a Phorcys.Domain.User row linked via AspNetUserId, and an associated Contact row.
	/// Runs both inserts (plus the User.ContactId back-fill) in a single transaction so a
	/// failure part-way through does not leave a half-provisioned User/Contact pair.
	/// </summary>
	public async Task<User> ProvisionNewUserAsync(string aspNetUserId, string email)
	{
		var strategy = _context.Database.CreateExecutionStrategy();
		return await strategy.ExecuteAsync(async () =>
		{
			using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				var user = new User
				{
					AspNetUserId = aspNetUserId,
					LoginId = email,
					Password = string.Empty,
					LoginCount = 0,
					Created = DateTime.Now,
					LastModified = DateTime.Now
				};
				_context.Users.Add(user);
				await _context.SaveChangesAsync();

				var contact = new Contact
				{
					UserId = user.UserId,
					Email = email,
					Company = string.Empty,
					FirstName = string.Empty,
					LastName = string.Empty,
					Address1 = string.Empty,
					Address2 = string.Empty,
					City = string.Empty,
					State = string.Empty,
					PostalCode = string.Empty,
					CountryCode = null,
					CellPhone = string.Empty,
					HomePhone = string.Empty,
					WorkPhone = string.Empty,
					Gender = string.Empty,
					Notes = string.Empty,
					Created = DateTime.Now,
					LastModified = DateTime.Now
				};
				_context.Contacts.Add(contact);
				await _context.SaveChangesAsync();

				user.ContactId = contact.ContactId;
				user.LastModified = DateTime.Now;
				await _context.SaveChangesAsync();

				await transaction.CommitAsync();
				return user;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_context.ChangeTracker.Clear();
				_logger.LogError(ex, "Error provisioning domain User/Contact for AspNetUserId {AspNetUserId}: {Message}", aspNetUserId, ex.Message);
				throw;
			}
		});
	}
}
