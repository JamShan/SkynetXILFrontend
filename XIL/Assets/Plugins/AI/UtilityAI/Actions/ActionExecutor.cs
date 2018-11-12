﻿using System;


namespace XIL.AI.UtilityAI
{
	/// <summary>
	/// wraps an Action for use as an IAction without having to create a new class
	/// </summary>
	public class ActionExecutor<T> : IAction<T>
	{
		Action<T> _action;


		public ActionExecutor( Action<T> action )
		{
			_action = action;
		}


		void IAction<T>.execute( T context )
		{
			_action( context );
		}
	}
}

