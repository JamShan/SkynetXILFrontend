using System;
using System.Collections.Generic;
using System.Reflection;


namespace XIL.AI
{
	/// <summary>
	/// helper class to fetch property delegates
	/// </summary>
	class ReflectionUtils
	{
		public static Assembly getAssembly( Type type )
		{
			return type.Assembly;
		}

		
		public static FieldInfo getFieldInfo( System.Object targetObject, string fieldName )
		{
			FieldInfo fieldInfo = null;
			var type = targetObject.GetType();

			do
			{
				fieldInfo = type.GetField( fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
				type = type.BaseType;
			} while ( fieldInfo == null && type != null );

			return fieldInfo;
		}


		public static IEnumerable<FieldInfo> getFields( Type type )
		{
			return type.GetFields( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
		}


		public static object getFieldValue( object targetObject, string fieldName )
		{
			var fieldInfo = getFieldInfo( targetObject, fieldName );
			return fieldInfo.GetValue( targetObject );
		}


		public static PropertyInfo getPropertyInfo( System.Object targetObject, string propertyName )
		{
			return targetObject.GetType().GetProperty( propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public );
		}

		public static IEnumerable<PropertyInfo> getProperties( Type type )
		{
			return type.GetProperties( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
		}


		public static MethodInfo getPropertyGetter( PropertyInfo prop )
		{
			return prop.GetGetMethod( true );
		}


		public static MethodInfo getPropertySetter( PropertyInfo prop )
		{
			return prop.GetSetMethod( true );
		}

		public static object getPropertyValue( object targetObject, string propertyName )
		{
			var propInfo = getPropertyInfo( targetObject, propertyName );
			var methodInfo = getPropertyGetter( propInfo );
			return methodInfo.Invoke( targetObject, new object[] { } );
		}

		public static IEnumerable<MethodInfo> getMethods( Type type )
		{
			return type.GetMethods( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
		}

		public static MethodInfo getMethodInfo( System.Object targetObject, string methodName )
		{
			return getMethodInfo( targetObject.GetType(), methodName );
		}

		public static MethodInfo getMethodInfo( System.Object targetObject, string methodName, Type[] parameters )
		{
			return getMethodInfo( targetObject.GetType(), methodName, parameters );
		}

		public static MethodInfo getMethodInfo( Type type, string methodName, Type[] parameters = null )
		{
			if( parameters == null )
				return type.GetMethod( methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public );
			return type.GetMethod( methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, Type.DefaultBinder, parameters, null );
		}

		public static T createDelegate<T>( System.Object targetObject, MethodInfo methodInfo )
		{
			return (T)(object)Delegate.CreateDelegate( typeof( T ), targetObject, methodInfo );
		}

		
		/// <summary>
		/// either returns a super fast Delegate to set the given property or null if it couldn't be found
		/// via reflection
		/// </summary>
		public static T setterForProperty<T>( System.Object targetObject, string propertyName )
		{
			// first get the property
			var propInfo = getPropertyInfo( targetObject, propertyName );
			if( propInfo == null )
				return default(T);

			return createDelegate<T>( targetObject, propInfo.GetSetMethod());
		}


		/// <summary>
		/// either returns a super fast Delegate to get the given property or null if it couldn't be found
		/// via reflection
		/// </summary>
		public static T getterForProperty<T>( System.Object targetObject, string propertyName )
		{
			// first get the property
			var propInfo = getPropertyInfo( targetObject, propertyName );
			if( propInfo == null )
				return default(T);
            // GetSetMethod 待验证正确与否
            return createDelegate<T>( targetObject, propInfo.GetSetMethod());
		}

	}
}

