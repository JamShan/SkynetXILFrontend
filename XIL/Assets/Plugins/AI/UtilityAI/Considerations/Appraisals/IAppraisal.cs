using System;


namespace XIL.AI.UtilityAI
{
	/// <summary>
	/// scorer for use with a Consideration
	/// </summary>
	public interface IAppraisal<T>
	{
		float getScore( T context );
	}
}

