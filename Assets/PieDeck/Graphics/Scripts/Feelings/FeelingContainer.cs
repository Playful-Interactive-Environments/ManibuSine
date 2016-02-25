using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System;
using System.Linq;

public enum Feeling {
	[Description("Happy")]
	Happy = 0,
	[Description("Sad")]
	Sad,
	[Description("Anger")]
	Anger,
	[Description("Scared")]
	Scared,
	[Description("Ugly")]
	Ugly,
	[Description("Tired")]
	Tired,
	[Description("Confused")]
	Confused,
	[Description("Embarrassed")]
	Embarrassed,
	[Description("Neutral")]
	Neutral
}

public static class FeelingContainer {
	private static Material[] materials;
	public static Material[] Materials {
		get {
			if (materials == null || materials.Length == 0) {
				Material[] mArray = Resources.FindObjectsOfTypeAll<Material> ();
				if (mArray == null)
					return null;

				materials = new Material[(int)Feeling.Embarrassed + 1];
				for (int i = 0; i < materials.Length; i++) {
					string desciption = ((Feeling)i).GetDescription ();
					if (desciption == null)
						desciption = "Neutral";
					Material m = mArray.FirstOrDefault (x => x != null && x.name != null && x.name.EndsWith(desciption));
					materials [i] = m; 
				}

			}

			return materials;
		}
	}

	private static GameObject[] bubbleColorContainer;
	public static void SetBubbleColorContainer (GameObject[] bubbleContainer) {
		bubbleColorContainer = new GameObject[(int)Feeling.Embarrassed + 1];
		for (int i = 0; i < bubbleColorContainer.Length; i++) {
			string desciption = ((Feeling)i).GetDescription ();
			bubbleColorContainer [i] = bubbleContainer.First (x => x.name.Contains (desciption));
		}
	}
	public static GameObject[] BubbleColorContainer {
		get {
			return bubbleColorContainer;
		}
	}

	public static Feeling GetFeeling(this Material m) {
		for (int i = 0; i < Materials.Length; i++) {
			if (m.name.Contains(Materials [i].name))
				return (Feeling)i;
		}
		return Feeling.Neutral;
	}

	public static GameObject GetBubbleColorContainer(this Material m) {
		Feeling f = m.GetFeeling ();

		if (f != Feeling.Neutral)
			return BubbleColorContainer [(int)f];

		return null;
	}

	public static string GetDescription(this Enum value){
		DescriptionAttribute attr = ((DescriptionAttribute)Attribute.GetCustomAttribute (
			value.GetType ().GetFields (BindingFlags.Public | BindingFlags.Static)
			.Single (x => x.GetValue (null).Equals (value)),
			typeof(DescriptionAttribute)));
		if (attr != null)
			return attr.Description;
		else
			return "NULL";
	}
}

