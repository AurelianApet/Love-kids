using UnityEngine;

public class TestComboBox : MonoBehaviour 
{
	public ComboBox comboBox;
	public Sprite image;

	private void Start() 
	{
		var itemMakeBig = new ComboBoxItem("Make me big!");
		var itemMakeNormal = new ComboBoxItem("Normal", image, true);
		var itemMakeSmall = new ComboBoxItem("Make me small!");
		itemMakeBig.OnSelect += () =>
		{
			comboBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180);
			comboBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
			comboBox.UpdateGraphics();
			itemMakeBig.Caption = "Big";
			itemMakeBig.IsDisabled = true;
			itemMakeNormal.Caption = "Make me normal!";
			itemMakeNormal.IsDisabled = false;
			itemMakeSmall.Caption = "Make me small!";
			itemMakeSmall.IsDisabled = false;
		};

	//	comboBox.AddItems(itemMakeBig, itemMakeNormal, itemMakeSmall);
		comboBox.SelectedIndex = 1;
		comboBox.OnSelectionChanged += (int index) =>
		{
			Camera.main.backgroundColor = new Color32((byte)Random.Range(0, 256), 
			                                          (byte)Random.Range(0, 256), 
			                                          (byte)Random.Range(0, 256), 
			                                          255);
		};
	}
}
