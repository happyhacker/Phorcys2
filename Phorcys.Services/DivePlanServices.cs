using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Phorcys.Data;
using Phorcys.Data.DTOs;
using Phorcys.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Phorcys.Services;

public class DivePlanServices
{
	PhorcysContext context = new PhorcysContext();

	public IEnumerable<DivePlan> GetDivePlans()
	{
		var divePlans = context.DivePlans.Include(d => d.DiveSite).ThenInclude(u => u.User).AsNoTracking().OrderByDescending(dp => dp.ScheduledTime).ToList();
		return divePlans;
	}

	public async Task<IEnumerable<DivePlan>> GetDivePlansAsync()
	{
		try
		{
			var divePlans = context.DivePlans.Include(d => d.DiveSite).ThenInclude(u => u.User).AsNoTracking().OrderByDescending(dp => dp.ScheduledTime).ToList();
			return divePlans;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw new Exception("Can't connect to database");
		}
	}

	public void SaveNewDivePlan(DivePlan divePlan)
	{
		try
		{
			context.DivePlans.Add(divePlan);
			context.SaveChanges();
		}
		catch (SqlException ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	public void EditDivePlan(DivePlanDto divePlanDto)
	{
		var divePlan = GetDivePlan(divePlanDto.DivePlanId);
		divePlan.Title = divePlanDto.Title;
		divePlan.ScheduledTime = divePlanDto.ScheduledTime;
		divePlan.MaxDepth = divePlanDto.MaxDepth;	
		divePlan.Minutes = divePlanDto.Minutes;
		divePlan.Notes = divePlanDto.Notes;
		divePlan.DiveSiteId = divePlanDto.DiveSiteId;
		divePlan.LastModified = DateTime.Now;

		context.Entry(divePlan).State = EntityState.Modified;
		context.SaveChanges();
	}
	public DivePlan GetDivePlan(int divePlanId)
	{
		DivePlan divePlan = null;
		try
		{
			divePlan = context.DivePlans.FirstOrDefault(dp => dp.DivePlanId == divePlanId);
		} 
		catch (SqlException ex)
		{ 
			Console.WriteLine(ex.Message);
		}
		return divePlan;

	}
	public void Delete(int id)
	{
		var divePlan = context.DivePlans.Find(id);
		if (divePlan != null)
		{
			context.DivePlans.Remove(divePlan);
			context.SaveChanges();
		}
	}

}
