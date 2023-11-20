using System;
using UnityEngine;

namespace Game
{
    public static class DelegateExtensions
    {
        public static void Fire(this Action action)
        {
            if (action == null) return;
            
            Delegate[] delegates = action.GetInvocationList();
            
            foreach (Delegate del in delegates)
            {
                Action delegateAction = (Action)del;
                
                if (!IsSafeDelegate(delegateAction)) continue; 
                
                delegateAction();
            }
        }

        public static void Fire<T>(this Action<T> action, T param)
        {
            if (action == null) return;
            
            Delegate[] delegates = action.GetInvocationList();
            
            foreach (var del in delegates)
            {
                Action<T> delegateAction = (Action<T>)del;
                
                if (!IsSafeDelegate(delegateAction)) continue;
                
                delegateAction(param);
            }
        }

        public static void Fire<T1, T2>(this Action<T1, T2> action, T1 param1, T2 param2)
        {
            if (action == null) return;
            
            Delegate[] delegates = action.GetInvocationList();
            
            foreach (var del in delegates)
            {
                Action<T1, T2> delegateAction = (Action<T1, T2>)del;
                
                if (!IsSafeDelegate(delegateAction)) continue;
                
                delegateAction(param1, param2);
            }
        }

        public static void Fire<T1, T2, T3>(this Action<T1, T2, T3> action, T1 param1, T2 param2, T3 param3)
        {
            if (action == null) return;
            
            Delegate[] delegates = action.GetInvocationList();
            
            foreach (var del in delegates)
            {
                Action<T1, T2, T3> delegateAction = (Action<T1, T2, T3>)del;
                
                if (!IsSafeDelegate(delegateAction)) return; 
                
                delegateAction(param1, param2, param3);
            }
        }

        public static void Fire<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (action == null) return;
            
            Delegate[] delegates = action.GetInvocationList();
            
            foreach (var del in delegates)
            {
                Action<T1, T2, T3, T4> delegateAction = (Action<T1, T2, T3, T4>)del;
                
                if (!IsSafeDelegate(delegateAction)) continue; 
                
                delegateAction(param1, param2, param3, param4);
            }
        }

        public static TResult Fire<TResult>(this Func<TResult> func)
	    {
            TResult result = default(TResult);
            
		    if (func == null) return result;
            
		    Delegate[] delegates = func.GetInvocationList();
            
		    foreach (var del in delegates)
            {
                Func<TResult> delegateFunc = (Func<TResult>)del;
                
                if (!IsSafeDelegate(delegateFunc)) continue;
                
                result = delegateFunc();
            }
            
            return result;
	    }

	    public static TResult Fire<TParam, TResult>(this Func<TParam, TResult> func, TParam param)
	    {
            TResult result = default(TResult);
            
		    if (func == null) return result;
            
		    Delegate[] delegates = func.GetInvocationList();
            
		    foreach (var del in delegates)
            {
                Func<TParam, TResult> delegateFunc = (Func<TParam, TResult>)del;
                
                if (!IsSafeDelegate(delegateFunc)) continue;
                
                result = delegateFunc(param);
            }
            
            return result;
	    }

	    private static bool IsSafeDelegate(Delegate del)
	    {
		    object target = del.Target;

            if (target == null)
            {
                if (del.Method.IsStatic) return true;
                
#if UNITY_EDITOR
                Debug.LogWarning($"Delegate target is null along with not static method: {del.Method.Name} {del.Method.DeclaringType}");
#endif
                return false;
            }

            if ((target is not UnityEngine.Object) || !target.Equals(null)) return true;
            
#if UNITY_EDITOR
            Debug.LogWarning($"Delegate target was destroyed in native side but exist managed object: {del.Method.Name} {del.Method.DeclaringType}\n{StackTraceUtility.ExtractStackTrace()}");
#endif
            return false;
        }
    }
}