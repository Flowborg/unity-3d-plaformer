using UnityEngine;
using UnityEngine.UI;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Scroller")]
	public class Scroller : MonoBehaviour
	{
		public bool unscaledTime;
		public Graphic targetGraphic;

		[Header("Scroll Settings")]
		public float baseSpeed = 0.1f;
		public Vector2 normalSpeedRange = new Vector2(0.2f, 0.5f);
		public Vector2 burstSpeedRange = new Vector2(2.0f, 5.0f);
		public float burstChance = 0.4f; // 40% of the time it does a speed burst!

		[Header("Tiling Settings")]
		public Vector2 scaleRange = new Vector2(0.5f, 2.0f);

		[Header("Color Settings")]
		public Color[] tintColors;

		[Header("Rotation Settings")]
		public bool enableRotation = true;
		public float maxRotationAngle = 15f;

		private Vector2 currentScrollSpeed;
		private Vector2 targetScrollSpeed;
		private Vector2 scrollVelocity;

		private Vector2 currentTiling;
		private Vector2 targetTiling;
		private Vector2 tilingVelocity;

		private Color currentColor;
		private Color targetColor;

		private float currentRotation;
		private float targetRotation;
		private float rotationVelocity;

		private Material runtimeMaterial;
		private float timeSinceLastChange = 0f;
		private float changeInterval = 2f; // faster change cycles!

		void Start()
		{
			InitializeGraphic();
			InitializeMaterial();

			currentScrollSpeed = RandomDirection() * baseSpeed;
			targetScrollSpeed = currentScrollSpeed;

			currentTiling = runtimeMaterial.mainTextureScale;
			targetTiling = currentTiling;

			currentColor = targetGraphic.color;
			targetColor = GetRandomColor();

			currentRotation = 0f;
			targetRotation = 0f;
		}

		void Update()
		{
			float deltaTime = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
			timeSinceLastChange += deltaTime;

			if (timeSinceLastChange >= changeInterval)
			{
				timeSinceLastChange = 0f;
				changeInterval = Random.Range(1f, 3f); // quick switches!

				bool burst = Random.value < burstChance;
				var speedMultiplier = burst 
					? Random.Range(burstSpeedRange.x, burstSpeedRange.y)
					: Random.Range(normalSpeedRange.x, normalSpeedRange.y);

				targetScrollSpeed = RandomDirection() * baseSpeed * speedMultiplier;
				targetTiling = Vector2.one * Random.Range(scaleRange.x, scaleRange.y);
				targetColor = GetRandomColor();
				targetRotation = enableRotation ? Random.Range(-maxRotationAngle, maxRotationAngle) : 0f;
			}

			currentScrollSpeed = Vector2.SmoothDamp(currentScrollSpeed, targetScrollSpeed, ref scrollVelocity, 1f);
			currentTiling = Vector2.SmoothDamp(currentTiling, targetTiling, ref tilingVelocity, 2f);
			currentColor = Color.Lerp(currentColor, targetColor, deltaTime * 0.5f);
			currentRotation = Mathf.SmoothDamp(currentRotation, targetRotation, ref rotationVelocity, 1.5f);

			runtimeMaterial.mainTextureOffset += currentScrollSpeed * deltaTime;
			runtimeMaterial.mainTextureScale = currentTiling;
			targetGraphic.color = currentColor;

			if (enableRotation)
				targetGraphic.rectTransform.localRotation = Quaternion.Euler(0f, 0f, currentRotation);
		}

		protected virtual void InitializeGraphic()
		{
			if (!targetGraphic)
				targetGraphic = GetComponent<Graphic>();
		}

		protected virtual void InitializeMaterial()
		{
			runtimeMaterial = Instantiate(targetGraphic.material);
			targetGraphic.material = runtimeMaterial;
		}

		private Vector2 RandomDirection()
		{
			float angle = Random.Range(0f, Mathf.PI * 2);
			return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}

		private Color GetRandomColor()
		{
			if (tintColors != null && tintColors.Length > 0)
				return tintColors[Random.Range(0, tintColors.Length)];
			else
				return new Color(Random.value, Random.value, Random.value);
		}
	}
}
