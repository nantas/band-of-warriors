// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

#if !(UNITY_IPHONE || UNITY_ANDROID || UNITY_FLASH)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets a named texture in a game object's material to a movie texure.")]
	public class SetMaterialMovieTexture : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		public FsmInt materialIndex;
		
		[UIHint(UIHint.NamedTexture)]
		public FsmString namedTexture;

		[RequiredField]
		[ObjectType(typeof(MovieTexture))]
		public FsmObject movieTexture;

		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			namedTexture = "_MainTex";
			movieTexture = null;
		}

		public override void OnEnter()
		{
			DoSetMaterialTexture();
			Finish();
		}

		void DoSetMaterialTexture()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}

			if (go.renderer == null)
			{
				LogError("Missing Renderer!");
				return;
			}

			if (go.renderer.material == null)
			{
				LogError("Missing Material!");
				return;
			}

			var movie = movieTexture.Value as MovieTexture;

			var namedTex = namedTexture.Value;
			if (namedTex == "") namedTex = "_MainTex";

			if (materialIndex.Value == 0)
			{
				go.renderer.material.SetTexture(namedTex, movie);
			}
			else if (go.renderer.materials.Length > materialIndex.Value)
			{
				var materials = go.renderer.materials;
				materials[materialIndex.Value].SetTexture(namedTex, movie);
				go.renderer.materials = materials;
			}
		}
	}
}

#endif