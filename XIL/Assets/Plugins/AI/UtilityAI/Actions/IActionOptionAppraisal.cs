﻿using System;


namespace XIL.AI.UtilityAI
{
	/// <summary>
	/// Appraisal for use with an ActionWithOptions
	/// </summary>
	public interface IActionOptionAppraisal<T,U>
	{
		float getScore( T context, U option );
	}
}

