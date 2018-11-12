using System;


namespace XIL.AI.UtilityAI
{
	public interface IAction<T>
	{
		void execute( T context );
	}
}

