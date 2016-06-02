using System;
using System.Collections.Generic;

namespace UnityEngine
{
	public class InputManager
	{
		internal bool GetTouchSupported()
		{
			return true;
		}

		internal Vector3 mousePosition;
		internal Dictionary<int, Touch> touches = new Dictionary<int, Touch>();
		internal Dictionary<long, int> fingerIDmap = new Dictionary<long, int>();
		List<int> fingerIds = new List<int>();
		int touchCount {get { return fingerIds.Count; } }
		public InputManager()
		{
		}
		internal bool GetMouseButton(int button)
		{
			return mouseDown[button];
		}

		internal bool GetMouseButtonDown(int button)
		{
			return firstMouseDown[button];
		}

		internal bool GetMouseButtonUp(int button)
		{
			return firstMouseUp[button];
		}

		internal Vector3 GetMousePosition()
		{
			return mousePosition;
		}

		internal Touch GetTouch(int index)
		{
			return touches[fingerIds[index]];
		}

		internal int GetTouchCount()
		{
			return touchCount;
		}

		internal void Update()
		{
			for (var i = 0; i < firstMouseDown.Length; i++) {
				firstMouseDown[i] = false;
				firstMouseUp[i] = false;
			}
			UpdateInternal();
			DoUpdate(this);
		}

		internal void UpdateTouchStart(float x, float y, long index)
		{
			pendingData.Add(new Data(Data.Type.TouchStart, x, y, index));
		}

		internal void UpdateTouchMove(float x, float y, long index)
		{
			pendingData.Add(new Data(Data.Type.TouchMove, x, y, index));
		}

		internal void UpdateTouchCancel(float x, float y, long index)
		{
			pendingData.Add(new Data(Data.Type.TouchCancel, x, y, index));
		}

		internal void UpdateTouchEnd(float x, float y, long index)
		{
			pendingData.Add(new Data(Data.Type.TouchEnd, x, y, index);
		}

		bool[] mouseDown = new bool[10];
		bool[] firstMouseDown = new bool[10];
		bool[] firstMouseUp = new bool[10];
		internal void UpdateMouseDown(float x, float y, long index)
		{
			pendingData.Add(new Data(Data.Type.MouseDown, x, y, index));
		}

		internal void UpdateMouseMove(float x, float y, long index)
		{
			pendingData.Add(new Data(Data.Type.MouseMove, x, y, index));
		}

		internal void UpdateMouseUp(float x, float y, long index)
		{
			pendingData.Add(new Data(Data.Type.MouseUp, x, y, index));
		}

		int mappedIDCount = 0;
		int mapFingerID (long id) {
			if (fingerIDmap.ContainsKey(id)) {
				return fingerIDmap[id];
			} else {
				fingerIDmap[id] = mappedIDCount++;
				return fingerIDmap[id];
			}
		}

		void unmapFingerID (long id) {
			if (fingerIDmap.ContainsKey(id)) {
				fingerIDmap.Remove(id);
			}
		}

		List<Data> left = new List<Data>();
		void UpdateInternal()
		{
			bool shouldContinue = true;
			if (pendingData.Count == 0)
			{
				bool began = false;
				foreach (var item in touches) {
					var touch = item.Value;
					if (touch.phase == TouchPhase.Began)
					{
						touch.Move(touch.position.x, touch.position.y);
						began = true;
					}
				}
				if (began) {
					return;
				}
			}
			foreach (var d in pendingData)
			{
				if (!shouldContinue)
				{
					left.Add(d);
					continue;
				}

				switch (d.type)
				{
					case Data.Type.MouseDown:
					{
						mouseDown[d.index] = true;
						firstMouseDown[d.index] = true;
						mousePosition.x = d.x; mousePosition.y = d.y;
						break;
					}
					case Data.Type.MouseUp:
					{
						mouseDown[d.index] = false;
						firstMouseUp[d.index] = true;
						mousePosition.x = d.x; mousePosition.y = d.y;
						break;
					}
					case Data.Type.MouseMove:
					{
						mousePosition.x = d.x; mousePosition.y = d.y;
						break;
					}
					case Data.Type.TouchStart:
					{
						int index = mapFingerID(d.index);
						Touch touch = new Touch();
						touch.fingerId = 0;
						touch.tapCount = 1;
						touch.deltaTime = 0;
						touch.Start(d.x, d.y);
						touch.fingerId = index;
						touches[index] = touch;
						shouldContinue = false;
						break;
					}
					case Data.Type.TouchMove:
					{
						int index = mapFingerID(d.index);
						touches[index].Move(d.x, d.y);
						break;
					}
					case Data.Type.TouchCancel:
					{
						int index = mapFingerID(d.index);
						touches[index].Cancel(d.x, d.y);
						unmapFingerID(d.index);
						shouldContinue = false;
						break;
					}
					case Data.Type.TouchEnd:
					{
						int index = mapFingerID(d.index);
						touches[index].End(d.x, d.y);
						unmapFingerID(d.index);
						shouldContinue = false;
						break;
					}
				}
			}
			if (left.Count > 0)
			{
				var t = left;
				left = pendingData;
				left.Clear();
				pendingData = t;
			}
			else
			{
				pendingData.Clear();
			}


			fingerIds = new List<int>(touches.Keys);
			foreach (var id in fingerIds) {
				var touch = touches[id];
				if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && touch.time < Time.time)
				{
					touches.Remove(id);
				}
			}

			if (fingerIds.Count != touches.Count) {
				fingerIds = new List<int>(touches.Keys);
			}
			// Console.WriteLine("touchCount"+touchCount);
		}

		List<Data> pendingData = new List<Data>();

		class Data
		{
			public enum Type
			{
				MouseDown,
				MouseUp,
				MouseMove,
				TouchStart,
				TouchMove,
				TouchCancel,
				TouchEnd,
			}
			public Type type;
			public float x;
			public float y;
			public long index;
			public Data(Type type, float x, float y, long index) {
				this.type = type;
				this.x = x;
				this.y = y;
				this.index = index;
			}
		}

		static extern void DoUpdate(InputManager inputManager);
	}
}
