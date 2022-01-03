using System;
using UnityEngine;
using UniRx;

public static class TransformExtension
{
    //--------------------------------------------------------------------------------------------------------------
    public static IObservable<Unit> LerpPosition(this Transform _transform, float _duration, Vector3 _targetPosition)
    {

        return Observable.Create<Unit>(_observer =>
        {

            int milliseconds = Mathf.RoundToInt(_duration * 1000f);
            var progress = 0f;
            var startPosition = _transform.position;

            IDisposable disposable = LerpThread
                .Execute
                (
                    milliseconds,
                    _count =>
                    {
                        progress += Time.deltaTime / _duration;
                        var targetInProgress = Vector3.Lerp(startPosition, _targetPosition, progress);
                        _transform.position = targetInProgress;
                    },
                    () =>
                    {
                        _transform.position = _targetPosition;
                        _observer.OnNext(default(Unit));
                        _observer.OnCompleted();

                    }
                );

            return Disposable.Create(() => disposable?.Dispose());
        });
    }

    public static IObservable<Unit> LerpLocalPosition(this Transform _transform, float _duration, Vector3 _targetPosition)
    {

        return Observable.Create<Unit>(_observer =>
        {

            int milliseconds = Mathf.RoundToInt(_duration * 1000f);
            var progress = 0f;
            var startPosition = _transform.localPosition;

            IDisposable disposable = LerpThread
                .Execute
                (
                    milliseconds,
                    _count =>
                    {
                        progress += Time.deltaTime / _duration;
                        var targetInProgress = Vector3.Lerp(startPosition, _targetPosition, progress);
                        _transform.localPosition = targetInProgress;
                    },
                    () =>
                    {
                        _transform.localPosition = _targetPosition;
                        _observer.OnNext(default(Unit));
                        _observer.OnCompleted();

                    }
                );

            return Disposable.Create(() => disposable?.Dispose());
        });
    }
    //--------------------------------------------------------------------------------------------------------------

    internal static class LerpThread
	{
		//--------------------------------------------------------------------------------------------------------------

		public static IDisposable Execute(int _miliseconds, Action<long> _onNext, Action _onComplete = null)
		{
			return Observable
				.EveryUpdate()
				.Take(TimeSpan.FromMilliseconds(_miliseconds))
				.Subscribe
				(
					_onNext,
					_onComplete
				);
		}
		
		//--------------------------------------------------------------------------------------------------------------
	}
}